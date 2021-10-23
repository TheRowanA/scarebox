using Sandbox;

namespace scarebox
{
	public partial class ScareboxCarriable : BaseCarriable, IUse
	{

		public override void CreateViewModel()
		{
			Host.AssertClient();

			if ( string.IsNullOrEmpty( ViewModelPath ) )
				return;

			ViewModelEntity = new ScareboxViewModel
			{
				Position = Position,
				Owner = Owner,
				EnableViewmodelRendering = true
			};

			ViewModelEntity.SetModel( ViewModelPath );
		}
		public virtual bool IsUsable( Entity user )
		{
			return Owner == null;
		}

		public bool OnUse( Entity user )
		{
			return false;
		}

	}
}
