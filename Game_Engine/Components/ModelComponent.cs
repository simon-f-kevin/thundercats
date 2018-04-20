using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class ModelComponent : Component
    {
        public Model Model { get; set; }
        public Matrix[] BoneTransformations { get; set; }
        public Matrix World { get; set; }
        public ModelComponent(Entity id, Model model) : base(id)
        {
            Model = model;
            BoneTransformations = new Matrix[Model.Bones.Count];
            World = this.Model.Bones[0].Transform;
        }
    }
}
