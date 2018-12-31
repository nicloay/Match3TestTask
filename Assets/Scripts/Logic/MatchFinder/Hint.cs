using Match3.Utils;
using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public class Hint : Tuple<Vector2Int, Vector2Int>
    {
        internal Hint(Vector2Int first, Vector2Int second) : base(first, second)
        {
        }
    }
}