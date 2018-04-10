using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thundercats.Menu;

namespace thundercats.Menu.States
{
    public class MultiplayerMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly MenuControls controls;

        public MultiplayerMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            // Adding the options interval and gamemanager.
            controls = new MenuControls(0, 2, gameManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


        }

        public void Update(GameTime gameTime)
        {
            controls.GoBackButton();

        }
    }
}
