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
            Assert.IsTrue(_grid.ColumnNumber > 1 || _grid.RowNumber > 1);
        }
        
        
        public Match[] GetMatches()
        {
            List<Vector2Int[]> sequences = GetSequences(MinDiceNumberToMatch);
            Match[] result = new Match[sequences.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Match(sequences[i]);
            }            
            return result;
        }

        public Hint[] GetHints()
        {            
            List<Hint> result = new List<Hint>();
            Vector2Int mainOffset = new Vector2Int();
            Vector2Int spareOffset = new Vector2Int();
            Vector2Int currentPosition, previousPosition;
            //iterate over all axis and check for hints
            for (int mainAxis = 0; mainAxis < 2; mainAxis++)
            {
                int spareAxis = Mathf.Abs(mainAxis - 1);                
                mainOffset[mainAxis] = 1;                
                spareOffset[spareAxis] = 1;
                for (int y = 0; y < _grid.Size[mainAxis]; y++)
                {                                        
                    for (int previousX = 0, currentX = 1; currentX < _grid.Size[spareAxis]; previousX = currentX++) //current and previousX just for convenince, on second iteration it will contains Y coordinates
                    {
                        currentPosition = GetCombinedPosition(mainAxis, y, spareAxis, currentX);
                        previousPosition = GetCombinedPosition(mainAxis, y, spareAxis, previousX);
                        if (_grid[currentPosition].DiceType == _grid[previousPosition].DiceType)
                        {
                            continue;
                        }
                        _grid.Swap(currentPosition, previousPosition);
                        if (ContainsMatchAt(currentPosition) || ContainsMatchAt(previousPosition))
                        {
                            result.Add(new Hint(previousPosition, currentPosition));
                        }
                        _grid.Swap(currentPosition, previousPosition); //swap dices back
                    }                
                }
            }

            return result.ToArray();
        }

        public bool ContainsMatchAt(Vector2Int position)
        {
            // need to scan left then right, and then repeat for another axis
            Vector2Int offset;
            int matchLength;
            for (int axis = 0; axis < 2; axis++)
            {
                offset = new Vector2Int();
                offset[axis] = 1;                
                matchLength = 1;
                // scan right
                for (int i = 1; i < MinDiceNumberToMatch; i++)
                {
                    Vector2Int neighbourPosition = position + offset * i;                    
                    if (!_grid.IsCellExists(neighbourPosition) 
                        || _grid[neighbourPosition].DiceType != _grid[position].DiceType)
                    {
                        break;
                    }
                    matchLength++;                    
                }
                // scan left
                for (int i = 1; i < MinDiceNumberToMatch; i++)
                {
                    Vector2Int neighbourPosition = position - offset * i;                    
                    if (!_grid.IsCellExists(neighbourPosition) 
                        || _grid[neighbourPosition].DiceType != _grid[position].DiceType)
                    {
                        break;
                    }
                    matchLength++;                    
                }

                if (matchLength >= MinDiceNumberToMatch)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Vector2Int[]> GetSequences(int minSequenceLen)
        {
            Vector2Int current = new Vector2Int();
            Vector2Int previous = new Vector2Int();            
            List<Vector2Int> matchPositions = new List<Vector2Int>();
            List<Vector2Int[]> result = new List<Vector2Int[]>();
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
                            if (matchPositions.Count >= minSequenceLen)
                            {
                                result.Add(matchPositions.ToArray());
                            } 
                            matchPositions.Clear();
                            matchPositions.Add(current);
                        }
                    }

                    if (matchPositions.Count >= minSequenceLen)
                    {
                        result.Add(matchPositions.ToArray());
                    }
                }
            }
            return result; 
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