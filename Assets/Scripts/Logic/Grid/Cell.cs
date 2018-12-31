using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Grid
{
    public struct Cell
    {
        public bool IsDirty { get; private set; }
        public int DiceType { get; private set; }

        private int previousValue;
        
        public bool IsEmpty
        {
            get { return DiceType < 0; }
        }


                
        public void Clear()
        {
            SavePreivouValue();
            IsDirty = true;
            DiceType = -1;            
        }

        public void SetDice(int dice)
        {
            if (IsDirty)
            {                
                Debug.LogWarning("you are changin value on dirty cell");
            }
            SavePreivouValue();
            Assert.IsTrue(dice >= 0);
            DiceType = dice;
            IsDirty = true;            
        }

        
        /// <summary>
        /// Call this method when you want to remove dirty flag and validate content for consistency
        /// </summary>
        public void Commit()
        {
            IsDirty = false;            
        }


        public void RollBackChanges()
        {
            if (IsDirty)
            {
                DiceType = previousValue;
                IsDirty = false;
            }
        }
        
        private void SavePreivouValue()
        {
            if (!IsDirty)
            {
                previousValue = DiceType;                
            }
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