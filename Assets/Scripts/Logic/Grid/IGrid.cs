using System.Runtime.CompilerServices;
using UnityEngine;

namespace Logic.Grid
{
    public interface IGrid
    {
        int ColumnNumber { get; }
        int RowNumber { get; }
        Vector2Int Size { get; }
        Cell this[int x, int y] { get; }
        Cell this[Vector2Int position] { get; }
    } 
}