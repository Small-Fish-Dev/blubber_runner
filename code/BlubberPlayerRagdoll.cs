using Sandbox;
using System;
using System.Linq;

namespace BlubberRunner
{

	partial class BlubberPlayer
	{

		[ClientRpc]
		void Ragdollise()
		{
			var ent = new ModelEntity();
			ent.Position = Position;
			ent.Rotation = Rotation;
			ent.MoveType = MoveType.Physics;
			ent.UsePhysicsCollision = true;
			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.TakeDecalsFrom( this );
			ent.SetBodyGroup( 0, Fat > 0 ? 2 : 1 );
			ent.SetRagdollVelocityFrom( this );
			ent.DeleteAsync( 3f );

			Corpse = ent;

		}
	}
}
