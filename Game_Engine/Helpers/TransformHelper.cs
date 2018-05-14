using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Helpers
{
    public static class TransformHelper
    {

        /// <summary>
        /// Updates the positions of an entity, including any models and bounding shapes attached.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="translation"></param>
        public static void TransformEntity(Entity entity, Matrix translation, bool threadSafety)
        {
            TransformComponent transformComponent = null;
            ModelComponent modelComponent = null;

            if(threadSafety)
            {
                transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(entity);
                modelComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<ModelComponent>(entity);
            }
            else
            {
                transformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(entity);
                modelComponent = ComponentManager.Instance.GetComponentOfEntity<ModelComponent>(entity);
            }

            if(transformComponent != null)
            {
                transformComponent.Position = translation.Translation;

                if(modelComponent != null)
                {
                    modelComponent.World *= translation;
                }
                TransformBoundingShapes(entity, translation, threadSafety);
            }
        }

        /// <summary>
        /// Updates the positions of bounding shapes
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="translation"></param>
        private static void TransformBoundingShapes(Entity entity, Matrix translation, bool threadSafety)
        {
            CollisionComponent collisionComponent = null;

            if(threadSafety)
            {
                collisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CollisionComponent>(entity);
            }
            else
            {
                collisionComponent = ComponentManager.Instance.GetComponentOfEntity<CollisionComponent>(entity);
            }
            

            if(collisionComponent != null)
            {
                var boundingShape = collisionComponent.BoundingShape;
                boundingShape = collisionComponent.BoundingShape.Transform(translation);
                collisionComponent.BoundingShape = boundingShape;
            }
        }

        /*
        * Translates a model component to be at the same world position as a transform component.
        */
        public static void SetInitialModelPos(ModelComponent modelComponent, TransformComponent transformComponent)
        {
            modelComponent.World = Matrix.CreateTranslation(transformComponent.Position.X, transformComponent.Position.Y, transformComponent.Position.Z);
        }

        /*
         * Translates a bounding sphere component to be at the same world position as a transform component.
         */
        public static void SetInitialBoundingSpherePos(CollisionComponent collisionComponent, TransformComponent transformComponent)
        {
            Matrix translation = Matrix.CreateTranslation(transformComponent.Position.X, transformComponent.Position.Y, transformComponent.Position.Z);
            var boundingSphere = collisionComponent.BoundingShape;
            boundingSphere = collisionComponent.BoundingShape.Transform(translation);
            collisionComponent.BoundingShape = boundingSphere;
        }

        /*
        * Translates a bounding box component to be at the same world position as a transform component.
        */
        public static void SetInitialBoundingBoxPos(CollisionComponent collisionComponent, TransformComponent transformComponent)
        {
            var boundingBox = collisionComponent.BoundingShape;
            var lengthX = (boundingBox.Max.X - boundingBox.Min.X) / 2;
            var lengthY = (boundingBox.Max.Y - boundingBox.Min.Y) / 2;
            var lengthZ = (boundingBox.Max.Z - boundingBox.Min.Z) / 2;

            var min = new Vector3(transformComponent.Position.X - lengthX, transformComponent.Position.Y - lengthY, transformComponent.Position.Z - lengthZ);
            var max = new Vector3(transformComponent.Position.X + lengthX, transformComponent.Position.Y + lengthY, transformComponent.Position.Z + lengthZ);
            collisionComponent.BoundingShape = new BoundingBox(min, max);
        }
    }
}
