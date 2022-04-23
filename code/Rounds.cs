
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace BlubberRunner
{

	public partial class BlubberGame : Sandbox.Game
	{

		public static int CurrentRound { get; set; }

		public static void StartRound( int roundNum )
		{

			CurrentRound = roundNum;

			LoadLevel( CurrentRound % Levels.Count );

			if (Host.IsServer)
			{

				Event.Run("Blubber_Round_Start");

			}

		}

	}

	public partial class BlubberPlayer : Player
	{

		public async void GameOver()
		{

			Ragdollise();

			EnableDrawing = false;
			Alive = false;
			Controller = null;
			Animator = null;

			Dance = 0;

			await Task.Delay( 3000 );

			Respawn();
			Points = 0;

			BlubberGame.StartRound( 0 );

		}

		public async void EpicWin()
		{

			Celebrate( 10000 );

			await Task.Delay( 10000 );

			BlubberGame.StartRound( BlubberGame.CurrentRound + 1 );

		}

		public async void Celebrate( int ms)
		{

			Dance = Rand.Int( 1, 3 );

			await Task.Delay( ms );

			Dance = 0;

		}

	}

}
