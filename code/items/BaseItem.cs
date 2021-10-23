using Sandbox;
using System;

namespace scarebox
{
	[Library( "scareb_items" )]
	public abstract partial class BaseItem : Prop
	{
		public virtual string ItemName { get; set; }

		public virtual int ItemAmount { get; set; }

		public virtual int MaxCanHave { get; set; }

		[Net]
		private int CurrentItemAmount { get; set; }
		private int ItemEntMax { get; set; }


		protected Output OnPickup { get; set; }

		public virtual string ModelDir => "models/citizen_props/coin01.vmdl";

		public override void Spawn()
		{
			base.Spawn();

			SetModel( ModelDir );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			//CollisionGroup = CollisionGroup.Prop;
			//SetInteractsAs(CollisionLayer.PhysicsProp);

			CurrentItemAmount = ItemAmount;
			ItemEntMax = MaxCanHave;
		}

		public override void Touch( Entity ent )
		{
			base.Touch( ent );

			if ( !IsServer || ent is not ScareboxPlayer player ) return;

			string itemsName = ItemName.ToLower();
			ScareboxInventory inventory = player.Inventory;

			int playerAmount = inventory.Items.Count( itemsName );

			if ( !(MaxCanHave >= (playerAmount + Math.Ceiling( CurrentItemAmount * 0.25 ))) )
			{
				return;
			}

			int amountGiven = Math.Min( CurrentItemAmount, MaxCanHave - playerAmount );
			inventory.Items.Give( itemsName, amountGiven );
			CurrentItemAmount -= amountGiven;
			OnPickup.Fire( ent );

			if ( CurrentItemAmount <= 0 || Math.Ceiling( ItemEntMax * 0.25 ) > CurrentItemAmount )
			{
				Delete();
				Log.Info( "Given Item: '" + itemsName + "' Amount: " + amountGiven );
				Log.Info( "Current Amount: " + CurrentItemAmount );
			}
		}

		public override void TakeDamage( DamageInfo info )
		{
			PhysicsBody body = info.Body;
			if ( !body.IsValid() )
			{
				body = PhysicsBody;
			}

			if ( body.IsValid() && !info.Flags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				body.ApplyImpulseAt( info.Position, info.Force * 100 );
			}
			return;
		}
	}
}
