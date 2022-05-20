using Sandbox;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlubberRunner
{
	partial class BlubberPlayer : Player
	{

		[Net] public int Fat { get; set; }
		[Net] public int Dance { get; set; }
		[Net] public int Points { get; set; } 
		[Net] public bool Alive { get; set; }

		public override void Respawn()
		{
			SetModel( "models/fatfuck/fatfuck.vmdl" );

			Alive = true;

			Controller = new PlayerController2D();
			Animator = new PlayerAnimator();
			CameraMode = new ThirdPerson();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();

			Fat = Rand.Int( 1 , 12 );
			Dance = 0;

			SetBodyGroup( 0, 0 );

		}

		[Event("Blubber_Round_Start")]
		void RoundRespawn()
		{

			Respawn();

		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			SimulateActiveChild( cl, ActiveChild );

			if ( IsServer && Input.Pressed( InputButton.PrimaryAttack ) )
			{

				var ent = new Collectible
				{
					Position = this.Position + Vector3.Forward * 100f + Vector3.Up * 20f
				};

				ent.Velocity = this.EyeRotation.Forward * 1000;

			}
		}

		public async void HandleMusic()
		{

			this.PlaySound( "megatron" );

			await Task.Delay( 94000 );

			HandleMusic();

		}

		[Event.Tick]
		public void Tick()
		{

			foreach ( Entity ent in Entity.FindInSphere( Position, 100 ) )
			{

				if ( ent is Collectible && ent.IsValid() )
				{

					Vector2 entFlatPos = new( ent.Position.x, ent.Position.y );
					Vector2 plyFlatPos = new( Position.x, Position.y );

					Collectible col = ent as Collectible;

					if ( Vector2.GetDistance( entFlatPos, plyFlatPos ) < col.Diameter + Fat * 1.5f )
					{

						Fat = Math.Clamp( Fat + col.Calories, -1, 14 );
						Points += col.Points;

						if (Host.IsServer)
						{

							GameServices.SubmitScore(Client.PlayerId, col.Points);

						}

						ent.Delete();

						if ( Fat == 14 || Fat == -1)
						{

							GameOver();

						}

					}

				}

			}

			if ( Position.x >= 2670 && Dance == 0)
			{

				EpicWin();

			}

			if ( Dance != 0 ) //Manual rotation towards camera if root bone doesn't work
			{

				Rotation = Rotation.Slerp( Rotation, Rotation.FromYaw( 180 ), Time.Delta * 2f );
				
			}

		}

	}

	public class ThirdPerson : CameraMode
	{

		public override void Update()
		{

			var pawn = Local.Pawn as AnimatedEntity;

			if ( pawn == null )
			{

				return;

			}


			Position = pawn.Position + Vector3.Up * 150 + Vector3.Backward * 200;
			Rotation = Rotation.FromAxis( Vector3.Right, -15 );

			FieldOfView = 70f;

		}

	}

	public class PlayerAnimator : PawnAnimator
	{

		public override void Simulate()
		{

			BlubberPlayer ply = Pawn as BlubberPlayer;

			SetAnimParameter( "move_direction", (-Vector3.VectorAngle( -Velocity ).yaw + 180f) / 90f ); //TODO: Fix this shit
			SetAnimParameter( "wishspeed", Velocity.Length * 1.2f );
			SetAnimParameter( "burger_number", Math.Clamp( ply.Fat, 0, 13 ) );
			SetAnimParameter( "dances", ply.Dance);

		}

	}

	public class PlayerController2D : BasePlayerController
	{

		public override void Simulate()
		{

			var alwaysForward = true;
			var walkSpeed = 160f;
			var invisibleWalls = new Vector2( 52f, 204.5f );

			BlubberPlayer ply = Pawn as BlubberPlayer;

			if( ply.Dance != 0 )
			{

				Velocity = Vector3.Zero;

				AnimatedEntity anim = Pawn as AnimatedEntity;
				Position += anim.RootMotion * Rotation.z * Time.Delta;

				return;

			}

			var inputs = Vector3.Forward * (alwaysForward ? 1 : Input.Forward) + Vector3.Left * Input.Left;

			var vel = new Vector3( Math.Max( inputs.x, 0f ), Math.Clamp( inputs.y, Position.y == invisibleWalls.x ? 0 : -1, Position.y == invisibleWalls.y ? 0 : 1 ), inputs.z );
			Velocity = Vector3.Lerp( Velocity, vel * walkSpeed, Time.Delta * 5f );

			Position += Velocity * Time.Delta;
			Position = new Vector3( Position.x, Math.Clamp( Position.y, invisibleWalls.x, invisibleWalls.y ), Position.z ); //Invisible walls

		}

	}

}
