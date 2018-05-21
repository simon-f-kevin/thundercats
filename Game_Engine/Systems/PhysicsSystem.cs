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
    /*
     * System to handle all physics updates including 3D transformations based on velocity, friction, and collision.
     * PhysicsSystem uses parallel foreach loops to improve performance, this does not require locks as the component manager is thread safe.
     */
    public class PhysicsSystem : IUpdateableSystem
    {

        ComponentManager componentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            UpdatePositionsOfModels();
            CheckCollision();
            RunGravity(gameTime);
        }


        /// <summary>
        /// Updates TransformComponents, ModelComponents, and CollisionComponents with the velocities of any attached VelocityComponent.
        /// </summary>
        private void UpdatePositionsOfModels()
        {
            ConcurrentDictionary<Entity, Component> velocityComponentPairs = componentManager.GetConcurrentDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponentPairs, velocityComponentPair =>
            {
                VelocityComponent velocityComponent = velocityComponentPair.Value as VelocityComponent;
                TransformComponent transformationComponent = componentManager.ConcurrentGetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                CollisionComponent collisionComponent = componentManager.ConcurrentGetComponentOfEntity<CollisionComponent>(velocityComponentPair.Key);

                transformationComponent.Position += velocityComponent.Velocity;
                Matrix translation = EngineHelper.Instance().WorldMatrix * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(transformationComponent.Position);
                TransformHelper.TransformEntity(velocityComponentPair.Key, translation, true);

                collisionComponent.UpdateShape(transformationComponent.Position);

                Console.WriteLine("transform Position" + transformationComponent.Position.ToString());
                //UpdateFriction(velocityComponentPair.Key);
            });
        }

        /// <summary>
        /// Checks intersections for all BoundingSphereComponents.
        /// Currently only identifies collision, taking action based on collision is TODO.
        /// </summary>
        private void CheckCollision()
        {
            ConcurrentDictionary<Entity, Component> collisionComponentPairs = componentManager.GetConcurrentDictionary<CollisionComponent>();
            bool found = false; //Temp debug flag

            Parallel.ForEach(collisionComponentPairs, sourceCollisionComponentPair =>
            {
                Entity sourceEntity = sourceCollisionComponentPair.Key;
                var sourceCollisionComponent = sourceCollisionComponentPair.Value as CollisionComponent;
                var cameraComponent = componentManager.GetConcurrentDictionary<CameraComponent>().Values.First() as CameraComponent; //get the cameracomponent
                
                /*
                * Only check collsion on blocks within the cameraComponent farplane,
                * otherwise we get horrendous lag when we check all blocks on entire map
                */
                if (cameraComponent.BoundingFrustum.Intersects(sourceCollisionComponent.BoundingShape))
                {
                    foreach (var targetCollisionComponentPair in collisionComponentPairs)
                    {
                        Entity targetEntity = targetCollisionComponentPair.Key;
                        CollisionComponent targetCollisionComponent = targetCollisionComponentPair.Value as CollisionComponent;
                        if (sourceCollisionComponent.ComponentId != targetCollisionComponent.ComponentId &&
                        sourceCollisionComponent.BoundingShape.Intersects(targetCollisionComponent.BoundingShape))
                        {
                            CollisionManager.Instance.AddCollisionPair(sourceEntity, targetEntity);
                            found = true; //Temp debug flag
                                          //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                        }
                    }
                }
                
            });
            if (!found) //Temp debug check
            {
                //Console.WriteLine("No BoundingSphereComponents intersect");
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

                var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var acceleration = gravityComponent.GravityCoefficient / gravityComponent.Mass;
                velocityComponent.Velocity.Y -= acceleration * dt;
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
