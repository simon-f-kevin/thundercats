using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;

namespace thundercats.Actions
{
    static class CollisionActions
    {

        public static void AccelerateColliderForwards(GameTime gameTime, Entity entity)
        {
            
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z += 5f * (Math.Abs(velocityComponent.Velocity.Z) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void AccelerateColliderBackwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z -= 5f * (Math.Abs(velocityComponent.Velocity.Z) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void AccelerateColliderLeftwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X += 5f * (Math.Abs(velocityComponent.Velocity.X) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void AccelerateColliderRightwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X -= 5f * (Math.Abs(velocityComponent.Velocity.X) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void AccelerateColliderUpwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.Y += 0.1f; //Disabled until smoother adjustment is implemented
            }
        }

        public static void AccelerateColliderDownwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.Y -= 0.1f; //Disabled until smoother adjustment is implemented
            }
        }
    }
}
