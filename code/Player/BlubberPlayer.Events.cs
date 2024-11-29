using System.Threading.Tasks;

public partial class BlubberPlayer
{
	public async Task GameOver()
	{
		Ragdollise();
		Alive = false;

		await Task.DelaySeconds( 6 );

		Alive = true;
		Points = 0;
		Respawn();
		BlubberGame.StartRound( 0 );
	}

	bool winning = false;
	public async Task EpicWin()
	{
		Log.Info( "next round" );
		_ = Celebrate( 6 );
		await Task.DelaySeconds( 6 );
		winning = false;

		BlubberGame.StartRound( BlubberGame.CurrentRound + 1 );
	}

	public async Task Celebrate( int s )
	{
		Dance = Game.Random.Int( 1, 3 );
		await Task.DelaySeconds( s );
		Dance = 0;

	}
}
