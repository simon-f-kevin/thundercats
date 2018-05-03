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
            Random random = new Random();
            //Ai values
            var aiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(AiKey);
            var aiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(AiKey);
            //Player values
            var playerTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(PlayerKey);

            if (aiTransformComponent.Position.Z < (playerTransformComponent.Position.Z + 10))
            {
                aiComponent.CurrentState = AiComponent.AiState.Losing;
                //if we r losing we want to get(2,3,4?) from next row? 
                //random random int(1,5)?
                targetValue = random.Next(2,4);
                CheckNextRow(AiKey, targetValue);

            }
            else if (aiTransformComponent.Position.Z > (playerTransformComponent.Position.Z + 5))
            {
                aiComponent.CurrentState = AiComponent.AiState.Winning;
                //if we r winning we want to avoid 0, we dont care about anything else, maybe have a random? that gives a 50% chance of getting killed? 
                //IDEA: if(aiTransformComponent.Position.Z > 200) targetValue = random.Next(0,2) if targetValue == 1 || targetValue == 2 then targetValue = 1)
                if (aiTransformComponent.Position.Z > 200)
                {
                    targetValue = random.Next(0, 100);
                    if (targetValue > 30)
                    {
                        targetValue = 1;
                    }
                    else
                    {
                        targetValue = 0;
                    }
                }
                else
                {
                    targetValue = 1;
                }
                CheckNextRow(AiKey, targetValue);

            }
           
        }
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
                }
            }
            //for (int i=0; i< nextRow.Length; i++) {
            //    if (nextRow[i] > 0) {
            //        tempVal = nextRow[i];
            //    }
            //    if (targetValue == nextRow[i]) {
            //        if (i == 0) aiComponent.CurrentMove = AiComponent.AiMove.Left;
            //        if (i == 1) aiComponent.CurrentMove = AiComponent.AiMove.Run;
            //        if (i == 2) aiComponent.CurrentMove = AiComponent.AiMove.Right;
            //        //return nextRow[i] 
            //        break; 
            //    }
            //    }
 
            //return move
        }
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
