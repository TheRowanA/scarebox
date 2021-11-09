using System.Collections.Generic;
using Sandbox;

namespace scarebox
{
	public partial class ScareboxPlayer
	{
		/*
			All Clientside commands go HERE!
		*/

		// Hides the hud TODO
		[ClientCmd( "cl_hidehud" )]
		public static void HideHud( int i )
		{

			if ( i > 1 || i < 0 ) return;

			if ( i == 1 )
			{
				Log.Info( "Hud Hidden" );
			}
			else
			{
				Log.Info( "Hud Shown" );
			}
		}

		// Sets the max Camera viewroll
		[ClientCmd( "cl_setviewroll" )]
		public static void SetViewRoll( float i )
		{
			var player = Local.Pawn;
			if ( player == null ) return;

			if ( i < 0 || i > 8 ) return;

			ViewRoll = i;
			Log.Info( "ViewRoll Set to: " + i );
		}
	}
}
