using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace thundercats.GameStates.States.PlayingStates
{
    public class PlayingSinglePlayerState : IPlaying
    {
        private GameManager gameManager;
        private Viewport viewport;

        public PlayingSinglePlayerState(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = gameManager.game.GraphicsDevice.Viewport;
        }

        public void Initialize()
        {
            CreateBlob();
        }

        public void CreateBlob()
        {
            Entity blob = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(blob, AssetManager.Instance.GetContent<Model>("Models/p1_wedge"));
            TransformComponent transformComponent = new TransformComponent(blob, new Vector3(650, viewport.Height * 0.45f, 150));
            VelocityComponent velocityComponent = new VelocityComponent(blob);
            PlayerComponent playerComponent = new PlayerComponent(blob);
            KeyboardComponent keyboardComponent = new KeyboardComponent(blob);
            GamePadComponent gamePadComponent = new GamePadComponent(blob, 0);
            //TextureComponent textureComponent = new TextureComponent(blob)
            //{
            //    Texture = AssetManager.Instance.GetContent<Texture2D>("farmhouse-texture")//CreateTexture(gameManager.game.GraphicsDevice, 1, 1, pixel => Color.Gold)
            //};
            CameraComponent cameraComponent = new CameraComponent(blob)
            {
                AspectRatio = viewport.AspectRatio,
                FieldOfView = MathHelper.PiOver2,
                position = new Vector3(0, 0, -10),
                target = Vector3.Zero,
                FollowPlayer = true
            };

            ComponentManager.Instance.AddComponentToEntity(blob, cameraComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, gamePadComponent);
            //ComponentManager.Instance.AddComponentToEntity(blob, textureComponent);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemManager.Instance.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);
        }

        private Texture2D CreateTexture(GraphicsDevice device, int width, int height, System.Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
    }
}
