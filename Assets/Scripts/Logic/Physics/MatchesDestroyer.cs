using System.Collections.Generic;
using Logic.Actions;
using Match3.Logic.MatchFinder;
using UnityEngine;

namespace Logic.Physics
{
    public class MatchesDestroyer 
    {
        private IMatchFinder _matcher;
        public MatchesDestroyer(IMatchFinder matcher)
        {
            _matcher = matcher;
        }
        
        public List<DestroyAction> Apply(Grid.Grid grid)
        {
            List<DestroyAction> result = new List<DestroyAction>();
            HashSet<Vector2Int> destroyPositions = new HashSet<Vector2Int>();
            Match[] matches = _matcher.GetMatches();

            for (int i = 0; i < matches.Length; i++)
            {
                for (int j = 0; j < matches[i].CellPositions.Length; j++)
                {
                    Vector2Int cellPosition = matches[i].CellPositions[j];                    
                    if (!destroyPositions.Contains(cellPosition))
                    {
                        grid.ClearCells(cellPosition);
                        destroyPositions.Add(cellPosition);
                        result.Add(new DestroyAction(cellPosition));
                    }
                }
            }
            return result;
        }
    }
}