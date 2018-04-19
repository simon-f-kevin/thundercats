using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Entity blob1 = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(blob1, AssetManager.Instance.GetContent<Model>("Models/Blob"));
            TransformComponent transformComponent = new TransformComponent(blob1, new Vector3(600, viewport.Height * 0.45f, 100));
            VelocityComponent velocityComponent = new VelocityComponent(blob1);
            PlayerComponent playerComponent = new PlayerComponent(blob1);
            KeyboardComponent keyboardComponent = new KeyboardComponent(blob1);
            GamePadComponent gamePadComponent = new GamePadComponent(blob1, 0);
            CameraComponent cameraComponent = new CameraComponent(blob1);
            GravityComponent gravityComponent = new GravityComponent(blob1);
            cameraComponent.AspectRatio = viewport.AspectRatio;
            cameraComponent.FieldOfView = MathHelper.PiOver2;
            cameraComponent.position = new Vector3(0, 0, -10);
            cameraComponent.target = Vector3.Zero;
            cameraComponent.FollowPlayer = true;

            ComponentManager.Instance.AddComponentToEntity(blob1, cameraComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(blob1, gravityComponent);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemManager.Instance.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);
        }
    }
}
