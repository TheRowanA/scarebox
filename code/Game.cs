using Sandbox;

namespace scarebox
{
	[Library( "scarebox", Title = "Scare&box" )]
	public partial class Scarebox : Game
	{
		public Scarebox()
		{
			if ( IsServer )
			{
				Log.Info( "Scarebox Serverside Loaded!" );
				new ScareboxHud();
			}

			if ( IsClient )
			{
				Log.Info( "Scarebox Clientside Loaded!" );
				Event.Register("scareb.invchildadded");
				Event.Register("scareb.inventorymenuopen");
	        }
		}

		public override void DoPlayerNoclip( Client player )
		{
			//Log.Info(player.UserId);
			
			if (!player.HasPermission("noclip"))
				return;

			if (player.UserId !> 1)
				return;

			base.DoPlayerNoclip( player );
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new ScareboxPlayer();
			cl.Pawn = player;

			player.Respawn();
		}
	}
}
