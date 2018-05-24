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
        public Winning() : base(new Random()){}

        public void Update(GameTime gameTime, ref Point matrixPosition,Vector3 Position,VelocityComponent aiVelocity, GravityComponent gravity)
        {
            worldMatrix = GameService.Instance.GameWorld;
            worldEntityMatrix = GameService.Instance.EntityGameWorld;
            matrixPosition = ExecuteState(gameTime, matrixPosition, Position,aiVelocity, gravity);
        }

        protected override Point ChooseBlock(int[,] world, int row)
        {
            int column;
            bool found = false;

            // Choose the right block on that row
            for(column = 0; column < world.GetLength(0); column++)
            {
                // Logic should be here to choose column/block
                if(world[column, row] == 1)
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                return ChooseBlock(world, row + 1);
            }
            return new Point(column, row);
            // Index of the next block the ai is moving to;
        }
    }
}
