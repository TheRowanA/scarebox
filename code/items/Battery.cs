using Hammer;
using Sandbox;

namespace scarebox
{

	[Library( "scareb_item_battery", Title = "Battery", Spawnable = true )]
	[EditorModel( "models/citizen_props/coin01.vmdl" )]
	partial class Battery : BaseItem
	{
		public override string ItemName => "battery";

		public override int ItemAmount => 1;

		public override int MaxCanHave => 3;

		public override string ModelDir => "models/citizen_props/coin01.vmdl";
	}
}
