using System;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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
            AudioManager.Instance.ClearSongs();
            AudioManager.Instance.EnqueueSongs("lounge");
        }


        // This method displays all the options
        // in an ordered fashion.
        private void MainMenuDisplay(SpriteBatch sb)    
        {
            String txtMultiplayer = "Multiplayer";
            String txtSingleplayer = "Singelplayer";
            String txtExit = "Quit";

            SpriteFont font = AssetManager.Instance.GetContent<SpriteFont>("menu");

            Texture2D arrow = AssetManager.Instance.GetContent<Texture2D>("2DTextures/option-marker");

            Vector2 positionSinglePlayer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtSingleplayer).X * 0.5f), viewport.Height * 0.55f);
            Vector2 positionMultiPlayer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtMultiplayer).X * 0.5f), viewport.Height * 0.65f);
            Vector2 positionExit = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtExit).X * 0.5f), viewport.Height * 0.75f);

            sb.Draw(AssetManager.Instance.GetContent<Texture2D>("2DTextures/bg-menu"), viewport.Bounds, Color.White);
            sb.DrawString(font, txtSingleplayer, positionSinglePlayer, Color.White);
            sb.DrawString(font, txtMultiplayer, positionMultiPlayer , Color.White);
            sb.DrawString(font, txtExit, positionExit, Color.White);

            // draws a sprite next to current pos

            switch (currentPosition)
            {
                case OptionsState.Singleplayer:
                    sb.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionSinglePlayer.Y - (font.MeasureString(txtSingleplayer).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.Multiplayer:
                    sb.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionMultiPlayer.Y - (font.MeasureString(txtMultiplayer).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionExit.Y - (font.MeasureString(txtExit).Y * 0.5f)), Color.White);
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
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                AudioManager.Instance.PlaySound("disaster");
            }
            if(!AudioManager.Instance.IsPlaying) AudioManager.Instance.PlayNextInQueue(gameTime);

            currentPosition = (OptionsState) controls.MoveOptionPositionVertically((int) currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Singleplayer:
                    controls.ContinueButton(GameManager.GameState.PlayingSinglePlayer);
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
