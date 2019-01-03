using System;
using System.Runtime.Remoting;
using Match3.Utils;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace Logic.RNG
{
    
    public class SimpleRandomDiceGenerator : IRandomDiceGenerator
    {
        const int ShuffleStackNumber = 20;
        
        private int[] _shuffledStack;
        private Random _random;
        private int _currentId;
        
        public SimpleRandomDiceGenerator(int colorNumber, int seed)
        {        
            Debug.Log("Start rng with seed ="+seed);
            Assert.IsTrue(colorNumber > 1);
            
            _random = new Random(seed);
            _shuffledStack = ArrayUtil.GenerateSequencedArray(colorNumber, ShuffleStackNumber); 
            ShuffleStack();
        }

        public SimpleRandomDiceGenerator(int colorNumber) : this(colorNumber, UnityEngine.Random.Range(int.MinValue, int.MaxValue))
        {            
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