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
        public enum State
        {
            Winning,
            Losing
        };

        protected Random Random { get; private set; }
        protected bool MadeMove { get; private set; } = true;
        protected int[,] worldMatrix;
        protected Entity[,] worldEntityMatrix{ get; set;}

        private int targetRow;
        private Point targetBlockTile;
        private Vector3 targetBlockPos;

        protected AiState(Random random)
        {
            this.Random = random;
        }

        protected abstract Point ChooseBlock(int[,] world, int rowIndex);

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
            if (worldEntityMatrix[cellPosition.X, cellPosition.Y] == null)
            {
                throw new Exception("worldEntityMatrix[" + cellPosition.X + ", " + cellPosition.Y + "] was null");
            }
            var transform = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(worldEntityMatrix[cellPosition.X, cellPosition.Y]);
            return transform.Position;
        }

        protected void ExecuteMove(GameTime gameTime, Vector3 currentBlock, Vector3 nextBlock, Vector3 position, VelocityComponent velocity, GravityComponent gravity, AiComponent aiComponent)
        {
            PlayerActions.AcceleratePlayerForwards(gameTime, velocity);
            if (aiComponent.CurrentState == State.Winning)
            {
                if (Random.Next(0, 11) == 1) //20% chance to make random move
                {
                    Console.WriteLine("RandomMove ");
                    if (Random.Next(0, 2) == 1) //50% chance to go left or right
                    {
                        PlayerActions.AcceleratePlayerLeftwards(gameTime, velocity);
                        if (!gravity.HasJumped)
                        {
                            PlayerActions.PlayerJump(gameTime, velocity, null);
                            gravity.HasJumped = true;
                        }
                    }
                    PlayerActions.AcceleratePlayerRightwards(gameTime, velocity);
                    if (!gravity.HasJumped)
                    {
                        PlayerActions.PlayerJump(gameTime, velocity, null);
                        gravity.HasJumped = true;
                    }
                    return;
                }
            }
            Console.WriteLine("Regular move");
            if (currentBlock.X < nextBlock.X) //if the block that AI wants to go to is "lower" X value AKA left of the current we need to jump left
            {                
     
                PlayerActions.AcceleratePlayerLeftwards(gameTime, velocity);
                if (!gravity.HasJumped)
                {
                    PlayerActions.PlayerJump(gameTime, velocity, null);
                    gravity.HasJumped = true;
                }

            }
            if (currentBlock.X > nextBlock.X) //if the block that AI wants to go to is "higher" X value AKA right of the current we need to jump right
            {
                PlayerActions.AcceleratePlayerRightwards(gameTime, velocity);
                if (!gravity.HasJumped) 
                {
                    PlayerActions.PlayerJump(gameTime, velocity, null);
                    gravity.HasJumped = true;
                }
            }
            if(currentBlock.X == nextBlock.X)
            {
                PlayerActions.AcceleratePlayerForwards(gameTime, velocity);
                if (!gravity.HasJumped) 
                {
                    PlayerActions.PlayerJump(gameTime, velocity, null);
                    gravity.HasJumped = true;
                }
            }
        }

        protected Point ExecuteState(GameTime gameTime, Point matrixPosition, Vector3 position, VelocityComponent aiVelocity, GravityComponent gravity, AiComponent aiComponent)
        {
            //Get the block at the AI's current position
            var currentBlock = GetBlock(matrixPosition);

            // Debug
            //WriteWorld(worldMatrix);

            // Get the "real" values of the next block (destination) and the players "real" position
            // to make the move to the next block:
            //int row = 0;

            //if (matrixPosition.Y < worldMatrix.GetLength(1) - 1)
            //    row = matrixPosition.Y + 1;
            //else
            //    row = matrixPosition.Y;

            //var decision = ChooseBlock(worldMatrix, row);
            //var destinationBlock = GetBlock(decision);

            int row = 0;

            if (matrixPosition.Y < worldMatrix.GetLength(1) - 1)
                row = matrixPosition.Y + 1;
            else
                row = matrixPosition.Y;

            if (targetBlockTile.Y <= matrixPosition.Y)
            {
                targetBlockTile = ChooseBlock(worldMatrix, row);
                targetBlockPos = GetBlock(targetBlockTile);
            }

            // Execute the move to the next block
            ExecuteMove(gameTime, currentBlock, targetBlockPos, position, aiVelocity, gravity, aiComponent);

            // We look to see if the player is in the same block in the "real" world as
            // in the matrix. if he is, we "wait" until the move is completed and return the same
            // position, or we make the move.
            // TODO: Need to fix
            if (currentBlock.Z + 50 <= position.Z)
                return targetBlockTile;
            else
                return matrixPosition;
        }

        public static void WriteRow(int[] row)
        {
            Debug.Write("Row: {");
            for (int i = 0; i < row.Length; i++)
            {
                Debug.Write(row[i]);
            }
            Debug.WriteLine("}");
        }

        public static void WriteWorld(int[,] worldMatrix)
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
