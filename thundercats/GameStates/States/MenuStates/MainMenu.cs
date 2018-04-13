using System;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace thundercats.GameStates.States.MenuStates
{
    /// <summary>
    /// The main game menu state. This state is used as the first
    /// state that the users come to when starting the game
    /// (exl. intro state). It presents options that the players
    /// can choose from.
    /// </summary>
    class MainMenu : IMenu
    {
        private GameManager gameManager;
        private MenuControls controls;
        private OptionsState currentPosition = OptionsState.Singleplayer;
        private Viewport viewport;

        // different menu options
        private enum OptionsState
        {
            Singleplayer,
            Multiplayer,
            Exit
        }

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
            controls = new MenuControls(0, 2, gameManager);
        }


        // This method displays all the options
        // in an ordered fashion.
        private void MainMenuDisplay(SpriteBatch sb)    
        {
            String txtMultiplayer = "Multiplayer";
            String txtSingleplayer = "Singelplayer";
            String txtExit = "Quit";

            //sb.Draw(null, viewport.Bounds, Color.White);
            sb.DrawString(AssetManager.Instance().GetContent<SpriteFont>("menu"), txtSingleplayer, new Vector2(600, viewport.Height * 0.45f), Color.White);
            sb.DrawString(AssetManager.Instance().GetContent<SpriteFont>("menu"), txtMultiplayer, new Vector2(600, viewport.Height * 0.55f), Color.White);
            sb.DrawString(AssetManager.Instance().GetContent<SpriteFont>("menu"), txtExit, new Vector2(600, viewport.Height * 0.65f), Color.White);

            // draws a sprite next to current pos

            switch (currentPosition)
            {
                case OptionsState.Singleplayer:
                    sb.Draw(AssetManager.Instance().GetContent<Texture2D>("2DTextures/arrow"), new Vector2(250, viewport.Height * 0.40f), Color.White);
                    break;
                case OptionsState.Multiplayer:
                    sb.Draw(AssetManager.Instance().GetContent<Texture2D>("2DTextures/arrow"), new Vector2(250, viewport.Height * 0.50f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(AssetManager.Instance().GetContent<Texture2D>("2DTextures/arrow"), new Vector2(250, viewport.Height * 0.60f), Color.White);
                    break;
            }
        }

        public void Initialize()
        {

        }

        // Draws
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();         
            MainMenuDisplay(spriteBatch);
            spriteBatch.End();
        }

        // Updates. When the players clicks continue we go to
        // the next state that is specified in the switch case,
        // depending on which option we currently are at.
        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState) controls.MoveOptionPositionVertically((int) currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Singleplayer:
                    controls.ContinueButton(GameManager.GameState.SinglePlayer);
                    break;
                case OptionsState.Multiplayer:
                    controls.ContinueButton(GameManager.GameState.MultiPlayer);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.Quit);
                    break;
            }
        }
    }
}
