using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Systems
{
    public class UIRenderSystem : IDrawableSystem
    {
        private SpriteBatch sb;

        public void Initialize(SpriteBatch spritebatch)
        {
            this.sb = spritebatch;
        }

        public void Draw(GameTime gameTime)
        {
            var components = ComponentManager.Instance.GetComponentDictionary<UIComponent>();

            components.ToList().ForEach(c => DrawUI(c.Value as UIComponent));
        }

        private void DrawUI(UIComponent c)
        {
            if (c.Texture != null)  
                sb.DrawString(c.SpriteFont, c.Text, c.Position, c.Color);
            else if (c.Text != null)
                sb.Draw(c.Texture, new Rectangle(c.Position.ToPoint(), c.Texture.Bounds.Size), c.Color);
        }
    }
}
