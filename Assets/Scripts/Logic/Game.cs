using Logic.RNG;
using Match3.Logic.MatchFinder;
using UnityEngine;
using Grid = Logic.Grid.Grid;

namespace Match3.Logic
{
    public class Game
    {
        public const int InitialSpawnAttemptNumber = 1000;
        public readonly int ColumnNumber;
        public readonly int RowNumber;
        public readonly int ColorNumber;


        public readonly int InitialNumberOfAttemptToFillGrid;
        public readonly Grid Grid;
        private readonly IMatchFinder _matcher;
        
        
        private IRandomDiceGenerator _randomDiceGenerator;
        
        
        public Game(int columnNumber, int rowNumber, int colorNumber)
        {                        
            ColumnNumber = columnNumber;
            RowNumber = rowNumber;
            ColorNumber = colorNumber;            
            Grid = Grid.CreateWithSize(ColumnNumber, RowNumber);
            _matcher = new LineMatcher(Grid);
            _randomDiceGenerator = new SimpleRandomDiceGenerator(ColorNumber);
            InitialNumberOfAttemptToFillGrid = FillEmptyCells();
        }

        
        /// <summary>
        /// Fill grid with random gems
        /// </summary>
        /// <returns>Number of attempts performed to generate level</returns>
        /// <exception cref="LevelDesignProblemException"></exception>
        private int FillEmptyCells()
        {
            int attemp = 0;
            do
            {
                Grid.ClearWholeGridAndCommit();
                if (++attemp > InitialSpawnAttemptNumber)
                {
                    throw new LevelDesignProblemException(string.Format(
                        "Performed {0} attempts to generate level with no luck. Try to increase number of dices, change board size or use different RNG",
                        attemp));
                }
                Grid.FillEmptyCellsWithDices(_randomDiceGenerator);                
            } while (_matcher.GetMatches().Length > 0 || _matcher.GetHints().Length == 0);
            return attemp;
        }
    }
}