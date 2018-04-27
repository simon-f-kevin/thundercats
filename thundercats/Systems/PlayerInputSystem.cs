using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Concurrent;
using System.Collections.Generic;
using thundercats.Actions;

namespace thundercats.Systems
{

    public class PlayerInputSystem : IUpdateableSystem
    {
        private ComponentManager componentManager = ComponentManager.Instance;

        /*
         * Parses keyboard actions for all players.
         * NOTE: While it's technically possible to have multiple players at the moment, they will always share the same keyboard states.
         */
        public void Update(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> playerComponents = componentManager.GetComponentDictionary<PlayerComponent>();

            foreach(Entity playerEntity in playerComponents.Keys)
            {
                ParsePlayerInput(gameTime, playerEntity);
            }
        }

        /*
         * Gets player keyboard input and takes appropriate action.
         */
        public void ParsePlayerInput(GameTime gameTime, Entity playerEntity)
        {
            VelocityComponent velocityComponent = componentManager.GetComponentOfEntity<VelocityComponent>(playerEntity);
            KeyboardComponent keyboardComponent = componentManager.GetComponentOfEntity<KeyboardComponent>(playerEntity);
            GamePadComponent gamePadComponent = componentManager.GetComponentOfEntity<GamePadComponent>(playerEntity);
            

            /* Keyboard actions */
            if(keyboardComponent != null && velocityComponent != null)
            {
                KeyboardState state = Keyboard.GetState();

                if(state.IsKeyDown(Keys.Up) && !state.IsKeyDown(Keys.Down))
                {
                    PlayerActions.AcceleratePlayerForwards(velocityComponent);
                }
                if(state.IsKeyDown(Keys.Down) && !state.IsKeyDown(Keys.Up))
                {
                    PlayerActions.AcceleratePlayerBackwards(velocityComponent);
                }

                if(state.IsKeyDown(Keys.Left) && !state.IsKeyDown(Keys.Right))
                {
                    PlayerActions.AcceleratePlayerLeftwards(velocityComponent);
                }
                if(state.IsKeyDown(Keys.Right) && !state.IsKeyDown(Keys.Left))
                {
                    PlayerActions.AcceleratePlayerRightwards(velocityComponent);
                }

                if (state.IsKeyDown(Keys.Space))
                {
                    PlayerActions.PlayerJumpSpeed(velocityComponent);
                }

            }

            /* Gamepad actions */
            if (gamePadComponent != null && velocityComponent != null)
            {
                GamePadState state = GamePad.GetState(gamePadComponent.Index);

                if(state.IsButtonDown(Buttons.A) && !state.IsButtonDown(Buttons.B))
                {
                    PlayerActions.AcceleratePlayerForwards(velocityComponent);
                }
                if(state.IsButtonDown(Buttons.B) && !state.IsButtonDown(Buttons.A))
                {
                    PlayerActions.AcceleratePlayerBackwards(velocityComponent);
                }

                if(state.IsButtonDown(Buttons.LeftThumbstickLeft) && !state.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    PlayerActions.AcceleratePlayerLeftwards(velocityComponent);
                }
                if(state.IsButtonDown(Buttons.LeftThumbstickRight) && !state.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    PlayerActions.AcceleratePlayerRightwards(velocityComponent);
                }

                if (state.IsButtonDown(Buttons.Y) && !state.IsButtonDown(Buttons.A))
                {
                    PlayerActions.PlayerJumpSpeed(velocityComponent);
                }
            }
        }
    }
}