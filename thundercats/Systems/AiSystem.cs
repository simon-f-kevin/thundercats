using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Components;

namespace thundercats.Systems
{
    public class AiSystem : IUpdateableSystem
    {
        /* Check if "Lane" is clear
         * Check NextRow if winning, dont care for powerups only survival!
         * if! losing! the AI should check after powerups! 
         * in Nextrow we need to evaluate where to go!
         */
        public AiSystem() {


        }
        public void AiGameState(Entity AiKey, Entity PlayerKey) {
            int targetValue = 0;
            //Ai values
            var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(AiKey);
            var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(AiKey);
            //Player values
            var playerTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(PlayerKey);

            if (aiTransformComponent.Position.Z < playerTransformComponent.Position.Z)
            {
                aiComponent.CurrentState = AiComponent.AiState.Losing;
                //if we r losing we want to get(2,3,4?) from next row? 
                //random random int(1,5)?
                CheckNextRow(AiKey, targetValue);

            }
            else if (aiTransformComponent.Position.Z > playerTransformComponent.Position.Z)
            {
                aiComponent.CurrentState = AiComponent.AiState.Winning;
                //if we r winning we want to avoid 0, we dont care about anything else, same for Even?
                CheckNextRow(AiKey, targetValue);

            }
            else {
                aiComponent.CurrentState = AiComponent.AiState.Even;
                CheckNextRow(AiKey, targetValue);
            }
           
        }
        public void CheckNextRow(/*ArrayOfMap,*/ Entity key, int targetValue) {

           var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(key);
           var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(key);
            
            int[] nextRow;
            nextRow = new int[3];
            //nextRow = ArrayMap[AiTransformComponent.Position+1];
            for(int i=0; i< nextRow.Length; i++) {

                switch (nextRow[i]) {
                    case 0:
                        //lane is clear this is option NR1 if WinningState
                        break;
                    case 1:
                        // 1 is obstacle
                        break;
                }
                 
                }
            //return move
        }
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
