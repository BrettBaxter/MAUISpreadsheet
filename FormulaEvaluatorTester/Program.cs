using FormulaEvaluator;
using System.Linq.Expressions;

///<summary>
/// A simple test lookup that receives a variable given to it from the evaluator class.
/// Only returns a number for two test variables.
///</summary>
///<param name="value"> The variable passed into the function. </param>
///<returns> The int associated with the variable. </returns>
static int TestLookup(String value)
{
    if (value == "Z6")
    {
        return 20;
    }
    else if(value == "G6")
    {
        return 62;
    }
    else
    {
        return 0;
    }
}

///<summary>
/// Tests simple, single operator expressions such as: no operation, single addition, subtraction, multiplication,
/// division, also flips values to check if subtraction and division evaluates properly.
///</summary>
static void simpleExpressions()
{
    // Testing with no operators
    String expression = "5";
    int answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing single addition
    expression = "5 + 5";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 10 actual: " + answer);

    // Testing single subtraction
    expression = "5 - 10";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: -5 actual: " + answer);

    // Testing single subtraction reversal
    expression = "10 - 5";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing single multiplication
    expression = "5 * 4";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 20 actual: " + answer);

    // Testing single division
    expression = "5 / 1";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing single division reversal
    expression = "1 / 5";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 0 actual: " + answer);
}

///<summary>
/// Tests multiple operators in expressions, but they are the same. Tests for 
/// multiple addition, subtraction, multiplication, and division.
///</summary>
static void multipleSameOperations()
{
    // Testing multiple addition
    String expression = "5 + 5 + 10 + 20";
    int answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 40 actual: " + answer);
    // Testing multiple subtraction
    expression = "5 - 4 - 1 - 10";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: -10 actual: " + answer);
    // Testing multiple multiplication
    expression = "5 * 10 * 10 * 2";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 1000 actual: " + answer);
    // Testing multiple division
    expression = "100 / 10 / 2 / 2";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 2 actual: " + answer);
}

///<summary>
/// Tests expressions with multiple different operators.
///</summary>
static void multipleDiffOperations()
{
    // Testing one of each operation.
    String expression = "5 + 5 - 4 * 100 / 2";
    int answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: -190 actual: " + answer);
    // Testing multiple of each operation.
    expression = "5 - 4 * 20 / 5 + 16 * 2 / 2";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);
}

///<summary>
/// Tests expressions with parenthesis, including nested parenthesis.
///</summary>
static void parenthesisOperations()
{
    // Testing parenthesis
    String expression = "5 + (5-5)";
    int answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing parenthesis 2
    expression = "(5+5)-5";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing parenthesis 3
    expression = "10 + 3 - 3 * (5 + 10) / 3";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: -2 actual: " + answer);

    // Testing nested parenthesis
    expression = "5 + (5 + (5 - 5))";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 10 actual: " + answer);

    // Testing parenthesis first
    expression = "(5+5) - 5";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 5 actual: " + answer);

    // Testing illegal parenthesis
    expression = "8 * () + 2";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }
}

///<summary>
/// Tests to make sure invalid expressions throw the correct exceptions. Tests for
/// invalid format, too many operators, too many values, incorrect variables, and divide by zero exceptions.
///</summary>
static void errorOperations()
{
    int answer = 0;
    // Testing divide by zero
    String expression = "12309/0";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Success! Caught divide by zero exception!");
    }

    //Testing too many operators
    expression = "(5+5)-5+";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }

    //Testing too many operands
    expression = "(5+5)-5+5 5 5";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }

    //Testing illegal value
    expression = "1ab2f";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }

    //Testing illegal value 2
    expression = "5 + 10 + 2 + 12bas2";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }
}

///<summary>
/// Tests expressions with variables, tests with correct variables that are assigned a value by the lookup function,
/// and invalid ones that throw an argument exception.
///</summary>
static void variableTests()
{
    // Testing valid variable
    String expression = "5+Z6";
    int answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 25 actual: " + answer);

    // Testing valid variable 2
    expression = "10 * G6";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 620 actual: " + answer);

    // Testing valid variable 3
    expression = "10 * A1";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 0 actual: " + answer);

    // Testing valid variable 4
    expression = "10 * AAAAAAAAADDDFSD112312345";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 0 actual: " + answer);

    // Testing valid variable 5
    expression = "10 * adc125";
    answer = Evaluator.Evaluate(expression, TestLookup);
    Console.WriteLine("expected: 0 actual: " + answer);

    // Test invalid variable 1
    expression = "5 + 10 + 2 + 12bas2";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }

    // Test invalid variable 2
    expression = "5 + 10 + 2 + ba2a";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }

    // Test invalid variable 2
    expression = "5 + 10 + 2 + ba2a2";
    try
    {
        answer = Evaluator.Evaluate(expression, TestLookup);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Success! Caught argument exception!");
    }
}

///<summary>
/// Main function runs all the above test functions.
///</summary>
///<param name="args"> </param>
static void Main(String[] args)
{
    simpleExpressions();
    multipleSameOperations();
    multipleDiffOperations();
    parenthesisOperations();
    errorOperations();
    variableTests();
}

Main(null);