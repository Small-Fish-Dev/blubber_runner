public sealed class Collectible : Component, Component.ITriggerListener
{
	[Property] SkinnedModelRenderer Body { get; set; }
	[Property] SphereCollider Collider { get; set; }

	public int Points { get; set; } = 0;
	public int Calories { get; set; } = 0;

	public void SetupItem( BlubberGame.ItemType properties, BlubberGame.Item item )
	{
		WorldPosition = item.Position;

		Body.WorldScale = properties.Scale;
		Body.Model = Model.Load( properties.Model );

		Points = properties.Points;
		Calories = properties.Calories;
		Collider.Radius = properties.Diameter / 2;
	}

	void ITriggerListener.OnTriggerEnter( Sandbox.Collider other )
	{
		if ( other.GameObject.Components.TryGet<BlubberPlayer>( out var player ) )
		{
			player.Points += Points;
			player.Fat = (player.Fat + Calories).Clamp( -1, 14 );
			if ( player.Fat == 14 || player.Fat == -1 )
			{
				_ = player.GameOver();

			}


			DestroyGameObject();
		}
	}
}
