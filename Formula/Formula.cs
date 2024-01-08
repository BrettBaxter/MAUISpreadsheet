// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

// Implemented by: Brett Baxter (2/1/2023)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        // Stores the individual formula tokens.
        private List<String> tokens;
        // Stores the normalized variables.
        private List<String> normalizedVariables;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        /// <param name="formula"> The input formula. </param>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        /// <param name="formula"> The input formula. </param>
        /// <param name="isValid"> The validation function. </param>
        /// <param name="normalize"> The normalizer function. </param>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            // initialized the tokens list with a new list with all tokens in the formula string.
            tokens = new List<String>(GetTokens(formula));

            // A list of normalized variables to be used for a later function.
            normalizedVariables = new List<String>();

            // Check if the tokens list is empty.
            if(tokens.Count == 0)
            {
                throw new FormulaFormatException("Error, empty string input! Please provide a valid formula!");
            }

            #region - CHECK VALIDITY -
            // Check the beginning of the formula, if it is not a number, variable, or opening parenthesis, throw an exception.
            if (!isVariable(tokens[0]))
            {
                if (!isDouble(tokens[0]))
                {
                    if(tokens[0] != "(")
                    {
                        throw new FormulaFormatException("Error, improper formatting! Formula does not begin correctly!");
                    }
                }
            }
            // Check the end of the formula, if it is not a number, variable, or closing parenthesis, throw and exception.
            if (!isVariable(tokens[tokens.Count - 1]))
            {
                if(!isDouble(tokens[tokens.Count - 1]))
                {
                    if(tokens[tokens.Count - 1] != ")")
                    {
                        throw new FormulaFormatException("Error, improper formatting! Formula does not end correctly!");
                    }
                }
            }

            // Keep track of the parenthesis!
            int openParenthesisCount = 0;
            int closeParenthesisCount = 0;

            // Loop through all the tokens.
            for (int i = 0; i < tokens.Count; i++)
            {
                // The current token.
                String token = tokens[i];
                // The previous token.
                String previousToken = "";
                // Set previous token when index is not 0.
                if (i > 0)
                {
                    previousToken = tokens[i - 1];
                }
                // If the token is a double, variable, or operator, skip to next.
                if (isDouble(token) || isVariable(token) || isOperator(token))
                {

                }
                // If the token is an opening parenthesis: increment it's counter.
                else if (token == "(")
                {
                    openParenthesisCount++;
                }
                // If the token is a closing parenthesis: increment it's counter.
                else if (token == ")")
                {
                    closeParenthesisCount++;
                    // Check right parenthesis rule.
                    if(closeParenthesisCount > openParenthesisCount)
                    {
                        throw new FormulaFormatException("Error! Right parenthesis rule violation! Check the number of parenthesis in your formula!");
                    }
                }
                // If the token was none of the above, throw an error.
                else
                {
                    throw new FormulaFormatException("Error! Invalid character detected in formula! Formula should only contain: doubles, variables, (, ), +, -, *, and /.");
                }
                // If the previous token was an opening parenthesis or operator:
                if (previousToken == "(" || isOperator(previousToken))
                {
                    // If the current token is not a double, variable, or another opening parenthesis, throw an exception.
                    if (isDouble(token) || isVariable(token) || token == "(")
                    {

                    }
                    else
                    {
                        throw new FormulaFormatException("Error! An opening parenthesis or operator must be followed up by a number, variable, or another opening parenthesis!");
                    }
                }
                // If the previous token was a closing parenthesis, double, or variable:
                else if (previousToken == ")" || isDouble(previousToken) || isVariable(previousToken))
                {
                    // If the current token is not a closing parenthesis or operator, throw an exception.
                    if (token == ")" || isOperator(token))
                    {

                    }
                    else
                    {
                        throw new FormulaFormatException("Error! A closing parenthesis must be followed up by another closing parenthesis or an operator!");
                    }
                }
            }

            // Check if the number of parenthesis don't match up.
            if (openParenthesisCount != closeParenthesisCount)
            {
                throw new FormulaFormatException("Error! Opening and closing parenthesis count does not match! Check for missing or extra parenthesis.");
            }

            #endregion

            #region - NORMALIZE -
            // Now that we have verified the formatting is correct, we can normalize the tokens.
            for (int i = 0; i < tokens.Count; i++)
            {
                // The current token.
                String token = tokens[i];
                // If the token is a variable:
                if (isVariable(token))
                {
                    // If the normalized variable is not valid, throw exception
                    if (!(isVariable(normalize(token))))
                    {
                        throw new FormulaFormatException("Error! The normalized variable is not valid!");
                    }
                    // If the normalized variable does not meet the isValid guidelines, throw an exception.
                    if (!(isValid(normalize(token))))
                    {
                        throw new FormulaFormatException("Error! The normalized variable does not meet the given validity guidelines!");
                    }
                    else
                    {
                        // If the variable is valid, replace the old variable in the list with the normalized version.
                        tokens[i] = normalize(token);
                        // Add to the normalized variables list.
                        normalizedVariables.Add(tokens[i]);
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        /// <param name="lookup"> The lookup function for variables. </param>
        /// <returns> An object, whether that be a final answer (double) or an error (divide by zero or invalid variable). </returns>
        public object Evaluate(Func<string, double> lookup)
        {
            // Copy the tokens list.
            List<String> expressions = new List<String>(tokens);
            // Create a stack for operators and a stack for values.
            Stack<String> opStack = new Stack<String>();
            Stack<double> valStack = new Stack<double>();

            // Sort the tokens into their respective stacks, evaluate when needed.
            foreach (String value in expressions)
            {
                #region - INTEGER CHECK -
                // Determine if the value is a double or not.
                double num = 0;
                bool isNum = Double.TryParse(value, out num);
                // If the value is a number, push it to the value stack.
                if (isNum)
                {
                    // If there is no operator in the operator stack, e.g. this is the first number:
                    if (opStack.Count == 0)
                    {
                        valStack.Push(num);
                    }
                    // Check if * or / is at the top of the operator stack, evaluate.
                    else if (opStack.Peek() == "*" || opStack.Peek() == "/")
                    {
                        valStack.Push(num);
                        if(opStack.Peek() == "/")
                        {
                            // Ensure a division by zero cannot occur.
                            if (valStack.Peek() == 0.0)
                            {
                                return new FormulaError("Error! Division by zero occurs!");
                            }
                            evaluateOperation(opStack, valStack);
                        }
                        else
                        {
                            evaluateOperation(opStack, valStack);
                        }
                    }
                    // Otherwise add number to value stack.
                    else
                    {
                        valStack.Push(num);
                    }
                }
                #endregion

                #region - VARIABLE CHECK -
                // If the value is a variable:
                else if (isVariable(value))
                {
                    // Try to send the variable to the delegate to retrieve the real value.
                    try
                    {
                        Double number = lookup(value);
                        // If there is no operator in the operator stack, e.g. this is the first number:
                        if (opStack.Count == 0)
                        {
                            valStack.Push(number);
                        }
                        // Check if * or / is at the top of the operator stack, evaluate.
                        else if (opStack.Peek() == "*" || opStack.Peek() == "/")
                        {
                            valStack.Push(number);
                            if (opStack.Peek() == "/")
                            {
                                // Ensure a division by zero cannot occur.
                                if (valStack.Peek() == 0.0)
                                {
                                    return new FormulaError("Error! Division by zero occurs!");
                                }
                                evaluateOperation(opStack, valStack);
                            }
                            else
                            {
                                evaluateOperation(opStack, valStack);
                            }
                        }
                        // Otherwise add number to value stack.
                        else
                        {
                            valStack.Push(number);
                        }
                    }
                    catch
                    {
                        return new FormulaError("Error! Invalid variable value!");
                    }
                }
                #endregion

                #region - +-*/ CHECK -
                // If the value is a + or -.
                else if (value == "+" || value == "-")
                {
                    // If this is the first operator, push operator to stack.
                    if (opStack.Count == 0)
                    {
                        opStack.Push(value);
                    }
                    // If the top of the operator stack is a + or -, evaluate with the previous + or -, then push this one to the top.
                    else if (opStack.Peek() == "+" || opStack.Peek() == "-")
                    {
                        evaluateOperation(opStack, valStack);
                        opStack.Push(value);
                    }
                    // Otherwise, push to operator stack.
                    else
                    {
                        opStack.Push(value);
                    }
                }
                // If the value is a * or /, push it to the stack.
                else if (value == "*" || value == "/")
                {
                    opStack.Push(value);
                }
                #endregion

                #region - PARENTHESIS CHECK -
                // If the value is an opening parenthesis, push it to the operator stack.
                else if (value == "(")
                {
                    opStack.Push(value);
                }
                // Handle closing parenthesis.
                else if (value == ")")
                {
                    // If operator at top of stack is + or -, evaluate and then pop the (.
                    if (opStack.Peek() == "+" || opStack.Peek() == "-")
                    {
                        evaluateOperation(opStack, valStack);
                        opStack.Pop();
                    }
                    // If operator at top of stack is the left parenthesis, simply remove it.
                    else if (opStack.Peek() == "(")
                    {
                        opStack.Pop();

                        // If the operator stack is not empty after removing the left parenthesis:
                        if (opStack.Count != 0)
                        {
                            // If the operator is multiply or divide, evaluate the final value in the parenthesis with the next value in the stack.
                            if (opStack.Peek() == "*" || opStack.Peek() == "/")
                            {
                                if(opStack.Peek() == "/")
                                {
                                    // Ensure a division by zero cannot occur.
                                    if (valStack.Peek() == 0.0)
                                    {
                                        return new FormulaError("Error! Division by zero occurs!");
                                    }
                                    evaluateOperation(opStack, valStack);
                                }
                                else
                                {
                                    evaluateOperation(opStack, valStack);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            // While there are still operations to perform, evaluate them.
            while (opStack.Count > 0)
            {
                // Check for divide by zero.
                if(opStack.Peek() == "/")
                {
                    if (valStack.Peek() == 0.0)
                    {
                        return new FormulaError("Error! Division by zero occurs!");
                    }
                }
                evaluateOperation(opStack, valStack);
            }

            // return last value on the stack.
            Double output = valStack.Pop();
            return output;

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        /// <returns> A list of all variables in the function. </returns>
        public IEnumerable<String> GetVariables()
        {
            // return a copy of the normalizedVariables list.
            return new HashSet<String>(normalizedVariables);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        /// <returns> The formula in a standard string format. </returns>
        public override string ToString()
        {
            // Loop through tokens and append them to the output string.
            String output = "";
            foreach(String token in tokens)
            {
                output += token;
            }
            return output;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        /// <param name="obj"> The object to check if equal. </param>
        /// <returns> If the objects are the same: true, if not false. </returns>
        public override bool Equals(object? obj)
        {
            // If the type of object is not formula: return false.
            if(!(obj is Formula))
            {
                return false;
            }

            Formula other = (Formula)obj;
            // Loop through this objects tokens:
            for (int i = 0; i < this.tokens.Count; i++)
            {
                // The current token for both formulas.
                String thisToken = this.tokens[i];
                String otherToken = other.tokens[i];

                // Storage for if the token is a double.
                Double thisDouble, otherDouble;

                // If both tokens are doubles:
                if (Double.TryParse(thisToken, out thisDouble) && Double.TryParse(otherToken, out otherDouble))
                {
                    // If the doubles do not equal each other: return false.
                    if (thisDouble != otherDouble)
                    {
                        return false;
                    }
                }
                else
                {
                    // If the tokens are not doubles, but some other operator or variable, if they do not equal: return false.
                    if (!thisToken.Equals(otherToken))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        /// <param name="f1"> The formula on the left. </param>
        /// <param name="f2"> The formula on the right. </param>
        /// <returns> True if the two are equal, false if not. </returns>
        public static bool operator ==(Formula f1, Formula f2)
        {
            // This depends on Equals method working.
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <param name="f1"> The formula on the left. </param>
        /// <param name="f2"> The formula on the right. </param>
        /// <returns> If the formulas are not equal: true, if they are equal: false. </returns>
        public static bool operator !=(Formula f1, Formula f2)
        {
            // This depends on == working.
            if(f1 == f2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        /// <returns> A unique hash code. </returns>
        public override int GetHashCode()
        {
            // Loop through all of the tokens, if the token is a double normalize it using the built in double parse function.
            List<string> tokens = new List<string>(this.tokens);
            string output = "";
            foreach(string token in tokens)
            {
                if (isDouble(token))
                {
                    output += Double.Parse(token);
                }
                else
                {
                    output += token;
                }
            }
            // return the hashcode of the string of tokens.
            return output.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// This helper function, given a stack of operators and values will add the correct evaluation to the value stack
        /// based on the top operator in the operator stack and the top two values in the value stack.
        /// </summary>
        /// <param name="operators"> A stack of operators to pull from. </param>
        /// <param name="values"> A stack of values to pull from. </param>
        private static void evaluateOperation(Stack<String> operators, Stack<Double> values)
        {
            // The left operand.
            Double y = values.Pop();
            // The right operand.
            Double x = values.Pop();
            // The final answer.
            Double answer = 0;
            // The operator.
            String op = operators.Pop();
            // Check which operator op is and calculate the answer.
            if (op == "+")
            {
                answer = x + y;
            }
            else if (op == "-")
            {
                answer = x - y;
            }
            if (op == "*")
            {
                answer = x * y;
            }
            if (op == "/")
            {
                answer = x / y;
            }
            // Push the final value to the value stack.
            values.Push(answer);
        }

        /// <summary>
        /// Returns if the input string is in the variable format or not.
        /// </summary>
        /// <param name="input"> The input token string. </param>
        /// <returns> True if it is a variable, false if not. </returns>
        private static bool isVariable(String input)
        {
            //string pattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            string pattern = @"^[a-zA-z_]+\d*$";
            Regex regexp = new Regex(pattern);
            // If the value matches the variable pattern, return true.
            if (regexp.IsMatch(input))
            {
                return true;
            }
            // Otherwise return false.
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns if the token string input is a double or not.
        /// </summary>
        /// <param name="input"> The Token to check. </param>
        /// <returns> If the token is a double return true, otherwise return false. </returns>
        private static bool isDouble(String input)
        {
            double output;
            return (Double.TryParse(input, out output));
        }

        /// <summary>
        /// Returns if the token string input is an operator or not.
        /// </summary>
        /// <param name="input"> The Token to check. </param>
        /// <returns> If the token is an operator return true, otherwise return false. </returns>
        private static bool isOperator(String input)
        {
            string pattern = @"^[\+\-*/]$";
            Regex regexp = new Regex(pattern);
            // If the input is a match to the pattern return true.
            if (regexp.IsMatch(input))
            {
                return true;
            }
            // Otherwise return false.
            else
            {
                return false;
            }
        }
    }
}

  /// <summary>
  /// Used to report syntactic errors in the argument to the Formula constructor.
  /// </summary>
  public class FormulaFormatException : Exception
  {
    /// <summary>
    /// Constructs a FormulaFormatException containing the explanatory message.
    /// </summary>
    public FormulaFormatException(String message)
        : base(message)
    {
    }
  }

  /// <summary>
  /// Used as a possible return value of the Formula.Evaluate method.
  /// </summary>
  public struct FormulaError
  {
    /// <summary>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </summary>
    /// <param name="reason"></param>
    public FormulaError(String reason)
        : this()
    {
      Reason = reason;
    }

    /// <summary>
    ///  The reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
