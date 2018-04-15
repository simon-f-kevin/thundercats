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
        private ComponentManager componentManager = ComponentManager.Instance;

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
            var velocityComponents = componentManager.GetComponentDictionary<VelocityComponent>();
            foreach(var velocityComponentPair in velocityComponents)
            {
                var velocityComponent = velocityComponentPair.Value as VelocityComponent;
                var transformationComponent = componentManager.GetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                var modelComponent = componentManager.GetComponentOfEntity<ModelComponent>(velocityComponentPair.Key);
                var boundingSphereComponent = componentManager.GetComponentOfEntity<BoundingSphereComponent>(velocityComponentPair.Key);

                transformationComponent.position += velocityComponent.Velocity;
                Matrix translation = Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z)
                        * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z);

                if(modelComponent != null)
                {
                    //UpdateModel(modelComponent, transformation, velocity);
                    modelComponent.Model.Bones[0].Transform *= translation;
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

        public void UpdateModel(ModelComponent modelComponent, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(velocityComponent.Velocity.X, 0, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, 0, 0);
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(0, velocityComponent.Velocity.Y, 0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0, velocityComponent.Velocity.Y, 0);
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(0, 0, velocityComponent.Velocity.Z) * Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(0, 0, velocityComponent.Velocity.Z);
        }

        /*
         * Checks intersections for all BoundingSphereComponents.
         * BoundingSphereComponents that equal themselves are ignored.
         * Currently only identifies collision, taking action based on collision is TODO.
         */
        public void CheckCollision()
        {
            Dictionary<Entity, Component> boundingSphereComponents = componentManager.GetComponentDictionary<BoundingSphereComponent>();
            bool found = false;

            foreach(BoundingSphereComponent sourceBoundingSphereComponent in boundingSphereComponents.Values)
            {
                foreach(BoundingSphereComponent targetBoundingSphereComponent in boundingSphereComponents.Values)
                {
                    if(sourceBoundingSphereComponent.ComponentId != targetBoundingSphereComponent.ComponentId &&
                        sourceBoundingSphereComponent.BoundingSphere.Intersects(targetBoundingSphereComponent.BoundingSphere))
                    {
                        found = true;
                        Console.WriteLine(sourceBoundingSphereComponent.ComponentId.ToString() + " Intersects " + targetBoundingSphereComponent.ComponentId.ToString());
                    }
                }
            }
            if(!found)
            {
                Console.WriteLine("No BoundingSphereComponents intersect");
            }
        }
    }
}
