using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class ModelComponent : Component
    {
        public Model Model { get; set; }

        public ModelComponent(Model model) : base()
        {
            Model = model;
        }
    }
}
