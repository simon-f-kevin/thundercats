using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Actions
{
    static class CollisionActions
    {

        public static void AccelerateColliderForwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z += 0.1f;
            }
        }

        public static void AccelerateColliderBackwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z -= 0.1f;
            }
        }

        public static void AccelerateColliderLeftwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X += 0.1f;
            }
        }

        public static void AccelerateColliderRightwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X -= 0.1f;
            }
        }

        public static void AccelerateColliderUpwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.Y += 0.1f; //Disabled until smoother adjustment is implemented
            }
        }

        public static void AccelerateColliderDownwards(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.Y -= 0.1f; //Disabled until smoother adjustment is implemented
            }
        }
    }
}
