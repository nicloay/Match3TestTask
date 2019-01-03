using System.Collections.Generic;
using Logic.Physics;

namespace Logic.Actions
{
    public class DestroySpawnGravityAction 
    {
        public List<DestroyAction> Destroys;
        public List<SpawnDiceAction> Spawns;
        public List<DiceMovement> Moves;

        public DestroySpawnGravityAction(List<DestroyAction> destroys, List<SpawnDiceAction> spawns, List<DiceMovement> moves)
        {
            Destroys = destroys;
            Spawns = spawns;
            Moves = moves;
        }
    }
    
}