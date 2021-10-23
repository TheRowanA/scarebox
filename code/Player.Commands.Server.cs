using System.Collections.Generic;
using Sandbox;

namespace scarebox
{
    public partial class ScareboxPlayer
    {
		
        [ServerCmd("inventory_getcurrent")]
		public static void GetInventoryCurrent()
		{
			var pawn = ConsoleSystem.Caller.Pawn;
			if (pawn == null) return;

			var inventory = pawn.Inventory;
			if (inventory == null) return;

			for (int i = 0; i < inventory.Count(); ++i)
			{
				var slot = inventory.GetSlot(i);
				if (!slot.IsValid()) continue;

				Log.Info("Slot (" + i + ") = " + slot.ClassInfo.FullName);
			}
		}

		[ServerCmd("inventory_getcurrentitems")]
		public static void GetInventoryCurrentItems()
		{
			var pawn = ConsoleSystem.Caller.Pawn;
			if (pawn == null) return;

			ScareboxInventory inventory = (ScareboxInventory)pawn.Inventory;
			if (inventory == null) return;

			foreach (KeyValuePair<string, int> items in inventory.Items.GetItems())
			{
				Log.Info("Item: " + items.Key + ", " + "Amount: " + items.Value);
			}

		}

		[ServerCmd("inventory_removeitem")]
		public static void RemoveInventoryItem(string item)
		{
			var pawn = ConsoleSystem.Caller.Pawn;
			if (pawn == null) return;

			ScareboxInventory inventory = (ScareboxInventory)pawn.Inventory;
			if (inventory == null) return;

			if (inventory.Items.Remove(item))
			{
				Log.Info("Removed: " + item);
			}
			else
			{
				Log.Info("Failed To Remove: " + item);
			}

		}

		[ServerCmd("inventory_setitem")]
		public static void SetInventoryItem(string item, int amount)
		{
			var pawn = ConsoleSystem.Caller.Pawn;
			if (pawn == null) return;

			ScareboxInventory inventory = (ScareboxInventory)pawn.Inventory;
			if (inventory == null) return;

			if (inventory.Items.Set(item, amount))
			{
				Log.Info("Successfully Set: " + item + ", Amount: " + amount);
			}
			else
			{
				Log.Info("Failed To Set: " + item);
			}

		}
    }
}
