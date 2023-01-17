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
			ent.Tags.Add( "ragdoll", "solid", "debris" );
			ent.UsePhysicsCollision = true;

			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.TakeDecalsFrom( this );
			ent.SetBodyGroup( 0, Fat > 0 ? 2 : 1 );
			ent.SetRagdollVelocityFrom( this );
			ent.Scale = Scale;
			ent.EnableAllCollisions = true;
			ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ent.RenderColor = RenderColor;
			ent.PhysicsEnabled = true;


		}
	}
}
