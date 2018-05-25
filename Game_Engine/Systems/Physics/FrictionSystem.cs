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
    public class FrictionSystem : IUpdateableSystem
    {
        public void Update(GameTime gameTime)
        {
            var velocityComponents = ComponentManager.Instance.GetConcurrentDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponents, velocityComponent =>
            {
                var velocity = velocityComponent.Value as VelocityComponent;
                var frictionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<FrictionComponent>(velocityComponent.Key);

                if (frictionComponent != null)
                {
                    if (velocity != null && velocity.Velocity != Vector3.Zero)
                    {
                        velocity.Velocity.X *= frictionComponent.Friction;
                        velocity.Velocity.Z *= frictionComponent.Friction;
                    }

                }
            });

        }
    }
}
