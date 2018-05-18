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
        public ParticleCreationSystem()
        {
            particleSystem = new ParticleSystem(GameEntityFactory.GraphicsDevice);
        }

        public void Initialize()
        {
            var updatableSystems = SystemManager.Instance.UpdateableSystems;
            foreach (var system in updatableSystems)
            {
                if (system.GetType() == particleSystem.GetType())
                {
                    particleSystem = system as ParticleSystem;
                    break;
                }    
            }
        }

        public void Update(GameTime gameTime)
        {
            var playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();

            foreach(var playerComponentKeyValuePair in playerComponents)
            {
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(playerComponentKeyValuePair.Key);
                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(playerComponentKeyValuePair.Key);
                var particleSettings = ComponentManager.Instance.ConcurrentGetComponentOfEntity<ParticleSettingsComponent>(playerComponentKeyValuePair.Key);
                for (int i = 0; i < particleSettings.MaximumParticles; i++)
                {
                    particleSystem.SetCamera(cameraComponent.ViewMatrix, cameraComponent.ProjectionMatrix);
                    particleSystem.AddParticle(transformComponent.Position, Vector3.Zero);
                }
            }
            
        }
    }
}
