using Microsoft.Xna.Framework;
using thundercats.GameStates;
using Game_Engine.Entities;

namespace thundercats.Worldgen
{
    internal interface IWorldgenEntityDef
    {
		int Index { get; }
        int Weight { get; }
        float SelectionValue { get; set; }

        Entity RunWorldGenEntityCreator(GameManager gameManager, Vector3 position);
    }
}
