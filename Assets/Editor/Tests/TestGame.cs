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
            int colorNumber = Random.Range(4, 8);
            Game game = new Game(columnNumber, rowNumber, colorNumber);
            game.FillEmptyGrid();
            Assert.Pass();
        }

        [Test]
        public void InitialGameContainsDices()
        {
            Game game = new Game(5,5,5);
            game.FillEmptyGrid();
            game.Grid.ForEachCell(position =>
            {
                Assert.That(game.Grid[position].IsEmpty, Is.False, string.Format("cell {0} is empty ", position));
                                                
            });                        
        }

        [Test]
        
        public void ThrowLevelDesignExceptionBecauseOfColors()
        {            
            Assert.That(() =>
            {                
                return new Game(5, 5, 1).FillEmptyGrid();                
            }, Throws.TypeOf<LevelDesignProblemException>());    
        }
    }
}