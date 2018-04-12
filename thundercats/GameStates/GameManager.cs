﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thundercats.GameStates.States;
using thundercats.GameStates.States.MenuStates;
using thundercats.GameStates.States.PlayingStates;

namespace thundercats.GameStates
{
    public class GameManager
    {
        // Here we just say that the first state is the Intro
        private GameState _currentGameState = GameState.MainMenu;

        protected internal KeyboardState OldKeyboardState;
        protected internal GamePadState OldGamepadState;

        private Dictionary<GameState, IState> gameStates;

        protected internal SpriteFont menufont;
        protected internal Model blobModel; //TODO: need a better structure for storing models
        protected internal Game game;

        protected internal GameState PreviousGameState { get; set; }
        protected internal GameState CurrentGameState
        {
            get{ return _currentGameState; }
            set{
                PreviousGameState = CurrentGameState;
                _currentGameState = value;
                gameStates[_currentGameState].Initialize();
            }
        }

        // Game states
        public enum GameState
        {   
            MainMenu,
            MultiPlayer,
            SinglePlayer,
            Quit,
            Credits,
            Paused,
            PlayingSinglePlayer,
        };

        public GameManager(Game game, SpriteFont font)
        {
            this.game = game;
            menufont = font;

            gameStates = new Dictionary<GameState, IState>();
            gameStates.Add(GameState.MainMenu, new MainMenu(this));
            gameStates.Add(GameState.SinglePlayer, new SinglePlayer(this));
            gameStates.Add(GameState.MultiPlayer, new MultiplayerMenu(this));
            gameStates.Add(GameState.Paused, new PausedMenu(this));
            gameStates.Add(GameState.Credits, new Credits(this));
            gameStates.Add(GameState.PlayingSinglePlayer, new PlayingSinglePlayerState(this));
        }

        // Draw method consists of a switch case with all
        // the different states that we have, depending on which
        // state we are we use that state's draw method.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            gameStates[CurrentGameState].Draw(gameTime, sb);
        }

        // Same as the draw method, the update method
        // we execute is the one of the current state.
        public void Update(GameTime gameTime)
        {
            gameStates[CurrentGameState].Update(gameTime);

        }

        /*
         * Changes game state and runs the Initialize method for that state.
         */
        public void ChangeGameState(GameState newState)
        {
            PreviousGameState = CurrentGameState;
            CurrentGameState = newState;
            gameStates[CurrentGameState].Initialize();
        }
    }
}
