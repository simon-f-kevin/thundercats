using System;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace thundercats.GameStates.States.MenuStates
{
    class SinglePlayer : IMenu
    {

        private GameManager gameManager;
        private MenuControls controls;
        private Viewport viewport;

        public SinglePlayer(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new MenuControls(gameManager);
            viewport = gameManager.game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            String txtSingleplayer = "Singelplayer";

            //sb.Draw(null, viewport.Bounds, Color.White);
            sb.Begin();
            sb.DrawString(AssetManager.Instance.GetContent<SpriteFont>("menu"), txtSingleplayer, new Vector2(600, viewport.Height * 0.45f), Color.White);
            sb.End();

        }

        public void Update(GameTime gameTime)
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
            }
            controls.GoBackButton();
        }

        }
    }
