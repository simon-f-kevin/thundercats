using System.Reflection;

namespace thundercats.Worldgen
{
    internal abstract class WorldgenEntityDef
    {
        public delegate void CreateWorldgenEntity(params object[] args);
        private MethodInfo methodInfo;

        public int Weight { get; }
        public float SelectionValue { get; set; }

        public WorldgenEntityDef(int weight)
        {
            Weight = weight;
            SelectionValue = 0f;
            methodInfo = null;
        }

        public void SetEntityCreationMethodInfo(MethodInfo entityCreationMethodInfo)
        {
            methodInfo = entityCreationMethodInfo;
        }

        public CreateWorldgenEntity RunWorldGenEntityCreator()
        {
            if(methodInfo == null){
                return null;
            }
            return args => methodInfo.Invoke(null, args);
        }
    }
}
