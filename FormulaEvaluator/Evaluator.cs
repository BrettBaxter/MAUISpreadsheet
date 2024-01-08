using System.Data;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Author: Brett Baxter
    /// Partner: None
    /// Date: 1/12/2023
    /// Course: CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Brett Baxter - This work may not
    ///            Be copied for use in Academic Coursework.
    ///            
    /// I, Brett Baxter, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    /// 
    /// File Contents
    /// 
    /// This class contains two functions and a delegate: Evaluate, evaluateOperation, and Lookup.
    /// Evaluate is public and the main function, it takes in a string, being the user input expression, and
    /// a function to evaluate variables and outputs the answer. evaluateOperation is a private helper method
    /// that is used by Evaluate. Lookup is the delegate to help evaluate the value associated with variables
    /// input by the user.
    /// 
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// This delegate finds the number (if any) associated with the parameter variable name.
        /// </summary>
        /// <param name="variable_name"> The name of the variable to look up. </param>
        /// <returns> The number associated with the given variable. </returns>
        public delegate int Lookup(String variable_name);

        /// <summary>
        /// This function processes operators, integers, and variables from a given string input expression,
        /// separates them into stacks and then evaluates them into a final answer. If the input expression is
        /// invalid, such as division by zero, an invalid variable, too many values, or too many operators, an
        /// exception is thrown.
        /// </summary>
        /// <param name="expression"> The input expression to evaluate. </param>
        /// <param name="variableEvaluator"> The delegate to evaluate variables. </param>
        /// <returns> The answer to the given expression. </returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            #region - SETUP -
            // The final output variable.
            int output = 0;
            // Split the expression into usable tokens.
            expression = expression.Trim();
            string[] expressions = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            // Stacks to store parts of the expression.
            Stack<String> opStack = new Stack<String>();
            Stack<int> valStack = new Stack<int>();
            #endregion

            // Sort the tokens into their respective stacks, evaluate when needed.
            foreach (String value in expressions)
            {
                // Trim whitespace out of each value.
                String trimmedValue = value.Trim();
                #region - WHITESPACE -
                // If the current value is whitespace, continue to the next value.
                if (trimmedValue == " " || trimmedValue == "")
                {
                    continue;
                }
                #endregion

                #region - INTEGER CHECK -
                // Determine if the value is an integer or not.
                int num = 0;
                bool isNum = int.TryParse(trimmedValue, out num);
                // Determine if the value is a variable.
                bool isLetter = !String.IsNullOrEmpty(trimmedValue) && Char.IsLetter(trimmedValue[0]);
                // If the value is a number, push it to the value stack.
                if (isNum)
                {
                    // If there is no operator in the operator stack, e.g. this is the first number:
                    if(opStack.Count == 0)
                    {
                        valStack.Push(num);
                    }
                    // Check if * or / is at the top of the operator stack, evaluate.
                    else if(opStack.Peek() == "*" || opStack.Peek() == "/")
                    {
                        valStack.Push(num);
                        evaluateOperation(opStack, valStack);
                    }
                    // Otherwise add number to value stack.
                    else
                    {
                        valStack.Push(num);
                    }
                }
                #endregion

                #region - VARIABLE CHECK -
                // If the first character of the value is a letter, evaluate as variable:
                else if (isLetter)
                {
                    // ^ = at the start then [a-zA-z]* any letter for any amount then \d* any digit for any amount then $ = the end.
                    string pattern = @"^[a-zA-z]+\d+$";
                    Regex regexp = new Regex(pattern);
                    // If the value matches the pattern, get its value.
                    if(regexp.IsMatch(trimmedValue))
                    {
                        // Send the variable to the delegate to retrieve the real value.
                        int number = variableEvaluator(trimmedValue);
                        // If there is no operator in the operator stack, e.g. this is the first number:
                        if (opStack.Count == 0)
                        {
                            valStack.Push(number);
                        }
                        // Check if * or / is at the top of the operator stack, evaluate.
                        else if (opStack.Peek() == "*" || opStack.Peek() == "/")
                        {
                            valStack.Push(number);
                            evaluateOperation(opStack, valStack);
                        }
                        // Otherwise add number to value stack.
                        else
                        {
                            valStack.Push(number);
                        }
                    }
                }
                #endregion

                #region - +-*/ CHECK -
                // If the value is a + or -.
                else if (trimmedValue == "+" || trimmedValue == "-")
                {
                    // If this is the first operator, push operator to stack.
                    if(opStack.Count == 0)
                    {
                        opStack.Push(trimmedValue);
                    }
                    // If the top of the operator stack is a + or -, evaluate with the previous + or -, then push this one to the top.
                    else if(opStack.Peek() == "+" || opStack.Peek() == "-")
                    {
                        evaluateOperation(opStack, valStack);
                        opStack.Push(trimmedValue);
                    }
                    // Otherwise, push to operator stack.
                    else
                    {
                        opStack.Push(trimmedValue);
                    }
                }
                // If the value is a * or /, push it to the stack.
                else if (trimmedValue == "*" || trimmedValue == "/")
                {
                    opStack.Push(trimmedValue);
                }
                #endregion

                #region - PARENTHESIS CHECK -
                // If the value is an opening parenthesis, push it to the operator stack.
                else if (value == "(")
                {
                    opStack.Push(value);
                }
                // Handle left parenthesis (special)
                else if (value == ")")
                {
                    // If the operator stack is empty, then something went wrong, there should always be atleast a right parenthesis.
                    if (opStack.Count == 0)
                    {
                        throw new ArgumentException();
                    }
                    // If operator at top of stack is + or -, evaluate and then pop the (.
                    else if (opStack.Peek() == "+" || opStack.Peek() == "-")
                    {
                        evaluateOperation(opStack, valStack);
                        // If the operator stack is at 0 after the evaluation (meaning there was no left parenthesis, throw and exception.
                        if (opStack.Count == 0)
                        {
                            throw new ArgumentException();
                        }
                        opStack.Pop();

                        // If the operator stack is not empty after removing the left parenthesis:
                        if (opStack.Count != 0)
                        {
                            // If the operator is multiply or divide, evaluate the final value in the parenthesis with the next value in the stack.
                            if (opStack.Peek() == "*" || opStack.Peek() == "/")
                            {
                                evaluateOperation(opStack, valStack);
                            }
                        }
                    }
                    // If operator at top of stack is * or /, evaluate.
                    else if (opStack.Peek() == "*" || opStack.Peek() == "/")
                    {
                        evaluateOperation(opStack, valStack);
                    }
                    // If operator at top of stack is the left parenthesis, simply remove it.
                    else if (opStack.Peek() == "(")
                    {
                        opStack.Pop();

                        // If the operator stack is not empty after removing the left parenthesis:
                        if(opStack.Count != 0)
                        {
                            // If the operator is multiply or divide, evaluate the final value in the parenthesis with the next value in the stack.
                            if (opStack.Peek() == "*" || opStack.Peek() == "/")
                            {
                                evaluateOperation(opStack, valStack);
                            }
                        }
                    }
                }
                #endregion
            }
            // While there are still operations to perform, evaluate them.
            while (opStack.Count > 0)
            {
                evaluateOperation(opStack, valStack);
            }
            #region - RETURNING -
            // If the value stack is empty or greater than 1, a valid expression was not given, therefore throw an exception.
            if (valStack.Count == 0 || valStack.Count > 1)
            {
                throw new ArgumentException();
            }
            // Otherwise, return the last item in the value stack.
            else
            {
                output = valStack.Pop();
                return output;
            }
            #endregion
        }

        /// <summary>
        /// This helper function, given a stack of operators and values will add the correct evaluation to the value stack
        /// based on the top operator in the operator stack and the top two values in the value stack.
        /// </summary>
        /// <param name="operators"> A stack of operators to pull from. </param>
        /// <param name="values"> A stack of values to pull from. </param>
        /// <exception cref="ArgumentException"> If the operator is division, and the divisor is 0. </exception>
        private static void evaluateOperation(Stack<String> operators, Stack<int> values)
        {
            // If there aren't enough values, or operators to evaluate the operation, throw an argument exception.
            if(values.Count < 2 || operators.Count < 1)
            {
                throw new ArgumentException();
            }
            else
            {
                // The left operand.
                int y = values.Pop();
                // The right operand.
                int x = values.Pop();
                // The final answer.
                int answer = 0;
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
                    // Check if the divisor is 0.
                    if (y == 0)
                    {
                        throw new ArgumentException();
                    }
                    // If the divisor is not 0 then divide like normal.
                    else
                    {
                        answer = x / y;
                    }
                }
                // Push the final value to the value stack.
                values.Push(answer);
            }
        }
    }
}