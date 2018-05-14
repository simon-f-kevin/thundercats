using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            RunGravity(gameTime);
            CheckCollision();
            UpdatePositionsOfModels();
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

                Matrix translation = Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z)
                        * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z);

                TransformHelper.TransformEntity(velocityComponentPair.Key, translation, true);
                UpdateFriction(velocityComponentPair.Key);
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
                //(if (!(gravityComponentKeyValuePair.Value is GravityComponent)) continue;
                var gravityComponent = gravityComponentKeyValuePair.Value as GravityComponent;
                var velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(gravityComponentKeyValuePair.Key);
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(gravityComponentKeyValuePair.Key);
                var collisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<BoundingSphereComponent>(gravityComponentKeyValuePair.Key);

                //if(transformComponent.Position.Y != 0)
                //velocityComponent.Velocity.Y -= 0.001f;//0.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
 
                //Debug.WriteLine(velocityComponent.Velocity);
              //  Debug.WriteLine("Position: " + transformComponent.Position);
         


                //        //velocityComponent.Velocity.Y = +1;
                //        //acceleration = force(time, position) / mass;
                //        //time += timestep;
                //        //position += timestep * velocity;
                //        //velocity += timestep * acceleration;
                if (gravityComponent.IsFalling)
                {
                    var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    var acceleration = 9.8f / gravityComponent.Mass;
                    velocityComponent.Velocity.Y -= acceleration * dt;
                    Debug.WriteLine("Velocity: " + velocityComponent.Velocity.ToString());
                }
                gravityComponent.IsFalling = true;
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
