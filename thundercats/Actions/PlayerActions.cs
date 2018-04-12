
using Game_Engine.Components;

namespace thundercats.Actions
{
    static class PlayerActions
    {
        private static float _playerForwardAcceleration = 0.5f;
        private static float _playerStrafeAcceleration = 0.25f;
        private static float _playerMaxRunningSpeed = 50f;
        private static float _playerMaxStrafeSpeed = 25f;

        public static void AcceleratePlayerForwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z < _playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z += _playerForwardAcceleration;
            }
        }

        public static void AcceleratePlayerBackwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.Z > -_playerMaxRunningSpeed)
            {
                velocityComponent.Velocity.Z -= _playerForwardAcceleration;
            }
        }

        public static void AcceleratePlayerLeftwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X < _playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X += _playerStrafeAcceleration;
            }
        }

        public static void AcceleratePlayerRightwards(VelocityComponent velocityComponent)
        {
            if(velocityComponent.Velocity.X > -_playerMaxStrafeSpeed)
            {
                velocityComponent.Velocity.X -= _playerStrafeAcceleration;
            }
        }
    }
}
