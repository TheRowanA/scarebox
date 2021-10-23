using System;
using System.Collections.Generic;
using Sandbox;

namespace scarebox
{
    public partial class ScareboxPlayer : Player
    {
		private TimeSince timeSinceDropped;
		private DamageInfo lastDamage;

		public new ScareboxInventory Inventory
		{
			get => (ScareboxInventory) base.Inventory;
			private init => base.Inventory = value;
		}

		public ScareboxPlayer()
		{
			Inventory = new ScareboxInventory(this);
		}
		
        public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			Controller = new WalkController();

			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			Animator = new StandardPlayerAnimator();

			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			Camera = new FirstPersonCamera();

			if ( DevController is NoclipController )
			{
				DevController = null;
			}


			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			base.Respawn();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			Inventory.DropActive();
			Inventory.DeleteContents();

			BecomeRagdollOnClient(Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone(lastDamage.HitboxIndex));

			Camera = new SpectateRagdollCamera();

			EnableAllCollisions = false;
			EnableDrawing = false;
			
		}

		public override void Simulate(Client cl)
		{
			base.Simulate(cl);

			if (Input.ActiveChild != null)
			{
				ActiveChild = Input.ActiveChild;
			}

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );

			if ( Input.Pressed( InputButton.Drop ) )
			{
				var dropped = Inventory.DropActive();
				if ( dropped != null )
				{
					dropped.PhysicsGroup.ApplyImpulse( Velocity + EyeRot.Forward * 500.0f + Vector3.Up * 100.0f, true );
					dropped.PhysicsGroup.ApplyAngularImpulse( Vector3.Random * 100.0f, true );

					timeSinceDropped = 0;
				}
			}

			// Camera view switch
			if (Input.Pressed(InputButton.View))
			{
				if ( Camera is ThirdPersonCamera)
				{
					Camera = new FirstPersonCamera();
				}
				else
				{
					Camera = new ThirdPersonCamera();
				}
			}
		}

		public override void StartTouch( Entity other )
		{
			if ( timeSinceDropped < 1 ) return;

			base.StartTouch( other );
		}

		Rotation lastCameraRot = Rotation.Identity;

		// REDO CAM SETUP
		public override void PostCameraSetup( ref CameraSetup setup )
		{
			base.PostCameraSetup( ref setup );

			if ( lastCameraRot == Rotation.Identity )
				lastCameraRot = setup.Rotation;

			var angleDiff = Rotation.Difference( lastCameraRot, setup.Rotation );
			var angleDiffDegrees = angleDiff.Angle();
			var allowance = 20.0f;

			if ( angleDiffDegrees > allowance )
			{
				// We could have a function that clamps a rotation to within x degrees of another rotation?
				lastCameraRot = Rotation.Lerp( lastCameraRot, setup.Rotation, 1.0f - (allowance / angleDiffDegrees) );
			}
			else
			{
				//lastCameraRot = Rotation.Lerp( lastCameraRot, Camera.Rotation, Time.Delta * 0.2f * angleDiffDegrees );
			}

			// uncomment for lazy cam
			//camera.Rotation = lastCameraRot;

			if ( setup.Viewer != null )
			{
				AddCameraEffects( ref setup );
			}
		}

		float walkBob = 0;
		float lean = 0;
		float fov = 0;

		// TODO REDO ALL CAM EFFECTS
		private void AddCameraEffects( ref CameraSetup setup )
		{
			var speed = Velocity.Length.LerpInverse( 0, 320 );
			var forwardspeed = Velocity.Normal.Dot( setup.Rotation.Forward );

			var left = setup.Rotation.Left;
			var up = setup.Rotation.Up;

			if ( GroundEntity != null )
			{
				walkBob += Time.Delta * 25.0f * speed;
			}

			setup.Position += up * MathF.Sin( walkBob ) * speed * 2;
			setup.Position += left * MathF.Sin( walkBob * 0.6f ) * speed * 1;

			// Camera lean
			lean = lean.LerpTo( Velocity.Dot( setup.Rotation.Right ) * 0.03f, Time.Delta * 15.0f );

			var appliedLean = lean;
			appliedLean += MathF.Sin( walkBob ) * speed * 0.2f;
			setup.Rotation *= Rotation.From( 0, 0, appliedLean );

			speed = (speed - 0.7f).Clamp( 0, 1 ) * 3.0f;

			fov = fov.LerpTo( speed * 20 * MathF.Abs( forwardspeed ), Time.Delta * 2.0f );

			setup.FieldOfView += fov;

		//	var tx = new Sandbox.UI.PanelTransform();
		//	tx.AddRotation( 0, 0, lean * -0.1f );

		//	Hud.CurrentPanel.Style.Transform = tx;
		//	Hud.CurrentPanel.Style.Dirty(); 

		}

    }
}
