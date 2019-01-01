using Logic.Grid;
using Logic.Physics;
using Logic.RNG;
using Match3.Logic.MatchFinder;
using NUnit.Framework;

namespace Match3.Editor.Tests
{
    [TestFixture]
    public class TestSpawner
    {
        [Test]
        public void TestSpawnerSingleField()
        {
            Grid grid = Grid.CreateWithSize(3, 3);
            SimpleRandomDiceGenerator _rng = new SimpleRandomDiceGenerator(4);
            LineMatcher _matcher = new LineMatcher(grid);
            Spawner spawner = new Spawner(_rng, _matcher);
            Assert.That(grid[0,0].IsEmpty, Is.True, "Initial value is wrong");
            spawner.Apply(grid);
            Assert.That(grid[0,0].IsEmpty, Is.False, "spawner didn't work properly");
        }
    }
}