using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using thundercats.GameStates;
using thundercats.Systems;

namespace thundercats
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameManager gameManager;
        Viewport viewport;

        ModelRenderSystem modelRenderSystem;
        PhysicsSystem physicsSystem;
        PlayerInputSystem playerInputSystem;
        CameraSystem cameraSystem;
        UIRenderSystem uiSystem;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.PreferMultiSampling = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //thread1 = new ThreadStart();
            //thread2 = Thread.CurrentThread;
           

            modelRenderSystem = new ModelRenderSystem();
            modelRenderSystem.graphicsDevice = GraphicsDevice;
            physicsSystem = new PhysicsSystem();
            playerInputSystem = new PlayerInputSystem();
            cameraSystem = new CameraSystem();
            physicsSystem = new PhysicsSystem();
            uiSystem = new UIRenderSystem();
           
            //SystemManager.Instance.AddToDrawables(uiSystem);
            SystemManager.Instance.AddToUpdateables(cameraSystem);
            SystemManager.Instance.AddToDrawables(modelRenderSystem);
            SystemManager.Instance.AddToUpdateables(physicsSystem);
            SystemManager.Instance.AddToUpdateables(playerInputSystem);
            SystemManager.Instance.AddToUpdateables(physicsSystem);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiSystem.Initialize(spriteBatch, this);
            AssetManager.Instance.AddContent<Model>(Content,"Models/Blob");
            AssetManager.Instance.AddContent<Model>(Content,"Models/Block");
            AssetManager.Instance.AddContent<Texture2D>(Content, "2DTextures/option-marker");
            AssetManager.Instance.AddContent<Texture2D>(Content, "2DTextures/bg-menu");
            AssetManager.Instance.AddContent<Texture2D>(Content, "2DTextures/stars");
            AssetManager.Instance.AddContent<SpriteFont>(Content, "menu");



            gameManager = new GameManager(this);

            viewport = gameManager.game.GraphicsDevice.Viewport;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameManager.Update(gameTime);
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                rasterizerState.FillMode = GraphicsDevice.RasterizerState.FillMode;
                GraphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullClockwiseFace;
                rasterizerState.FillMode = GraphicsDevice.RasterizerState.FillMode;
                GraphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
                rasterizerState.FillMode = GraphicsDevice.RasterizerState.FillMode;
                GraphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                rasterizerState.CullMode = GraphicsDevice.RasterizerState.CullMode;
                GraphicsDevice.RasterizerState = rasterizerState;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.Solid;
                rasterizerState.CullMode = GraphicsDevice.RasterizerState.CullMode;
                GraphicsDevice.RasterizerState = rasterizerState;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            gameManager.Draw(gameTime, spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
