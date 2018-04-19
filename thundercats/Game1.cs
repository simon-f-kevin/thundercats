using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        MovementSystem movementSystem;
        PlayerInputSystem playerInputSystem;
        CameraSystem cameraSystem;
        PhysicsSystem physicsSystem;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
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
            modelRenderSystem = new ModelRenderSystem();
            modelRenderSystem.graphicsDevice = GraphicsDevice;
            movementSystem = new MovementSystem();
            playerInputSystem = new PlayerInputSystem();
            cameraSystem = new CameraSystem();
            physicsSystem = new PhysicsSystem();
            
            SystemManager.Instance.AddToUpdateables(cameraSystem);
            SystemManager.Instance.AddToDrawables(modelRenderSystem);
            SystemManager.Instance.AddToUpdateables(movementSystem);
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

            AssetManager.Instance.AddContent<Model>(Content,"Models/Blob");
            AssetManager.Instance.AddContent<Model>(Content,"Models/Block");
            AssetManager.Instance.AddContent<Texture2D>(Content, "2DTextures/arrow");
            AssetManager.Instance.AddContent<Texture2D>(Content, "farmhouse-texture");
            AssetManager.Instance.AddContent<SpriteFont>(Content, "menu");
            AssetManager.Instance.AddContent<Model>(Content, "blob_stl");
            AssetManager.Instance.AddContent<Model>(Content, "blob1");
            AssetManager.Instance.AddContent<Model>(Content, "farmhouse_obj");
            AssetManager.Instance.AddContent<Model>(Content, "Models/p1_wedge");
            AssetManager.Instance.AddContent<Model>(Content, "STL");



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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameManager.Draw(gameTime, spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
