using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thundercats.Factory;
using System;
using System.Linq;

namespace thundercats.GameStates.States.PlayingStates
{
    public class PlayingSinglePlayerState : IPlaying
    {
        private GameManager gameManager;
        private Viewport viewport;
        private UiFactory uiFactory;

        public PlayingSinglePlayerState(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {
            uiFactory = new UiFactory(viewport);
            GameEntityFactory.NewPlayerWithCamera("Models/Blob", 0, new Vector3(600, viewport.Height * 0.45f, 100),
                new Vector3(0, 0, -10),viewport.AspectRatio, false,
                AssetManager.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));
            GameEntityFactory.NewPlayer("Models/Blob", 0, new Vector3(600, viewport.Height * 0.45f, 100), AssetManager.CreateTexture(Color.Gold, gameManager.game.GraphicsDevice));
            // Creating Static ui stuff.
            //uiFactory.CreateEntity(new Vector2(20, 20), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //uiFactory.CreateEntity(new Vector2(150, 20), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //uiFactory.CreateEntity(new Vector2(20, 150), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //CreateBlob();

            /* Below is a temporary object you can use to test collision. (rendering both this and the player seems to result in weird scaling issues but that is a separate issue)
            Entity player = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>("Models/Blob"));
            TransformComponent transformComponent = new TransformComponent(player, new Vector3(600, viewport.Height * 0.45f, 100));
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);*/

            GenerateMap();
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemManager.Instance.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);
        }

        private void GenerateMap()
        {
            //throw new NotImplementedException();
        }
    }
}
