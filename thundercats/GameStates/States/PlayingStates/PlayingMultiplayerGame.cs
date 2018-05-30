using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Managers.Network;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thundercats.Components;
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
        private ParticleSystem particleSystem;
        private ParticleCreationSystem particleCreationSystem;

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
            Entity local;
            Entity remote;
            this.connectionManager = gameManager.NetworkConnectionManager;
            var HostPosition = new Vector3(10, 45, -10);
            var ClientPosition = new Vector3(-100, 45, 10);
            if (connectionManager.IsHost)
            {

                local = GameEntityFactory.NewLocalPlayer("Models/Blob", 0, HostPosition,
                    new Vector3(0, 0, -150), viewport.AspectRatio, true,
                    AssetManager.Instance.CreateTexture(Color.Red, gameManager.game.GraphicsDevice));
                GameEntityFactory.NewParticleSettingsEntity(local, 100, 1, "smoke");
                remote = GameEntityFactory.NewBasePlayer("Models/Blob", 1, ClientPosition, AssetManager.Instance.CreateTexture(Color.Blue, gameManager.game.GraphicsDevice), GameEntityFactory.REMOTE_PLAYER);
            }
            else
            {
                local = GameEntityFactory.NewLocalPlayer("Models/Blob", 0, ClientPosition,
                    new Vector3(0, 0, -150), viewport.AspectRatio, true,
                    AssetManager.Instance.CreateTexture(Color.Blue, gameManager.game.GraphicsDevice));
                GameEntityFactory.NewParticleSettingsEntity(local, 100, 1, "smoke");
                remote = GameEntityFactory.NewBasePlayer("Models/Blob", 1, HostPosition, AssetManager.Instance.CreateTexture(Color.Red, gameManager.game.GraphicsDevice), GameEntityFactory.REMOTE_PLAYER);
            }
            GameEntityFactory.NewOutOfBounds(new Vector3(-10000, -1000, -10000), new Vector3(10000, -50, 10000));
            NetworkHandlingSystem networkSystem = new NetworkHandlingSystem(connectionManager.GetPeer());
            networkSystem.InitPlayers();
            SystemManager.Instance.AddToUpdateables(networkSystem);

            //particleSystem = new ParticleSystem(gameManager.game.GraphicsDevice);
            //particleSystem.InitializeParticleSystem(ComponentManager.Instance.ConcurrentGetComponentOfEntity<ParticleSettingsComponent>(local));
            //particleCreationSystem = new ParticleCreationSystem(particleSystem);
            //SystemManager.Instance.AddToDrawables(particleSystem);
            //SystemManager.Instance.AddToUpdateables(particleSystem, particleCreationSystem);

            AudioManager.Instance.ClearSongs();
            AudioManager.Instance.EnqueueSongs("playMusic1", "playMusic2");
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
            if (!AudioManager.Instance.IsPlaying)
            {
                AudioManager.Instance.PlayNextInQueue(gameTime);
            }
            SystemManager.Instance.Update(gameTime);
        }

        /// <summary>
        /// Initiates the gameworld by generating a world matrix.
        /// Creates blocks and places them on the positions in the world matrix. 
        /// </summary>
        private void InitWorld()
        {
            worldGenerator = new WorldGenerator("Somebody once told me the world is gonna roll me", WorldGenerator.GetWorldgenEntityDefs(), gameManager, viewport);
            worldGenerator.GenerateWorld(3, 100);
            worldGenerator.MoveBlocks();
        }
    }
}
