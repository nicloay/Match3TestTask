using JetBrains.Annotations;
using Logic.Grid;
using NUnit.Framework;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TtestGrid
    {
        
        [Test]
        public void TestConstructorWithDicesSquare()
        {
            Grid grid = Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2,3},
                {4,5,6},
                {7,8,9}
            });
            Assert.That(grid[0,0].DiceType, Is.EqualTo(7));
            Assert.That(grid[0,2].DiceType, Is.EqualTo(1));
            Assert.That(grid[1,1].DiceType, Is.EqualTo(5));
            Assert.That(grid[2,0].DiceType, Is.EqualTo(9));
            Assert.That(grid[2,2].DiceType, Is.EqualTo(3));                        
        }
        
        [Test]
        public void TestConstructorWithDicesHorizontal()
        {
            Grid grid = Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2,3},
                {4,5,6}                
            });
            Assert.That(grid[0,0].DiceType, Is.EqualTo(4));
            Assert.That(grid[0,1].DiceType, Is.EqualTo(1));            
            Assert.That(grid[2,0].DiceType, Is.EqualTo(6));
            Assert.That(grid[2,1].DiceType, Is.EqualTo(3));                        
        }
        
        [Test]
        public void TestConstructorWithDicesVertical()
        {
            Grid grid =  Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2},
                {3,4},
                {5,6}
            });
            Assert.That(grid[0,0].DiceType, Is.EqualTo(5));
            Assert.That(grid[0,2].DiceType, Is.EqualTo(1));            
            Assert.That(grid[1,0].DiceType, Is.EqualTo(6));
            Assert.That(grid[1,2].DiceType, Is.EqualTo(2));                        
        }
    }
}