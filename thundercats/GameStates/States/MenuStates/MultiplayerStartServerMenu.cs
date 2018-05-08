using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Game_Engine.Managers.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.GameStates.States.MenuStates
{
    public class MultiplayerStartServerMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly MenuControls controls;
        private OptionsState currentPosition = OptionsState.StartServer;
        private Viewport viewport;
        private NetworkConnectionManager networkConnectionManager;
        private string ServerName;

        // different menu options depending on what menu you are in
        private enum OptionsState
        {
            StartServer,
            ServerName,
            StartMatch,
            Exit
        }

        public MultiplayerStartServerMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
            controls = new MenuControls(0, 3, gameManager);
            ServerName = "Server Offline";
        }

        public void Initialize()
        {
            var network = EntityFactory.NewEntity();
            NetworkConnectionComponent networkConnectionComponent = new NetworkConnectionComponent(network);
            networkConnectionComponent.Hostname = NetworkHelper.GetCurrentHostname();
            networkConnectionComponent.Port = NetworkHelper.GetPort();
            ComponentManager.Instance.AddComponentToEntity(network, networkConnectionComponent);
            networkConnectionManager = new NetworkConnectionManager(NetworkHelper.ConnectionType.Host);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            StartServerDisplay(spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            CheckServerName();
            currentPosition = (OptionsState)controls.MoveOptionPositionVertically((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.StartServer:
                    controls.StartServerButton(networkConnectionManager);
                    break;
                case OptionsState.ServerName:

                case OptionsState.StartMatch:
                    controls.ContinueButton(GameManager.GameState.MultiplayerPlaying);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.Quit);
                    break;
            }
        }

        private void CheckServerName()
        {
            if (networkConnectionManager.ServerName != "")
            {
                ServerName = networkConnectionManager.ServerName;
            }
        }

        private void StartServerDisplay(SpriteBatch spriteBatch)
        {
            string txtStartServer = "Initialize a new server";
            string txtServerName = ServerName;
            string txtStartGame = "Play Multiplayer";
            string txtExit = "Quit";

            SpriteFont font = AssetManager.Instance.GetContent<SpriteFont>("menu");

            Texture2D arrow = AssetManager.Instance.GetContent<Texture2D>("2DTextures/option-marker");

            Vector2 positionStartServer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtStartServer).X * 0.5f), viewport.Height * 0.55f);
            Vector2 positionConnectServer = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtServerName).X * 0.5f), viewport.Height * 0.65f);
            Vector2 positionStartGame = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtStartGame).X * 0.5f), viewport.Height * 0.75f);
            Vector2 positionExit = new Vector2(viewport.TitleSafeArea.Center.X - (font.MeasureString(txtExit).X * 0.5f), viewport.Height * 0.85f);

            spriteBatch.Draw(AssetManager.Instance.GetContent<Texture2D>("2DTextures/bg-menu"), viewport.Bounds, Color.White);
            spriteBatch.DrawString(font, txtStartServer, positionStartServer, Color.White);
            spriteBatch.DrawString(font, txtServerName, positionConnectServer, Color.White);
            spriteBatch.DrawString(font, txtStartGame, positionStartGame, Color.White);
            spriteBatch.DrawString(font, txtExit, positionExit, Color.White);

            switch (currentPosition)
            {
                case OptionsState.StartServer:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionStartServer.Y - (font.MeasureString(txtStartServer).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.ServerName:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionConnectServer.Y - (font.MeasureString(txtServerName).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.StartMatch:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionStartGame.Y - (font.MeasureString(txtServerName).Y * 0.5f)), Color.White);
                    break;
                case OptionsState.Exit:
                    spriteBatch.Draw(arrow, new Vector2(viewport.TitleSafeArea.Center.X - (arrow.Width * 0.5f), positionExit.Y - (font.MeasureString(txtExit).Y * 0.5f)), Color.White);
                    break;
            }
        }
    }
}
