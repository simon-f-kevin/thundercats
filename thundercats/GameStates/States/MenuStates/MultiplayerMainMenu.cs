using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.GameStates.States.MenuStates
{
    public class MultiplayerMainMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly MenuControls controls;
        private OptionsState currentPosition = OptionsState.StartServer;
        private Viewport viewport;

        // different menu options depending on what menu you are in
        private enum OptionsState
        {
            StartServer,
            StartClient,
            Back
        }

        public MultiplayerMainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            // Adding the options interval and gamemanager.
            viewport = gameManager.game.GraphicsDevice.Viewport;
            controls = new MenuControls(0, 2, gameManager);
        }

        public void Initialize()
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            MultiplayerMenuDisplay(spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState)controls.MoveOptionPositionVertically((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.StartServer:
                    controls.ContinueButton(GameManager.GameState.MultiplayerStartServer);
                    break;
                case OptionsState.StartClient:
                    controls.ContinueButton(GameManager.GameState.MultiplayerConnectServer);
                    break;
                case OptionsState.Back:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
            }
            //controls.GoBackButton();

        }

        private void MultiplayerMenuDisplay(SpriteBatch spriteBatch)
        {
            string txtStartServer = "Start a new server";
            string txtConnectToServer = "Connect to a server";
            string txtBack = "Back";

            SpriteFont font = AssetManager.Instance.GetContent<SpriteFont>("menu");

            Texture2D arrow = AssetManager.Instance.GetContent<Texture2D>("2DTextures/option-marker");

            Vector2 positionStartServer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtStartServer).X * 0.5f), viewport.Height * 0.55f);
            Vector2 positionConnectServer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtConnectToServer).X * 0.5f), viewport.Height * 0.65f);
            Vector2 positionExit = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtBack).X * 0.5f), viewport.Height * 0.75f);

            spriteBatch.Draw(AssetManager.Instance.GetContent<Texture2D>("2DTextures/bg-menu"), viewport.Bounds, Color.White);
            spriteBatch.DrawString(font, txtStartServer, positionStartServer, Color.White);
            spriteBatch.DrawString(font, txtConnectToServer, positionConnectServer, Color.White);
            spriteBatch.DrawString(font, txtBack, positionExit, Color.White);

            switch (currentPosition)
            {
                case OptionsState.StartServer:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionStartServer.Y - (font.MeasureString(txtStartServer).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.StartClient:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionConnectServer.Y - (font.MeasureString(txtConnectToServer).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.Back:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionExit.Y - (font.MeasureString(txtBack).Y * 0.5f)), Color.White);
                    break;
            }
        }
    }
}
