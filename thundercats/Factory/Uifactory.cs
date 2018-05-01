using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Factory
{
    public static class UiFactory
    {

        public static void CreateUiEntity(Vector2 position, Viewport viewport, Texture2D texture = null, string text = null)
        {
            Entity entityKey = EntityFactory.NewEntity();

            UIComponent uiComponent = new UIComponent(
                entityKey,
                new Vector2(viewport.TitleSafeArea.X + position.X, viewport.TitleSafeArea.Y + position.Y),
                AssetManager.Instance.GetContent<SpriteFont>("menu"),
                Color.White,
                texture,
                text);

            ComponentManager.Instance.AddComponentToEntity(entityKey, uiComponent);

        }

    }

}
