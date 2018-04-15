using Game_Engine.Components;
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
        public void Update(GameTime gameTime)
        {
            var cameras = ComponentManager.Instance.GetComponentDictionary<CameraComponent>();

            foreach (var cameraKeyValuePair in cameras)
            {
                CameraComponent camera = cameraKeyValuePair.Value as CameraComponent;
                camera.ViewMatrix = Matrix.CreateLookAt(camera.Position, camera.Target, Vector3.Up);
                camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView, camera.AspectRatio, 1f, 1000f);
            }

        }
    }
}
