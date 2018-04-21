using System;
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
            CheckCollision();
            UpdatePositions();
            var gravityComponents = ComponentManager.Instance.GetComponentPairDictionary<GravityComponent>();
            foreach (var gravityComponent in gravityComponents)
            {
                var gravity = gravityComponent.Value as GravityComponent;
                var velocity = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(gravityComponent.Key);
            }
        }

        /*
         * Updates TransformComponents, ModelComponents, and BoundingSphereComponents with the velocities of any attached VelocityComponent.
         */
        public void UpdatePositions()
        {
            Dictionary<Entity, Component> velocityComponents = componentManager.GetComponentPairDictionary<VelocityComponent>();

            foreach(var velocityComponentPair in velocityComponents)
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
            Dictionary<Entity, Component> boundingSphereComponents = componentManager.GetComponentPairDictionary<BoundingSphereComponent>();
            bool found = false; //Temp debug flag

            foreach(BoundingSphereComponent sourceBoundingSphereComponent in boundingSphereComponents.Values)
            {
                foreach(BoundingSphereComponent targetBoundingSphereComponent in boundingSphereComponents.Values)
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
    }
}
