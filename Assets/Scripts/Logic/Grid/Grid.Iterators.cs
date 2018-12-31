using System;

namespace Logic.Grid
{
    public partial class Grid
    {
        private delegate bool CellCondition(Cell cell);

        private delegate void CellAction(int cellX, int cellY);
        
        
        public void ForEachEmptyCell(Action<Cell> cellAction)
        {
            ForEachCell(cellAction, cell => cell.IsEmpty);			
        }
		
        public void ForEachNonEmptyCell(Action<Cell> cellAction)
        {
            ForEachCell(cellAction, cell => !cell.IsEmpty);			
        }

        private void ForEachCell(Action<Cell> cellAction, CellCondition cellCondition)
        {
            ForEachCellId((x, y) =>
            {
                if (cellCondition(Cells[x, y]))
                {
                    cellAction(Cells[x, y]);
                }
            });            
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

        public void ForEachCell(Action<Cell> cellAction)
        {
            ForEachCellId((x, y) => cellAction(Cells[x, y]));            
        }
    }
}