using UnityEngine;

namespace Match3.Logic.MatchFinder
{
    public interface IMatchFinder
    {
        Match[] GetMatches();
        bool ContainsMatchAt(Vector2Int position);
    }
}