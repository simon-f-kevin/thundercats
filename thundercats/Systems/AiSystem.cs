﻿using Game_Engine.Components;
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
       
        private object componentManager;
        protected GameTime gameTime;
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
        public void AiGameState(Entity AiKey, Entity PlayerKey) {
            Random random = new Random();
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
            else if (aiTransformComponent.Position.Z > (playerTransformComponent.Position.Z + 5))
            {
                aiComponent.CurrentState = AiState.Winning;
   
            }
            aiStates[aiComponent.CurrentState].Update(gameTime);

        }
        //not 100% sure of how to get the array of Map in to this method (how to represent it in a nice way, and can we get the Exact position in array
        // with only the AI position? isent this a problem? cuz the position isent same as Array[3][2]??? 
        public void CheckNextRow(/*ArrayOfMap,*/ Entity key, int targetValue) {
            int tempVal = 0;
           var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(key);
           var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(key);


            int[] nextRow;
            nextRow = new int[3];
            //nextRow = ArrayMap[AiTransformComponent.Position+1];
            if (nextRow.Contains<int>(targetValue))
            {
                tempVal = targetValue;
            }
            else if(nextRow.Contains<int>(1))
            {
                tempVal = 1;
            }
            for (int i = 0; i < nextRow.Length; i++) {
                if (nextRow[i] == tempVal)
                {
                    if (i == 0) aiComponent.CurrentMove = AiComponent.AiMove.Left;
                    if (i == 1) aiComponent.CurrentMove = AiComponent.AiMove.Run;
                    if (i == 2) aiComponent.CurrentMove = AiComponent.AiMove.Right;
                    MakeMove(key,aiComponent);
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> playerComponents = ComponentManager.Instance.GetConcurrentDictionary<PlayerComponent>();
            ConcurrentDictionary<Entity, Component> aiComponents = ComponentManager.Instance.GetConcurrentDictionary<AiComponent>();
            
            foreach (var aiComponent in aiComponents.Keys)
            {
                foreach (var playerComponent in playerComponents.Keys) {
                    AiGameState(aiComponent, playerComponent);
                }
            }
        }
        private void MakeMove(Entity aiEntity, AiComponent aiComponent) {
            if (aiComponent != null)
            {
                if (aiComponent.CurrentMove == AiComponent.AiMove.Left)
                {
                    //PlayerActions.AcceleratePlayerLeftwards(velocityComponent);
                    //PlayerActions.PlayerJumpSpeed(velocityComponent);
                }
                if (aiComponent.CurrentMove == AiComponent.AiMove.Run)
                {
                    //PlayerActions.AcceleratePlayerForwards(velocityComponent);
                }
                if (aiComponent.CurrentMove == AiComponent.AiMove.Right)
                {
                    //PlayerActions.AcceleratePlayerRightwards(velocityComponent);
                    //PlayerActions.PlayerJumpSpeed(velocityComponent);
                }
            }

        }
    }
}
