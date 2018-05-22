using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Systems
{
    public class CameraSystem : IUpdateableSystem
    {
        CameraComponent cameraComponent;

        public void Update(GameTime gameTime)
        {
            var cameraComponentPairs = ComponentManager.Instance.GetConcurrentDictionary<CameraComponent>();

            foreach (var cameraComponentPair in cameraComponentPairs)
            {
                cameraComponent = cameraComponentPair.Value as CameraComponent;
                if (cameraComponent == null){
                    continue;
                }
                cameraComponent.ViewMatrix = Matrix.CreateLookAt(cameraComponent.Position, cameraComponent.Target, Vector3.Up);
                cameraComponent.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                cameraComponent.FieldOfView, cameraComponent.AspectRatio, 1f, 700f);
                cameraComponent.BoundingFrustum = new BoundingFrustum(cameraComponent.ViewMatrix * cameraComponent.ProjectionMatrix);
                //Console.WriteLine(cameraComponent.BoundingFrustum.Matrix);
                if (cameraComponent.FollowPlayer){
                    FollowPlayer(cameraComponentPair.Key);
                }

            }

        }

        /// <summary>
        /// Camera will follow player from a set distance
        /// </summary>
        private void FollowPlayer(Entity entity)
        {
            ModelComponent modelComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<ModelComponent>(entity);
            TransformComponent transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(entity);
            //cameraComponent.Position = new Vector3(transformComponent.Position.X, transformComponent.Position.Y, transformComponent.Position.Z) + new Vector3(0, 10, -50);
            //cameraComponent.Target = new Vector3(transformComponent.Position.X, transformComponent.Position.Y, transformComponent.Position.Z);

            cameraComponent.Position = modelComponent.World.Translation + (modelComponent.World.Forward * 20f) + (modelComponent.World.Up * 20f);
            cameraComponent.Target = modelComponent.World.Translation + (modelComponent.World.Backward * 20f);
            Console.WriteLine("Position: " + cameraComponent.Position.ToString()); //For debugging
            Console.WriteLine("Target: " + cameraComponent.Target.ToString()); //For debugging

            cameraComponent.ViewMatrix = Matrix.CreateLookAt(cameraComponent.Position, cameraComponent.Target, Vector3.Up);

        }
    }
}