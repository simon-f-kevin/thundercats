using Game_Engine.Components;
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
    public class LightSystem : IDrawableSystem
    {
        ComponentManager componentManager = ComponentManager.Instance;
        public void Draw(GameTime gameTime)
        {
            ProjectLight();
        }
        public void ProjectLight()
        {
            var lightComponents = componentManager.GetConcurrentDictionary<LightComponent>();
            
            foreach (var lightComponentPair in lightComponents)
            {
                var light = lightComponentPair.Value as LightComponent;
                var cameraComponent = componentManager.ConcurrentGetComponentOfEntity<CameraComponent>(lightComponentPair.Key);
                var effectComponent = componentManager.ConcurrentGetComponentOfEntity<EffectComponent>(lightComponentPair.Key);
                var transformComponent = componentManager.ConcurrentGetComponentOfEntity<TransformComponent>(lightComponentPair.Key);

                light.LightDirection = transformComponent.Position;

                effectComponent.Effect.Parameters["World"].SetValue(EngineHelper.Instance().WorldMatrix);
                effectComponent.Effect.Parameters["View"].SetValue(cameraComponent.ViewMatrix);
                effectComponent.Effect.Parameters["Projection"].SetValue(cameraComponent.ProjectionMatrix);
                effectComponent.Effect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
                effectComponent.Effect.Parameters["AmbientColor"].SetValue(Color.Purple.ToVector4());
                effectComponent.Effect.Parameters["AmbientIntensity"].SetValue(1f);
                effectComponent.Effect.Parameters["DiffuseColor"].SetValue(Color.DarkGray.ToVector4());
                effectComponent.Effect.Parameters["DiffuseIntensity"].SetValue(5f);
                effectComponent.Effect.Parameters["LightDirection"].SetValue(transformComponent.Position);
                effectComponent.Effect.Parameters["EyePosition"].SetValue(transformComponent.Position);
            }
        }
    }
}
