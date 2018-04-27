
using Game_Engine.Components;

namespace thundercats.Actions
{
    static class PlayerActions
    {
        private static float _playerForwardAcceleration = 0.5f;
        private static float _playerStrafeAcceleration = 0.25f;
        private static float _playerMaxRunningSpeed = 0.50f;
        private static float _playerMaxStrafeSpeed = 0.25f;
        private static float _playerJumpSpeed = 5f;
        
        /// <summary>
        /// Accelerates the player forward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerForwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z < _playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z += _playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player backward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerBackwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z > -_playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z -= _playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the left until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerLeftwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X < _playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X += _playerStrafeAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the right until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerRightwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X > -_playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X -= _playerStrafeAcceleration;
            }
        }

        /// <summary>
        /// Accelerates the player upward in a jumping motion until it reaches the maximum jumping speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void PlayerJumpSpeed(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Y < -_playerJumpSpeed)
            {
               velocityComponent.Velocity.Y += _playerJumpSpeed; 
            }


           
        }
    }
}
