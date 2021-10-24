using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

namespace scarebox
{
    public class BatteryLevel : Panel
    {
		public Label batteryLabel;

        public BatteryLevel()
		{
			batteryLabel = Add.Label("100", "batterylevel");
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if (player == null) return;

			Entity ent = Entity.FindByName("Flashlight");
			if (!player.Inventory.Contains(ent)) return;

			batteryLabel.Text = $"{ent.Name}";
		}
    }
}
