using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
            AudioManager.Instance.PlaySound("quit");
            System.Threading.Thread.Sleep(3500);
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
