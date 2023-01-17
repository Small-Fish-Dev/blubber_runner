
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlubberRunner
{

	[Library( "blubber_runner", Title = "Blubber Runner" )]
	public partial class BlubberGame : GameManager
	{

		public static BlubberGame Instance;

		public BlubberGame()
		{

			Instance = this;

			if ( Game.IsServer )
			{

				new BlubberHud();

			}

			LoadItemTypes();
			LoadLevels();

		}

		public override void ClientJoined( IClient client )
		{

			base.ClientJoined( client );

			var ply = new BlubberPlayer();
			client.Pawn = ply;

			ply.Respawn();
			ply.HandleMusic();
			StartRound( 0 );

		}

		/*[ServerCmd("resetscores")]
		public static async void ResetScores()
		{

			var leaderBoard = await GameServices.Leaderboard.Query("facepunch.platformer");

			foreach (var entry in leaderBoard.Entries)
			{

				await GameServices.SubmitScore(entry.PlayerId, -entry.Rating);
				Log.Info($" Reset score of {entry.DisplayName}");

			}

		}

		[ServerCmd("set_score")]
		public static async void ResetScores( string input )
		{

			long steamid = long.Parse(input);
			var leaderBoard = await GameServices.Leaderboard.Query("fish.blubber_runner");
			var entry = leaderBoard.Entries.Find(e => e.PlayerId == steamid);
			

			await GameServices.SubmitScore(steamid, 999);
			Log.Info($" Reset score of {entry.DisplayName}");

		}*/

	}

}
