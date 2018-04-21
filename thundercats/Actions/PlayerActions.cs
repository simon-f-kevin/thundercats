
using Game_Engine.Components;

namespace thundercats.Actions
{
    static class PlayerActions
    {
        private static float playerForwardAcceleration = 0.25f;
        private static float playerStrafeAcceleration = 0.1f;
        private static float playerMaxRunningSpeed = 0.25f;
        private static float playerMaxStrafeSpeed = 0.25f;
        private static float _playerJumpSpeed = 0.5f;
        

        public static void AcceleratePlayerForwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z < playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z += playerForwardAcceleration;
            }
        }

        public static void AcceleratePlayerBackwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z > -playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z -= playerForwardAcceleration;
            }
        }

        public static void AcceleratePlayerLeftwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X < playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X += playerStrafeAcceleration;
            }
        }

        public static void AcceleratePlayerRightwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X > -playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X -= playerStrafeAcceleration;
            }
        }
        public static void PlayerJumpSpeed(VelocityComponent velocityComponent)
        {
            if (velocityComponent.Velocity.Y > -_playerJumpSpeed)
            {
               velocityComponent.Velocity.Y += _playerJumpSpeed; 
            }


           
        }
    }
}
