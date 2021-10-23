using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace scarebox
{
    public partial class InventoryMenu : Panel
    {
        public static InventoryMenu Instance;

		public InventoryMenu()
		{
			Instance = this;

			StyleSheet.Load("/styles/InventoryMenu.scss");

			var main = Add.Panel("main");
			{
				var tabs = main.AddChild<ButtonGroup>();
				tabs.AddClass("tabs");

				var body = Add.Panel("body");
				{
					tabs.Add.Label("Menu");
					var inventorycontents = body.AddChild<InventoryBase>();
					tabs.SelectedButton = tabs.AddButtonActive("Inventory", (b) => inventorycontents.SetClass("active", b));

					var settings = body.AddChild<SettingsList>();
					tabs.AddButtonActive("Settings", (b) => settings.SetClass("active", b));
					
				}
			}

		}

		public override void Tick()
		{
			base.Tick();

			Parent.SetClass( "inventorymenuopen", Input.Down( InputButton.Menu ) );
			Event.Run("scareb.inventorymenuopen");
		}

    }
}
