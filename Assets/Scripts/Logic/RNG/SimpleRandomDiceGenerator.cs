using System;
using System.Runtime.Remoting;
using Match3.Utils;
using NUnit.Framework;

namespace Logic.RNG
{
    
    public class SimpleRandomDiceGenerator : IRandomDiceGenerator
    {
        const int ShuffleStackNumber = 20;
        
        private int[] _shuffledStack;
        private Random _random;
        private int _currentId;
        
        public SimpleRandomDiceGenerator(int colorNumber)
        {        
            Assert.IsTrue(colorNumber > 1);
            _random = new Random();
            _shuffledStack = ArrayUtil.GenerateSequencedArray(colorNumber, ShuffleStackNumber);            
        }


        private void ShuffleStack()
        {
            _currentId = 0;
            ArrayUtil.ShuffleArray(_shuffledStack, _random);
        }


        public int GetNext()
        {
            if (_currentId >= _shuffledStack.Length)
            {
                ShuffleStack();
            }
            return _shuffledStack[_currentId++];
        }
    }
}