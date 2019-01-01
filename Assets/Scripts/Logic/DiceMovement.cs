using UnityEngine;

namespace Logic.Physics
{
    public struct DiceMovement
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