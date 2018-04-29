using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;

namespace thundercats.Systems
{
    class CollisionHandlingSystem : IUpdateableSystem
    {
        private CollisionManager collisionManager = CollisionManager.Instance;

        /*
        * Gets all collisions from engine and resolves them according to collision rules
        */
        public void Update(GameTime gameTime)
        {
            ConcurrentQueue <Tuple<Entity, Entity>> collisionPairs = collisionManager.CurrentCollisionPairs;
            Tuple<Entity, Entity> currentCollisionPair;

            for(int i = 0; i < collisionPairs.Count; i++)
            {
                currentCollisionPair = collisionManager.RemoveCollisionPair();	
            }
        }

        private void ResolveCollision(Tuple<Entity, Entity> collisionPair){
            Entity sourceEntity = collisionPair.Item1;
            Entity targetEntity = collisionPair.Item2;
            TransformComponent sourceTransformComponent = ComponentManager.GetComponentOfEntity<TransformComponent>(sourceEntity);
            TransformComponent targetTransformComponent = ComponentManager.GetComponentOfEntity<TransformComponent>(targetEntity);
            Tuple<String, String, String> collisionSides = FindCollisionSides(sourceTransformComponent, targetTransformComponent);

            switch(sourceEntity.EntityTypeName)
            {
                case "local_player":
                    Console.WriteLine("local_player collision at sides: " + collisionSides.Item1 + " " + collisionSides.Item2 + " " + collisionSides.Item3);
                    break;
                case "default":
                    break;
            }       
		}

		private Tuple<String, String, String> FindCollisionSides(TransformComponent sourceTransformComponent, TransformComponent targetTransformComponent)
        {
            //float xDiff = sourceTransformComponent.Posision.X - targetTransformComponent.Position.X
            //float yDiff = sourceTransformComponent.Posision.Y - targetTransformComponent.Position.Y
            //float zDiff = sourceTransformComponent.Posision.Z - targetTransformComponent.Position.Z

            //float maxCoord = Math.Max(xDiff, Math.Max(yDiff, zDiff));

            Tuple<String, String, String> sides;

            if(sourceTransformComponent.Posision.X < targetTransformComponent.Position.X){
				sides.Item1 = "left";
            }
            else
            {
                sides.Item1 = "right";
            }
            if(sourceTransformComponent.Posision.Y < targetTransformComponent.Position.Y){
				sides.Item2 = "down"
            }
            else
            {
                sides.Item2 = "up";
            }
            if(sourceTransformComponent.Posision.Z < targetTransformComponent.Position.Z){
				sides.Item3 = "back";
            }
            else
            {
                sides.Item3 = "forward";
            }
            return sides;
        }

        private void UpdateSourceCollider(TransformComponent sourceTransformComponent, TransformComponent targetTransformComponent, Tuple<String, String, String> sides){
            if(sides.Item1 = "left"){
            }
            else
            {
            }
            if(sides.Item2 = "left"){
            }
            else
            {
            }
            if(sides.Item3 = "back"){
            }
            else
            {
            }
		}
    }
}
