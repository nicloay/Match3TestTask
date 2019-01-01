using Logic.Actions;
using UnityEngine;

namespace Logic
{
        
    public struct SpawnDiceAction : IDiceAction
    {
        public Vector2Int Destination;

        public SpawnDiceAction(Vector2Int destination)
        {
            Destination = destination;
        }
    }
}