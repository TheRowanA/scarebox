using System.Collections.Generic;
using System;
using Sandbox;

namespace scarebox
{
	public partial class ItemInventory
	{
		private Dictionary<string, int> ItemsList { get; } = new();
		private readonly ScareboxPlayer player;

		public ItemInventory( ScareboxPlayer owner )
		{
			player = owner;
		}

		public int Count( string itemName )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null || string.IsNullOrEmpty( item ) )
			{
				return 0;
			}

			if ( !ItemsList.ContainsKey( item ) )
			{
				return 0;
			}

			return ItemsList[item];
		}

		public Dictionary<string, int> GetItems()
		{
			return ItemsList;
		}

		public bool Set( string itemName, int amount )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null ) return false;

			while ( !ItemsList.ContainsKey( item ) )
			{
				ItemsList.Add( item, 0 );
			}

			ItemsList[item] = amount;

			if ( Host.IsServer )
			{
				player.ClientSetItem( To.Single( player ), item, amount );
			}

			return true;
		}

		public bool Give( string itemName, int amount )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null ) return false;

			Set( item, Count( item ) + amount );

			return true;
		}

		public int Take( string itemName, int amount )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null ) return 0;

			int available = Count( itemName );
			amount = Math.Min( available, amount );

			Set( item, available - amount );

			return amount;
		}

		public void Clear()
		{
			ItemsList.Clear();

			if ( Host.IsServer )
			{
				player.ClientClearItem( To.Single( player ) );
			}
		}

		public bool Remove( string itemName )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null ) return false;

			if ( !ItemsList.ContainsKey( item ) ) return false;

			Take( item, ItemsList[item] );
			return ItemsList.Remove( item );
		}

		// Drops given item and amount***
		public bool Drop( string itemName, int amount )
		{
			string item = itemName.ToLower();

			if ( ItemsList == null ) return false;

			if ( !ItemsList.ContainsKey( item ) ) return false;

			if ( Count( item ) < 1 ) return false;

			if ( !SpawnItem( "scareb_item_" + item ) ) return false;

			Take( item, amount );

			return true;
		}

		// Spawn Item at player eye trace
		public bool SpawnItem( string itemName )
		{
			var attribute = Library.GetAttribute( itemName );

			if ( attribute == null || !attribute.Spawnable ) return false;

			var tr = Trace.Ray( player.EyePos, player.EyePos + player.EyeRot.Forward * 100 )
				.UseHitboxes()
				.Ignore( player )
				.Size( 2 )
				.Run();

			var ent = Library.Create<Entity>( itemName );

			ent.Position = tr.EndPos;
			ent.Rotation = Rotation.From( new Angles( 0, player.EyeRot.Angles().yaw, 0 ) );

			return true;
		}

	}
}
