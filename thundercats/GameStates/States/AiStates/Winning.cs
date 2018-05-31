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
        public Winning() : base(new Random(DateTime.Now.Second.ToString().GetHashCode())){}

        public void Update(GameTime gameTime, ref Point matrixPosition,Vector3 Position,VelocityComponent aiVelocity, GravityComponent gravity, AiComponent aiComponent)
        {
            worldMatrix = GameService.Instance.GameWorld;
            worldEntityMatrix = GameService.Instance.EntityGameWorld;
            matrixPosition = ExecuteState(gameTime, matrixPosition, Position,aiVelocity, gravity, aiComponent);
        }

        protected override Point ChooseBlock(int[,] world, int row)
        {
            int column;
            int chosenColumn = 0;
            bool found = false;
            int randomChoice = Random.Next(0, 11);
            // Choose a block on the row
            for(column = 0; column < world.GetLength(0); column++)
            {
                switch (worldMatrix[column, row])
                {
                case(1): case(2):
                    found = true;
                    chosenColumn = column;
                    break;
                default:
                    break;
                }

                if(found)
                {
                    break;
                }
            }
            if(!found && row < world.GetLength(1)) //If there is no block on the row attempt to find one on the next one
            {
                return ChooseBlock(world, row + 1);
            }
            else if(!found)
            {
                return new Point(-1, -1);
            }
           
            return new Point(chosenColumn, row); // Index of the next block the ai is moving to;
        }
    }
}
