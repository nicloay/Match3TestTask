using System.Collections.Generic;
using Logic.Actions;
using Match3.Utils;
using UnityEngine;

namespace Logic.Physics
{
    public class GravityPhysics : IPhysics
    {
        public List<IDiceAction> Apply(Grid.Grid grid)
        {
            List<IDiceAction> movements = new List<IDiceAction>();
            for (int columnId = 0; columnId < grid.ColumnNumber; columnId++)
            {
                for (int rowId = 0; rowId < grid.RowNumber; rowId++)
                {
                    if (grid[columnId, rowId].IsEmpty)
                    {
                        Vector2Int upperPosition;       
                        Vector2Int bottomPosition = new Vector2Int(columnId,  rowId);
                        if (HasUpperNonEmptyPosition(grid, bottomPosition, out upperPosition))
                        {
                            grid.Swap(upperPosition, bottomPosition);
                            movements.Add(new DiceMovement(upperPosition, bottomPosition));                            
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return movements;
        }

        // search upper cells for non empty cell
        bool HasUpperNonEmptyPosition(Grid.Grid grid, Vector2Int startPosition, out Vector2Int position)
        {
            position = startPosition;
            for (int rowId = startPosition.y + 1; rowId < grid.RowNumber; rowId++)
            {
                position.y = rowId;
                if (!grid[position].IsEmpty)
                {
                    return true;
                }
            }            
            return false;
        }
                          
    }
}