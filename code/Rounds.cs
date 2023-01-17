
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace BlubberRunner
{

	public partial class BlubberGame
	{

		public static int CurrentRound { get; set; }

		public static void StartRound( int roundNum )
		{

			CurrentRound = roundNum;

			LoadLevel( CurrentRound % Levels.Count );

			if (Game.IsServer)
			{

				Event.Run("Blubber_Round_Start");

			}

		}

	}

	public partial class BlubberPlayer
	{

		public async void GameOver()
		{

			Ragdollise();

			EnableDrawing = false;
			Alive = false;

			Dance = 0;

			await GameTask.Delay( 3000 );

			Respawn();
			Points = 0;

			BlubberGame.StartRound( 0 );

		}

		public async void EpicWin()
		{

			Celebrate( 10000 );

			await GameTask.Delay( 10000 );

			BlubberGame.StartRound( BlubberGame.CurrentRound + 1 );

		}

		public async void Celebrate( int ms)
		{

			Dance = Game.Random.Int( 1, 3 );

			await GameTask.Delay( ms );

			Dance = 0;

		}

	}

}
