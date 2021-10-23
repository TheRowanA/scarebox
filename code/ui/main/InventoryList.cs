using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

namespace scarebox
{
	[Library]
    public partial class InventoryList : Panel
    {
		VirtualScrollPanel Canvas;

		public InventoryList()
		{
			AddChild( out Canvas, "listcanvas" );

			var namePanel = Add.Panel("name");
			namePanel.Add.Label("Pockets");

			Canvas.Layout.AutoColumns = true;
			Canvas.Layout.ItemSize = new Vector2( 100, 100 );
			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var file = (string)data;
				var panel = new ItemIcon(file, cell);
				
				var ItemPopup = new ItemPopup(file, this);
				//panel.AddEventListener( "onclick", () => ConsoleSystem.Run( "spawn", "models/" + file ) );
				//panel.AddEventListener("onclick", () => ItemPopup.SetClass("active", cell.IsVisibleSelf));

				panel.Style.BackgroundImage = Texture.Load( $"/ui/items/scareb_{file}.png", false );
				
			};

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
					if (!Canvas.Data.Contains(items.Key) && items.Value >= 1)
					{
						Canvas.AddItem(items.Key);
					}

					if (Canvas.Data.Contains(items.Key) && items.Value < 1)
					{
						Canvas.Data.Remove(items.Key);
						Canvas.Clear();
						Log.Info(Canvas.Data.Contains(items.Key));
					}

				}
			}
		}
			
    }
}
