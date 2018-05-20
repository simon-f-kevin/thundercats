using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Actions;

namespace thundercats.Systems
{
    public class NetworkInputSystem : IUpdateableSystem
    {
        public void Update(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> networkInputComponents = ComponentManager.Instance.GetConcurrentDictionary<NetworkInputComponent>();

            foreach (Entity playerEntity in networkInputComponents.Keys)
            {
                if (playerEntity.EntityTypeName.Equals(GameEntityFactory.REMOTE_PLAYER))
                {
                    ParseNetworkInput(gameTime, playerEntity);
                }
            }
        }

        /// <summary>
        /// Check if networkinputcomponent's parameters are true and do actions on remote player
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="playerEntity"></param>
        private void ParseNetworkInput(GameTime gameTime, Entity playerEntity)
        {
            NetworkInputComponent networkInputComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<NetworkInputComponent>(playerEntity);
            VelocityComponent velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(playerEntity);
            TransformComponent transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(playerEntity);
            GravityComponent gravityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<GravityComponent>(playerEntity);

            if (networkInputComponent.MoveForward) PlayerActions.AcceleratePlayerForwards(gameTime, velocityComponent);
            if (networkInputComponent.MoveLeft) PlayerActions.AcceleratePlayerLeftwards(gameTime, velocityComponent);
            if (networkInputComponent.MoveRight) PlayerActions.AcceleratePlayerRightwards(gameTime, velocityComponent);
            if (networkInputComponent.Jump) PlayerActions.PlayerJump(gameTime, velocityComponent);
            if (networkInputComponent.MoveBackward) PlayerActions.AcceleratePlayerBackwards(gameTime, velocityComponent);

        }
    }
}
