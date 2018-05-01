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
        public void CheckNextRow(/*ArrayOfMap,*/ Entity key) {
           var AiComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<AiComponent>(key);
           var AiTransformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(key);
            
            int[] nextRow;
            nextRow = new int[3];
            //nextRow = ArrayMap[AiTransformComponent.Position+1];
            for(int i=0; i< nextRow.Length; i++) {
                /*if(AiState == losing)
                 *  move == 1  (1 is bad)?
                 * 
                 * 
                 * 
                 */
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
