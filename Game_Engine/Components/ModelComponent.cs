using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game_Engine.Managers; 
namespace Game_Engine.Components
{
    public class ModelComponent : Component
    {
        public Model Model { get; set; }
        public Matrix[] BoneTransformations { get; set; }
        public State<Matrix> World { get; set; }
        public ModelComponent(Entity id, Model model) : base(id)
        {
            Model = model;
            BoneTransformations = new Matrix[Model.Bones.Count];
            World = new State<Matrix>(this.Model.Bones[0].Transform);
        }
    }
}
