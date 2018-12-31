using System.Collections.Generic;
using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public struct Match
    {
        public readonly Vector2Int[] CellPositions;

        public Match(Vector2Int[] positions)
        {
            CellPositions = positions;
        }
    }
}