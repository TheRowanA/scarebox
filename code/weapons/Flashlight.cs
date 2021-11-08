using Sandbox;

namespace scarebox
{
	[Library( "scareb_flashlight", Title = "Flashlight" )]
	[Hammer.EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
	partial class Flashlight : BaseScareboxWeapon
	{
		public override string ViewModelPath => "weapons/rust_flashlight/v_rust_flashlight.vmdl";
		public override float SecondaryRate => 2.0f;

		protected virtual Vector3 LightOffset => Vector3.Forward * 10;

		private SpotLightEntity worldLight;
		private SpotLightEntity viewLight;

		[Net, Local, Predicted]
		private bool LightEnabled { get; set; } = true;

		TimeSince timeSinceLightToggled;
		public TimeSince TimeSinceLightLife { get; set; }
		float preTimeSinceLightLife;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );

			worldLight = CreateLight();
			worldLight.SetParent( this, "slide", new Transform( LightOffset ) );
			worldLight.EnableHideInFirstPerson = true;
			worldLight.Enabled = false;
		}

		public override void CreateViewModel()
		{
			base.CreateViewModel();

			viewLight = CreateLight();
			viewLight.SetParent( ViewModelEntity, "light", new Transform( LightOffset ) );
			viewLight.EnableViewmodelRendering = true;
			viewLight.Enabled = LightEnabled;
		}

		private SpotLightEntity CreateLight()
		{
			var light = new SpotLightEntity
			{
				Enabled = true,
				DynamicShadows = true,
				Range = 512,
				Falloff = 1.0f,
				LinearAttenuation = 0.0f,
				QuadraticAttenuation = 1.0f,
				Brightness = 2,
				Color = Color.White,
				InnerConeAngle = 20,
				OuterConeAngle = 40,
				FogStength = 1.0f,
				Owner = Owner,
				LightCookie = Texture.Load( "materials/effects/lightcookie.vtex" )
			};

			return light;
		}

		public override void Simulate( Client cl )
		{
			if ( cl == null )
				return;

			base.Simulate( cl );

			bool toggle = Input.Pressed( InputButton.Flashlight ) || Input.Pressed( InputButton.Attack1 );

			if ( timeSinceLightToggled > 0.1f && toggle )
			{
				LightEnabled = !LightEnabled;

				PlaySound( LightEnabled ? "flashlight-on" : "flashlight-off" );

				if ( worldLight.IsValid() )
				{
					worldLight.Enabled = LightEnabled;
				}

				if ( viewLight.IsValid() )
				{
					viewLight.Enabled = LightEnabled;
				}

				timeSinceLightToggled = 0;

				preTimeSinceLightLife = TimeSinceLightLife;
			}


			if ( !LightEnabled && preTimeSinceLightLife < 100.0f)
			{
				TimeSinceLightLife = preTimeSinceLightLife;
				//Log.Info("FIRE");
			}


			if ( TimeSinceLightLife > 100.0f && LightEnabled )
			{
				LightEnabled = false;
				Log.Info( "Flashlight flat battery" );

				PlaySound( "flashlight-off" );

				if ( worldLight.IsValid() )
				{
					worldLight.Enabled = LightEnabled;
				}

				if ( viewLight.IsValid() )
				{
					viewLight.Enabled = LightEnabled;
				}

				preTimeSinceLightLife = TimeSinceLightLife;
			}

			//Log.Info( "Light Life = " + timeSinceLightLife );
		}

		public override bool CanReload()
		{
			if ( !Owner.IsValid() || !Input.Pressed( InputButton.Reload ) ) return false;

			if ( TimeSinceLightLife < 100.0f ) return false;

			var inventory = (ScareboxInventory)Owner.Inventory;
			if ( inventory.Items.Count( "battery" ) < 1 ) return false;

			//Log.Info( "Can Reload" );
			return true;
		}

		public override void Reload()
		{
			var inventory = (ScareboxInventory)Owner.Inventory;

			Log.Info( "Reloading Taken 1 (battery) from Items" );
			inventory.Items.Take( "battery", 1 );
			TimeSinceLightLife = 0;

		}

		public override void AttackSecondary()
		{
			if ( MeleeAttack() )
			{
				OnMeleeHit();
			}
			else
			{
				OnMeleeMiss();
			}

			PlaySound( "rust_flashlight.attack" );
		}

		private bool MeleeAttack()
		{
			var forward = Owner.EyeRot.Forward;
			forward = forward.Normal;

			bool hit = false;

			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * 80, 20.0f ) )
			{
				if ( !tr.Entity.IsValid() ) continue;

				tr.Surface.DoBulletImpact( tr );

				hit = true;

				if ( !IsServer ) continue;

				using ( Prediction.Off() )
				{
					var damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100, 25 )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}

			return hit;
		}

		[ClientRpc]
		private void OnMeleeMiss()
		{
			Host.AssertClient();

			if ( IsLocalPawn )
			{
				_ = new Sandbox.ScreenShake.Perlin();
			}

			ViewModelEntity?.SetAnimBool( "attack", true );
		}

		[ClientRpc]
		private void OnMeleeHit()
		{
			Host.AssertClient();

			if ( IsLocalPawn )
			{
				_ = new Sandbox.ScreenShake.Perlin( 1.0f, 1.0f, 3.0f );
			}

			ViewModelEntity?.SetAnimBool( "attack_hit", true );
		}

		private void Activate()
		{
			if ( worldLight.IsValid() )
			{
				worldLight.Enabled = LightEnabled;
			}
		}

		private void Deactivate()
		{
			if ( worldLight.IsValid() )
			{
				worldLight.Enabled = false;
			}
		}

		public override void ActiveStart( Entity ent )
		{
			base.ActiveStart( ent );

			if ( IsServer )
			{
				Activate();
			}
		}

		public override void ActiveEnd( Entity ent, bool dropped )
		{
			base.ActiveEnd( ent, dropped );

			if ( IsServer )
			{
				if ( dropped )
				{
					Activate();
				}
				else
				{
					Deactivate();
				}
			}
		}
	}
}
