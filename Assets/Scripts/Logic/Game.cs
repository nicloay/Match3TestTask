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
        public readonly int DiceNumber;

        public readonly Grid Grid;
        private readonly IMatchFinder _matcher;
        private readonly Spawner _spawner;
        private readonly MatchesDestroyer _destroyer;
        private readonly GravityPhysics _gravity;
        
        private IRandomDiceGenerator _randomDiceGenerator;

        public Game(Vector2Int boardSize, int diceNumber) : this(boardSize.x, boardSize.y, diceNumber)
        {
        }

        public Game(Vector2Int boardSize, int diceNumber, int seed) : this(boardSize.x, boardSize.y, diceNumber, seed)
        {
        }
        
        public Game(int columnNumber, int rowNumber, int diceNumber) : this(columnNumber, rowNumber, diceNumber, null)
        {            
        }

        private Game(int columnNumber, int rowNumber, int diceNumber, int? seed)
        {        
            DiceNumber = diceNumber;
            if (DiceNumber < 4)
            {
                throw new LevelDesignProblemException("minimum number of dices must be 4");
            }
            ColumnNumber = columnNumber;
            RowNumber = rowNumber;

            Assert.IsTrue(ColumnNumber > 0 && RowNumber > 0);
            Assert.IsTrue(DiceNumber >= 3 || RowNumber >= 3);
            
                        
            Grid = Grid.CreateWithSize(ColumnNumber, RowNumber);
            _matcher = new LineMatcher(Grid);
            _randomDiceGenerator = seed == null
                ? new SimpleRandomDiceGenerator(DiceNumber)
                : new SimpleRandomDiceGenerator(DiceNumber, seed.Value);
            _spawner = new Spawner(_randomDiceGenerator, _matcher);
            _destroyer = new MatchesDestroyer(_matcher);
            _gravity = new GravityPhysics();            
        }

        public bool IsSwapPossible(Vector2Int from, Vector2Int to)
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
        public List<DestroySpawnGravityAction> MakeSwap(Vector2Int from, Vector2Int to)
        {
            Assert.IsTrue(IsSwapPossible(from, to), " Wrong move");            
            Grid.Swap(from, to);
            List<DestroySpawnGravityAction> result = new List<DestroySpawnGravityAction>();
            
            while (_matcher.GetMatches().Length > 0)
            {                
                //destroy
                List<DestroyAction> destroyActions = _destroyer.Apply(Grid);
                //gravity
                List<DiceMovement> movement = _gravity.Apply(Grid);            
                //spawn
                List<SpawnDiceAction> spawns = _spawner.Apply(Grid);
                
                result.Add(new DestroySpawnGravityAction(destroyActions, spawns, movement));
            }

            return result;
        }
        
        
        /// <summary>
        /// Fill grid with random gems
        /// </summary>
        /// <returns>Number of attempts performed to generate level</returns>
        /// <exception cref="LevelDesignProblemException"></exception>
        public List<SpawnDiceAction> FillEmptyGrid()
        {
            List<SpawnDiceAction> result; 
            int safeCounter = 20000;
            do
            {    
                Grid.ClearWholeGrid();
                if (safeCounter-- <=0)
                {
                    Debug.LogError("problem with loop here");
                    throw new Exception();
                }                
                result = _spawner.Apply(Grid);
            } while (_matcher.GetHints().Length == 0);

            return result;
        }
    }
}