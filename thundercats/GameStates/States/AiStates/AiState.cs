using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thundercats.Actions;
using thundercats.Components;

namespace thundercats.GameStates.States.AiStates
{
    public abstract class AiState
    {
        protected Random Random { get; private set; }
        protected bool MadeMove { get; private set; }
        protected int[,] worldMatrix;
        protected Entity[,] worldEntityMatrix;

        protected AiState(Random random)
        {
            this.Random = random;
        }

        protected abstract Point ChooseBlock(int[] row, int RowIndex);

        protected int[] GetRow(int[,] worldMatrix, int rowIndex)
        {
            int[] worldRow;
            if ((worldMatrix.GetLength(1) - 1) == rowIndex)
                worldRow = new int[] { worldMatrix[0, rowIndex], worldMatrix[1, rowIndex], worldMatrix[2, rowIndex] };
            else
                worldRow = new int[] { worldMatrix[0, rowIndex + 1], worldMatrix[1, rowIndex + 1], worldMatrix[2, rowIndex + 1] };
            return worldRow;
        }

        /// <summary>
        /// Gives the actual position of the block, based on cell position.
        /// </summary>
        /// <param name="cellPosition"></param>
        /// <returns></returns>
        protected Vector3 GetBlock(Point cellPosition)
        {
            var transform = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(worldEntityMatrix[cellPosition.X, cellPosition.Y]);
            return transform.Position;
        }

        protected void ExecuteMove(Vector3 currentBlock, Vector3 nextBlock, Vector3 position)
        {
            //Debug.WriteLine("currentBlock: " + currentBlock.ToString());
            //Debug.WriteLine("aiPos: " + position.ToString());
            //var VelocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(entity);
            if (currentBlock.X > nextBlock.X) //if the block that AI wants to go to is "lower" X value AKA left of the current we need to jump left
            {
                //jump left
                //AiActions.MoveAiLeftwards();
                //PlayerActions.AcceleratePlayerLeftWards(VelocityComponent);
                //PlayerActions.PlayerJumpSpeed(VelocityComponent);
                MadeMove = true;
            }
            if (currentBlock.X < nextBlock.X) //if the block that AI wants to go to is "higher" X value AKA right of the current we need to jump right
            {
                //    AiActions.MoveAiLeftwards();
                //jump Right
                //PlayerActions.AcceleratePlayerRightwards(VelocityComponent);
                //PlayerActions.PlayerJumpSpeed(VelocityComponent);
                MadeMove = true;
            }
            else
            {
                //Continue run
                //PlayerActions.AcceleratePlayerForwards(VelocityComponent);
                MadeMove = true;
            }
        }
        protected Point ExecuteState(Point matrixPosition, Vector3 position)
        {


            var currentBlock = GetBlock(matrixPosition);

            // We need the players current matrix row position to determine the next move
            // the ai should do in the real world:
            var nextMatrixRow = GetRow(worldMatrix, matrixPosition.Y);

            // Debug
            //WriteWorld(worldMatrix);
            WriteRow(nextMatrixRow);

            // Then we need the "real" values of the next block (destination) and the players "real" position
            // to make the move to the next block:
            int index = 0;

            if (matrixPosition.Y < worldMatrix.GetLength(1) - 1)
                index = matrixPosition.Y + 1;
            else
                index = matrixPosition.Y;

            var decision = ChooseBlock(nextMatrixRow, index);
            var destinationBlock = GetBlock(decision);
            // Execute the move to the next block
            ExecuteMove(currentBlock, destinationBlock, position);

            // We look to see if the player is in the same block in the "real" world as
            // in the matrix. if he is, we "wait" until the move is completed and return the same
            // position, or we make the move.
            // TODO: Need to fix
            if (currentBlock.Z + 50 <= position.Z)
                return decision;
            else
                return matrixPosition;
        }

        public void WriteRow(int[] row)
        {
            Debug.Write("Row: {");
            for (int i = 0; i < row.Length; i++)
            {
                Debug.Write(row[i]);
            }
            Debug.WriteLine("}");
        }

        public void WriteWorld(int[,] worldMatrix)
        {
            Debug.WriteLine(" World:");
            for (int j = 0; j < worldMatrix.GetLength(1); j++)
            {
                Debug.Write("{");
                for (int i = 0; i < worldMatrix.GetLength(0); i++)
                {
                    Debug.Write(worldMatrix[i, j]);
                }
                Debug.WriteLine("}");
            }

        }
    }
}
