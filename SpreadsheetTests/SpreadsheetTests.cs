// Tests by: Brett Baxter - u1310459 - (2/15/2023)

using SpreadsheetUtilities;
using SS;
using System.Runtime.Intrinsics.X86;

namespace SpreadsheetTests
{
    /// <summary>
    /// Tests all function in Spreadsheet class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        #region - TEST EMPTY CONSTRUCTOR -
        /// <summary>
        /// Creates a new spreadsheet and tests if it is not null, meaning the constructor worked.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor()
        {
            Spreadsheet ss = new Spreadsheet();
            Assert.IsNotNull(ss);
        }

        #region - TEST SET CELL CONTENTS DOUBLE -
        /// <summary>
        /// Sets the double version of set cell contents and then tests to see if it assigned correctly.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDoubleValidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a4", "5.0");
            Assert.AreEqual(5.0, ss.GetCellContents("a4"));
            ss.SetContentsOfCell("_a4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("_a4"));
            ss.SetContentsOfCell("_a_4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("_a_4"));
        }

        /// <summary>
        /// Sets the double version of set cell contents with an invalid name, expecting an invalid name expection.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsDoubleInvalidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("45A2", "5.0");
        }

        /// <summary>
        /// Sets the double version of set cell contents with a double, then reassigns that cell with a new value.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDoubleNameAlreadyInSS()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a4", "5.0");
            Assert.AreEqual(5.0, ss.GetCellContents("a4"));
            ss.SetContentsOfCell("a4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("a4"));
        }
        #endregion

        #region - TEST SET CELL CONTENTS STRING -
        /// <summary>
        /// Sets the string version of set cell contents using a valid name and value.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringValidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("d9", "afljq9uq4jl;efhjaopfbnljffj;a");
            Assert.AreEqual("afljq9uq4jl;efhjaopfbnljffj;a", ss.GetCellContents("d9"));
        }

        /// <summary>
        /// Sets the string version of set cell contents using an invalid name, expecting an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsStringInvalidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("$$14901abcd23", "hello :)");
        }

        /// <summary>
        /// Sets the string version of set cell contents using a valid name and value, then resets that cell with a new string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringNameAlreadyInSS()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("d9", "afljq9uq4jl;efhjaopfbnljffj;a");
            Assert.AreEqual("afljq9uq4jl;efhjaopfbnljffj;a", ss.GetCellContents("d9"));
            ss.SetContentsOfCell("d9", "hello :))");
            Assert.AreEqual("hello :))", ss.GetCellContents("d9"));
        }

        /// <summary>
        /// Sets the string version of set cell contents using a valid name and an empty string. Should return an empty string when getting because
        /// of how get cell contents works. Because d9 has no value associated with it, it returns an empty string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringTextEmpty()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("d9", "");
            Assert.AreEqual("", ss.GetCellContents("d9"));
        }
        #endregion

        #region - TEST SET CELL CONTENTS FORMULA -
        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula. Also tests to make sure the value
        /// associated with the name is of type formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsFormulaValidName()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("5+5");
            ss.SetContentsOfCell("g6", "=5+5");
            Assert.IsInstanceOfType(ss.GetCellContents("g6"), typeof(Formula));
        }

        /// <summary>
        /// Sets the formula version of set cell contents with an invalid name, expecting an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaInvalidName()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("5+5");
            ss.SetContentsOfCell("66g6", "=5+5");
        }

        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula, but then reassigns the cell to another formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsFormulaNameAlreadyInSS()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("5+5");
            Formula formula2 = new Formula("9*9/2");
            ss.SetContentsOfCell("g6", "=5+5");
            Assert.AreEqual(formula, ss.GetCellContents("g6"));
            ss.SetContentsOfCell("g6", "=9*9/2");
            Assert.AreEqual(formula2, ss.GetCellContents("g6"));
        }

        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula, but the formula has a circular dependence, expecting a
        /// circular exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellContentsFormulaCircularException()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("d6+d6");
            ss.SetContentsOfCell("d6", "=d6+d6");
        }
        #endregion

        #region - TEST GETTERS -

        /// <summary>
        /// Tries to get cell contents using an invalid name, expecting invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("9a8_");
        }

        /// <summary>
        /// Tests get cell contents on a valid name that has nothing assigned to it, should return an empty string.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsEmptyReturn()
        {
            Spreadsheet ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with string value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnString()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a5", "hello there!");
            Assert.AreEqual("hello there!", ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with double value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnDouble()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a5", "12e5");
            Assert.AreEqual(12e5, ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with formula value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnFormula()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("5*5/2+(15+2)");
            ss.SetContentsOfCell("d6", "=5*5/2+(15+2)");
            Assert.AreEqual(formula, ss.GetCellContents("d6"));
        }

        /// <summary>
        /// Gets the names of all non empty cells, there are four in this test.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("d6", "10.0");
            ss.SetContentsOfCell("_d6", "15.0");
            ss.SetContentsOfCell("b883", "5.0");
            ss.SetContentsOfCell("god99", "10e-4");
            List<string> names = ss.GetNamesOfAllNonemptyCells().ToList();
            Assert.AreEqual(4, names.Count());
            Assert.AreEqual("d6", names[0]);
            Assert.AreEqual("_d6", names[1]);
            Assert.AreEqual("b883", names[2]);
            Assert.AreEqual("god99", names[3]);
        }

        /// <summary>
        /// Gets the names of all non empty cells, there are none in this test, should return an empty list.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsEmpty()
        {
            Spreadsheet ss = new Spreadsheet();
            IEnumerable<string> names = ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, names.Count());
        }
        #endregion
        #endregion

        #region - TEST 3 PARAM CONSTRUCTOR -

        /// <summary>
        /// Creates a new spreadsheet and tests if it is not null, meaning the constructor worked.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Assert.IsNotNull(ss);
        }

        #region - TEST SET CELL CONTENTS DOUBLE -
        /// <summary>
        /// Sets the double version of set cell contents and then tests to see if it assigned correctly.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDoubleValidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("a4", "5.0");
            Assert.AreEqual(5.0, ss.GetCellContents("a4"));
            ss.SetContentsOfCell("_a4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("_a4"));
            ss.SetContentsOfCell("_a_4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("_a_4"));
        }

        /// <summary>
        /// Sets the double version of set cell contents with an invalid name, expecting an invalid name expection.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsDoubleInvalidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("45A2", "5.0");
        }

        /// <summary>
        /// Sets the double version of set cell contents with a double, then reassigns that cell with a new value.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDoubleNameAlreadyInSS3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("a4", "5.0");
            Assert.AreEqual(5.0, ss.GetCellContents("a4"));
            ss.SetContentsOfCell("a4", "15.0");
            Assert.AreEqual(15.0, ss.GetCellContents("a4"));
        }
        #endregion

        #region - TEST SET CELL CONTENTS STRING -
        /// <summary>
        /// Sets the string version of set cell contents using a valid name and value.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringValidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("d9", "afljq9uq4jl;efhjaopfbnljffj;a");
            Assert.AreEqual("afljq9uq4jl;efhjaopfbnljffj;a", ss.GetCellContents("d9"));
        }

        /// <summary>
        /// Sets the string version of set cell contents using an invalid name, expecting an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsStringInvalidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("$$14901abcd23", "hello :)");
        }

        /// <summary>
        /// Sets the string version of set cell contents using a valid name and value, then resets that cell with a new string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringNameAlreadyInSS3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("d9", "afljq9uq4jl;efhjaopfbnljffj;a");
            Assert.AreEqual("afljq9uq4jl;efhjaopfbnljffj;a", ss.GetCellContents("d9"));
            ss.SetContentsOfCell("d9", "hello :))");
            Assert.AreEqual("hello :))", ss.GetCellContents("d9"));
        }

        /// <summary>
        /// Sets the string version of set cell contents using a valid name and an empty string. Should return an empty string when getting because
        /// of how get cell contents works. Because d9 has no value associated with it, it returns an empty string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsStringTextEmpty3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("d9", "");
            Assert.AreEqual("", ss.GetCellContents("d9"));
        }
        #endregion

        #region - TEST SET CELL CONTENTS FORMULA -
        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula. Also tests to make sure the value
        /// associated with the name is of type formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsFormulaValidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Formula formula = new Formula("5+5");
            ss.SetContentsOfCell("g6", "=5+5");
            Assert.IsInstanceOfType(ss.GetCellContents("g6"), typeof(Formula));
        }

        /// <summary>
        /// Sets the formula version of set cell contents with an invalid name, expecting an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaInvalidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Formula formula = new Formula("5+5");
            ss.SetContentsOfCell("66g6", "=5+5");
        }

        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula, but then reassigns the cell to another formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsFormulaNameAlreadyInSS3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Formula formula = new Formula("5+5");
            Formula formula2 = new Formula("9*9/2");
            ss.SetContentsOfCell("g6", "=5+5");
            Assert.AreEqual(formula, ss.GetCellContents("g6"));
            ss.SetContentsOfCell("g6", "=9*9/2");
            Assert.AreEqual(formula2, ss.GetCellContents("g6"));
        }

        /// <summary>
        /// Sets the formula version of set cell contents with a valid name and formula, but the formula has a circular dependence, expecting a
        /// circular exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellContentsFormulaCircularException3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Formula formula = new Formula("d6+d6");
            ss.SetContentsOfCell("d6", "=d6+d6");
        }
        #endregion

        #region - TEST GETTERS -

        /// <summary>
        /// Tries to get cell contents using an invalid name, expecting invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidName3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.GetCellContents("9a8_");
        }

        /// <summary>
        /// Tests get cell contents on a valid name that has nothing assigned to it, should return an empty string.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsEmptyReturn3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Assert.AreEqual("", ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with string value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnString3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("a5", "hello there!");
            Assert.AreEqual("hello there!", ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with double value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnDouble3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("a5", "12e5");
            Assert.AreEqual(12e5, ss.GetCellContents("a5"));
        }

        /// <summary>
        /// Tests get cell contents on a valid name with formula value assigned to it.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentsNormalReturnFormula3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            Formula formula = new Formula("5*5/2+(15+2)");
            ss.SetContentsOfCell("d6", "=5*5/2+(15+2)");
            Assert.AreEqual(formula, ss.GetCellContents("d6"));
        }

        /// <summary>
        /// Gets the names of all non empty cells, there are four in this test.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            ss.SetContentsOfCell("d6", "10.0");
            ss.SetContentsOfCell("_d6", "15.0");
            ss.SetContentsOfCell("b883", "5.0");
            ss.SetContentsOfCell("god99", "10e-4");
            List<string> names = ss.GetNamesOfAllNonemptyCells().ToList();
            Assert.AreEqual(4, names.Count());
            Assert.AreEqual("d6", names[0]);
            Assert.AreEqual("_d6", names[1]);
            Assert.AreEqual("b883", names[2]);
            Assert.AreEqual("god99", names[3]);
        }

        /// <summary>
        /// Gets the names of all non empty cells, there are none in this test, should return an empty list.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsEmpty3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            IEnumerable<string> names = ss.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, names.Count());
        }
        #endregion

        #endregion

        #region - TEST NEW A5 METHODS -

        /// <summary>
        /// Tests get version on a no param constructor, should return default.
        /// </summary>
        [TestMethod]
        public void TestGetVersion()
        {
            Spreadsheet ss = new Spreadsheet();
            string version = ss.Version;
            Assert.AreEqual("default", version);
        }

        /// <summary>
        /// Tests get version on a 3 param constructor, should return 1.0.
        /// </summary>
        [TestMethod]
        public void TestGetVersion3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "1.0");
            string version = ss.Version;
            Assert.AreEqual("1.0", version);
        }

        /// <summary>
        /// Tests get IsValid on a no param constructor, should return a function that always returns true.
        /// </summary>
        [TestMethod]
        public void TestGetIsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            Func<string, bool> isValid = ss.IsValid;
            Assert.IsTrue(isValid("aflajdlf"));
        }

        /// <summary>
        /// Tests get IsValid on a 3 param constructor, should return a function that always returns false.
        /// </summary>
        [TestMethod]
        public void TestGetIsValid3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => false, s => s.ToLower(), "1.0");
            Func<string, bool> isValid = ss.IsValid;
            Assert.IsFalse(isValid("a6"));
        }

        /// <summary>
        /// Tests get Normalize on a no param constructor, should return a function that does nothing.
        /// </summary>
        [TestMethod]
        public void TestGetNormalize()
        {
            Spreadsheet ss = new Spreadsheet();
            Func<string, string> normalize = ss.Normalize;
            Assert.AreEqual("ABC", normalize("ABC"));
            Assert.AreEqual("abc", normalize("abc"));
            Assert.AreEqual("_ABC", normalize("_ABC"));
        }

        /// <summary>
        /// Tests get Normalize on a 3 param constructor, should return a function that lowercases the input.
        /// </summary>
        [TestMethod]
        public void TestGetNormalize3Param()
        {
            Spreadsheet ss = new Spreadsheet(s => false, s => s.ToLower(), "1.0");
            Func<string, string> normalize = ss.Normalize;
            Assert.AreEqual("abc", normalize("ABC"));
            Assert.AreEqual("abc", normalize("abc"));
            Assert.AreEqual("_abc", normalize("_ABC"));
        }

        /// <summary>
        /// Test get cell value on three different cells, one contains a double, one contains a string, and the last contains a formula.
        /// Also tests for a cell with no value.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("a6", "wololo");
            ss.SetContentsOfCell("d9", "6.0");
            ss.SetContentsOfCell("g6", "=5+5");
            Formula formula = new Formula("5+5");
            Assert.AreEqual("wololo", ss.GetCellValue("a6"));
            Assert.AreEqual("", ss.GetCellValue("b6"));
            Assert.AreEqual(6.0, ss.GetCellValue("d9"));
            Assert.AreEqual(formula, ss.GetCellContents("g6"));
        }

        /// <summary>
        /// Tests get cell value on an invalid cell name, should throw an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValueInvalidName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellValue("45BDC");
        }

        /// <summary>
        /// Tests Constructor with custom functions. If the cell is g6 it is valid, everything else isn't. Should throw an invalid name exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestWithCustomIsValidAndNormalizer()
        {
            Spreadsheet ss = new Spreadsheet(s => s == "g6" ? true : false, s => s.ToLower(), "Version 1.0");
            ss.SetContentsOfCell("g6", "6.0");
            ss.SetContentsOfCell("d9", "hello");
        }

        /// <summary>
        /// Tests Constructor with custom functions. If the cell is g6 it is valid, everything else isn't. Shouldn't throw an error because we only use g6.
        /// </summary>
        [TestMethod]
        public void TestWithCustomIsValidAndNormalizer2()
        {
            Spreadsheet ss = new Spreadsheet(s => s == "g6" ? true : false, s => s.ToLower(), "Version 1.0");
            ss.SetContentsOfCell("g6", "6.0");
            Assert.AreEqual(6.0, ss.GetCellValue("g6"));
        }

        /// <summary>
        /// Tests saving a spreadsheet, and then creating a new spreadsheet using the file path.
        /// </summary>
        [TestMethod]
        public void TestSaveSpreadsheet()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("g6", "6.0");
            ss.Save("save.txt");
            Spreadsheet ss2 = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "default");
            Assert.AreEqual(6.0, ss2.GetCellValue("g6"));
        }

        /// <summary>
        /// Tests saving a spreadsheet with multiple cells with multiple value types, and then creating a new spreadsheet using the filepath.
        /// </summary>
        [TestMethod]
        public void TestSaveSpreadsheetMultipleCells()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula formula = new Formula("12+5e-6*2");
            ss.SetContentsOfCell("g6", "6.0");
            ss.SetContentsOfCell("b3", "=12+5e-6*2");
            ss.SetContentsOfCell("d9", "wololo");
            ss.Save("save.txt");
            Spreadsheet ss2 = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "default");
            Assert.AreEqual(6.0, ss2.GetCellValue("g6"));
            Assert.AreEqual("wololo", ss2.GetCellValue("d9"));
            Assert.AreEqual(formula, ss2.GetCellContents("b3"));
        }

        /// <summary>
        /// Tests getting the version from two different saved spreadsheets. One with a custom version name and the other default.
        /// </summary>
        [TestMethod]
        public void TestGetVersionFromSavedSpreadsheet()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("g6", "6.0");
            ss.Save("save.txt");
            Spreadsheet ss2 = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "default");
            Assert.AreEqual(6.0, ss2.GetCellValue("g6"));
            ss2.GetSavedVersion("save.txt");
            Assert.AreEqual("default", ss.GetSavedVersion("save.txt"));
            ss2.Save("save2.txt");
            Assert.AreEqual("default", ss2.GetSavedVersion("save2.txt"));
        }

        /// <summary>
        /// Tests get version from save on an empty file path. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetVersionFromInvalidSave()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetSavedVersion("");
        }

        /// <summary>
        /// Tests get version from save on a real save but an empty file path. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetVersionFromInvalidSaveEmpty()
        {
            Spreadsheet ss = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "Version 1.0");
            ss.Save("save.txt");
            ss.GetSavedVersion("");
        }

        /// <summary>
        /// Tests trying to save with an empty file path. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveInvalidFilePath()
        {
            Spreadsheet ss = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "Version 2.0");
            ss.Save("");
        }

        /// <summary>
        /// Tests the 4 param constructor with an empty filepath. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test4ParamConstructorEmptyFilepath()
        {
            Spreadsheet ss = new Spreadsheet("", s => true, s => s.ToLower(), "Version 1.0");
        }

        /// <summary>
        /// Tests the 4 param constructor with an empty version name. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test4ParamConstructorEmptyVersion()
        {
            Spreadsheet ss = new Spreadsheet("", s => true, s => s.ToLower(), "");
        }

        /// <summary>
        /// Tests a 3 param constructor with an empty verison name. Should throw a read write error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Test4ParamVersionMismatch()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "default");
            ss.Save("save.txt");
            Spreadsheet ss2 = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "v2");
        }

        /// <summary>
        /// A cumulative test of every function in one method.
        /// 
        /// Adds 100 doubles to 100 cells, saves and makes a new spreadsheet with that save. Checks the new spreadsheet that everything was copied over.
        /// </summary>
        [TestMethod]
        public void TestCumulitiveFunction()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToLower(), "Version 1.0");
            // 100 times add string number i to the cell a(i)
            for(int i = 0; i < 100; i++)
            {
                ss.SetContentsOfCell("a" + i, "" + i);
            }
            ss.Save("save.txt");
            string savedVersion = ss.GetSavedVersion("save.txt"); // should be Version 1.0
            Spreadsheet ss2 = new Spreadsheet("save.txt", s => true, s => s.ToLower(), "Version 1.0");
            // Check that the old spreadsheet and the new spreadsheet are the same.
            Assert.AreEqual(savedVersion, ss2.Version);
            // Check to make sure all values were successfully copied over.
            for(int i = 0; i < 100; i++)
            {
                Assert.AreEqual((double) i, ss2.GetCellContents("a" + i));
            }
            ss2.SetContentsOfCell("b6", "=a5+a9");
            Assert.AreEqual(14.0, ss2.GetCellValue("b6"));
        }

        #endregion
    }
}