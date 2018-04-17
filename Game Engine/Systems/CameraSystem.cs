using Game_Engine.Components;
using Game_Engine.Entities;
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
            var cameras = ComponentManager.Instance.GetComponentDictionary<CameraComponent>();

            foreach (var cameraKeyValuePair in cameras)
            {
                cameraComponent = cameraKeyValuePair.Value as CameraComponent;
                if (cameraComponent == null) continue;
                cameraComponent.ViewMatrix =
                    Matrix.CreateLookAt(cameraComponent.position, cameraComponent.target, Vector3.Up);
                cameraComponent.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(cameraComponent.FieldOfView,
                    cameraComponent.AspectRatio, 1f, 1000f);
                if (cameraComponent.FollowPlayer) FollowPlayer(cameraKeyValuePair.Key);
            }

        }

        /// <summary>
        /// Camera will follow player from a set distance
        /// </summary>
        private void FollowPlayer(Entity cameraEntity)
        {
            ModelComponent modelComponent = ComponentManager.Instance.GetComponentOfEntity<ModelComponent>(cameraEntity);

            cameraComponent.position = modelComponent.Model.Bones[0].Transform.Translation + (modelComponent.Model.Bones[0].Transform.Backward * 30f) + (modelComponent.Model.Bones[0].Transform.Up * 20f);
            cameraComponent.target = modelComponent.Model.Bones[0].Transform.Translation + (modelComponent.Model.Bones[0].Transform.Forward * 20f);
            //Console.WriteLine(cameraComponent.position.ToString()); //For debugging

            cameraComponent.ViewMatrix = Matrix.CreateLookAt(cameraComponent.position, cameraComponent.target, Vector3.Up);

        }
    }
}
