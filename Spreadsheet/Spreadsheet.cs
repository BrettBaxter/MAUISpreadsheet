// Implemented by: Brett Baxter - u1310459 - (2/15/2023)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using SS;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {

        // A dictionary that maps the cell name to the cell object.
        Dictionary<string, Cell> cells;
        // A dependency graph for the cells.
        DependencyGraph dg;
        // Tells whether the spreadsheet has been modified or not.
        public override bool Changed { get; protected set; }

        /// <summary>
        /// No arguments constructor.
        /// All inputs that meet the spreadsheet definition of an input are valid.
        /// Does not normalize variables.
        /// Default versioning.
        /// </summary>
        public Spreadsheet() : base(s=> true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Constructor with inputs for validity function, normalize function, and custom versioning scheme.
        /// </summary>
        /// <param name="isValid"> Additional requirements for valid inputs. </param>
        /// <param name="normalize"> Changes input into a standard format. </param>
        /// <param name="version"> Custom name scheme for versioning. </param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Constructor with inputs for a file path, validity function, normalize function, and custom versioning scheme.
        /// Creates a spreadsheet based on the file in the filepath param.
        /// </summary>
        /// <param name="filePath"> The filepath to the file to read. </param>
        /// <param name="isValid"> Additional requirements for valid inputs. </param>
        /// <param name="normalize"> Changes input into a standard format. </param>
        /// <param name="version"> Custom name scheme for versioning. </param>
        /// <exception cref="SpreadsheetReadWriteException"> If the versions do not match. </exception>
        public Spreadsheet(String filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            // Check for empty version names, filepaths, or mismatching versioning schemes.
            if (filePath == "")
            {
                throw new SpreadsheetReadWriteException("Error! File path cannot be empty!");
            }
            if (version != GetSavedVersion(filePath))
            {
                throw new SpreadsheetReadWriteException("Error! Version Mismatch!");
            }
            // Try reading through the whole file from the filepath and adding that data to the new spreadsheet.
            try
            {
                using(XmlReader fileReader = XmlReader.Create(filePath))
                {
                    string name = "";
                    string contents = "";
                    while(fileReader.Read())
                    {
                        if (fileReader.IsStartElement())
                        {
                            bool validContents = false;
                            if(fileReader.Name == "spreadsheet")
                            {
                                if (fileReader["version"] != null)
                                {
                                    this.Version = fileReader["version"];
                                }
                                else
                                {
                                    throw new SpreadsheetReadWriteException("Error! File path version is null!");
                                }
                            }
                            if(fileReader.Name == "cell")
                            {

                            }
                            if(fileReader.Name == "name")
                            {
                                fileReader.Read();
                                name = fileReader.Value;
                            }
                            if(fileReader.Name == "contents")
                            {
                                fileReader.Read();
                                contents = fileReader.Value;
                                validContents = true;
                            }
                            if (validContents)
                            {
                                SetContentsOfCell(name, contents);
                            }
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch(IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            Changed = false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="InvalidNameException"><inheritdoc/></exception>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);
            // Check name validity.
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell output;
            if (cells.TryGetValue(name, out output))
            {
                return output.contents;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="InvalidNameException"><inheritdoc/></exception>
        public override object GetCellValue(string name)
        {
            name = Normalize(name);
            // Check name validity.
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell? output;
            if(cells.TryGetValue(name, out output))
            {
                return output.value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="filename"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="SpreadsheetReadWriteException"><inheritdoc/></exception>
        public override string GetSavedVersion(string filename)
        {
            string? version = "";
            // Check filename validity.
            if(filename == "")
            {
                throw new SpreadsheetReadWriteException("The filename cannot be empty!");
            }
            try
            {
                using(XmlReader fileReader = XmlReader.Create(filename))
                {
                    // Read the file to the spreadsheet xml tag name, get the version.
                    while(fileReader.Read())
                    {
                        if (fileReader.IsStartElement())
                        {
                            if(fileReader.Name == "spreadsheet")
                            {
                                version = fileReader["version"];
                            }
                        }
                    }
                }
            }
            catch(XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            if (version is null)
            {
                throw new SpreadsheetReadWriteException("Error! Version is null!");
            }
            return version;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="filename"><inheritdoc/></param>
        /// <exception cref="SpreadsheetReadWriteException"><inheritdoc/></exception>
        public override void Save(string filename)
        {
            // Check filename validity.
            if (filename == "")
            {
                throw new SpreadsheetReadWriteException("The filename cannot be empty!");
            }
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using(XmlWriter fileWriter = XmlWriter.Create(filename, settings))
                {
                    // Write the starter with the spreadsheet tag and the version.
                    fileWriter.WriteStartDocument();
                    fileWriter.WriteStartElement("spreadsheet");
                    fileWriter.WriteAttributeString("version", null, Version);
                    // Loop through all the cells in the spreadsheet, check the type, convert it to string and make an xml section for it.
                    foreach(string cell in cells.Keys)
                    {
                        fileWriter.WriteStartElement("cell");
                        fileWriter.WriteElementString("name", cell);
                        string cellContents;
                        if (cells[cell].contents is double)
                        {
                            cellContents = cells[cell].contents.ToString();
                        }
                        else if (cells[cell].contents is Formula)
                        {
                            cellContents = "=" + cells[cell].contents.ToString();
                        }
                        else
                        {
                            cellContents = (string)cells[cell].contents;
                        }

                        fileWriter.WriteElementString("contents", cellContents);
                        fileWriter.WriteEndElement();
                    }
                    // End the xml doc.
                    fileWriter.WriteEndElement();
                    fileWriter.WriteEndDocument();
                }
            }
            catch (XmlException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            catch (IOException e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            // We saved so there are no new changes to the version.
            Changed = false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="content"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="InvalidNameException"><inheritdoc/></exception>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);
            // Check name validity.
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            List<string> dependents = new List<string>();
            double number;
            // If the string is empty, add it to the cell.
            if (content == "")
            {
                dependents = new List<string>(SetCellContents(name, content));
            }
            // If the string is a double, add it to the cell.
            else if (Double.TryParse(content, out number))
            {
                dependents = new List<string>(SetCellContents(name, number));
            }
            // If the string begins with an =, this indicates a formula.
            else if(content.Substring(0, 1) == "=")
            {
                string formulaString = content.Substring(1, content.Length - 1);
                Formula formula = new Formula(formulaString, Normalize, IsValid);
                dependents = new List<string>(SetCellContents(name, formula));
            }
            // Otherwise its going to be a string, add it to the cell.
            else
            {
                dependents = new List<string>(SetCellContents(name, content));
            }
            // We modified the spreadsheet so set to true.
            Changed = true;

            foreach (string dependent in dependents)
            {
                Cell cell;
                if(cells.TryGetValue(dependent, out cell))
                {
                    cell.evaluateCellContents(lookup);
                }
            }
            
            return dependents;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="InvalidNameException"><inheritdoc/></exception>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            name = Normalize(name);
            // Check name validity.
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            return dg.GetDependents(name);
        }

        #region - PROTECTED SET CELL METHODS -
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="number"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            Cell cell = new Cell(number);
            // If the dictionary already contains the cell name, reset its value to the new cell.
            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            // Otherwise add a new name and cell to the dictionary.
            else
            {
                cells.Add(name, cell);
            }

            dg.ReplaceDependees(name, new HashSet<String>());
            // Recalculate dependency graph.
            List<String> dependees = new List<String>(GetCellsToRecalculate(name));
            return dependees;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="text"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            Cell cell = new Cell(text);
            // If the dictionary already contains the cell name, reset its value to the new cell.
            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            // Otherwise add a new name and cell to the dictionary.
            else
            {
                cells.Add(name, cell);
            }
            // If the contents of the cell is an empty string, remove it from the dictionary.
            if ((string)cells[name].contents == "")
            {
                cells.Remove(name);
            }

            dg.ReplaceDependees(name, new HashSet<String>());

            List<String> dependees = new List<String>(GetCellsToRecalculate(name));
            return dependees;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="formula"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="CircularException"><inheritdoc/></exception>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            // Save the old dependees just in case we encounter a circular exception.
            IEnumerable<string> oldDependees = dg.GetDependees(name);
            // Replace the dependees with the variables in the new formula.
            dg.ReplaceDependees(name, formula.GetVariables());
            try
            {
                List<string> newDependees = new List<string>(GetCellsToRecalculate(name));
                Cell cell = new Cell(formula, lookup);
                // If the dictionary already contains the cell name, reset its value to the new cell.
                if (cells.ContainsKey(name))
                {
                    cells[name] = cell;
                }
                // Otherwise add a new name and cell to the dictionary.
                else
                {
                    cells.Add(name, cell);
                }
                // If the contents of the cell is an empty string, remove it from the dictionary.
                return newDependees;
            }
            // If a circular exception is encountered, replace the dependees again with the old ones, and throw an exception.
            catch (CircularException e)
            {
                dg.ReplaceDependees(name, oldDependees);
                throw new CircularException();
            }
        }
        #endregion

        /// <summary>
        /// Checks if the cell name fits the criteria for a valid name.
        /// </summary>
        /// <param name="name"> The name to check validity. </param>
        /// <returns> True if valid, false if not. </returns>
        private bool IsValidName(string name)
        {
            // Pattern taken from A3 variable name pattern.
            String namePattern = @"^[a-zA-z_]+\d*$";
            // @"[a-zA-Z_](?: [a-zA-Z_]|\d)*$";
            Regex regexp = new Regex(namePattern);
            // If the name matches the pattern, it is valid.
            if (regexp.IsMatch(name))
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
        /// Returns the value of the cell if it is in the spreadsheet.
        /// </summary>
        /// <param name="s"> The name of the cell. </param>
        /// <returns> The value of the cell associated with the name s.</returns>
        /// <exception cref="ArgumentException"> If the value of the cell is not a number. </exception>
        private double lookup(string s)
        {
            Cell cell;
            // If the cell name is in the dictionary:
            if(cells.TryGetValue(s, out cell))
            {
                // If the value of the cell is a double, return it.
                if(cell.value is double)
                {
                    return (double)cell.value;
                }
                // Otherwise throw an argument exception.
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Cell object contains contents and values, can be created with strings, doubles, or formulas.
        /// </summary>
        private class Cell
        {
            // What goes in the text input box of the spreadsheet.
            public Object contents { get; private set; }

            // What goes in the cells of the spreadsheet.
            public Object value { get; private set; }

            /// <summary>
            /// Constructor with a string input.
            /// </summary>
            /// <param name="input"> For string values. </param>
            public Cell(string input)
            {
                contents = input;
                value = input;
            }

            /// <summary>
            /// Constructor with a double input.
            /// </summary>
            /// <param name="input"> For double values. </param>
            public Cell(double input)
            {
                contents = input;
                value = input;
            }

            /// <summary>
            /// Constructor with a formula input.
            /// </summary>
            /// <param name="input"> For formula values. </param>
            /// <param name="lookup"> A function to lookup the values of variables. </param>
            public Cell(Formula input, Func<string, double> lookup)
            {
                contents = input;
                value = input.Evaluate(lookup);
            }

            /// <summary>
            /// If the contents of a cell is a formula, evaluate the formula.
            /// </summary>
            /// <param name="lookup"> A function for looking up variables. </param>
            public void evaluateCellContents(Func<string, double> lookup)
            {
                if (contents is Formula)
                {
                    Formula formula = (Formula)contents;
                    value = formula.Evaluate(lookup);
                }
            }
        }
    }
}
