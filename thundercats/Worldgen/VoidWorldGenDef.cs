using Microsoft.Xna.Framework;
using thundercats.GameStates;
using Game_Engine.Entities;

namespace thundercats.Worldgen
{
    internal class VoidWorldgenDef : IWorldgenEntityDef
    {
		public int Index { get; }
        public int Weight { get; }
        public float SelectionValue { get; set; }

        public VoidWorldgenDef(int weight)
        {
            Weight = weight;
			Index = -1;
            SelectionValue = 0f;
        }

        public Entity RunWorldGenEntityCreator(GameManager gameManager, Vector3 position)
        {
			return null;
        }
    }
}
