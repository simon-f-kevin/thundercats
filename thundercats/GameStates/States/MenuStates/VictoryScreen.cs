using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thundercats.GameStates.States.MenuStates
{
    public class VictoryScreen : IMenu
    {
        private GameManager gameManager;
        private MenuControls controls;
        private OptionsState currentPosition = OptionsState.Exit;
        private Viewport viewport;

        // different menu options
        private enum OptionsState
        {
            Exit
        }

        public VictoryScreen(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
            controls = new MenuControls(0, 2, gameManager);

        }


        // This method displays all the options
        // in an ordered fashion.
        private void VictoryScreenDisplay(SpriteBatch sb)
        {
            String txtExit = "Quit";
            String txtMessage1 = "You have endured the gauntlet, and you are free.";
            String txtMessage2 = "The Supreme Leader smiles upon you.";

            SpriteFont font = AssetManager.Instance.GetContent<SpriteFont>("menu");

            Texture2D arrow = AssetManager.Instance.GetContent<Texture2D>("2DTextures/option-marker");

            Vector2 positionExit = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtExit).X * 0.5f), viewport.Height * 0.75f);
            Vector2 positionMessage = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtMessage1).X * 0.5f), viewport.Height * 0.55f);
            Vector2 positionMessage2 = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtMessage2).X * 0.5f), viewport.Height * 0.60f);


            sb.Draw(AssetManager.Instance.GetContent<Texture2D>("2DTextures/bg-menu"), viewport.Bounds, Color.White);
            sb.DrawString(font, txtExit, positionExit, Color.White);
            sb.DrawString(font, txtMessage1, positionMessage, Color.White);
            sb.DrawString(font, txtMessage2, positionMessage2, Color.White);

            // draws a sprite next to current pos

            switch (currentPosition)
            {
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
            VictoryScreenDisplay(spriteBatch);
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
            if (!AudioManager.Instance.IsPlaying) AudioManager.Instance.PlayNextInQueue(gameTime);

            currentPosition = (OptionsState)controls.MoveOptionPositionVertically((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.Quit);
                    break;
            }
        }
    }
}