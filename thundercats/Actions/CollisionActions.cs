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

        public static void AccelerateColliderForwards(Entity entity, float zDiff)
        {
            
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.Z += 0f;
                velocityComponent.Velocity.Z += (0.1f * (Math.Abs(velocityComponent.Velocity.Z) + 1));
            }
        }

        public static void AccelerateColliderBackwards(Entity entity, float zDiff)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Z -= (0.1f * (Math.Abs(velocityComponent.Velocity.Z) + 1));
                //velocityComponent.Velocity.Z = 0f;
            }
        }

        public static void AccelerateColliderLeftwards(Entity entity, float xDiff)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.X = 0f;
                velocityComponent.Velocity.X += (0.1f * (Math.Abs(velocityComponent.Velocity.X) + 1));
            }
        }

        public static void AccelerateColliderRightwards(Entity entity, float xDiff)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                //velocityComponent.Velocity.X = 0f;
                velocityComponent.Velocity.X -= (0.1f * (Math.Abs(velocityComponent.Velocity.X) + 1));
            }
        }

        public static void HandleCollisionFromAbove(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);
            GravityComponent gravity = ComponentManager.Instance.GetComponentOfEntity<GravityComponent>(entity);

            if(velocityComponent != null)
            {
                if(velocityComponent.Velocity.Y < 0)
                {
                    // if we collide with an acceleration downwards then we want a counter force up.
                    velocityComponent.Velocity.Y += Math.Abs(velocityComponent.Velocity.Y);
                }
                gravity.HasJumped = false;
            } 
            
        }

        public static void HandleCollisionFromBelow(Entity entity)
        {
            VelocityComponent velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);

            if(velocityComponent != null)
            {
                velocityComponent.Velocity.Y -= 0.1f; //Disabled until smoother adjustment is implemented
                //velocityComponent.Velocity.Y = 0f;
            }
        }

        internal static void RunParticleSystem(Entity player)
        {
            GameService.DrawParticles = true;
            
        }
    }
}
