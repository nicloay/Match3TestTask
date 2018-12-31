using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Grid
{
	public partial class Grid : IGrid
	{
		public int ColumnNumber
		{
			get { return Size.x; }
		}
		public int RowNumber
		{
			get { return Size.y; }
		}
		public Vector2Int Size { get; private set; }

		private readonly Cell[,] Cells;
		
		public Grid(int columnNumber, int rowNumber)
		{
			Assert.IsTrue(columnNumber > 0);
			Assert.IsTrue(rowNumber > 0);
			Size = new Vector2Int(columnNumber, rowNumber);						
			Cells = new Cell[columnNumber, rowNumber];
		}

		
		/// <summary>
		/// Initialize Grid with dices content.
		///
		///    WARNING:
		///     1. diceTypes has inverted x and y indexes
		///			e.g. dice[2,5] means columnId = 2, rowId = 5
		///     2. y axis starts from the top dice[0, 0]  means
		///			that you will assign value for the TOP LEFT corner of the grid 
		///
		///    EXAMPLE:
		/// 		You can initialize this grid with following parameter
		///			{
		/// 			{1,2,3},
		///		        {4,5,6}
		///         }
		///			And on UI or in Cell array it will have the same representation where
		/// 		Cell[0,0] = 4
		/// 		Cell[2,1] = 3
		///   
		/// </summary>
		/// <param name="diceTypes">Array of dice types</param>
		public static Grid CreateWithHumanReadableData(int[,] diceTypes) 
		{
			Grid result = new Grid(diceTypes.GetLength(1), diceTypes.GetLength(0));
			result.ForEachCellId((x, y) => result.Cells[x,y].SetDice(diceTypes[result.RowNumber - y - 1, x]));			
			result.Commit();
			return result;
		}

		public static Grid CreateWithData(int[,] diceTypes)
		{
			Grid result = new Grid(diceTypes.GetLength(0), diceTypes.GetLength(1));
			result.ForEachCellId((x, y) => result.Cells[x,y].SetDice(diceTypes[x, y]));			
			result.Commit();
			return result;
		}

		public void SetCellValue(Vector2Int position, int value)
		{
			Cells[position.x, position.y].SetDice(value);
		}
				
		public void Commit()
		{
			ForEachCell(cell => cell.Commit());
		}
		
		private void ClearWholeGrid()
		{
			ForEachCell(cell => cell.Clear());			
		}

		

		public Cell this[int columnId, int rowId]
		{
			get { return Cells[columnId, rowId]; }
		}

		public Cell this[Vector2Int position]
		{
			get { return Cells[position.x, position.y]; }
		}
	}
}