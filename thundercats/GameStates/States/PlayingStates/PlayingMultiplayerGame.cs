﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Game_Engine.Managers.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thundercats.Factory;
using thundercats.Systems;

namespace thundercats.GameStates.States.PlayingStates
{
    public class PlayingMultiplayerGame : IPlaying
    {
        private GameManager gameManager;
        private Viewport viewport;
        private WorldGenerator worldGenerator;
        private NetworkConnectionManager connectionManager;

        public PlayingMultiplayerGame(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = this.gameManager.game.GraphicsDevice.Viewport;
        }

        /// <summary>
        /// The host is always the red player and always on the left side
        /// </summary>
        public void Initialize()
        {
            this.connectionManager = gameManager.NetworkConnectionManager;
            var HostPosition = new Vector3(0, viewport.Height * 0.45f, 0);
            var ClientPosition = new Vector3(100, viewport.Height * 0.45f, 0);
            if (connectionManager.IsHost)
            {

                GameEntityFactory.NewLocalHostPlayer("Models/Blob", 0, HostPosition,
                    new Vector3(0, 0, -150), viewport.AspectRatio, true,
                    AssetManager.Instance.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));

                GameEntityFactory.NewRemotePlayer("Models/Blob", 1, ClientPosition, AssetManager.Instance.CreateTexture(Color.Blue, gameManager.game.GraphicsDevice));
            }
            else
            {
                GameEntityFactory.NewLocalClientPlayer("Models/Blob", 0, ClientPosition,
                    new Vector3(0, 0, -150), viewport.AspectRatio, true,
                    AssetManager.Instance.CreateTexture(Color.Blue, gameManager.game.GraphicsDevice));

                GameEntityFactory.NewRemoteHostPlayer("Models/Blob", 1, HostPosition, AssetManager.Instance.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));
            }

            NetworkHandlingSystem networkSystem = new NetworkHandlingSystem(connectionManager.GetPeer());
            networkSystem.InitPlayers();
            SystemManager.Instance.AddToUpdateables(networkSystem);

            //NetworkInputSystem networkInputSystem = new NetworkInputSystem();
            //SystemManager.Instance.AddToUpdateables(networkInputSystem);
            InitWorld();
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
            SystemManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Initiates the gameworld by generating a world matrix.
        /// Creates blocks and places them on the positions in the world matrix. 
        /// </summary>
        private void InitWorld()
        {
            worldGenerator = new WorldGenerator("Somebody once told me the world is gonna roll me");
            var world = worldGenerator.GenerateWorld(2, 5);
            int distanceBetweenBlocksX = 100;
            int distanceBetweenBlocksZ = 50;
            int iter = 0;
            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int z = 0; z < world.GetLength(1); z++)
                {
                    if (world[x, z] == 1)
                    {
                        GameEntityFactory.NewBlock(new Vector3((x * distanceBetweenBlocksX), (viewport.Height * 0.45f), (z * distanceBetweenBlocksZ)),
     AssetManager.Instance.CreateTexture(Color.BlueViolet, gameManager.game.GraphicsDevice), "block");
                    }

                    iter++; //for debugging
                }
            }
            worldGenerator.MoveBlocks();
        }
    }
}
