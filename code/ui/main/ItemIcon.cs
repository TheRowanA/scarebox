using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace scarebox
{
    public partial class ItemIcon : Panel
    {

		public Label Label;

		public Label Amount;

		public string thisItem;

        public ItemIcon(string item, Panel parent)
		{
			thisItem = item;
			Parent = parent;
			Label = Add.Label(item, "item-name");
			Amount = Add.Label("---", "item-amount");
		}

		public override void Tick()
		{
			base.Tick();

			
			if ( Parent.IsVisible)
			{
				var player = Local.Pawn;
				if (player == null) return;
				

				ScareboxInventory inventory = (ScareboxInventory)player.Inventory;
				if (inventory == null) return;

				foreach (KeyValuePair<string, int> items in inventory.Items.GetItems())
				{
					if (thisItem == items.Key)
					{
						Amount.Text = $"{items.Value}";
					}

				}
			}
		}


    }
}
