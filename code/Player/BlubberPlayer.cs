using System;
public partial class BlubberPlayer : Component
{
	public int Fat { get; set; }
	public int Dance { get; set; } = 0;
	public int Points { get; set; }
	public bool Alive { get; set; } = true;

	Vector3 Velocity { get; set; }

	[Property] SkinnedModelRenderer Body { get; set; }

	[Property] ModelPhysics Ragdoll { get; set; }

	[Property] float WalkSpeed { get; set; } = 160;


	[Property] BoxCollider Collider { get; set; }

	Vector2 invisibleWalls = new Vector2( 52f, 204.5f );

	protected override void OnStart()
	{
		Respawn();
	}

	public void Respawn()
	{
		var spawn = Scene.GetAllComponents<SpawnPoint>().First();
		WorldPosition = spawn.WorldPosition;
		WorldRotation = spawn.WorldRotation;
		Transform.ClearInterpolation();

		Ragdoll.Enabled = false;
		Ragdoll.GameObject.LocalPosition = Vector3.Zero;
		Ragdoll.GameObject.LocalRotation = Rotation.Identity;

		Fat = Game.Random.Int( 3, 6 );
		Dance = 0;
	}

	protected override void OnUpdate()
	{
		Scene.Camera.WorldPosition = WorldPosition + Vector3.Up * 150 + Vector3.Backward * 200;
		Scene.Camera.WorldRotation = Rotation.FromAxis( Vector3.Right, -15 );

		if ( !Body.IsValid() )
			return;

		Body.Set( "move_direction", (-Vector3.VectorAngle( -Velocity ).yaw + 180f) / 90f ); //TODO: Fix this shit
		Body.Set( "wishspeed", Velocity.Length * 1.2f );
		Body.Set( "burger_number", Math.Clamp( Fat, 0, 13 ) );
		Body.Set( "dances", Dance );
	}

	protected override void OnFixedUpdate()
	{
		if ( !Alive )
			return;

		if ( Collider.IsValid() )
			Collider.Scale = new Vector3( 5.3f * Fat, 5.3f * Fat, 100 );

		HandleMovement();

		if ( WorldPosition.x >= 2670 && Dance == 0 && winning == false )
		{
			winning = true;
			_ = EpicWin();
		}

		if ( Dance != 0 ) //Manual rotation towards camera if root bone doesn't work
		{
			WorldRotation = Rotation.Slerp( WorldRotation, Rotation.FromYaw( 180 ), Time.Delta * 2f );
		}
	}

	void HandleMovement()
	{
		if ( Dance != 0 )
		{
			Velocity = Vector3.Zero;
			// TODO: Fix?
			WorldPosition += Body.RootMotion.Position * WorldRotation.z * Time.Delta;
			return;
		}

		BuildInputs();

		var inputs = InputDirection.WithX( 1 );
		var vel = new Vector3( Math.Max( inputs.x, 0f ), Math.Clamp( inputs.y, WorldPosition.y == invisibleWalls.x ? 0 : -1, WorldPosition.y == invisibleWalls.y ? 0 : 1 ), inputs.z );
		Velocity = Vector3.Lerp( Velocity, vel * WalkSpeed, Time.Delta * 5f );

		WorldPosition += Velocity * Time.Delta;
		WorldPosition = new Vector3( WorldPosition.x, Math.Clamp( WorldPosition.y, invisibleWalls.x, invisibleWalls.y ), WorldPosition.z ); //Invisible walls
	}

	public Vector3 InputDirection { get; set; }
	public Angles InputLook { get; set; }
	void BuildInputs()
	{
		InputLook += Input.AnalogLook;
		InputDirection = Input.AnalogMove;
	}

	public void Ragdollise()
	{
		Ragdoll.Enabled = true;
	}
}
