namespace Logic.Grid
{
    public struct Cell
    {
        public bool IsDirty { get; private set; }
        public int DiceType { get; private set; }
        public bool IsEmpty { get; private set; }

        
        
        public void Clear()
        {
            IsDirty = false;
            DiceType = -1;
            IsEmpty = true;
        }

        public void SetDice(int dice)
        {
            DiceType = dice;
            IsDirty = true;
            IsEmpty = false;
        }

        
        /// <summary>
        /// Call this method when you want to remove dirty flag and validate content for consistency
        /// </summary>
        public void Commit()
        {
            IsDirty = false;
            IsEmpty = DiceType >= 0;
        }
        
        public bool HasTheSameDiceWith(Cell anotherCell)
        {
            if (IsEmpty || anotherCell.IsEmpty)
            {
                return false;
            }
            return DiceType == anotherCell.DiceType;
        }
    }
}