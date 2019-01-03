using Match3.Logic.MatchFinder;
using UnityEngine;

namespace Logic.Actions
{
    public class DestroyAction
    {
        public Vector2Int CellPosition;

        public DestroyAction(Vector2Int cellPosition)
        {
            CellPosition = cellPosition;
        }
    }
}