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
            ConcurrentDictionary<Entity, Component> playerComponents = componentManager.GetConcurrentDictionary<PlayerComponent>();


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
            VelocityComponent velocityComponent = componentManager.ConcurrentGetComponentOfEntity<VelocityComponent>(playerEntity);
            KeyboardComponent keyboardComponent = componentManager.ConcurrentGetComponentOfEntity<KeyboardComponent>(playerEntity);
            GamePadComponent gamePadComponent = componentManager.ConcurrentGetComponentOfEntity<GamePadComponent>(playerEntity);
            NetworkInputComponent networkInputComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<NetworkInputComponent>(playerEntity);

            /* Keyboard actions */
            if (keyboardComponent != null && velocityComponent != null)
            {
                KeyboardState state = Keyboard.GetState();

                if(state.IsKeyDown(Keys.Up) && !state.IsKeyDown(Keys.Down))
                {
                    PlayerActions.AcceleratePlayerForwards(velocityComponent);
                    networkInputComponent.MoveForward = true;
                }
                if(state.IsKeyDown(Keys.Down) && !state.IsKeyDown(Keys.Up))
                {
                    //This shouldn't be possible?
                    PlayerActions.AcceleratePlayerBackwards(velocityComponent);
                }
                if(state.IsKeyDown(Keys.Left) && !state.IsKeyDown(Keys.Right))
                {
                    PlayerActions.AcceleratePlayerLeftwards(velocityComponent);
                    networkInputComponent.MoveLeft = true;
                }
                if(state.IsKeyDown(Keys.Right) && !state.IsKeyDown(Keys.Left))
                {
                    PlayerActions.AcceleratePlayerRightwards(velocityComponent);
                    networkInputComponent.MoveRight = true;
                }
                if (state.IsKeyDown(Keys.Space))
                {
                    PlayerActions.PlayerJumpSpeed(velocityComponent);
                    networkInputComponent.Jump = true;
                }
            }

            /* Gamepad actions */
            if (gamePadComponent != null && velocityComponent != null)
            {
                GamePadState state = GamePad.GetState(gamePadComponent.Index);

                if(state.IsButtonDown(Buttons.A) && !state.IsButtonDown(Buttons.B))
                {
                    PlayerActions.AcceleratePlayerForwards(velocityComponent);
                    networkInputComponent.MoveForward = true;
                }
                if(state.IsButtonDown(Buttons.B) && !state.IsButtonDown(Buttons.A))
                {
                    PlayerActions.AcceleratePlayerBackwards(velocityComponent);
                }

                if(state.IsButtonDown(Buttons.LeftThumbstickLeft) && !state.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    PlayerActions.AcceleratePlayerLeftwards(velocityComponent);
                    networkInputComponent.MoveLeft = true;
                }
                if(state.IsButtonDown(Buttons.LeftThumbstickRight) && !state.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    PlayerActions.AcceleratePlayerRightwards(velocityComponent);
                    networkInputComponent.MoveRight = true;
                }

                if (state.IsButtonDown(Buttons.Y) && !state.IsButtonDown(Buttons.A))
                {
                    PlayerActions.PlayerJumpSpeed(velocityComponent);
                    networkInputComponent.Jump = true;
                }
            }
        }
    }
}