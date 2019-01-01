using System;
using Logic.Physics;
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
        private readonly Spawner _spawner;
        
        private IRandomDiceGenerator _randomDiceGenerator;
        
        
        public Game(int columnNumber, int rowNumber, int colorNumber)
        {        
            ColorNumber = colorNumber;
            if (ColorNumber < 4)
            {
                throw new LevelDesignProblemException("minimum number of dices must be 4");
            }
            ColumnNumber = columnNumber;
            RowNumber = rowNumber;

            Assert.IsTrue(ColumnNumber > 0 && RowNumber > 0);
            Assert.IsTrue(ColorNumber >= 3 || RowNumber >= 3);
            
                        
            Grid = Grid.CreateWithSize(ColumnNumber, RowNumber);
            _matcher = new LineMatcher(Grid);
            _randomDiceGenerator = new SimpleRandomDiceGenerator(ColorNumber);
            _spawner = new Spawner(_randomDiceGenerator, _matcher);
            FillEmptyGrid();
        }

        
        /// <summary>
        /// Fill grid with random gems
        /// </summary>
        /// <returns>Number of attempts performed to generate level</returns>
        /// <exception cref="LevelDesignProblemException"></exception>
        private void FillEmptyGrid()
        {
            int safeCounter = 20000;
            do
            {    
                Grid.ClearWholeGrid();
                if (safeCounter-- <=0)
                {
                    Debug.LogError("problem with loop here");
                    throw new Exception();
                }                
                _spawner.Apply(Grid);
            } while (_matcher.GetHints().Length == 0);
        }
    }
}