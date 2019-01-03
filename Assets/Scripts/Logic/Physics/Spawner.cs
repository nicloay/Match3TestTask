using System.Collections.Generic;
using Logic.Actions;
using Logic.RNG;
using Match3.Logic;
using Match3.Logic.MatchFinder;
using UnityEngine;

namespace Logic.Physics
{
    /// <summary>
    /// Spawn dices to empty fields
    /// </summary>
    public class Spawner
    {
        private IRandomDiceGenerator _randomDiceGenerator;
        private IMatchFinder _matcher;
        public Spawner(IRandomDiceGenerator randomDiceGenerator, IMatchFinder matcher)
        {
            _randomDiceGenerator = randomDiceGenerator;
            _matcher = matcher;
        }
        
        public List<SpawnDiceAction> Apply(Grid.Grid grid)
        {
            List<SpawnDiceAction> result = new List<SpawnDiceAction>();
            List<Vector2Int> positions = new List<Vector2Int>();
            grid.ForEachEmptyCell(position =>
            {
                int safeCounter = 2000;
                int diceId;
                do
                {
                    if (safeCounter-- <= 0)
                    {
                        throw new LevelDesignProblemException("can't spawn dice without matching with neigbhours");
                    }

                    diceId = _randomDiceGenerator.GetNext();
                    grid.SetDiceToCell(position, diceId);
                } while (_matcher.ContainsMatchAt(position));
                result.Add(new SpawnDiceAction(diceId, position));
            });
            return result;
        }
    }
}