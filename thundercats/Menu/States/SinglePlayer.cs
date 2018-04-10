using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using thundercats.Menu;

namespace thundercats.Menu.States
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
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            String txtSingleplayer = "Singelplayer";

            //sb.Draw(null, viewport.Bounds, Color.White);
            sb.Begin();
            sb.DrawString(gameManager.menufont, txtSingleplayer, new Vector2(600, viewport.Height * 0.45f), Color.White);
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
