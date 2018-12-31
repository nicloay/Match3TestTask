using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public interface IMatchFinder
    {
        Match[] GetMatches();
        Hint[] GetHints();
        bool ContainsMatchAt(Vector2Int position);
    }
}