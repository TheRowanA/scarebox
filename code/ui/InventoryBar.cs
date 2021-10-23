using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;

namespace scarebox
{
    public class InventoryBar : Panel
    {
		public static TimeSince TimeSinceInvShow { get; set; }

		readonly List<InventoryIcon> slots = new();

		public InventoryBar()
		{
			for (int i = 0; i < 9; i++)
			{
				var icon = new InventoryIcon(i + 1, this);
				slots.Add(icon);
			}
		}

		public override void Tick()
		{
			base.Tick();

			Parent.SetClass("inventorybarshow", InventoryBarShow());
			//Log.Info("Inv show is " + InventoryBarShow());

			var player = Local.Pawn;
			if (player == null) return;
			if (player.Inventory == null) return;

			for (int i = 0; i < slots.Count; i++)
			{
				UpdateIcon(player.Inventory.GetSlot(i), slots[i], i);
			}
		}

		private static bool InventoryBarShow()
		{
			var player = Local.Pawn;
			if (player.Inventory.Count() == 0) return false;

			if (TimeSinceInvShow < 2.5f)
			{
				return true;
			}

			return false;

		}

		[Event("scareb.invchildadded")]
		public static void OnInvChildAdded()
		{
			TimeSinceInvShow = 0;
		}

		private static void UpdateIcon(Entity entity, InventoryIcon inventoryIcon, int i)
		{
			if (entity == null)
			{
				inventoryIcon.Clear();
				return;
			}

			inventoryIcon.TargetEnt = entity;
			inventoryIcon.Label.Text = entity.ClassInfo.Title;

			inventoryIcon.SetClass("active", entity.IsActiveChild());
		}

		[Event( "buildinput" )]
		public static void ProcessClientInput( InputBuilder input )
		{
			 if ( Local.Pawn is not ScareboxPlayer player )
				return;

			var inventory = player.Inventory;
			if ( inventory == null )
				return;

			if ( input.Pressed( InputButton.Slot1 ) ) SetActiveSlot( input, inventory, 0 );
			if ( input.Pressed( InputButton.Slot2 ) ) SetActiveSlot( input, inventory, 1 );
			if ( input.Pressed( InputButton.Slot3 ) ) SetActiveSlot( input, inventory, 2 );
			if ( input.Pressed( InputButton.Slot4 ) ) SetActiveSlot( input, inventory, 3 );
			if ( input.Pressed( InputButton.Slot5 ) ) SetActiveSlot( input, inventory, 4 );
			if ( input.Pressed( InputButton.Slot6 ) ) SetActiveSlot( input, inventory, 5 );
			if ( input.Pressed( InputButton.Slot7 ) ) SetActiveSlot( input, inventory, 6 );
			if ( input.Pressed( InputButton.Slot8 ) ) SetActiveSlot( input, inventory, 7 );
			if ( input.Pressed( InputButton.Slot9 ) ) SetActiveSlot( input, inventory, 8 );

			if ( input.MouseWheel != 0 ) SwitchActiveSlot( input, inventory, -input.MouseWheel );
		}

		private static void SetActiveSlot( InputBuilder input, IBaseInventory inventory, int v )
		{
			var player = Local.Pawn;
			if ( player == null )
				return;

			var ent = inventory.GetSlot( v );
			if ( player.ActiveChild == ent )
				return;

			if ( ent == null )
				return;

			input.ActiveChild = ent;
		}

		private static void SwitchActiveSlot( InputBuilder input, IBaseInventory inventory, int v )
		{
			var count = inventory.Count();
			if ( count == 0 ) return;

			var slot = inventory.GetActiveSlot();
			var nextSlot = slot + v;

			while ( nextSlot < 0 ) nextSlot += count;
			while ( nextSlot >= count ) nextSlot -= count;

			TimeSinceInvShow = 0;

			SetActiveSlot( input, inventory, nextSlot );
		}


	}
}
