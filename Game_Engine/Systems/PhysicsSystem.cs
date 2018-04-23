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
            CheckCollision();
            UpdatePositions();
        }

        /*
         * Updates TransformComponents, ModelComponents, and BoundingSphereComponents with the velocities of any attached VelocityComponent.
         */
        public void UpdatePositions()
        {
            ConcurrentDictionary<Entity, Component> velocityComponentPairs = componentManager.GetComponentPairDictionary<VelocityComponent>();

            Parallel.ForEach(velocityComponentPairs, velocityComponentPair =>
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
                velocityComponent.Velocity.Y *= 0.5f;
                velocityComponent.Velocity.Z *= 0.5f;
            });
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

            Parallel.ForEach(boundingSphereComponentPairs, boundingSphereComponentPair =>
            {
                BoundingSphereComponent sourceBoundingSphereComponent = boundingSphereComponentPair.Value as BoundingSphereComponent;

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
    }
}
