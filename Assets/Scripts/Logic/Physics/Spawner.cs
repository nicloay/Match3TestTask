using System.Collections.Generic;
using Logic.Actions;
using Logic.RNG;
using Match3.Logic;
using Match3.Logic.MatchFinder;

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
            grid.ForEachEmptyCell(position =>
            {
                int safeCounter = 2000;
                do
                {
                    if (safeCounter-- <= 0)
                    {
                        throw new LevelDesignProblemException("can't spawn dice without matching with neigbhours");
                    }
                    grid.SetDiceToCell(position, _randomDiceGenerator.GetNext());
                } while (_matcher.ContainsMatchAt(position));
                result.Add(new SpawnDiceAction(position));
            });
            return result;
        }
    }
}