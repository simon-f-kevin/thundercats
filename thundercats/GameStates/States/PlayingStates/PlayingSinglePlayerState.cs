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
        private WorldGenerator worldGenerator;

        public PlayingSinglePlayerState(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {
            uiFactory = new UiFactory(viewport);
            GameEntityFactory.NewPlayerWithCamera("Models/Blob", 0, new Vector3(600, viewport.Height * 0.45f, 100),
                new Vector3(0, 0, -50),viewport.AspectRatio, true,
                AssetManager.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));
            // Creating Static ui stuff.
            //uiFactory.CreateEntity(new Vector2(20, 20), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //uiFactory.CreateEntity(new Vector2(150, 20), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //uiFactory.CreateEntity(new Vector2(20, 150), AssetManager.Instance.GetContent<Texture2D>("2DTextures/arrow"));
            //CreateBlob();

            InitWorld();

        }
       
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemManager.Instance.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Initiates the gameworld by generating a world matrix.
        /// Creates blocks and places them on the positions in the world matrix. 
        /// </summary>
        private void InitWorld()
        {
            worldGenerator = new WorldGenerator("nick");
            var world = GenerateWorld(2, 2);
            int distanceBetweenBlocksX = 100;
            int distanceBetweenBlocksZ = 80;
            int iter = 0;
            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int z = 0; z < world.GetLength(1); z++)
                {
                    GameEntityFactory.NewBlock(new Vector2((x * distanceBetweenBlocksX), (z * distanceBetweenBlocksZ)), AssetManager.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice));
                    iter++; //for debugging
                }
            }
            worldGenerator.MoveBlocks();
        }

        private int[,] GenerateWorld(int nLanes, int nRows)
        {
            var world = worldGenerator.GenerateWorld(nLanes, nRows);
            return world;
        }

        private void TestCollisionEntity()
        {
            //Below is a temporary object you can use to test collision. (rendering both this and the player seems to result in weird scaling issues but that is a separate issue)
            Entity player = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>("Models/Blob"));
            TransformComponent transformComponent = new TransformComponent(player, new Vector3(600, viewport.Height * 0.45f, 100));
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);
        }

    }
}
