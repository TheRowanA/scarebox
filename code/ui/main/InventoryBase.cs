using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

namespace scarebox
{
	public partial class InventoryBase : Panel
	{

		// REDO THIS CLASS

		public IEnumerable<int> maxInvSlots = Enumerable.Range( 1, 9 );
		readonly VirtualScrollPanel BaseCanvas;

		public InventoryBase()
		{

			StyleSheet.Load( "/styles/InventoryBase.scss" );

			AddClass( "inventorypage" );
			AddChild( out BaseCanvas, "basecanvas" );
			var inventoryItems = AddChild<InventoryList>();

			var namePanel = Add.Panel( "name" );
			namePanel.Add.Label( "Inventory Bar" );

			BaseCanvas.Layout.AutoColumns = true;
			BaseCanvas.Layout.ItemHeight = 100;
			BaseCanvas.Layout.ItemWidth = 100;
			BaseCanvas.OnCreateCell = ( cell, data ) =>
			{
				var panel = cell.Add.Panel( "item" );
			};

			foreach ( int i in maxInvSlots )
			{
				BaseCanvas.AddItem( i );
			}

		}

	}
}
