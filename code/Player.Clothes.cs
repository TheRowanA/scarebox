using System.Collections.Generic;
using Sandbox;

namespace scarebox
{
    public partial class ScareboxPlayer
    {
		private readonly List<ModelEntity> clothingList = new();

		public ModelEntity AttachClothing(string model)
		{
			ModelEntity entity = new();
            entity.SetModel(model);
            entity.SetParent(this, true);
            entity.EnableShadowInFirstPerson = true;
            entity.EnableHideInFirstPerson = true;

            clothingList.Add(entity);

            return entity;
		}

		public void RemoveClothing()
        {
            clothingList.ForEach(entity => entity.Delete());
            clothingList.Clear();
        }
	}
}
