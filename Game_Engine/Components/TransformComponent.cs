using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Game_Engine.Managers;

namespace Game_Engine.Components
{
    public class TransformComponent : Component
    {
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix RotationMatrix { get; set; }

        public TransformComponent(Entity id) : base(id)
        {
            Position = Vector3.Zero;
        }

        public TransformComponent(Entity id, Vector3 pos) : base(id)
        {
            Position = pos;
        }
    }
}
