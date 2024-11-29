using System.Threading.Tasks;

public partial class BlubberPlayer
{
	public async Task GameOver()
	{
		Ragdollise();
		Alive = false;

		await Task.DelaySeconds( 3 );

		Alive = true;
		Points = 0;
		Respawn();
		BlubberGame.StartRound( 0 );
	}

	public async Task EpicWin()
	{
		_ = Celebrate( 5 );
		await Task.DelaySeconds( 5 );
		BlubberGame.StartRound( BlubberGame.CurrentRound + 1 );
	}

	public async Task Celebrate( int s )
	{
		Dance = Game.Random.Int( 1, 3 );
		await Task.DelaySeconds( s );
		Dance = 0;

	}
}
