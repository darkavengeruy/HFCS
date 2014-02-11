﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LCHtoADA;
using System.Data;
using System.Collections.Generic;

namespace LCHUnitTest
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void BasicSetUp()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report18 r18 = new Report18(testPath + "R18.txt");
            Report79 r79 = new Report79(testPath + "R79.txt");
            Report90 r90 = new Report90(testPath + "R90.txt");

            double x = (double)r79.DataTable.Rows[r79.DataTable.Rows.Count - 1]["DiscountFactor"];
            double y = (double)r90.DataTable.Rows[3]["6M"];
            double z = (double)r18.DataTable.Rows[3]["Exchangerate"];
            double u = r18["AUD"];

            Assert.AreEqual(0.0688413619, x, 1e-6);
            Assert.AreEqual(-1.9715, y, 1e-6);
            Assert.AreEqual(1.352, z, 1e-6);
            Assert.AreEqual(1.7135, u, 1e-6);
        }

        [TestMethod]
        public void TestBaseDates()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report18 r18 = new Report18(testPath + "R18.txt");
            Report79 r79 = new Report79(testPath + "R79.txt");
            Report90 r90 = new Report90(testPath + "R90.txt");

            Assert.AreEqual(new DateTime(2013, 11, 18), r18.BaseDate);
            Assert.AreEqual(new DateTime(2013, 11, 18), r79.BaseDate);
            Assert.AreEqual(new DateTime(2013, 11, 18), r90.BaseDate);
        }

        [TestMethod]
        public void TestHaveSameBaseDates()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report18 r18 = new Report18(testPath + "R18.txt");
            Report79 r79 = new Report79(testPath + "R79.txt");
            Report79 r79Wrong = new Report79(testPath + "R79Wrong.txt");
            Report90 r90 = new Report90(testPath + "R90.txt");

            Converter c = new Converter(r18, r79Wrong, r90);
            bool equalBaseDates = c.HaveSameBaseDate(new LCHFile[] { r18, r79Wrong, r90 });

            Assert.IsFalse(equalBaseDates);

            c = new Converter(r18, r79, r90);
            equalBaseDates = c.HaveSameBaseDate(new LCHFile[] { r18, r79, r90 });
            Assert.IsTrue(equalBaseDates);
        }

        [TestMethod]
        public void CanGenerateDAT()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report18 r18 = new Report18(testPath + "R18.txt");
            Report79 r79 = new Report79(testPath + "R79.txt");
            Report90 r90 = new Report90(testPath + "R90.txt");
            Converter c = new Converter(r18, r79, r90);

            c.ToDat(testPath + "outDat.dat");
        }

        [TestMethod]
        public void TestAvailableTenors()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report79 r79 = new Report79(testPath + "R79.txt");

            var availableTenors = r79.AvailableTenors("AUD_BBSW");

            Assert.AreEqual(1.498630137, availableTenors[8], 1e-6);
            Assert.AreEqual(30, availableTenors[availableTenors.Count - 1], 1e-6);
        }

        [TestMethod]
        public void TestAllDatesInReport90()
        {
            string testPath = LCHtoADA.Program.TestFilesDirectoryName();
            Report90 r90 = new Report90(testPath + "R90.txt");
            List<DateTime> listDates = r90.AllDates();
            Assert.AreEqual(2500, listDates.Count, 0);
            Assert.AreEqual(new DateTime(2013, 11, 11), listDates[0]);
            Assert.AreEqual(new DateTime(2004, 3, 10), listDates[listDates.Count - 1]);
        }
    }
}
