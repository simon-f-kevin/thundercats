using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
namespace Game_Engine.Systems
{
    public class PhysicsSystem : IUpdateableSystem
    {
 
        ComponentManager componentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            RunGravity();
            CheckCollision();
            UpdatePositions();
        }

        /*
         * Updates TransformComponents, ModelComponents, and BoundingSphereComponents with the velocities of any attached VelocityComponent.
         */
        public void UpdatePositions()
        {
            ConcurrentDictionary<Entity, Component> velocityComponentPairs = componentManager.GetComponentPairDictionary<VelocityComponent>();

            foreach(var velocityComponentPair in velocityComponentPairs)
            {
                VelocityComponent velocityComponent = velocityComponentPair.Value as VelocityComponent;
                TransformComponent transformationComponent = componentManager.GetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                ModelComponent modelComponent = componentManager.GetComponentOfEntity<ModelComponent>(velocityComponentPair.Key);
                BoundingSphereComponent boundingSphereComponent = componentManager.GetComponentOfEntity<BoundingSphereComponent>(velocityComponentPair.Key);

                transformationComponent.Position += velocityComponent.Velocity;
                Matrix translation = Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z)
                        * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z);

                if(modelComponent != null)
                {
                    modelComponent.World *= translation;
                }
                if(boundingSphereComponent != null)
                {
                    boundingSphereComponent.BoundingSphere = boundingSphereComponent.BoundingSphere.Transform(translation);
                }
                // Placeholder friction
                velocityComponent.Velocity.X *= 0.5f;
                velocityComponent.Velocity.Z *= 0.5f;
            }
        }

        /*
         * Checks intersections for all BoundingSphereComponents.
         * BoundingSphereComponents that equal themselves are ignored.
         * Currently only identifies collision, taking action based on collision is TODO.
         */
        public void CheckCollision()
        {
            ConcurrentDictionary<Entity, Component> boundingSphereComponentPairs = componentManager.GetComponentPairDictionary<BoundingSphereComponent>();
            bool found = false; //Temp debug flag

            foreach(BoundingSphereComponent sourceBoundingSphereComponent in boundingSphereComponentPairs.Values)
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
            }
            if(!found) //Temp debug check
            {
                //Console.WriteLine("No BoundingSphereComponents intersect");
            }
        }


        /// <summary>
        /// Applies gravity to all entities with a gravity-component and velocity-component. 
        /// If they have a velocity-compoent, but no gravity-component no gravity is applied. 
        /// </summary>
        private void RunGravity()
        {
            var gravityComponents = ComponentManager.Instance.GetComponentPairDictionary<GravityComponent>();
            foreach (var gravityComponentKeyValuePair in gravityComponents)
            {
                //if (!(gravityComponentKeyValuePair.Value is GravityComponent)) continue;
                var velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(gravityComponentKeyValuePair.Key);
                velocityComponent.Velocity.Y -= 0.5f;
            }
        }
    }
}
