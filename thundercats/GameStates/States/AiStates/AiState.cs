using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.GameStates.States.AiStates
{
    public abstract class AiState
    {
        protected int TargetValue { get; set; }

        protected int[,] worldMatrix;
        protected Entity[,] worldEntityMatrix;

        protected int[] GetRow(int[,] worldMatrix, int row)
        {
            int[] worldRow;

            // Debug
            WriteWorld(worldMatrix);

            // if the last row we cannot go out of bounds.
            if((worldMatrix.GetLength(1) - 1) == row)
                worldRow = new int[] { worldMatrix[0, row], worldMatrix[1, row], worldMatrix[2, row] };
            else
                worldRow = new int[] { worldMatrix[0, row + 1], worldMatrix[1, row + 1], worldMatrix[2, row + 1] };

            // Debug
            WriteRow(worldRow);

            return worldRow;
        }

        protected Vector3 GetBlock(Point cell)
        {
            var transform = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(worldEntityMatrix[cell.X, cell.Y]);
            return transform.Position;
        }

        protected void MakeMove(Vector3 currentBlock, Vector3 nextBlock, TransformComponent transform)
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
            Debug.WriteLine(" World {");
            for (int j = 0; j < worldMatrix.GetLength(1); j++)
            {
                Debug.Write("{");
                for (int i = 0; i < worldMatrix.GetLength(0); i++)
                {
                    Debug.Write(worldMatrix[i, j]);
                }
                Debug.WriteLine("}");
            }
            Debug.WriteLine("}");

        }
    }
}
