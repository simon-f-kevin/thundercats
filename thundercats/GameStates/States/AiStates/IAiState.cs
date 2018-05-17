using Game_Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.GameStates.States.AiStates
{
   public interface IAiState 
    {

        void Update(GameTime gameTime, ref Point matrixPosition, Vector3 position,VelocityComponent aiVelocity);
    }
}
