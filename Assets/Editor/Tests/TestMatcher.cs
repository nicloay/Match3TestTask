using Logic.Grid;
using Match3.Logic.MatchFinder;
using Match3.Utils;
using NUnit.Framework;
using UnityEngine;
using Grid = Logic.Grid.Grid;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TestMatcher
    {
        [Test, TestCaseSource("SingleMatchCases")]
        public void TestMatch3SingleMatch(int[,] layout, Vector2Int[] matchPositions)
        {
            LineMatcher lineMatcher = new LineMatcher(Grid.CreateWithHumanReadableData(layout));

            Match[] matches = lineMatcher.GetMatches();
            Assert.That(matches.Length, Is.EqualTo(1));
            Assert.That(matches[0].CellPositions, Is.EquivalentTo(matchPositions));
                        
        }

        private static object[] SingleMatchCases = new object[]
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


        [Test, TestCaseSource("MultipleMatchCases")]
        public void TestMatch3MultipleMatches(int[,] layout, Vector2Int[][] expectedMatches)
        {
            LineMatcher lineMatcher = new LineMatcher(Grid.CreateWithHumanReadableData(layout));

            Match[] matches = lineMatcher.GetMatches();
            Assert.That(matches.Length, Is.EqualTo( expectedMatches.Length));
            for (int i = 0; i < matches.Length; i++)
            {
                Assert.That(matches[i].CellPositions, Is.EquivalentTo(expectedMatches[i]));                
            }
        }

        private static object[] MultipleMatchCases = new object[]
        {
            new object[]
            {
                new [,]
                {
                    {0, 2, 0, 1, 2},
                    {1, 2, 1, 0, 2},
                    {0, 2, 0, 1, 2},
                    {1, 0, 1, 0, 1},
                    {2, 2, 2, 1, 0}
                },
                new[]
                {
                    new[] { new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4)},
                    new[] { new Vector2Int(4, 2), new Vector2Int(4, 3), new Vector2Int(4, 4)},
                    new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0)},
                }
            },
            new object[]
            {
                new [,]
                {
                    {0, 1, 2, 1, 0, 1, 0},                    
                    {1, 0, 2, 0, 1, 0, 1},
                    {2, 2, 2, 2, 2, 1, 0},                    
                    {1, 0, 2, 0, 1, 0, 1},
                    {0, 1, 2, 1, 0, 1, 0}                                                              
                },
                new[]
                {
                    new[] { new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2),new Vector2Int(2, 3), new Vector2Int(2, 4) },
                    new[] { new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2),new Vector2Int(3, 2), new Vector2Int(4, 2) },                    
                }
            },
            new object[]
            {
                new [,]
                {
                    {2, 1, 0, 1, 4, 1, 0},                    
                    {2, 0, 1, 0, 4, 0, 1},
                    {2, 3, 3, 3, 4, 1, 0},                    
                    {3, 0, 1, 0, 4, 4, 4},
                    {3, 1, 0, 1, 4, 1, 0},                    
                    {3, 0, 1, 0, 4, 0, 1},                                                                                                                          
                },
                new[]
                {
                    new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2)},
                    new[] { new Vector2Int(0, 3), new Vector2Int(0, 4), new Vector2Int(0, 5)},
                    new[] { new Vector2Int(4, 0), new Vector2Int(4, 1), new Vector2Int(4, 2), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(4, 5)},
                    new[] { new Vector2Int(4, 2), new Vector2Int(5, 2), new Vector2Int(6, 2)},                    
                    new[] { new Vector2Int(1, 3), new Vector2Int(2, 3), new Vector2Int(3, 3)}                    
                }
            },
        };

        [Test, TestCaseSource("MatchAtPositionCases")]
        public void ContainsMatchAtPositionTest(int[,] layout, bool[,] expectedResult)
        {
            Grid grid = Grid.CreateWithHumanReadableData(layout);

            bool[,] expectedResultNativeFormat = ArrayUtil.ConvertArrayFromHumanReadebleFormatToNative(expectedResult);
            LineMatcher lineMatcher = new LineMatcher(grid);
            for (int y = 0; y < grid.RowNumber; y++)
            {
                for (int x = 0; x < grid.ColumnNumber; x++)
                {
                    Assert.That(lineMatcher.ContainsMatchAt(new Vector2Int(x,y)), Is.EqualTo(expectedResultNativeFormat[x,y]), string.Format("problem at [{0}:{1}] position", x, y));
                }
            }
        }

        private static object[] MatchAtPositionCases = new object[]
        {
            new object[]
            {
                new [,]
                {              
                    {3},{0},{3},{3},{1}
                },
                new [,]
                {                 
                    {false}, {false}, {false}, {false},  {false}
                }
            },
            new object[]
            {
                new [,]
                {              
                    {1},{3},{3},{0},{3}
                },
                new [,]
                {                 
                    {false}, {false}, {false}, {false},  {false}
                }
            },
            new object[]
            {
                new [,]
                {              
                    {3,0,3,3,1}
                },
                new [,]
                {                 
                    {false, false, false, false,  false}
                }
            },
            new object[]
            {
                new [,]
                {              
                    {1,3,3,0,3}
                },
                new [,]
                {                 
                    {false, false, false, false,  false}
                }
            },
            new object[]
            {
                new [,]
                {
                    {1,0,2,0,2},
                    {0,1,2,1,0},
                    {1,2,2,2,3},
                    {0,1,0,1,3},
                    {1,0,1,0,3}
                },
                new [,]
                {
                    {false, false,  true, false, false},
                    {false, false,  true, false, false},
                    {false,  true,  true,  true,  true},
                    {false, false, false, false,  true},
                    {false, false, false, false,  true}
                }
            }
        };


        [Test]
        [TestCaseSource("HintCases")]
        public void TestHints(int[,] layout, Hint[] expectedHints)
        {
            LineMatcher lineMatcher = new LineMatcher(Grid.CreateWithHumanReadableData(layout));
            Hint[] hints = lineMatcher.GetHints();
            Assert.That(hints, Is.EquivalentTo(expectedHints));
        }

        private static object[] HintCases = new object[]
        {
            new object[]
            {
                new[,]
                {
                    {1, 0, 1, 1}
                },
                new Hint[]
                {
                    new Hint(new Vector2Int(0,0), new Vector2Int(1,0))
                }
            },
            new object[]
            {
                new[,]
                {
                    {1},
                    {0},
                    {1},
                    {1}
                },
                new Hint[]
                {
                    new Hint(new Vector2Int(0,2), new Vector2Int(0,3))
                }
            },
            new object[]
            {
                new[,]
                {
                    {2, 2, 1, 2, 1, 1}
                },
                new Hint[]
                {
                    new Hint(new Vector2Int(2,0), new Vector2Int(3,0))
                }
            },
            new object[]
            {
                new[,]
                {
                    {6, 2, 3, 4, 5},
                    {6, 0, 0, 9, 0},
                    {1, 2, 1, 1, 5},
                    {6, 1, 1, 9, 5},                    
                },
                new Hint[]
                {
                    new Hint(new Vector2Int(0,0), new Vector2Int(0,1)),
                    new Hint(new Vector2Int(1,0), new Vector2Int(1,1)),                                        
                    new Hint(new Vector2Int(3,0), new Vector2Int(3,1)),                                        
                    new Hint(new Vector2Int(4,2), new Vector2Int(4,3)),
                    new Hint(new Vector2Int(0,1), new Vector2Int(1,1)),
                    new Hint(new Vector2Int(3,2), new Vector2Int(4,2))                    
                }
            },
            
        };
        
        [Test]
        [Repeat(200)]
        public void SingleMatchStressTest()
        {
            int currentMatchLen = Random.Range(3, 6);
            
            int boardWidth = Random.Range(currentMatchLen, 21); 
            int boardHeight = Random.Range(currentMatchLen, 21);

            
            int[,] layout = new int[boardWidth, boardHeight];
            int diceId = 0;
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    layout[x, y] = diceId++;
                }
            }
            
            Grid grid = Grid.CreateWithData(layout);
            
            //insert one with last id to random position and rotation
            int axis = Random.Range(0, 2);
            int anotherAxis = Mathf.Abs(axis - 1);
            Vector2Int offset  = new Vector2Int();
            offset[axis] = 1;
            
            Vector2Int position = new Vector2Int();
            position[axis] = Random.Range(0, grid.Size[axis] - currentMatchLen);
            position[anotherAxis] = Random.Range(0, grid.Size[anotherAxis]);

            Vector2Int[] usedPositions = new Vector2Int[currentMatchLen];
            
            for (int i = 0; i < currentMatchLen; i++)
            {                
                grid.SetCellDirtyValue(position, diceId);
                usedPositions[i] = position;
                position += offset;
            }

            LineMatcher matcher = new LineMatcher(grid);
            Match[] matches = matcher.GetMatches();
            
            Assert.That(matches.Length, Is.EqualTo(1));
            Assert.That(matches[0].CellPositions, Is.EquivalentTo(usedPositions));
        }
        
        
        
    }
}