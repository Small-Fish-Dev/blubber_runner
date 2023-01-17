using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox
{

	[Library( "blubber_collectible" )]
	public partial class Collectible : ModelEntity
	{

		public int Points { get; set; }
		public int Calories { get; set; }
		public int Diameter { get; set; }
		public bool Collected { get; set; } = false;

		public override void Spawn()
		{

			base.Spawn();
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );

		}

		protected override void OnDestroy()
		{

			base.OnDestroy();

		}
		
	}

}
