using System.Collections.Generic;
using System;
using Sandbox;

namespace scarebox
{
    public partial class ItemInventory
    {
        private Dictionary<string, int> ItemsList { get; } = new();
		private readonly ScareboxPlayer player;

		public ItemInventory(ScareboxPlayer owner)
		{
			player = owner;
		}

		public int Count(string itemName)
		{
			string item = itemName.ToLower();

			if (ItemsList == null || string.IsNullOrEmpty(item))
			{
				return 0;
			}

			if (!ItemsList.ContainsKey(item))
			{
				return 0;
			}
			
			return ItemsList[item];
		}

		public Dictionary<string, int> GetItems()
		{
			return ItemsList;
		}

		public bool Set(string itemName, int amount)
		{
			string item = itemName.ToLower();

			if (ItemsList == null) return false;

			while (!ItemsList.ContainsKey(item))
			{
				ItemsList.Add(item, 0);
			}

			ItemsList[item] = amount;

			if (Host.IsServer)
			{
				player.ClientSetItem(To.Single(player), item, amount);
			}

			return true;
		}

		public bool Give(string itemName, int amount)
		{
			string item = itemName.ToLower();

			if (ItemsList == null) return false;

			Set(item, Count(item) + amount);

			return true;
		}

		public int Take(string itemName, int amount)
		{
			string item = itemName.ToLower();

			if (ItemsList == null) return 0;

			int available = Count(itemName);
			amount = Math.Min(available, amount);

			Set(item, available - amount);

			return amount;
		}

		public void Clear()
		{
			ItemsList.Clear();

			if (Host.IsServer)
			{
				player.ClientClearItem(To.Single(player));
			}
		}

		public bool Remove(string itemName)
		{
			string item = itemName.ToLower();

			if (ItemsList == null) return false;

			if (!ItemsList.ContainsKey(item)) return false;

			Take(item, ItemsList[item]);
			return ItemsList.Remove(item);
		}

		public bool Drop(string itemName, int amount)
		{
			string item = itemName.ToLower();
			
			if (ItemsList == null) return false;

			if (!ItemsList.ContainsKey(item)) return false;

			//BaseItem entity = (BaseItem)BaseItem.Create( "scareb_item_" + item );
			//entity.Position = player.EyeRot.Forward * 10.0f;
			//entity.ApplyLocalImpulse(player.Velocity + player.EyeRot.Forward * 300.0f + Vector3.Up * 100.0f);
			

			if (Take(item, amount) < 1)
			{
				Remove(item);
			}
			return true;
		}

    }
}
