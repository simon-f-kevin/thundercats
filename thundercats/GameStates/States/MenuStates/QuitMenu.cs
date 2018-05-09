using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.GameStates.States.MenuStates
{
    public class QuitMenu : IMenu
    {
        GameManager KillSwitch;
        public QuitMenu(GameManager gameManager)
        {
            KillSwitch = gameManager;
        }

        public void Initialize()
        {
            KillSwitch.game.Exit();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
