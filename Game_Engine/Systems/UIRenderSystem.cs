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
        private Game game;

        public void Initialize(SpriteBatch spritebatch, Game game)
        {
            this.sb = spritebatch;
            this.game = game;
        }

        public void Draw(GameTime gameTime)
        {
            var components = ComponentManager.Instance.GetConcurrentDictionary<UIComponent>();
            sb.Begin();
            components.Values.ToList().ForEach(c => DrawUI(c as UIComponent));
            sb.End();
            //Resets the graphics device to normal 3D-mode after spritebatch changes it.
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DrawUI(UIComponent c)
        {
            if (c.Texture != null)
                sb.Draw(c.Texture, new Rectangle(c.Position.ToPoint(), c.Texture.Bounds.Size), c.Color);
            else if (c.Text != null)
                sb.DrawString(c.SpriteFont, c.Text, c.Position, c.Color);

        }
    }
}
