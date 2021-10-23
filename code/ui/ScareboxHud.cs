using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace scarebox
{
	[Library]
	public partial class ScareboxHud : HudEntity<RootPanel>
	{
		public ScareboxHud()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "/styles/hud.scss" );

			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<Health>();
			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<KillFeed>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<InventoryMenu>();
			RootPanel.AddChild<InventoryBar>();
		}
	}
}
