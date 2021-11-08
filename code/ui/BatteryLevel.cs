using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace scarebox
{
	public class BatteryLevel : Panel
	{
		public Label batteryLabel;

		public BatteryLevel()
		{
			Add.Label("Battery", "battery-text");
			batteryLabel = Add.Label( "0", "batterylevel" );
		}

		public override void Tick()
		{
			base.Tick();

			Parent.SetClass("batterylevelshow", BatteryLevelShow());

			var player = Local.Pawn;
			if ( player == null ) return;

			var weapon = player.ActiveChild as Flashlight;
			SetClass("active", weapon != null);

			if (weapon == null) return;
			

			var roundedLightLife = Math.Round(weapon.TimeSinceLightLife);

			if (roundedLightLife > 100) return;

			batteryLabel.Text = $"{100 - roundedLightLife}";
		}

		private static bool BatteryLevelShow()
		{
			var player = Local.Pawn;
			if ( player == null ) return false;

			var inventory = player.Inventory;
			if ( inventory == null ) return false;

			if (inventory.Active == null) return false;

			if (inventory.Active.ClassInfo.Name != "scareb_flashlight") return false;

			return true;
		}
	}
}
