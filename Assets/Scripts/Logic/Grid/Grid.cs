using Logic.RNG;
using Match3.Utils;
using UnityEditor.Experimental.Build.AssetBundle;
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
		
		private Grid(int columnNumber, int rowNumber)
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
		///     1. diceIds has inverted x and y indexes
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
		/// <param name="diceIds">Array of dice ids</param>
		public static Grid CreateWithHumanReadableData(int[,] diceIds)
		{
			int[,] nativeFormat = ArrayUtil.ConvertArrayFromHumanReadebleFormatToNative(diceIds);
			return CreateWithData(nativeFormat);
		}

		public static Grid CreateWithData(int[,] diceIds)
		{
			Grid result = new Grid(diceIds.GetLength(0), diceIds.GetLength(1));
			result.ForEachCellId((x, y) => result.Cells[x,y].SetDice(diceIds[x, y]));						
			return result;
		}

		public static Grid CreateWithSize(int columnNumber, int rowNumber)
		{
			Grid result = new Grid(columnNumber, rowNumber);
			result.ClearWholeGrid();
			return result;
		}
		
		public void SetCellDirtyValue(Vector2Int position, int value)
		{
			Cells[position.x, position.y].SetDice(value);
		}
		
				
		
		public void ClearWholeGrid()
		{
			ForEachCell((x, y) => Cells[x, y].Clear());
			
		}

		public void Swap(Vector2Int from, Vector2Int to)
		{
			int currentFromDice = Cells[from.x, from.y].DiceId;
			int currentToDice = Cells[to.x, to.y].DiceId;
			
			SetCellDirtyValue(from, currentToDice);
			SetCellDirtyValue(to, currentFromDice);
			
		}

		public void ClearCells(params Vector2Int[] positions)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				Cells[positions[i].x, positions[i].y].Clear();
			}
		}
		

		public Cell this[int columnId, int rowId]
		{
			get { return Cells[columnId, rowId]; }
		}

		public Cell this[Vector2Int position]
		{
			get { return Cells[position.x, position.y]; }
		}


		public void FillEmptyCellsWithDices(IRandomDiceGenerator generator)
		{
			ForEachEmptyCell((x,y) => Cells[x, y].SetDice(generator.GetNext()));
		}

		public void SetDiceToCell(Vector2Int position, int diceId)
		{
			Cells[position.x, position.y].SetDice(diceId);
		}
		
		public bool IsCellExists(Vector2Int position)
		{
			for (int axis = 0; axis < 2; axis++)
			{
				if (position[axis] < 0 || position[axis] >= Size[axis])
				{
					return false;
				}
			}

			return true;
		}
		
	}
}