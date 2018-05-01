using Game_Engine.Entities;
using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{
    public class BoundingSphereComponent : Component
    {
        public BoundingSphere BoundingSphere {get; set;}

        public BoundingSphereComponent(Entity id, BoundingSphere boundingSphere) : base(id)
        {
            BoundingSphere = boundingSphere;
        }
    }
}
