using Logic.Grid;
using Logic.RNG;
using UnityEditor.IMGUI.Controls;

namespace Match3.Logic
{
    public class Game
    {
        public const int ColumnNumber = 9;
        public const int RowNumber = 9;
        public const int ColorNumber = 5;
        
        public readonly Grid Grid;
        private IRandomDiceGenerator _randomDiceGenerator;
        
        
        public Game()
        {
            Grid = new Grid(ColumnNumber, RowNumber);
            _randomDiceGenerator = new SimpleRandomDiceGenerator(ColorNumber);
        }

        private void FillEmptyCells()
        {
            Grid.ForEachEmptyCell(cell => cell.SetDice(_randomDiceGenerator.GetNext()));
            
        }
    }
}