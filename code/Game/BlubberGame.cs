public partial class BlubberGame : Component
{
	public struct ItemType
	{
		public string Model { get; set; }
		public float Scale { get; set; }
		public int Points { get; set; }
		public int Calories { get; set; }
		public int Diameter { get; set; }
	}

	public struct Item
	{
		public string Type { get; set; }
		public Vector3 Position { get; set; }

	};

	public struct Level
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public List<Item> Items { get; set; }

	};

	[Property] GameObject CollectiblePrefab { get; set; }

	public static List<Collectible> ItemList = new List<Collectible>();
	public static List<Level> Levels;
	public static Dictionary<string, ItemType> ItemTypes;

	public static BlubberGame Instance { get; set; }

	public static int CurrentRound { get; set; }


	protected override void OnStart()
	{
		Instance = this;
		LoadItemTypes();
		LoadLevels();

		StartRound( 0 );
	}

	public static void StartRound( int roundNum )
	{
		CurrentRound = roundNum;

		Game.ActiveScene.GetAllComponents<BlubberPlayer>().First().Respawn();

		Instance.LoadLevel( CurrentRound % Levels.Count );

	}

	public static void LoadItemTypes()
	{
		ItemTypes = FileSystem.Mounted.ReadJson<Dictionary<string, ItemType>>( "json/collectibles.json" );
	}

	public static void LoadLevels()
	{
		Levels = FileSystem.Mounted.ReadJson<List<Level>>( "json/levels.json" );
	}

	public void LoadLevel( int LevelNumber )
	{
		Level Level = Levels[LevelNumber];

		if ( ItemList != null )
		{
			foreach ( Collectible collectible in ItemList )
			{
				if ( collectible.IsValid() && collectible != null )
				{
					collectible.DestroyGameObject();
				}
			}
		}

		for ( int i = 0; i < Level.Items.Count; i++ )
		{

			Item item = Level.Items[i];
			ItemType properties = ItemTypes[item.Type];

			var go = CollectiblePrefab.Clone();
			var collect = go.GetComponent<Collectible>();
			collect.SetupItem( properties, item );
			ItemList.Add( collect );
		}
	}
}
