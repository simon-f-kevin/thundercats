using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game_Engine.Systems
{
    /// <summary>
    /// System to handle all physics updates including 3D transformations based on velocity, friction, and collision.
    /// PhysicsSystem uses parallel foreach loops to improve performance, this does not require locks as the component manager is thread safe.
    /// If the parameter passed to the constructor is true we compare collisions with all models instead of just players and other models
    /// </summary>
    public class PhysicsSystem : IUpdateableSystem
    {
        ComponentManager componentManager = ComponentManager.Instance;
        private bool compareAllModels;

        public PhysicsSystem(bool compareAllModels = false)
        {
            this.compareAllModels = compareAllModels;
        }

        public void Update(GameTime gameTime)
        {
            UpdatePositionsOfModels(gameTime);
            CheckCollision();
            RunGravity(gameTime);
        }


        /// <summary>
        /// Updates TransformComponents, ModelComponents, and CollisionComponents with the velocities of any attached VelocityComponent.
        /// </summary>
        private void UpdatePositionsOfModels(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> velocityComponentPairs = componentManager.GetConcurrentDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponentPairs, velocityComponentPair =>
            {
                VelocityComponent velocityComponent = velocityComponentPair.Value as VelocityComponent;
                TransformComponent transformationComponent = componentManager.ConcurrentGetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                CollisionComponent collisionComponent = componentManager.ConcurrentGetComponentOfEntity<CollisionComponent>(velocityComponentPair.Key);
                

                transformationComponent.Position += velocityComponent.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;
                Matrix translation = EngineHelper.Instance().WorldMatrix * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(transformationComponent.Position);
                TransformHelper.TransformEntity(velocityComponentPair.Key, translation, true);

                collisionComponent.UpdateShape(transformationComponent.Position);


                UpdateFriction(velocityComponentPair.Key);
            });
        }

        /// <summary>
        /// Checks intersections for all CollisionComponents if compareAllModels is set to true
        /// Otherwise we only compare players to all other models and reducing number of comparisons. 
        /// </summary>
        private void CheckCollision() 
        {
            ConcurrentDictionary<Entity, Component> collisionComponentPairs = componentManager.GetConcurrentDictionary<CollisionComponent>();
            var playerComponents = componentManager.GetConcurrentDictionary<PlayerComponent>();
            if (compareAllModels)
            {
                Parallel.ForEach(collisionComponentPairs, sourceCollisionPair =>
                {
                    Entity sourceEntity = sourceCollisionPair.Key;
                    var sourceCollisionComponent = sourceCollisionPair.Value as CollisionComponent;

                    foreach (var targetCollisionComponentPair in collisionComponentPairs)
                    {
                        Entity targetEntity = targetCollisionComponentPair.Key;
                        CollisionComponent targetCollisionComponent = targetCollisionComponentPair.Value as CollisionComponent;
                        if (sourceCollisionComponent.ComponentId != targetCollisionComponent.ComponentId &&
                        sourceCollisionComponent.BoundingShape.Intersects(targetCollisionComponent.BoundingShape))
                        {
                            CollisionManager.Instance.AddCollisionPair(sourceEntity, targetEntity);
                        }
                    }
                });
            }
            else
            {
                Parallel.ForEach(playerComponents, playerComponentPair =>
                {
                    Entity sourceEntity = playerComponentPair.Key;
                    var sourceCollisionComponent = componentManager.ConcurrentGetComponentOfEntity<CollisionComponent>(playerComponentPair.Key);

                    foreach (var targetCollisionComponentPair in collisionComponentPairs)
                    {
                        Entity targetEntity = targetCollisionComponentPair.Key;
                        CollisionComponent targetCollisionComponent = targetCollisionComponentPair.Value as CollisionComponent;
                        if (sourceCollisionComponent.ComponentId != targetCollisionComponent.ComponentId &&
                        sourceCollisionComponent.BoundingShape.Intersects(targetCollisionComponent.BoundingShape))
                        {
                            CollisionManager.Instance.AddCollisionPair(sourceEntity, targetEntity);
                        }
                    }
                });
            }
        }


        /// <summary>
        /// Applies gravity to all entities with a gravity-component and velocity-component. 
        /// If they have a velocity-component, but no gravity-component no gravity is applied. 
        /// </summary>
        private void RunGravity(GameTime gameTime)
        {
            var gravityComponents = ComponentManager.Instance.GetConcurrentDictionary<GravityComponent>();
            foreach (var gravityComponentKeyValuePair in gravityComponents)
            {
                var gravityComponent = gravityComponentKeyValuePair.Value as GravityComponent;
                var velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(gravityComponentKeyValuePair.Key);

                var acceleration = gravityComponent.GravityCoefficient / gravityComponent.Mass;
                velocityComponent.Velocity.Y -= acceleration;
            }
        }

            /// <summary>
            /// Updates the friction.
            /// </summary>
            /// <param name="velocityComponent"></param>
            private void UpdateFriction(Entity key)
            {
                var velocityComponent = componentManager.ConcurrentGetComponentOfEntity<VelocityComponent>(key);
                var frictionComponent = componentManager.ConcurrentGetComponentOfEntity<FrictionComponent>(key);
                // Placeholder friction
                if (frictionComponent != null)
                {
                    velocityComponent.Velocity.X *= frictionComponent.Friction;
                    velocityComponent.Velocity.Z *= frictionComponent.Friction;
                }

            }
        
    }
}
