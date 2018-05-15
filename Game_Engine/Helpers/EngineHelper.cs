using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Helpers
{
    public class EngineHelper
    {
        public Matrix WorldMatrix { get; set; }

        private static EngineHelper engineHelper;
        private EngineHelper()
        {
            WorldMatrix = Matrix.Identity;
        }
        public static EngineHelper Instance()
        {
            if (engineHelper == null)
            {

                engineHelper = new EngineHelper();
            }
            return engineHelper;
        }
    }
}
