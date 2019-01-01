using Match3.Logic;
using NUnit.Framework;
using UnityEngine;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TestGame
    {
        [Test]
        [Repeat(10)]
        public void GameInitializationStressTest()
        {
            int columnNumber = Random.Range(5, 16);
            int rowNumber = Random.Range(5, 16);
            int colorNumber = Random.Range(3, 8);
            Game game = new Game(columnNumber, rowNumber, colorNumber);  
            Assert.Pass();
        }

        [Test]
        public void InitialGameContainsDices()
        {
            Game game = new Game(5,5,5);            
            game.Grid.ForEachCellReadOnly(cell =>
            {
                if (cell.IsEmpty)
                {
                    Assert.Fail("one of the cell is empty");
                }                
            });                        
        }

        [Test]
        
        public void ThrowLevelDesignExceptionBecauseOfColors()
        {            
            Assert.That(() => new Game(5,5,1), Throws.TypeOf<LevelDesignProblemException>());    
        }
    }
}