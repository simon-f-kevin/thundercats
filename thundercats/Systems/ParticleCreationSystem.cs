using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Components;
using thundercats.Service;

namespace thundercats.Systems
{
    public class ParticleCreationSystem : IUpdateableSystem
    {
        private ParticleSystem particleSystem;
        public ParticleCreationSystem(ParticleSystem particleSystem)
        {
            this.particleSystem = particleSystem;
        }

        public void Update(GameTime gameTime)
        {
            //var playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();
            var particleSettingsComponentKeyValuePairs = ComponentManager.Instance.GetConcurrentDictionary<ParticleSettingsComponent>();

            foreach(var particleComponentKeyValuePair in particleSettingsComponentKeyValuePairs)
            {
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponentKeyValuePair.Key);
                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponentKeyValuePair.Key);
                if (transformComponent == null)
                {
                    throw new Exception("oops, no transformcomponent found");
                }
                if (cameraComponent == null)
                {
                    throw new Exception("oops, no cameracomponent found");
                }
                var particleSettings = particleComponentKeyValuePair.Value as ParticleSettingsComponent;
                for (int i = 0; i < particleSettings.MaximumParticles; i++)
                {
                    particleSystem.SetCamera(cameraComponent.ViewMatrix, cameraComponent.ProjectionMatrix);
                    particleSystem.AddParticle(transformComponent.Position, Vector3.Zero);
                }
            }
            
        }
    }
}
