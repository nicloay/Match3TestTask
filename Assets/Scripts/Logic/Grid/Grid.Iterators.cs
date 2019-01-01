using System;
using UnityEngine;

namespace Logic.Grid
{
    public partial class Grid
    {
        private delegate bool CellCondition(Cell cell);

        
        public void ForEachCellReadOnly(Action<Cell> cellAction)
        {
            ForEachCellId((x, y) => cellAction(this[x,y]));
        }
        
        
        #region Vector2Int position
        public delegate void CellActionPosition(Vector2Int position);

        public void ForEachEmptyCell(CellActionPosition cellAction)
        {
            ForEachCell(cellAction, cell => cell.IsEmpty);			
        }
		
        public void ForEachNonEmptyCell(CellActionPosition cellAction)
        {
            ForEachCell(cellAction, cell => !cell.IsEmpty);			
        }
        
        private void ForEachCellId(CellActionPosition cellAction)
        {            
            for (int y = 0; y < RowNumber; y++)
            {
                for (int x = 0; x < ColumnNumber; x++)
                {
                    cellAction(new Vector2Int(x, y));
                }
            }
        }

        public void ForEachCell(CellActionPosition cellAction)
        {
            ForEachCellId((x, y) => cellAction(new Vector2Int(x, y)));
        }
        
        private void ForEachCell(CellActionPosition cellAction, CellCondition cellCondition)
        {
            ForEachCellId((x, y) =>
            {
                if (cellCondition(Cells[x, y]))
                {
                    cellAction(new Vector2Int(x, y));
                }
            });            
        }
        
        #endregion




        #region XYPosition
        
        public delegate void CellAction(int cellX, int cellY);
        
        public void ForEachEmptyCell(CellAction cellAction)
        {
            ForEachCell(cellAction, cell => cell.IsEmpty);			
        }
		
        public void ForEachNonEmptyCell(CellAction cellAction)
        {
            ForEachCell(cellAction, cell => !cell.IsEmpty);			
        }
        
        private void ForEachCellId(CellAction cellAction)
        {            
            for (int y = 0; y < RowNumber; y++)
            {
                for (int x = 0; x < ColumnNumber; x++)
                {
                    cellAction(x, y);
                }
            }
        }

        public void ForEachCell(CellAction cellAction)
        {
            ForEachCellId((x, y) => cellAction(x, y));            
        }
        
        private void ForEachCell(CellAction cellAction, CellCondition cellCondition)
        {
            ForEachCellId((x, y) =>
            {
                if (cellCondition(Cells[x, y]))
                {
                    cellAction(x, y);
                }
            });            
        }
        #endregion
    }
}