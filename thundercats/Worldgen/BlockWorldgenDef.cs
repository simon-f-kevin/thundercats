using System;
using System.Reflection;

namespace thundercats.Worldgen
{
    internal class BlockWorldgenDef : WorldgenEntityDef
    {
        public BlockWorldgenDef(int weight) : base(3)
        {
            MethodInfo methodInfo = GetType().GetMethod("NewBlock");
            SetEntityCreationMethodInfo(methodInfo);
        }
    }
}
