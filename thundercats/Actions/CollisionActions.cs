using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using thundercats.Components;
using thundercats.Service;
using thundercats.Systems;

namespace thundercats.Actions
{
    static class CollisionActions
    {

        public static void AccelerateColliderForwards(GameTime gameTime, Entity entity)
        {
            
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z += (Math.Abs(velocityComponent.Velocity.Z) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
            }
        }

        public static void AccelerateColliderBackwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z -= (Math.Abs(velocityComponent.Velocity.Z) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
            }
        }

        public static void AccelerateColliderLeftwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X += (Math.Abs(velocityComponent.Velocity.X) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
            }
        }

        public static void AccelerateColliderRightwards(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.X -= (Math.Abs(velocityComponent.Velocity.X) + 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
            }
        }

        public static void HandleCollisionFromAbove(GameTime gameTime, Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);
            GravityComponent gravity = ComponentManager.Instance.GetComponentOfEntity<GravityComponent>(entity);

            if(velocityComponent != null)
            {
                if(velocityComponent.Velocity.Y < 0)
                {
                    // if we collide with an acceleration downwards then we want a counter force up.
                    velocityComponent.Velocity.Y += (Math.Abs(velocityComponent.Velocity.Y)) * (float)gameTime.ElapsedGameTime.TotalSeconds * 45f;
                }
                gravity.HasJumped = false;
            } 
            
        }

        public static void HandleCollisionFromBelow(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Y -= 0.1f;
            }
        }

        internal static void RunParticleSystem(Entity player)
        {
            //GameService.CreateParticles = true;
            DrawParticleComponent drawParticleComponent = new DrawParticleComponent(player);
            var oldDrawParticleComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<DrawParticleComponent>(player);
            if(oldDrawParticleComponent == null)
            {
                ComponentManager.Instance.AddComponentToEntity(player, drawParticleComponent);
            }
        }
    }
}
