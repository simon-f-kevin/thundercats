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
        * Gets all collisions for engine and handles them according to collision rules
        */
        public void Update(GameTime gameTime)
        {
            ConcurrentQueue <Tuple <Component, Component>> collisionPairs = collisionManager.CurrentCollisionPairs;

            for(int i = 0; i < collisionPairs.Count; i++)
            {
                //TODO: handle collision
                collisionManager.RemoveCollisionPair();
            }
        }
    }
}
