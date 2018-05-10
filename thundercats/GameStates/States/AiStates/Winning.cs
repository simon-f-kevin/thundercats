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
    public class Winning : IAiState
    {

        private int TargetValue { get; set; }

        private int[,] worldMatrix;

        private Random random;
       public int[] nextMatrixRow = new int[0];
        public Winning()
        {
            random = new Random();
            TargetValue = random.Next(2, 4);
            worldMatrix = GameService.Instance().GameWorld;
        }
        public void Update(GameTime gameTime)
        {

            worldMatrix = GameService.Instance().GameWorld;
            CalculateMove();
        }

        private void CalculateMove()
        {
            ConcurrentDictionary<Entity, Component> aiComponents = ComponentManager.Instance.GetConcurrentDictionary<AiComponent>();
            foreach (var aiComponent in aiComponents.Keys)
            {
        
                //we maybe need velocity when we make the move?
                VelocityComponent velocityComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(aiComponent);
                TransformComponent transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(aiComponent);
                // Player pos row should be updated depending on real position. (transformComponent)
                Point playerPosRow = new Point((int)transformComponent.Position.X, (int)transformComponent.Position.Z); // what row player should be on in matrix
                if (playerPosRow.X > 2) playerPosRow.X = 1;
                nextMatrixRow = GetRow(worldMatrix, playerPosRow.X); // get next row in front of player

                Console.WriteLine(playerPosRow.X.ToString());
                //Console.WriteLine(nextMatrixRow[0] +" ");
                var currentBlock = GetCurrentBlock(playerPosRow);
                Point decision = ChooseBlock(nextMatrixRow, playerPosRow.X);
                var nextBlock = GetNextRowBlock(decision);
                // Check What decision to do
                MakeMove(currentBlock, nextBlock);
            }
           
        }

        private void MakeMove(Vector3 currentBlock, Vector3 nextBlock)
        {  
            //var VelocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);
            if (currentBlock.X > nextBlock.X) //if the block that AI wants to go to is "lower" X value AKA left of the current we need to jump left
            {
                //jump left
                //PlayerActions.AcceleratePlayerLeftWards(VelocityComponent);
                //PlayerActions.PlayerJumpSpeed(VelocityComponent);
            }
            if (currentBlock.X < nextBlock.X) //if the block that AI wants to go to is "higher" X value AKA right of the current we need to jump right
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
            // return next block dimensions //get its X and Z coordinates Middle X is needed and atleast min Z
            return default(Vector3);
        }

        private Vector3 GetCurrentBlock(Point playerPosRow)
        {
            // return current block dimensions //get its X and Z coordinates Middle X is needed and atleast min Z
            return default(Vector3);
        }

        private int[] GetRow(int[,] worldMatrix, int row)
        {
            // should return row in the worldMatrix
            //WE GET EXCEPTION INDEX OUTOF RANGE HERE!!!!!!
            return new int[] { worldMatrix[row + 1, 0], worldMatrix[row + 1, 1], worldMatrix[row + 1, 2] };
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
            return new Point(RowIndex, currentChoice);
            // Index of the next block the ai is moving to;
        }
    }
}
