using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace scarebox
{
	public partial class ScareboxInventory : BaseInventory
	{
		public readonly ItemInventory Items;
		public ScareboxInventory( ScareboxPlayer player ) : base( player )
		{
			Items = new(player);
		}

		public override void OnChildAdded( Entity child )
		{
			Event.Run("scareb.invchildadded");
			base.OnChildAdded( child );
		}

		public override bool CanAdd( Entity entity )
		{
			if ( !entity.IsValid() )
				return false;

			if ( !base.CanAdd( entity ) )
				return false;

			return !IsCarryingType( entity.GetType() );
		}

		public override bool Add( Entity entity, bool makeActive = false )
		{
			var weapon = entity as BaseScareboxWeapon;
			
			if ( !entity.IsValid() )
				return false;

			if ( IsCarryingType( entity.GetType() ) )
				return false;

			if (weapon != null)
			{
				Sound.FromWorld("scareb.pickup_weapon", entity.Position);
			}
			return base.Add( entity, makeActive );
		}

		public bool IsCarryingType( Type t )
		{
			return List.Any( x => x?.GetType() == t );
		}

		public override bool Drop( Entity ent )
		{
			if ( !Host.IsServer )
				return false;

			if ( !Contains( ent ) )
				return false;

			ent.OnCarryDrop( Owner );

			return ent.Parent == null;
		}

	}
}
