﻿using Game_Engine.Components;
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
            Active = false;
        }

        public bool Active { get; internal set; }

        public void Update(GameTime gameTime)
        {
            //var playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();
            var particleSettingsComponentKeyValuePairs = ComponentManager.Instance.GetConcurrentDictionary<ParticleSettingsComponent>();
            var currentTime = gameTime.ElapsedGameTime.TotalSeconds;
           
            foreach (var particleComponentKeyValuePair in particleSettingsComponentKeyValuePairs)
            {
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponentKeyValuePair.Key);
                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponentKeyValuePair.Key);
                var drawParticleComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<DrawParticleComponent>(particleComponentKeyValuePair.Key);
                var particleSettings = particleComponentKeyValuePair.Value as ParticleSettingsComponent;
                if (drawParticleComponent != null)
                {
                    if (transformComponent == null)
                    {
                        throw new Exception("oops, no transformcomponent found");
                    }
                    if (cameraComponent == null)
                    {
                        //throw new Exception("oops, no cameracomponent found");
                        var otherCamera = ComponentManager.Instance.GetConcurrentDictionary<CameraComponent>().Values.First() as CameraComponent;
                        particleSystem.SetCamera(otherCamera.ViewMatrix, otherCamera.ProjectionMatrix);
                    }
                    else { particleSystem.SetCamera(cameraComponent.ViewMatrix, cameraComponent.ProjectionMatrix); }
                    for (int i = 0; i < particleSettings.MaximumParticles; i++)
                    {
                        particleSystem.AddParticle(transformComponent.Position, Vector3.Zero);
                    }
                    //GameService.DrawParticles = false;
                    //ComponentManager.Instance.RemoveComponentFromEntity<DrawParticleComponent>(particleComponentKeyValuePair.Key);
                }
            }
        }
    }
}
