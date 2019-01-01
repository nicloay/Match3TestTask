using Logic.Grid;
using Logic.Physics;
using NUnit.Framework;

namespace Match3.Editor.Tests
{
    public class TestGravity
    {
        
        [Test]
        [TestCaseSource("GravityCases")]
        public void TestGravitySimple(int[,] sourceLayout, int[,] expectedLayout)
        {
            Grid sourceGrid = Grid.CreateWithHumanReadableData(sourceLayout);
            GravityPhysics gravityPhysics = new GravityPhysics();
            gravityPhysics.Apply(sourceGrid);

            Grid expectedGrid = Grid.CreateWithHumanReadableData(expectedLayout);

            sourceGrid.ForEachCell(position =>
                Assert.That(sourceGrid[position].DiceType, Is.EqualTo(expectedGrid[position].DiceType),
                    string.Format("problem at {0}", position)));
        }


        private static object[] GravityCases = new object[]
        {
            new object[]
            {
                new[,]
                {
                    {-1},
                    {1},
                    {-1}
                },
                new[,]
                {
                    {-1},
                    {-1},
                    {1}
                }
            },
            new object[]
            {
                new[,]
                {
                    {-1},
                    {1},
                    {1},
                    {-1}
                },
                new[,]
                {
                    {-1},
                    {-1},
                    {1},
                    {1},
                }
            },
            new object[]
            {
                new[,]
                {
                    {1},
                    {-1},
                    {1},
                    {-1}
                },
                new[,]
                {
                    {-1},
                    {-1},
                    {1},
                    {1},
                }
            },
            new object[]
            {
                new[,]
                {
                    {-1, 2},
                    {1, -1},
                    {1, -1},
                    {-1, -1}
                },
                new[,]
                {
                    {-1, -1},
                    {-1, -1},
                    {1, -1},
                    {1, 2}
                }
            },
            new object[]
            {
                new[,]
                {
                    {1, 2},
                    {1, -1},
                    {1, 2},
                    {-1, -1}
                },
                new[,]
                {
                    {-1, -1},
                    {1, -1},
                    {1, 2},
                    {1, 2}
                }
            }

        };
    }
}