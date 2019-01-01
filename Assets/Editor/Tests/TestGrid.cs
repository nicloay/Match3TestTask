using System.Net.Configuration;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using Grid = Logic.Grid.Grid;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TestGrid
    {

        [Test]
        public void TestDefaultGridContainsEmptyCells()
        {
            Grid grid = Grid.CreateWithSize(5,5);
            
            grid.ForEachCellReadOnly(cell =>
            {
                if (!cell.IsEmpty)
                {                
                    Assert.Fail("Error, empty grid contains non empty cells");
                }
            });
            

        }
        
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

        [Test]
        public void TestSwapHorizontal()
        {
            Grid grid =  Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2},
                {3,4}               
            });
            grid.Swap(new Vector2Int(0,0),new Vector2Int(1,0) );
            Assert.That(grid[0,0].DiceType, Is.EqualTo(4));
            Assert.That(grid[1,0].DiceType, Is.EqualTo(3));
        }
        
        [Test]
        public void TestSwapVertical()
        {
            Grid grid =  Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2},
                {3,4}               
            });
            grid.Swap(new Vector2Int(0,0),new Vector2Int(0,1) );
            Assert.That(grid[0,0].DiceType, Is.EqualTo(1));
            Assert.That(grid[0,1].DiceType, Is.EqualTo(3));
        }

        [Test]
        public void TestIsNotDirtyAfterInitializationWithData()
        {
            Grid grid =  Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2},
                {3,4}               
            });
            grid.ForEachCell((x, y) =>Assert.That(grid[x,y].IsDirty, Is.False) );            
        }        
        
        [Test]
        public void TestSwapRollBack()
        {
            Grid grid =  Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2},
                {3,4}               
            });            
            grid.Swap(new Vector2Int(0,0),new Vector2Int(0,1) );
            grid.RollBackChanges(new Vector2Int(0,0),new Vector2Int(0,1));
            Assert.That(grid[0,0].DiceType, Is.EqualTo(3));
            Assert.That(grid[0,1].DiceType, Is.EqualTo(1));
        }
        
        
    }
}