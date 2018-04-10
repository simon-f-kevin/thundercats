using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thundercats.Menu;

namespace thundercats.Menu.States
{
    class Credits : IMenu
    {
        private readonly GameManager gameManager;
        private readonly MenuControls controls;
        private Viewport viewport;
        
        public Credits(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new MenuControls(gameManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(null, viewport.Bounds, Color.White);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            controls.GoBackButton();
        }
    }
}
