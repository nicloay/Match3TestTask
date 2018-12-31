using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SocialPlatforms;
using Grid = Logic.Grid.Grid;

namespace Match3.Logic.MatchFinder
{
    public class LineMatcher : IMatchFinder
    {
        private const int MinDiceNumberToMatch = 3;
        
        private Grid _grid;
        
        public LineMatcher(Grid grid)
        {
            _grid = grid;
            Assert.IsTrue(MinDiceNumberToMatch > 0);
            Assert.IsTrue(MinDiceNumberToMatch <= grid.ColumnNumber || MinDiceNumberToMatch <= grid.RowNumber);
        }
        
        
        public Match[] GetMatches()
        {
            Vector2Int current = new Vector2Int();
            Vector2Int previous = new Vector2Int();            
            List<Vector2Int> matchPositions = new List<Vector2Int>();
            List<Match> result = new List<Match>();
            // scan column by column then row by row                        
            for (int mainAxis = 0; mainAxis < 2; mainAxis++)
            {
                int spareAxis = Mathf.Abs(mainAxis - 1);
                
                for (int mainAxisId = 0; mainAxisId < _grid.Size[mainAxis] ; mainAxisId++)
                {
                    matchPositions.Clear();                    
                    matchPositions.Add(GetCombinedPosition(mainAxis, mainAxisId, spareAxis, 0));
                    for (int spareAxisId = 1, previousSpareId = 0; spareAxisId < (_grid.Size[spareAxis]); previousSpareId = spareAxisId++)
                    {
                        current = GetCombinedPosition(mainAxis, mainAxisId, spareAxis, spareAxisId);
                        previous = GetCombinedPosition(mainAxis, mainAxisId, spareAxis, previousSpareId);
                        if (_grid[current].HasTheSameDiceWith(_grid[previous]))
                        {
                            matchPositions.Add(current);
                        }
                        else
                        {
                            if (matchPositions.Count >= MinDiceNumberToMatch)
                            {
                                result.Add(new Match(matchPositions));
                            } 
                            matchPositions.Clear();
                            matchPositions.Add(current);
                        }
                    }

                    if (matchPositions.Count >= MinDiceNumberToMatch)
                    {
                        result.Add(new Match(matchPositions));
                    }
                }
            }
            return result.ToArray();
        }

        private static Vector2Int GetCombinedPosition(int axis1, int value1, int axis2, int value2)
        {
            Vector2Int result = new Vector2Int();
            result[axis1] = value1;
            result[axis2] = value2;
            return result;
        }
    }
}