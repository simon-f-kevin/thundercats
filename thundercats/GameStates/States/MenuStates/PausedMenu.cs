using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace thundercats.GameStates.States.MenuStates
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class PausedMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly MenuControls controls;
        private Viewport viewport;

        public PausedMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new MenuControls(gameManager);
            //viewport = this.gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(null, viewport.Bounds, Color.White);
            spriteBatch.End();
        }
        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            controls.PauseButton();
        }
    }
}
