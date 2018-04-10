﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.Menu
{
    interface IMenu
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Update(GameTime gameTime);

    }
}
