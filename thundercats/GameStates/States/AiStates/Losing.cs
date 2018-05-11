using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using thundercats.Systems;
using thundercats.Actions;
using Game_Engine.Components;
using thundercats.Service;
using Game_Engine.Entities;
using System.Collections.Concurrent;
using thundercats.Components;
using System.Diagnostics;

namespace thundercats.GameStates.States.AiStates
{
    public class Losing : AiState, IAiState
    {
        public Losing() : base(new Random()) { }
        public void Update(GameTime gameTime, Point matrixPosition, Vector3 Position)
        {
            worldMatrix = GameService.Instance().GameWorld;
            worldEntityMatrix = GameService.Instance().EntityGameWorld;
            ExecuteState(matrixPosition, Position);
        }

        protected override Point ChooseBlock(int[] row, int RowIndex)
        {
            int currentChoice = 0;
            // Choose the right block on that row
            for (int i = 0; i < row.Length; i++)
            {
                // Logic should be here to choose column/block
                if (row[i] == 1) currentChoice = i; // return new Point(RowIndex, i); 
                /*skulle kunna ha if( [i] == 1 || [i] != 0)??för att få en block o gå till,sen kollar vi specifikt värde */
                if (row[i] == 2)
                {
                    currentChoice = i;
                    break;
                }
                if (row[i] == 3)
                {
                    currentChoice = i;
                    break;
                }
                if (row[i] == 4)
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
