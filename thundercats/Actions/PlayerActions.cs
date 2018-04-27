
using Game_Engine.Components;
using Microsoft.Xna.Framework;

namespace thundercats.Actions
{
    static class PlayerActions
    {
        private static Vector3 _playerForwardAcceleration = new Vector3(0,0,0.5f);
        private static Vector3 _playerStrafeAcceleration = new Vector3(0.25f,0,0);
        private static float _playerMaxRunningSpeed = 0.50f;
        private static float _playerMaxStrafeSpeed = 0.25f;
        private static Vector3 _playerJumpSpeed = new Vector3(0,5f,0);
        
        /// <summary>
        /// Accelerates the player forward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerForwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Old.Z < _playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.New += velocityComponent.Velocity.Old + _playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player backward until it reaches maximum running speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerBackwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Old.Z > -_playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.New -= velocityComponent.Velocity.Old +_playerForwardAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the left until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerLeftwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Old.X < _playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.New += velocityComponent.Velocity.Old + _playerStrafeAcceleration;
            }
        }
        /// <summary>
        /// Accelerates the player to the right until it reaches maximum strafeing speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void AcceleratePlayerRightwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Old.X > -_playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.New -= _playerStrafeAcceleration;
            }
        }

        /// <summary>
        /// Accelerates the player upward in a jumping motion until it reaches the maximum jumping speed
        /// </summary>
        /// <param name="velocityComponent"></param>
        public static void PlayerJumpSpeed(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Old.Y < -_playerJumpSpeed.Y)
            {
               velocityComponent.Velocity.New += velocityComponent.Velocity.Old +_playerJumpSpeed; 
            }


           
        }
    }
}
