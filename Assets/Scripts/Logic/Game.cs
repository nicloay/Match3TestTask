using System;
using Logic.RNG;
using Match3.Logic.MatchFinder;
using UnityEngine;
using UnityEngine.Assertions;
using Grid = Logic.Grid.Grid;

namespace Match3.Logic
{
    public class Game
    {
        public const int InitialSpawnAttemptNumber = 1000;
        public readonly int ColumnNumber;
        public readonly int RowNumber;
        public readonly int ColorNumber;

        public readonly Grid Grid;
        private readonly IMatchFinder _matcher;
        
        
        private IRandomDiceGenerator _randomDiceGenerator;
        
        
        public Game(int columnNumber, int rowNumber, int colorNumber)
        {                                    
            ColumnNumber = columnNumber;
            if (ColorNumber < 4)
            {
                throw new LevelDesignProblemException("minimum number of dices must be 4");
            }
            RowNumber = rowNumber;

            Assert.IsTrue(ColumnNumber > 0 && RowNumber > 0);
            Assert.IsTrue(ColorNumber >= 3 || RowNumber >= 3);
            
            ColorNumber = colorNumber;            
            Grid = Grid.CreateWithSize(ColumnNumber, RowNumber);
            _matcher = new LineMatcher(Grid);
            _randomDiceGenerator = new SimpleRandomDiceGenerator(ColorNumber);
            FillEmptyCells();
        }

        
        /// <summary>
        /// Fill grid with random gems
        /// </summary>
        /// <returns>Number of attempts performed to generate level</returns>
        /// <exception cref="LevelDesignProblemException"></exception>
        private void FillEmptyCells()
        {
            Grid.ForEachEmptyCell(position => FillCellWithUnmatchableDice(position));
            Grid.Commit();
        }

        private void FillCellWithUnmatchableDice(Vector2Int cellPosition)
        {
            int safeCounter = 20000;
            do
            {
                Grid.SetDiceToCell(cellPosition, _randomDiceGenerator.GetNext());
                if (safeCounter-- <=0)
                {
                    Debug.LogError("problem with loop here");
                    throw new Exception();
                }
            } while (_matcher.ContainsMatchAt(cellPosition));
        }
    }
}