using Match3.Utils;
using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public class Hint : Tuple<Vector2Int, Vector2Int>
    {
        public Hint(Vector2Int first, Vector2Int second) : base(first, second)
        {
        }

        public override bool Equals(object other)
        {
            if (!(other is Hint))
            {
                return false;
            }

            Hint otherHint = (Hint) other;
            return First == otherHint.First && Second == otherHint.Second;
        }

        public override string ToString()
        {
            return string.Format("[{0}] -> [{1}]", First.ToString(), Second.ToString());
        }
    }
}