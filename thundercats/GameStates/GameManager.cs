using System.Collections.Generic;
using System.Threading.Tasks;
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

      
        public GameManager(Game game)
        {
            this.game = game;

            gameStates = new Dictionary<GameState, IState>();
            gameStates.Add(GameState.MainMenu, new MainMenu(this));
            gameStates.Add(GameState.SinglePlayer, new SinglePlayer(this));
            gameStates.Add(GameState.MultiPlayer, new MultiplayerMenu(this));
            gameStates.Add(GameState.Paused, new PausedMenu(this));
            gameStates.Add(GameState.Credits, new Credits(this));
            gameStates.Add(GameState.PlayingSinglePlayer, new PlayingSinglePlayerState(this));
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            gameStates[CurrentGameState].Draw(gameTime, sb);
        }

        public void Update(GameTime gameTime)
        {
            gameStates[CurrentGameState].Update(gameTime);
        }

        public void PararellExecution(GameTime gameTime, SpriteBatch sb)
        {

            Parallel.Invoke(
                () => Draw(gameTime, sb),
                () => Update(gameTime));
        }
    }
}
