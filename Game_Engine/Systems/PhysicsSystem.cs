using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
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
            RunGravity();
            CheckCollision();
            UpdatePositionsOfModels();
        }


        /// <summary>
        /// Updates TransformComponents, ModelComponents, and BoundingSphereComponents with the velocities of any attached VelocityComponent.
        /// </summary>
        private void UpdatePositionsOfModels()
        {
            ConcurrentDictionary<Entity, Component> velocityComponentPairs = componentManager.GetConcurrentDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponentPairs, velocityComponentPair =>
            {
                VelocityComponent velocityComponent = velocityComponentPair.Value as VelocityComponent;
                TransformComponent transformationComponent = componentManager.ConcurrentGetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                ModelComponent modelComponent = componentManager.ConcurrentGetComponentOfEntity<ModelComponent>(velocityComponentPair.Key);

                transformationComponent.Position += velocityComponent.Velocity;
                Matrix translation = Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z)
                        * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z);

                if(modelComponent != null)
                {
                    modelComponent.World *= translation;
                }

                UpdatePositionsOfBoundingSpheres(velocityComponentPair.Key, translation);
                UpdateFriction(velocityComponentPair.Key);
            }
        }

       

       /// <summary>
       /// Checks intersections for all BoundingSphereComponents.
       /// Currently only identifies collision, taking action based on collision is TODO.
       /// </summary>
        private void CheckCollision()
        {
            ConcurrentDictionary<Entity, Component> boundingSphereComponentPairs = componentManager.GetConcurrentDictionary<BoundingSphereComponent>();
            bool found = false; //Temp debug flag

            Parallel.ForEach(boundingSphereComponentPairs, boundingSphereComponentPair =>
            {
                foreach(BoundingSphereComponent targetBoundingSphereComponent in boundingSphereComponentPairs.Values)
                {
                    if(sourceBoundingSphereComponent.ComponentId != targetBoundingSphereComponent.ComponentId &&
                        sourceBoundingSphereComponent.BoundingSphere.Intersects(targetBoundingSphereComponent.BoundingSphere))
                    {
                        found = true; //Temp debug flag
                        //Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                    }
                }
            });
            if(!found) //Temp debug check
            {
                //Console.WriteLine("No BoundingSphereComponents intersect");
            }
        }


        /// <summary>
        /// Applies gravity to all entities with a gravity-component and velocity-component. 
        /// If they have a velocity-component, but no gravity-component no gravity is applied. 
        /// </summary>
        private void RunGravity()
        {
            var gravityComponents = ComponentManager.Instance.GetConcurrentDictionary<GravityComponent>();
            foreach (var gravityComponentKeyValuePair in gravityComponents)
            {
                //if (!(gravityComponentKeyValuePair.Value is GravityComponent)) continue;
                var velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(gravityComponentKeyValuePair.Key);
                velocityComponent.Velocity.Y -= 0.5f;
            }
        }

        /// <summary>
        /// Updates the positions of bounding spheres of models
        /// BoundingSphereComponents that equal themselves are ignored.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        private void UpdatePositionsOfBoundingSpheres(Entity key, Matrix translation)
        {
            BoundingSphereComponent boundingSphereComponent = componentManager.ConcurrentGetComponentOfEntity<BoundingSphereComponent>(key);

            if (boundingSphereComponent != null)
            {
                boundingSphereComponent.BoundingSphere = boundingSphereComponent.BoundingSphere.Transform(translation);
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
            velocityComponent.Velocity.X *= frictionComponent.Friction;
            velocityComponent.Velocity.Z *= frictionComponent.Friction;
        }
    }
}
