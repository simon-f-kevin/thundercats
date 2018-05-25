using System.Collections.Concurrent;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    /// <summary>
    /// System to handle all physics updates including 3D transformations based on velocity, friction, and collision.
    /// TransformSystem uses parallel foreach loops to improve performance, this does not require locks as the component manager is thread safe.
    /// If the parameter passed to the constructor is true we compare collisions with all models instead of just players and other models
    /// </summary>
    public class TransformSystem : IUpdateableSystem
    {
        /// <summary>
        /// Updates TransformComponents, ModelComponents, and CollisionComponents with the velocities of any attached VelocityComponent.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            var velocityComponentPairs = ComponentManager.Instance.GetConcurrentDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponentPairs, velocityComponentPair =>
            {
                VelocityComponent velocityComponent = velocityComponentPair.Value as VelocityComponent;
                TransformComponent transformationComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                CollisionComponent collisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CollisionComponent>(velocityComponentPair.Key);

                if (velocityComponent != null)
                    transformationComponent.Position +=
                        velocityComponent.Velocity; // * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;

                Matrix translation = EngineHelper.Instance().WorldMatrix * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(transformationComponent.Position);
                TransformHelper.TransformEntity(velocityComponentPair.Key, translation, true);

                collisionComponent.BoundingVolume.UpdateVolume(transformationComponent.Position);

            });
        }

    }
}
