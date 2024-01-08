using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        #region - TEST INVALID FORMULAS -

        /// <summary>
        /// Tests an invalid formula that begins with an invalid character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidBeginning1()
        {
            String formula = ")1+b2-5";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula that begins with a different invalid character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidBeginning2()
        {
            String formula = "*b3+89+10";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula that begins with a different invalid character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidBeginning3()
        {
            String formula = "+51+c3/9";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula that begins with an invalid ending character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEnding1()
        {
            String formula = "15+10-5/2+";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula that begins with a different invalid ending character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEnding2()
        {
            String formula = "51+c3/9(";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula that begins with a different invalid ending character.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidEnding3()
        {
            String formula = "50/10+5/*";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an formula with an invalid variable format.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidVariable()
        {
            String formula = "3cb+10/2";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid formula with not enough operators.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidFormat()
        {
            String formula = "1 + 5 * (5 + 10)2 ";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an empty string.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaEmptyString()
        {
            String formula = "";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests an invalid syntax from the assignment page.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaInvalidSyntax()
        {
            String formula = "A 1";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests a formula with only an operator.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaOnlyOperator()
        {
            String formula = "+";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests a formula with mismatching parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFormulaMismatchParenthesis()
        {
            String formula = "5 * 3 / ((2 + 2)";
            Formula testFormula = new Formula(formula);
        }

        /// <summary>
        /// Tests a formula that is only parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyParenthesis()
        {
            String formula = "()";
            Formula testFormula = new Formula(formula);
        }

        #endregion

        #region - TEST VALID FORMULAS -

        /// <summary>
        /// Tests a formula that is only 1 number.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaSingleNumber()
        {
            String formula = "5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5", formula.ToString());
        }

        /// <summary>
        /// Tests a formula that is a simple operation.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaSimple()
        {
            String formula = "5+10";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5+10", formula.ToString());
        }

        /// <summary>
        /// Tests a formula with simple operation and a valid variable.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaSimpleWithVariable()
        {
            String formula = "5+10-_a2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5+10-_a2", formula.ToString());
        }

        /// <summary>
        /// Tests a formula with simple operation and a valid variable in the alternative format.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaSimpleWithVariable2()
        {
            String formula = "5+10-a2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5+10-a2", formula.ToString());
        }

        /// <summary>
        /// Tests a valid formula with valid parenthesis.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaWithParenthesis()
        {
            String formula = "(15-10)/2+19*b3+(22/2)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("(15-10)/2+19*b3+(22/2)", formula.ToString());
        }

        /// <summary>
        /// Tests a valid formula with nested parenthesis.
        /// </summary>
        [TestMethod]
        public void TestValidFormulaWithNestedParenthesis()
        {
            String formula = "(((14-10)/2)+10)*2/5+b2*6";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("(((14-10)/2)+10)*2/5+b2*6", formula.ToString());
        }

        /// <summary>
        /// Tests a valid formula that is failed by the isValid function.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidFormulaIsValidInvalid()
        {
            String formula = "b1 * 2 / 3";
            Formula testFormula = new Formula(formula, var => var.ToLower(), x => false);
        }

        /// <summary>
        /// Tests a valid formula that is failed by the normalizer function.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidFormulaNormalizeInvalid()
        {
            String formula = "b1 * 2 / 3";
            Formula testFormula = new Formula(formula, var => "33B", x => false);
        }

        #endregion

        #region - TEST EVALUATE -

        /// <summary>
        /// Tests an evaluation for a single number.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSingleNumber()
        {
            String formula = "5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on a simple operation.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormula()
        {
            String formula = "5+10";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0 + 10.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on a more complex operation.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleMultipleOpsFormula()
        {
            String formula = "5+10*2/5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0 + 10.0 * 2.0 / 5.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on a more complex operation with parenthesis.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleMultipleOpsParenthesisFormula()
        {
            String formula = "5+10*(2/5)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0 + 10.0 * (2.0 / 5.0), testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate with nested parenthesis.
        /// </summary>
        [TestMethod]
        public void TestEvaluateNestedParenthesis()
        {
            String formula = "(((((10/2))))*5)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual((((((10.0/2.0))))*5.0), testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on a formula that divides by zero. Should return an error object.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDivideByZero()
        {
            String formula = "10/0";
            Formula testFormula = new Formula(formula);
            Object testError = testFormula.Evaluate(var => 4);
            Assert.IsTrue(testError != null);
        }

        /// <summary>
        /// Tests multiply by zero.
        /// </summary>
        [TestMethod]
        public void TestEvaluateMultiplyByZero()
        {
            String formula = "10*0";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(0.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate with a valid variable.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormulaWithVariable()
        {
            String formula = "15 * b1";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(15.0 * 4.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate with a valid variable in alternative format.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormulaWithUnderscoreVariable()
        {
            String formula = "15 * _b1";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(15.0 * 4.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate with multiple variables.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormulaWithMultipleVariables()
        {
            String formula = "15 * b1 + 9 / (4 * 3) * y3 / 2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(15.0 * 4.0 + 9.0 / (4.0 * 3.0) * 4.0 / 2.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on an operation with a variable first.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormulaWithVariableFirst()
        {
            String formula = "b3 * 2 / 5 + 10";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(4.0 * 2.0 / 5.0 + 10.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests evaluate on an operation with only variables.
        /// </summary>
        [TestMethod]
        public void TestEvaluateSimpleFormulaWithAllVariables()
        {
            String formula = "b3 + b4 * a2 / a6";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(4.0 + 4.0 * 4.0 / 4.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tries to divide a variable by zero.
        /// </summary>
        [TestMethod]
        public void TestEvaluateVariableDivideByZero()
        {
            String formula = "b3 / 0";
            Formula testFormula = new Formula(formula);
            Object testError = testFormula.Evaluate(var => 4);
            Assert.IsTrue(testError != null);
        }

        /// <summary>
        /// Tests an operation with multiple of the same operation.
        /// </summary>
        [TestMethod]
        public void TestMultiOneOperatorAddition()
        {
            String formula = "5 + 5 + 5 + 5 + 5 + 5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0+5+5+5+5+5, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests an operation with multiple of the same operation.
        /// </summary>
        [TestMethod]
        public void TestMultiOneOperatorSubtration()
        {
            String formula = "5 - 5 - 5 - 5 - 5 - 5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0-5-5-5-5-5, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests multiply then add.
        /// </summary>
        [TestMethod]
        public void TestMultiplyThenAdd()
        {
            String formula = "5 * 5 + 5";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5.0 + 5 * 5, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests simple parenthesis operation.
        /// </summary>
        [TestMethod]
        public void TestParenthesisFormula()
        {
            String formula = "(5+5)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual((5.0+5), testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tests multiple parenthesis operations.
        /// </summary>
        [TestMethod]
        public void TestParenthesisFormula2()
        {
            String formula = "(5*5*10)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual((5.0 * 5 * 10), testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Tries to divide by zero in parenthesis.
        /// </summary>
        [TestMethod]
        public void TestParenthesisFormulaDivideByZero()
        {
            String formula = "(5*5/0)";
            Formula testFormula = new Formula(formula);
            Object testError = testFormula.Evaluate(var => 4);
            Assert.IsTrue(testError != null);
        }

        #endregion

        #region - TEST OTHER METHODS -

        /// <summary>
        /// Tests get variables on one variable formula.
        /// </summary>
        [TestMethod]
        public void TestGetVariables1()
        {
            String formula = "a1";
            Formula testFormula = new Formula(formula);
            IEnumerable<String> output = testFormula.GetVariables();
            Assert.AreEqual(1, output.Count());
        }

        /// <summary>
        /// Tests get variables formula all variables and alternative format.
        /// </summary>
        [TestMethod]
        public void TestGetVariables2()
        {
            String formula = "a1 + b3 * d6 / z9 + _a99";
            Formula testFormula = new Formula(formula);
            IEnumerable<String> output = testFormula.GetVariables();
            Assert.AreEqual(5, output.Count());
        }

        /// <summary>
        /// Tests get variables with numbers in formula.
        /// </summary>
        [TestMethod]
        public void TestGetVariables3()
        {
            String formula = "a1 + b3 * 15 / z9 + 32";
            Formula testFormula = new Formula(formula);
            IEnumerable<String> output = testFormula.GetVariables();
            Assert.AreEqual(3, output.Count());
        }

        /// <summary>
        /// Tests simple formula to string.
        /// </summary>
        [TestMethod]
        public void TestToStringSmall()
        {
            String formula = "a1 + 3";
            Formula testFormula = new Formula(formula);
            String output = testFormula.ToString();
            Assert.AreEqual("a1+3", output);
        }

        /// <summary>
        /// Tests large formula to string.
        /// </summary>
        [TestMethod]
        public void TestToStringLarge()
        {
            String formula = "515131231412345 + _afaf9033213 / 1231230990 + 1509 * (12903 + _a49834)";
            Formula testFormula = new Formula(formula);
            String output = testFormula.ToString();
            Assert.AreEqual("515131231412345+_afaf9033213/1231230990+1509*(12903+_a49834)", output);
        }

        /// <summary>
        /// Tests get hash code on two identical formulas.
        /// </summary>
        [TestMethod]
        public void TestGetHashCode()
        {
            String formula = "55 + 3 / 10";
            String formula2 = "55 + 3 / 10";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            int output1 = testFormula.GetHashCode();
            int output2 = testFormula2.GetHashCode();
            Assert.AreEqual(output1, output2);
        }

        /// <summary>
        /// Tests get hash code on two identical formulas with variables.
        /// </summary>
        [TestMethod]
        public void TestGetHashCodeWithVariables()
        {
            String formula = "c7 + _d9 / 10";
            String formula2 = "c7 + _d9 / 10";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            int output1 = testFormula.GetHashCode();
            int output2 = testFormula2.GetHashCode();
            Assert.AreEqual(output1, output2);
        }

        /// <summary>
        /// Tests get hash code on two different formulas.
        /// </summary>
        [TestMethod]
        public void TestGetHashCodeUnEqual()
        {
            String formula = "c7 + _d9 / 10";
            String formula2 = "321 / 38 + 10";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            int output1 = testFormula.GetHashCode();
            int output2 = testFormula2.GetHashCode();
            Assert.IsFalse(output1 == output2);
        }

        #endregion

        #region - TEST EQUALS METHODS -

        /// <summary>
        /// Tests is equals on two identical formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsTrue()
        {
            String formula = "5 + 5 + 10";
            String formula2 = "5 + 5 + 10";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsTrue(testFormula.Equals(testFormula2));
        }

        /// <summary>
        /// Tests is equal on two different formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsFalse()
        {
            String formula = "5 + 5 + 10";
            String formula2 = "5 + 5 + 15";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula.Equals(testFormula2));
        }

        /// <summary>
        /// Tests is equal on two different object types: string and formula.
        /// </summary>
        [TestMethod]
        public void TestEqualsDifferentTypeFalse()
        {
            String formula = "5 + 5 + 10";
            String formula2 = "5 + 5 + 15";
            Formula testFormula = new Formula(formula);
            Assert.IsFalse(testFormula.Equals(formula2));
        }

        /// <summary>
        /// Tests equals again on different formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsFalse2()
        {
            String formula = "5 + 5 + 10";
            String formula2 = "(210 / 2) * (9 * 10)";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula.Equals(testFormula2));
        }

        /// <summary>
        /// Tests equals on two different formulas with variables.
        /// </summary>
        [TestMethod]
        public void TestEqualsFalseWithVariables()
        {
            String formula = "a6 / 10";
            String formula2 = "b9 * 3 + 10";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula.Equals(testFormula2));
        }

        /// <summary>
        /// Tests == on two identical formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsOperatorTrue()
        {
            String formula = "10 / 2";
            String formula2 = "10 / 2";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsTrue(testFormula == testFormula2);
        }

        /// <summary>
        /// Tests == on two identical formulas with variables.
        /// </summary>
        [TestMethod]
        public void TestEqualsOperatorTrueVariables()
        {
            String formula = "c3 / 2";
            String formula2 = "c3 / 2";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsTrue(testFormula == testFormula2);
        }

        /// <summary>
        /// Tests == on two different formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsOperatorFalse()
        {
            String formula = "c3 / 2";
            String formula2 = "15 / 2";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula == testFormula2);
        }

        /// <summary>
        /// Tests == on two identical large formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsOperatorTrueLarge()
        {
            String formula = "2139123 / acda9 + _893213 * (123120 / 12329085) - 328397";
            String formula2 = "2139123 / acda9 + _893213 * (123120 / 12329085) - 328397";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsTrue(testFormula == testFormula2);
        }

        /// <summary>
        /// Tests == on two different large formulas.
        /// </summary>
        [TestMethod]
        public void TestEqualsOperatorFalseLarge()
        {
            String formula = "2139123 / acda9 + _89313 * (123120 / 12329085) - 328397";
            String formula2 = "2139123 / acda9 + _893213 * (123120 / 12329085) - 328397";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula == testFormula2);
        }

        /// <summary>
        /// Tests != on two different large formulas.
        /// </summary>
        [TestMethod]
        public void TestNotEqualsOperatorTrueLarge()
        {
            String formula = "2139123 / acda9 + _89313 * (123120 / 12329085) - 328397";
            String formula2 = "2139123 / acda9 + _893213 * (123120 / 12329085) - 328397";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsTrue(testFormula != testFormula2);
        }
        
        /// <summary>
        /// Tests != on two identical formulas.
        /// </summary>
        [TestMethod]
        public void TestNotEqualsOperatorFalse()
        {
            String formula = "33 + 15 / d2";
            String formula2 = "33 + 15 / d2";
            Formula testFormula = new Formula(formula);
            Formula testFormula2 = new Formula(formula2);
            Assert.IsFalse(testFormula != testFormula2);
        }

        /// <summary>
        /// Tests to string after an evaluation.
        /// </summary>
        [TestMethod]
        public void TestEvaluateThenToString()
        {
            String formula = "b3 + b4 * a2 / a6";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(4.0 + 4.0 * 4.0 / 4.0, testFormula.Evaluate(var => 4));
            Assert.AreEqual("b3+b4*a2/a6", testFormula.ToString());
        }

        #endregion

        #region - TEST DOUBLE ALTERNATIVE FORMAT -

        /// <summary>
        /// Test simple formula with decimal input number.
        /// </summary>
        [TestMethod]
        public void TestFormulaDecimal()
        {
            String formula = "2.000014";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("2.000014", testFormula.ToString());
        }

        /// <summary>
        /// Test simple formula with multiple decimal input numbers.
        /// </summary>
        [TestMethod]
        public void TestFormulaMultiDecimal()
        {
            String formula = "213.31 / 384.45 + 14.2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("213.31/384.45+14.2", testFormula.ToString());
        }

        /// <summary>
        /// Test simple formula with scientific notation input number.
        /// </summary>
        [TestMethod]
        public void TestFormulaScientificNotation()
        {
            String formula = "5e2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5e2", testFormula.ToString());
        }

        /// <summary>
        /// Test simple formula with multiple scientific notation input numbers.
        /// </summary>
        [TestMethod]
        public void TestFormulaMultiScientificNotation()
        {
            String formula = "5e2 / 10e3 + 10 * 3e10";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual("5e2/10e3+10*3e10", testFormula.ToString());
        }

        /// <summary>
        /// Test evaluate with decimal numbers.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDecimal()
        {
            String formula = "0.15109 + 2.323 / 0.485 + (31.32 / 10.0) - b2";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(0.15109 + 2.323 / 0.485 + (31.32 / 10.0) - 4.0, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Test evaluate with scientific notation.
        /// </summary>
        [TestMethod]
        public void TestEvaluateScientificNotation()
        {
            String formula = "4e3 / 10e2 + 15e10 - (b3 / 1e1)";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(4e3 / 10e2 + 15e10 - (4.0 / 1e1), testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Test evaluate with all types of numbers.
        /// </summary>
        [TestMethod]
        public void TestComplexEvaluation()
        {
            String formula = "4e3 + 2 / ((b3 + 15)/0.319) * 15e1 - (0.55 / _y5) * 2 + 5e5 + 2.00001";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(4e3 + 2 / ((4.0 + 15) / 0.319) * 15e1 - (0.55 / 4.0) * 2 + 5e5 + 2.00001, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Test evaluate with large decimals.
        /// </summary>
        [TestMethod]
        public void TestBigDecimal()
        {
            String formula = "2.000000000000000000000000000000000000000000000000000000000000001 + 5e1";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(2.0 + 5e1, testFormula.Evaluate(var => 4));
        }

        /// <summary>
        /// Test evaluate with negative scientific notation.
        /// </summary>
        [TestMethod]
        public void TestSmallScientificNotation()
        {
            String formula = "5e-5 + 10";
            Formula testFormula = new Formula(formula);
            Assert.AreEqual(5e-5 + 10, testFormula.Evaluate(var => 4));
        }

        #endregion
    }
}