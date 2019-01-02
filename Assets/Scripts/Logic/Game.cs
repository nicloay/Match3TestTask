using System;
using System.Collections.Generic;
using Logic;
using Logic.Actions;
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
        private readonly MatchesDestroyer _destroyer;
        private readonly GravityPhysics _gravity;
        
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
            _destroyer = new MatchesDestroyer(_matcher);
            _gravity = new GravityPhysics();
            FillEmptyGrid();
        }


        public bool IsMovePossible(Vector2Int from, Vector2Int to)
        {
            Grid.Swap(from, to);
            bool result = _matcher.ContainsMatchAt(from) || _matcher.ContainsMatchAt(to);
            Grid.Swap(from, to);
            return result;
        }

        
        /// <summary>
        /// Make swap and get required actions
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<SingleChangeActions> MakeMove(Vector2Int from, Vector2Int to)
        {
            Assert.IsTrue(IsMovePossible(from, to), " Wrong move");            
            Grid.Swap(from, to);
            List<SingleChangeActions> result = new List<SingleChangeActions>();
            
            while (_matcher.GetMatches().Length > 0)
            {                
                //destroy
                List<DestroyAction> destroyActions = _destroyer.Apply(Grid);
                //gravity
                List<DiceMovement> movement = _gravity.Apply(Grid);            
                //spawn
                List<SpawnDiceAction> spawns = _spawner.Apply(Grid);
                
                result.Add(new SingleChangeActions(destroyActions, spawns, movement));
            }

            return result;
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