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
using thundercats.Components;
using thundercats.GameStates;
using thundercats.GameStates.States.AiStates;

namespace thundercats.Systems
{
    public class AiSystem : IUpdateableSystem
    {
        /* Check if "Lane" is clear
         * Check NextRow if winning, dont care for powerups only survival!
         * if! losing! the AI should check after powerups! 
         * in Nextrow we need to evaluate where to go!
         */
       
      
        //protected GameTime gameTime;
        public enum AiState {
            Winning,
            Losing
        };

        public Dictionary<AiState, IAiState> aiStates;

        public AiSystem() {
            aiStates = new Dictionary<AiState, IAiState>();
            aiStates.Add(AiState.Winning, new Winning());
            aiStates.Add(AiState.Losing, new Losing());
        }

        private void UpdateCurrentState(Entity AiKey, Entity PlayerKey, GameTime gameTime) {
            //Ai values
            var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(AiKey);
            var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(AiKey);
            //Player values
            var playerTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(PlayerKey);
            // we need +10 because we need a bufferZone to not change state like everysecond if players are close to eachother
            if (aiTransformComponent.Position.Z < (playerTransformComponent.Position.Z + 10))
            {
                aiComponent.CurrentState = AiState.Losing;
            }
            else if (aiTransformComponent.Position.Z > (playerTransformComponent.Position.Z + 2))
            {
                aiComponent.CurrentState = AiState.Winning;
  
            }

        }

        private void RunCurrentState(GameTime gameTime, Entity key)
        {
            var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(key);
            var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(key);
            var aiVelocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(key);
            var aiGravityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<GravityComponent>(key);
            //Player values
           
            var pos = aiComponent.MatrixPosition;
            aiStates[aiComponent.CurrentState].Update(gameTime, ref pos, aiTransformComponent.Position, aiVelocityComponent, aiGravityComponent);
            aiComponent.MatrixPosition = pos;
        }

        public void Update(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();
            ConcurrentDictionary<Entity, Component> aiComponents = ComponentManager.Instance.GetConcurrentDictionary<AiComponent>();
            
            foreach (var ai in aiComponents.Keys)
            {
                foreach (var playerComponent in playerComponents.Keys) {
                    UpdateCurrentState(ai, playerComponent,gameTime);
                    RunCurrentState(gameTime, ai);
                }
            }
        }
       
    }
}
