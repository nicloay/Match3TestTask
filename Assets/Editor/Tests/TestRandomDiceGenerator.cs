using System.Collections.Generic;
using JetBrains.Annotations;
using Logic.RNG;
using NUnit.Framework;
using UnityEngine;

namespace Match3.Editor.Tests
{
    public class TestRandomDiceGenerator
    {
        [Test]
        [Repeat(200)]
        public void TestContainsAllValues()
        {
            int maxIteration = 200;
            int diceNumber = Random.Range(2, 10);
            SimpleRandomDiceGenerator rng = new SimpleRandomDiceGenerator(diceNumber);
            HashSet<int> usedIds = new HashSet<int>();
            for (int i = 0; i < maxIteration; i++)
            {
                int newId = rng.GetNext();
                if (!usedIds.Contains(newId))
                {
                    usedIds.Add(newId);
                    if (usedIds.Count == diceNumber)
                    {
                        Assert.Pass();
                    }
                }
            }            
            Assert.Fail(string.Format("Used dices {0} original value {1}", usedIds.Count, diceNumber));
        }
    }
}