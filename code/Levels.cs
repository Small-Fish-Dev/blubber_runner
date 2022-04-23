
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace BlubberRunner
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

	public partial class BlubberGame : Sandbox.Game
	{

		public static List<Collectible> ItemList = new List<Collectible>();
		public static List<Level> Levels;
		public static Dictionary<string, ItemType> ItemTypes;
		

		public static void LoadItemTypes()
		{

			ItemTypes = FileSystem.Mounted.ReadJson<Dictionary<string, ItemType>>( "json/collectibles.json" );

		}
		
		public static void LoadLevels()
		{

			Levels = FileSystem.Mounted.ReadJson<List<Level>>( "json/levels.json" );
			
		}

		public static void LoadLevel(int LevelNumber)
		{

			if(Host.IsClient)
			{
				return;
			}

			Level Level = Levels[LevelNumber];

			if ( ItemList != null )
			{

				foreach ( Collectible collectible in ItemList )
				{

					if ( collectible.IsValid() && collectible != null )
					{

						collectible.Delete();

					}

				}

			}

			for(int i = 0; i < Level.Items.Count; i++ )
			{

				Item item = Level.Items[i];
				ItemType properties = ItemTypes[item.Type];

				Collectible spawnedItem = new() {
					Position = item.Position,
					Scale = properties.Scale,
					Points = properties.Points,
					Calories = properties.Calories,
					Diameter = properties.Diameter
				};

				spawnedItem.SetModel( properties.Model );

				ItemList.Add( spawnedItem );

			}

		}

	}

}
