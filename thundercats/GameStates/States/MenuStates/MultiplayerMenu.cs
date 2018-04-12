using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.GameStates.States.MenuStates
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

        public void Initialize()
        {

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
