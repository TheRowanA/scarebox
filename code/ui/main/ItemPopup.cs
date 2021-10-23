using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

namespace scarebox
{
    public partial class ItemPopup : Panel
    {
		public string thisItem;
        public ItemPopup(string item, Panel parent)
		{
			Parent = parent;
			thisItem = item;
			
			var player = Local.Pawn;
			if (player == null) return;
				

			ScareboxInventory inventory = (ScareboxInventory)player.Inventory;
			if (inventory == null) return;

			var options = AddChild<ButtonGroup>();
			options.AddClass("options");

			var body = Add.Panel("body");
			{
				options.Add.Label("(" + item + ") Options");
				options.AddButton("Drop", () => 
				{
					foreach (KeyValuePair<string, int> items in inventory.Items.GetItems())
					{
						if (item == items.Key)
						{
							inventory.Items.Clear();
						}
					}
				});

				options.AddButton("Cancel", () => this.Delete(true));
			}


		}
    }
}
