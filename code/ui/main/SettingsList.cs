using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

namespace scarebox
{
	[Library]
    public partial class SettingsList : Panel
    {

		VirtualScrollPanel sPanel;

        public SettingsList()
		{
			AddClass("settingspage");
			AddChild(out sPanel, "spanel");

			

		}
    }
}
