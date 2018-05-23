
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using System;
using System.Linq;
using thundercats.Components;
using thundercats.Service;
using thundercats.Systems;

namespace thundercats.Actions
{
    static class PlayerActions
    {

        private static float playerForwardAcceleration = 0.1f;
        private static float playerStrafeAcceleration = 0.4f;
        private static float playerMaxRunningSpeed = 0.6f;
        private static float playerMaxStrafeSpeed = 0.6f;
        private static float _playerJumpSpeed = 0.6f;


        /// <summary>
        /// Accelerates the player forward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerForwards(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Z < playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z += playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player backward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerBackwards(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Z > -playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z -= playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the left until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerLeftwards(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.X < playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X += playerStrafeAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the right until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerRightwards(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.X > -playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X -= playerStrafeAcceleration;
            }
        }

        /// <summary>
        /// Accelerates the player upward in a jumping motion until it reaches the maximum jumping speed
        /// If a playerEntity is passed, we will delete the DrawParticleComponent from the entity-
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void PlayerJump(VelocityComponent velocityComponent, GravityComponent gravity, Entity playerEntity)
        {
            var collisionComponentKeyValuePairs = ComponentManager.Instance.GetConcurrentDictionary<CollisionComponent>();

            if (velocityComponent.Velocity.Y < _playerJumpSpeed)
            {
                GameService.FreeParticleBuffer = true;
                if (playerEntity != null)
                {
                    ComponentManager.Instance.RemoveComponentFromEntity<DrawParticleComponent>(playerEntity);
                }
                velocityComponent.Velocity.Y += _playerJumpSpeed;
            }
        }
    }
}
