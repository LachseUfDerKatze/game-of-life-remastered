using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GameOfLife;

namespace UnitTest
{
    [TestClass]
    public class CellTest
    {
        [TestMethod]
        public void TestCostructor()
        {
            var cell = new Cell(false, 1, 3);
            Assert.IsFalse(cell.Alive);
            Assert.AreEqual(cell.Row, 1);
            Assert.AreEqual(cell.Column, 3);
        }
    }
}
