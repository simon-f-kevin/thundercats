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
            worldMatrix = GameService.Instance().GameWorld;
            worldEntityMatrix = GameService.Instance().EntityGameWorld;
            matrixPosition = ExecuteState(matrixPosition, Position,aiVelocity, gravity);
            Console.WriteLine($"player-Pos:{matrixPosition}");
        }

        protected override Point ChooseBlock(int[] row, int RowIndex)
        {
            int currentChoice = 0;
            // Choose the right block on that row
            for (int i = 0; i < row.Length; i++)
            {
                // Logic should be here to choose column/block
                if (row[i] != 0)/* this should be the only thing we need to do? cuz we just want to survive*/
                {
                    currentChoice = i;
                    break;
                }
            }
            return new Point(currentChoice, RowIndex);
            // Index of the next block the ai is moving to;
        }
    }
}
