
using Game_Engine.Components;
using Microsoft.Xna.Framework;
using Game_Engine.Managers;
using System;

namespace thundercats.Actions
{
    static class PlayerActions
    {
        private static float playerForwardAcceleration = 2f;
        private static float playerStrafeAcceleration = 2f;
        private static float playerMaxRunningSpeed = 2f;
        private static float playerMaxStrafeSpeed = 2f;
        private static float playerJumpSpeed = 15f;

        /// <summary>
        /// Accelerates the player forward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerForwards(GameTime gameTime, VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Z < playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z += playerForwardAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        /// <summary>
        /// Accelerates the player backward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerBackwards(GameTime gameTime, VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Z > -playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z -= playerForwardAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        /// <summary>
        /// Accelerates the player to the left until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerLeftwards(GameTime gameTime, VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.X < playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X += playerStrafeAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        /// <summary>
        /// Accelerates the player to the right until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerRightwards(GameTime gameTime, VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.X > -playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X -= playerStrafeAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Accelerates the player upward in a jumping motion until it reaches the maximum jumping speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void PlayerJump(GameTime gameTime, VelocityComponent velocityComponent)
        {
           velocityComponent.Velocity.Y += playerJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; 
        }
    }
}
