﻿using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thundercats.Factory;
using System;
using System.Linq;
using Game_Engine.Systems;
using thundercats.Service;
using thundercats.Systems;
using thundercats.Components;

namespace thundercats.GameStates.States.PlayingStates
{
    public class PlayingLocalGame : IPlaying
    {
        private GameManager gameManager;
        private Viewport viewport;
        public WorldGenerator worldGenerator;
        public int[,] world;
        public Entity[,] worldEntity;

        private ParticleSystem particleSystem;
        private ParticleCreationSystem particleCreationSystem;

        public PlayingLocalGame(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {   


            var playerEntity = GameEntityFactory.NewLocalPlayer("Models/Blob", 0, new Vector3(10, 40, 0),
                new Vector3(0, 500, -100), viewport.AspectRatio, true,
                AssetManager.Instance.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));
            //GameEntityFactory.NewParticleSettingsEntity(playerEntity, 100, 2, "fire");
            GameEntityFactory.NewParticleSettingsEntity(playerEntity, 100, 1, "smoke");

            GameEntityFactory.NewAiPlayer("Models/Blob", new Vector3(-80, 40, 1),
                AssetManager.Instance.CreateTexture(Color.Honeydew, gameManager.game.GraphicsDevice));
            GameEntityFactory.NewOutOfBounds(new Vector3(-10000, -1000, -10000), new Vector3(10000, -50, 10000));
            InitWorld();
            GameService.Instance.GameWorld = world;
           

            particleSystem = new ParticleSystem(gameManager.game.GraphicsDevice);
            particleSystem.InitializeParticleSystem(ComponentManager.Instance.ConcurrentGetComponentOfEntity<ParticleSettingsComponent>(playerEntity));
            particleCreationSystem = new ParticleCreationSystem(particleSystem);
            SystemManager.Instance.AddToDrawables(particleSystem);
            SystemManager.Instance.AddToUpdateables(particleSystem, particleCreationSystem);

            AudioManager.Instance.ClearSongs();
            AudioManager.Instance.EnqueueSongs("playMusic1", "playMusic2");
        }
       
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(AssetManager.Instance.GetContent<Texture2D>("2DTextures/stars"), viewport.TitleSafeArea, Color.White);
            spriteBatch.End();
            SystemManager.Instance.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (!AudioManager.Instance.IsPlaying) AudioManager.Instance.PlayNextInQueue(gameTime);
            SystemManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Initiates the gameworld by generating a world matrix.
        /// Creates blocks and places them on the positions in the world matrix. 
        /// </summary>
        private void InitWorld()
        {

            worldGenerator = new WorldGenerator("nick"); // the NICE WORKING SEED DO NOT CHANGE IF U DONT WANT TO FIX ALL WITH AI

            world = GenerateWorld(3, 100);
            worldEntity = new Entity[world.GetLength(0), world.GetLength(1)];
            int distanceBetweenBlocksX = -100;
            int distanceBetweenBlocksZ = 50;
            int iter = 0;
            for (int column = 0; column < world.GetLength(0); column++)
            {
                for (int row = 0; row < world.GetLength(1); row++)
                {
                    if (world[column, row] == 2)
                    {
                        Entity Block = GameEntityFactory.NewGoalBlock(new Vector3((column * distanceBetweenBlocksX), (0), (row * distanceBetweenBlocksZ)),
                        AssetManager.Instance.CreateTexture(Color.Gold, gameManager.game.GraphicsDevice));

                        worldEntity[column, row] = Block;
                    }
                    else if (world[column, row] == 1)
                    {
                        
                        Entity Block = GameEntityFactory.NewBlock(new Vector3((column * distanceBetweenBlocksX), (0), (row * distanceBetweenBlocksZ)),
                        AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice), GameEntityFactory.BLOCK);

                        worldEntity[column, row] = Block;
                        
                    }
                    iter++; //for debugging
                }
            }


            GameService.Instance.EntityGameWorld = worldEntity;
            worldGenerator.MoveBlocks();
        }

        private int[,] GenerateWorld(int nLanes, int nRows)
        {
            var world = worldGenerator.GenerateWorld(nLanes, nRows);
            return world;
        }
    }
}
