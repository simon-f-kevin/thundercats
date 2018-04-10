using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    class MovementSystem : IUpdateableSystem
    {
        public void Update(GameTime gameTime)
        {
            var velocityComponents = ComponentManager.Instance.GetComponentDictionary<VelocityComponent>();
            foreach(var velocityComponent in velocityComponents)
            {
                var velocity = velocityComponent.Value as VelocityComponent;
                var transformation = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(velocityComponent.Key);

                transformation.Position += velocity.Velocity;
            }

        }
    }
}
