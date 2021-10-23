using Sandbox;

namespace scarebox
{
    public partial class ScareboxPlayer
    {
        [ClientRpc]
        public void ClientSetItem(string itemName, int amount)
        {
            Inventory.Items.Set(itemName, amount);
        }

		[ClientRpc]
        public void ClientClearItem()
        {
            Inventory.Items.Clear();
        }
    }
}
