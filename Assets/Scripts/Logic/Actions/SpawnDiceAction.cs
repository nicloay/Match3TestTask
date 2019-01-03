using Logic.Actions;
using UnityEngine;

namespace Logic
{
        
    public struct SpawnDiceAction
    {
        public int DiceId;
        public Vector2Int Destination;

        public SpawnDiceAction(int diceId, Vector2Int destination)
        {
            DiceId = diceId;
            Destination = destination;
        }
    }
}