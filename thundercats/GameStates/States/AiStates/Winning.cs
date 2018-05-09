using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using thundercats.Systems;

namespace thundercats.GameStates.States.AiStates
{
    public class Winning : IAiState
    {
        private AiStateManager aiStateManager;

        private AiSystem aiSystem;

        private int TargetValue { get; set; }

        private int[,] WorldMatrix { get; set; }

        private Random random;

        public Winning(AiStateManager aiStateManager)
        {
            this.aiStateManager = aiStateManager;
            random = new Random();
            TargetValue = random.Next(2, 4);
        }
        public void Evaluate()
        {

        }
        public void Update(GameTime gameTime)
        {
            SystemManager.Instance.Update(gameTime);

            CalculateMove();
            //aiSystem.CheckNextRow(/*FUUCK*/ TargetValue);
        }

        private void CalculateMove()
        {
            // Player pos row should be updated depending on real position. (transformComponent)
            Point playerPosRow = new Point(1, 4); // what row player should be on in matrix
            int[] nextMatrixRow = GetRow(WorldMatrix, playerPosRow.X); // get next row in front of player

            var currentBlock = GetCurrentBlock(playerPosRow);
            Point decision = ChooseBlock(nextMatrixRow, playerPosRow.X);
            var nextBlock = GetNextRowBlock(decision);
            // Check What decision to do

            MakeMove(currentBlock, nextBlock);
        }

        private void MakeMove(Vector3 currentBlock, Vector3 nextBlock)
        {  
            //var VelocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);
            if (currentBlock.X > nextBlock.X)
            {
                //jump left
                //PlayerActions.AcceleratePlayerLeftWards(VelocityComponent);
                //PlayerActions.PlayerJumpSpeed(VelocityComponent);
            }
            if (currentBlock.X < nextBlock.X)
            {
                //jump Right
                //PlayerActions.AcceleratePlayerRightwards(VelocityComponent);
                //PlayerActions.PlayerJumpSpeed(VelocityComponent);
            }
            else
            {
                //Continue run
                //PlayerActions.AcceleratePlayerForwards(VelocityComponent);
            }
        }

        private Vector3 GetNextRowBlock(Point decision)
        {
            // return next block dimensions
            return default(Vector3);
        }

        private Vector3 GetCurrentBlock(Point playerPosRow)
        {
            // return current block dimensions
            return default(Vector3);
        }

        private int[] GetRow(int[,] worldMatrix, int row)
        {
            // should return row
            return new int[] { worldMatrix[row + 1, 0], worldMatrix[row + 1, 1], worldMatrix[row + 1, 2] };
        }

        private Point ChooseBlock(int[] row, int RowIndex)
        {
            int currentChoice = 0;
            // Choose the right block on that row
            for (int i = 0; i < row.Length; i++)
            {
                // Logic should be here to choose column/block
                if (row[i] != 0) return new Point(RowIndex, i); // this should be the only thing we need to do? cuz we just want to survive
               
            }
            return new Point(RowIndex, currentChoice);
            // Index of the next block the ai is moving to;
        }
    }
}
