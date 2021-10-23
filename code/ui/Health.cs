using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace scarebox
{
    public class Health : Panel
    {
        public Label healthLabel;

		public Health()
		{
			healthLabel = Add.Label("100", "health");
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if (player == null)
				return;

			healthLabel.Text = $"{player.Health.CeilToInt()}";
		}
    }
}
