using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using thundercats.Components;
using thundercats.Service;
using thundercats.Systems;

namespace thundercats.GameStates.States.AiStates
{
    public class Winning : AiState, IAiState
    {
        private Random random;
        public Winning()
        {
            random = new Random();
            TargetValue = random.Next(2, 4);
            worldMatrix = GameService.Instance().GameWorld;
        }
        public void Update(GameTime gameTime)
        {

            worldMatrix = GameService.Instance().GameWorld;
            worldEntityMatrix = GameService.Instance().EntityGameWorld;
            CalculateMove();
        }

        private void CalculateMove()
        {
            ConcurrentDictionary<Entity, Component> aiComponents = ComponentManager.Instance.GetConcurrentDictionary<AiComponent>();
            foreach (var aiComponent in aiComponents)
            {
                var ai = aiComponent.Value as AiComponent;
                //we maybe need velocity when we make the move?
                var velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(aiComponent.Key);
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(aiComponent.Key);

                // We need the players current matrix row position to determine the next move
                // the ai should do in the real world:
                var playerCellPosition = ai.MatrixPosition;
                var nextMatrixRow = GetRow(worldMatrix, playerCellPosition.Y);

                // Then we need the real values of the next block (destination) and the players real position
                // to make the move to the next block:
                Vector3 currentBlock = GetBlock(playerCellPosition);
                Point decision = ChooseBlock(nextMatrixRow, playerCellPosition.Y + 1);
                Vector3 destinationBlock = GetBlock(decision);
                // Check What decision to do
                MakeMove(currentBlock, destinationBlock, transformComponent);
            }
           
        }





        private Point ChooseBlock(int[] row, int RowIndex)
        {
            int currentChoice = 0;
            // Choose the right block on that row
            for (int i = 0; i < row.Length; i++)
            {
                // Logic should be here to choose column/block
                if (row[i] != 0)/* this should be the only thing we need to do? cuz we just want to survive*/ {
                    currentChoice = i;
                    break;
                } 
            }
            return new Point(currentChoice, RowIndex);
            // Index of the next block the ai is moving to;
        }
    }
}
