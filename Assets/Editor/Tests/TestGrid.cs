﻿using System.Net.Configuration;
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
            Assert.That(grid[0,0].DiceId, Is.EqualTo(7));
            Assert.That(grid[0,2].DiceId, Is.EqualTo(1));
            Assert.That(grid[1,1].DiceId, Is.EqualTo(5));
            Assert.That(grid[2,0].DiceId, Is.EqualTo(9));
            Assert.That(grid[2,2].DiceId, Is.EqualTo(3));                        
        }
        
        [Test]
        public void TestConstructorWithDicesHorizontal()
        {
            Grid grid = Grid.CreateWithHumanReadableData(new int[,]
            {
                {1,2,3},
                {4,5,6}                
            });
            Assert.That(grid[0,0].DiceId, Is.EqualTo(4));
            Assert.That(grid[0,1].DiceId, Is.EqualTo(1));            
            Assert.That(grid[2,0].DiceId, Is.EqualTo(6));
            Assert.That(grid[2,1].DiceId, Is.EqualTo(3));                        
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
            Assert.That(grid[0,0].DiceId, Is.EqualTo(5));
            Assert.That(grid[0,2].DiceId, Is.EqualTo(1));            
            Assert.That(grid[1,0].DiceId, Is.EqualTo(6));
            Assert.That(grid[1,2].DiceId, Is.EqualTo(2));                        
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
            Assert.That(grid[0,0].DiceId, Is.EqualTo(4));
            Assert.That(grid[1,0].DiceId, Is.EqualTo(3));
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
            Assert.That(grid[0,0].DiceId, Is.EqualTo(1));
            Assert.That(grid[0,1].DiceId, Is.EqualTo(3));
        }

               
        [Test]
        public void ClearTest()
        {
            Grid grid = Grid.CreateWithHumanReadableData(new int[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            });
            grid.ClearCells(new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(2,2));
            Assert.That(grid[Vector2Int.zero].IsEmpty, Is.True);
            Assert.That(grid[Vector2Int.one].IsEmpty, Is.True);
            Assert.That(grid[Vector2Int.one * 2].IsEmpty, Is.True);
            
            Assert.That(grid[0,2].DiceId == 1);
            Assert.That(grid[2,0].DiceId == 9);            
        }
    }
}