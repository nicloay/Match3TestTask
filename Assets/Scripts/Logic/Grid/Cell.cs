using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Grid
{
    public struct Cell
    {
        
        public int DiceType { get; private set; }
        
        public bool IsEmpty
        {
            get { return DiceType < 0; }
        }
                
        public void Clear()
        {
            SetDice(-1);            
        }

        public void SetDice(int dice)
        {    
            Assert.IsTrue(dice >= 0 || dice == -1);
            DiceType = dice;                     
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