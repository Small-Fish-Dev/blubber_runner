using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace BlubberRunner
{

	public class Points : Panel
	{
		public Label Label;

		public Points()
		{
			Label = Add.Label();
		}

		public override void Tick()
		{
			var player = Game.LocalPawn as BlubberPlayer;
			if ( player == null ) return;

			int points = player.Points;

			Label.Text = $"Points: {points}";
		}
	}


	[Library]
	public partial class BlubberHud : HudEntity<RootPanel>
	{
		public BlubberHud()
		{
			if ( !Game.IsClient )
				return;

			RootPanel.StyleSheet.Load( "BlubberHud.scss" );

			RootPanel.AddChild<Points>();
		}
	}

}
