using System.Collections.Generic;
using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public struct Match
    {
        public readonly Vector2Int[] CellPositions;

        public Match(List<Vector2Int> positions)
        {
            CellPositions = positions.ToArray();
        }
    }
}