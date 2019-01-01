using Logic.Actions;
using UnityEngine;

namespace Logic.Physics
{
    public struct DiceMovement : IDiceAction
    {        
        public Vector2Int Source;
        public Vector2Int Destination;

        public DiceMovement(Vector2Int source, Vector2Int destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}