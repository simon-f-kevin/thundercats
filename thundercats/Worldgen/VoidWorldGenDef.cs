using Microsoft.Xna.Framework;
using thundercats.GameStates;

namespace thundercats.Worldgen
{
    internal class VoidWorldgenDef : IWorldgenEntityDef
    {
        public int Weight { get; }
        public float SelectionValue { get; set; }

        public VoidWorldgenDef(int weight)
        {
            Weight = weight;
            SelectionValue = 0f;
        }

        public void RunWorldGenEntityCreator(GameManager gameManager, Vector3 position)
        {
        }
    }
}