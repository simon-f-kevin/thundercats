using Microsoft.Xna.Framework;
using thundercats.GameStates;

namespace thundercats.Worldgen
{
    internal interface IWorldgenEntityDef
    {
        int Weight { get; }
        float SelectionValue { get; set; }

        void RunWorldGenEntityCreator(GameManager gameManager, Vector3 position);
    }
}
