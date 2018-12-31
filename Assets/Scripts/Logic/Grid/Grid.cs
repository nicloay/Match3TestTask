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
			int[,] nativeFormat = ArrayUtil.ConvertArrayFromHumanReadebleFormatToNative(diceTypes);
			return CreateWithData(nativeFormat);
		}

		public static Grid CreateWithData(int[,] diceTypes)
		{
			Grid result = new Grid(diceTypes.GetLength(0), diceTypes.GetLength(1));
			result.ForEachCellId((x, y) => result.Cells[x,y].SetDice(diceTypes[x, y]));			
			result.Commit();
			return result;
		}

		public void SetCellDirtyValue(Vector2Int position, int value)
		{
			Cells[position.x, position.y].SetDice(value);
		}
				
		public void Commit()
		{
			ForEachCellId((x,y) => Cells[x,y].Commit());			
		}
		
		private void ClearWholeGrid()
		{
			ForEachCell(cell => cell.Clear());	
			Commit();
		}

		public void Swap(Vector2Int from, Vector2Int to)
		{
			int currentFromDice = Cells[from.x, from.y].DiceType;
			int currentToDice = Cells[to.x, to.y].DiceType;
			
			SetCellDirtyValue(from, currentToDice);
			SetCellDirtyValue(to, currentFromDice);
			
		}

		public void RollBackChanges(params Vector2Int[] positions)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				Cells[positions[i].x, positions[i].y].RollBackChanges();
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