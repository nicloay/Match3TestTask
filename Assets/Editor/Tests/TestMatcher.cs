using Logic.Grid;
using Match3.Logic.MatchFinder;
using NUnit.Framework;
using UnityEngine;
using Grid = Logic.Grid.Grid;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TestMatcher
    {
        [Test, TestCaseSource("HorizontalSingleMatchCases")]
        public void TestMatch3HorizontalSingleMatch(int[,] layout, Vector2Int[] matchPositions)
        {
            LineMatcher lineMatcher = new LineMatcher(Grid.CreateWithHumanReadableData(layout));

            Match[] matches = lineMatcher.GetMatches();
            Assert.That(matches.Length, Is.EqualTo(1));
            Assert.That(matches[0].CellPositions, Is.EquivalentTo(matchPositions));
                        
        }

        private static object[] HorizontalSingleMatchCases = new object[]
        {
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3},
                    {4, 4, 4},
                    {7, 8, 9}
                },
                new Vector2Int[]
                {
                    new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
                }
            },
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3, 1, 2, 3},
                    {4, 5, 4, 4, 4, 6},
                    {7, 8, 9, 7, 8, 9}
                },
                new Vector2Int[]
                {
                    new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(4, 1),
                }
            },
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3, 2, 2, 2},
                    {4, 5, 6, 4, 5, 6},
                    {7, 8, 9, 7, 8, 9}
                },
                new Vector2Int[]
                {
                    new Vector2Int(3, 2), new Vector2Int(4, 2), new Vector2Int(5, 2),
                }
            },
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3, 1, 2, 3},
                    {4, 5, 6, 4, 5, 3},
                    {7, 8, 9, 7, 8, 3}
                },
                new Vector2Int[]
                {
                    new Vector2Int(5, 0), new Vector2Int(5, 1), new Vector2Int(5, 2),
                }
            },
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3, 1, 2, 3},
                    {4, 0, 6, 4, 5, 6},
                    {7, 0, 9, 7, 8, 9},
                    {1, 0, 3, 1, 2, 3},
                    {4, 5, 6, 4, 5, 6},
                    {7, 8, 9, 7, 8, 9},
                },
                new Vector2Int[]
                {
                    new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4),
                }
            },
            new object[]
            {
                new int[,]
                {
                    {1, 2, 3},
                    {4, 2, 6},
                    {7, 2, 9}
                },
                new Vector2Int[]
                {
                    new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
                }
            }
        };


        
        
        [Test, Repeat(1000)]
        public void SingleMatchStressTest()
        {
            int matchLength = Random.Range(3, 6);
            
            int boardWidth = Random.Range(matchLength, 20); 
            int boardHeight = Random.Range(matchLength, 20);

            
            int[,] layout = new int[boardWidth, boardHeight];
            int i = 0;
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    layout[x, y] = i++;
                }
            }
            
            
            
        }
    }
}