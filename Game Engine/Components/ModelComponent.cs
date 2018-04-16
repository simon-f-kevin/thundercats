using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class ModelComponent : Component
    {
        public Model Model { get; set; }
        public Matrix[] boneTransformations { get; set; }
        public Matrix world { get; set; }
        public ModelComponent(Entity id, Model model) : base(id)
        {
            Model = model;
            boneTransformations = new Matrix[Model.Bones.Count];
            world = this.Model.Bones[0].Transform;
        }
    }
}
