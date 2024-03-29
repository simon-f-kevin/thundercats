﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.GameStates.States
{
    public interface IState
    {
        void Initialize();

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Update(GameTime gameTime);
    }
}
