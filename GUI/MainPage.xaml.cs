// Implemented by Brett Baxter - u1310459

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using SpreadsheetUtilities;
using System.Reflection.Metadata.Ecma335;
using SS;

namespace GUI
{
    /// <summary>
    /// The Main spreadsheet page, includes setting up the grid and all the functionality for the widgets.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // Determines the horizontal bound of the spreadsheet (only works from A to Z)
        char horizBound = 'Z';
        // Determines the vertical bound of the spreadsheet.
        int vertBound = 99;
        // The current spreadsheet. On main page initialization this is a default spreadsheet with no input parameters.
        Spreadsheet spreadsheet;
        // Dictionary of cells consisting of the cell ID and the entry associated with it.
        Dictionary<string, Entry> cells;
        // The current file name.
        string fileName;

        /// <summary>
        /// Constructor.
        /// Initialize the cell dictionary, Initialize component and the grid, Initialize the default spreadsheet.
        /// </summary>
        public MainPage()
        {
            cells = new Dictionary<string, Entry>();
            InitializeComponent();
            InitializeGrid();
            spreadsheet = new Spreadsheet(s => true, s => s, "six");
            fileName = "";
            currentSpreadsheet.Text = "No save name...";
        }

        /// <summary>
        /// Adds a horizontal stack top row for all letters from A to the horizontal bound.
        /// </summary>
        public void InitializeGrid()
        {
            // Top left corner placeholder cell.
            TopLabels.Add(
                   new Border
                   {
                       Stroke = Color.FromRgb(0, 0, 0),
                       StrokeThickness = 1,
                       HeightRequest = 20,
                       WidthRequest = 75,
                       HorizontalOptions = LayoutOptions.Center,
                       Content =
                           new Label
                           {
                               Text = "-",
                               BackgroundColor = Color.FromRgb(200, 200, 250),
                               HorizontalTextAlignment = TextAlignment.Center
                           }
                   }
               );
            // Top Row
            for (char c = 'A'; c <= horizBound; c++)
            {
                TopLabels.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 1,
                        HeightRequest = 20,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                            new Label
                            {
                                Text = $"{c}",
                                TextColor = new Color(0, 0, 0),
                                BackgroundColor = Color.FromRgb(200, 200, 250),
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                    }
                );
            }
            // Left Column
            for (int i = 1; i <= vertBound; i++)
            {
                var horiz = new HorizontalStackLayout();
                horiz.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 1,
                        HeightRequest = 20,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                            new Label
                            {
                                Text = $"{i}",
                                TextColor = new Color(0, 0, 0),
                                BackgroundColor = Color.FromRgb(200, 200, 250),
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                    }
                );
                // Rest of the grid
                for (char c = 'A'; c <= horizBound; c++)
                {
                    // The entry for each cell.
                    var entry = new Entry
                    {
                        Text = $"{c}{i}",
                        TextColor = new Color(0, 0, 0),
                        WidthRequest = 75,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        StyleId = $"{c}{i}",
                    };

                    cells.Add(entry.StyleId, entry);
                    entry.Completed += CellChangedValues;
                    entry.Focused += CellFocused;
                    entry.Loaded += DefaultFocus;
                    horiz.Add(entry);
                }
                Grid.Children.Add(horiz);
            }
        }

        #region - CELL LOGIC -

        /// <summary>
        /// Sets the default loaded cell to A1 when program starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultFocus(object sender, EventArgs e)
        {
            Entry entry = (Entry)sender;
            if (entry.StyleId == "A1")
            {
                selectedCell.Text = entry.StyleId;
                object selectedCellContent = spreadsheet.GetCellContents(entry.StyleId);
                // If the content is a formula add the = in front of it and set the selected cell content widget to it.
                if (selectedCellContent is Formula)
                {
                    selectedCellEntry.Text = "=" + selectedCellContent.ToString();
                }
                // Otherwise set the selected cell content widget to the focused cell content.
                else
                {
                    selectedCellEntry.Text = selectedCellContent.ToString();
                }
            }
        }

        /// <summary>
        /// When a cell is selected, set the selected cell label to the focused cell name.
        /// Set the selected cell entry to whatever is in the focused cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellFocused(object sender, FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            // Set the top right cell identifier to the focused cell
            selectedCell.Text = entry.StyleId;
            // The content within the focused cell
            object selectedCellContent = spreadsheet.GetCellContents(entry.StyleId);
            // If the content is a formula add the = in front of it and set the selected cell content widget to it.
            if(selectedCellContent is Formula)
            {
                selectedCellEntry.Text = "=" + selectedCellContent.ToString();
            }
            // Otherwise set the selected cell content widget to the focused cell content.
            else
            {
                selectedCellEntry.Text = selectedCellContent.ToString();
            }
        }

        /// <summary>
        /// After pressing enter the contents of the focused cell is set to the selected cell entry box.
        /// Update all the non empty cells after changing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellChangedValues(object sender, EventArgs e)
        {
            Entry entry = (Entry)sender;
            // Save the cell name, and a backup of its contents
            string cell = entry.StyleId;
            string cellContentsBackup = entry.Text;
            // Update the selected cell entry text
            selectedCellEntry.Text = entry.Text;
            // Set the unsaved changes widget
            unsavedChanges.Text = "Unsaved Changes";
            try
            {
                // Set the new contents
                spreadsheet.SetContentsOfCell(cell, cellContentsBackup);
                // Set the entry value to the new value
                entry.Text = spreadsheet.GetCellValue(cell).ToString();
                // Loop through all other non empty cells and update
                foreach (String names in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    Entry otherEntry = cells[names];
                    otherEntry.Text = spreadsheet.GetCellValue(names).ToString();
                }
                // If an error occurs, set the value to the backup content
                if (spreadsheet.GetCellValue(cell) is FormulaError || spreadsheet.GetCellValue(cell) is CircularException)
                {
                    entry.Text = cellContentsBackup;
                }
            }
            // If an exception is thrown, set the cell text to the contents backup.
            catch (Exception ex)
            {
                entry.Text = cellContentsBackup;
                unsavedChanges.Text = "";
            }
        }

        /// <summary>
        /// If content is altered in the selected cell contents, set the value of the cell to the contents of this entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedCellContentCompleted(Object sender, EventArgs e)
        {
            try
            {
                // Get the focused cell
                string cell = selectedCell.Text;
                // Set the new contents
                spreadsheet.SetContentsOfCell(cell, selectedCellEntry.Text);
                // Update all non empty cells.
                foreach (String names in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    Entry entry = cells[names];
                    entry.Text = spreadsheet.GetCellValue(names).ToString();
                    if (spreadsheet.GetCellValue(names) is FormulaError)
                    {
                        entry.Text = cell;
                    }
                }
            }
            catch
            {

            }
            
        }

        #endregion

        #region - FILE MENU BAR - 
        /// <summary>
        /// For the file menu flyout exit button.
        /// If there are unsaved changes prompt the user if they would like to continue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            // If the spreadsheet has unsaved changes, display an alert.
            if(spreadsheet.Changed)
            {
                bool action = await DisplayAlert("Unsaved Changes", "All unsaved changes will be lost upon closure, are you sure you want to quit?", "Yes", "Cancel");
                if(action)
                {
                    Application.Current.Quit();
                }
            }
            // If the spreadsheet save is up to date no prompt.
            else
            {
                Application.Current.Quit();
            }
        }

        /// <summary>
        /// For the file menu flyout save button.
        /// If changes have not been made to the spreadsheet, nothing happens.
        /// Saves the spreadsheet to the desktop given a user input name.
        /// Displays an error prompt if the save is unsuccessful.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            // If the spreadsheet save is not up to date:
            if (spreadsheet.Changed)
            {
                // Prompt the user for a save name
                string fileName = await DisplayPromptAsync("Save", "What is the save name?");
                this.fileName = fileName;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(path, fileName);
                filePath = filePath + ".sprd";
                // If the user input a null, display an alert.
                if (fileName is null)
                {
                    await DisplayAlert("Invalid Save Name", "Please enter a valid save name.", "Ok");
                }
                else
                {
                    try
                    {
                        // Save spreadsheet with the input file path, display an alert, update the spreadsheet save widgets.
                        spreadsheet.Save(filePath);
                        await DisplayAlert("Successful Save", "Your spreadsheet was successfully saved to the desktop!", "Ok");
                        currentSpreadsheet.Text = fileName;
                        unsavedChanges.Text = "";
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Unsuccessful Save", "Error! Something went wrong during your save!", "Ok");
                        unsavedChanges.Text = "Unsaved Changes";
                    }
                }
            }
        }
        
        /// <summary>
        /// For the file menu flyout open button.
        /// If there are unsaved changes in the current spreadsheet, prompt the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenButton_Clicked(object sender, EventArgs e)
        {
            // Create a backup spreadsheet in case nothing is opened or invalid is opened.
            Spreadsheet spreadsheetBackup = spreadsheet;
            string backupFileName = fileName;
            // If there are unsaved changes: prompt the user if they are sure.
            if (spreadsheet.Changed)
            {
                bool action = await DisplayAlert("Unsaved Changes", "All unsaved changes will be lost opening a new spreadsheet, are you sure you want to open?", "Yes", "Cancel");
                if (action)
                {
                    try
                    {
                        // Prompt the user to pick a file.
                        var file = await FilePicker.Default.PickAsync();
                        if(file != null)
                        {
                            // Set the spreadsheet to a new spreadsheet with the filepath as a parameter.
                            string filePath = file.FullPath;
                            this.fileName = file.FileName;
                            spreadsheet = new Spreadsheet(filePath, s => true, s => s, "six");
                            currentSpreadsheet.Text = file.FileName;
                            unsavedChanges.Text = "";
                            // Update the cells
                            foreach (String names in spreadsheet.GetNamesOfAllNonemptyCells())
                            {
                                Entry entry = cells[names];
                                entry.Text = spreadsheet.GetCellValue(names).ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        spreadsheet = spreadsheetBackup;
                        this.fileName = backupFileName;
                        unsavedChanges.Text = "Unsaved Changes";
                        await DisplayAlert("Unsuccessful Load", "Your file was not successfully loaded, reverting back to previous spreadsheet!", "Ok");
                    }
                }
            }
            // Do the same thing if there are no unsaved changes but without a prompt
            else
            {
                try
                {
                    var file = await FilePicker.Default.PickAsync();
                    string filePath = file.FullPath;
                    this.fileName = file.FileName;
                    spreadsheet = new Spreadsheet(filePath, s => true, s => s, "six");
                    currentSpreadsheet.Text = file.FileName;
                    unsavedChanges.Text = "";
                    foreach (String names in spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        Entry entry = cells[names];
                        entry.Text = spreadsheet.GetCellValue(names).ToString();
                    }

                }
                catch (Exception ex)
                {
                    spreadsheet = spreadsheetBackup;
                    this.fileName = backupFileName;
                    await DisplayAlert("Unsuccessful Load", "Your file was not successfully loaded, reverting back to previous spreadsheet!", "Ok");
                }
            }
        }

        /// <summary>
        /// For the file menu flyout new button.
        /// If there are unsaved changes in the current spreadsheet, prompt the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NewButton_Clicked(object sender, EventArgs e)
        {
            // If there are unsaved changes: prompt the user
            if (spreadsheet.Changed)
            {
                bool action = await DisplayAlert("Unsaved Changes", "All unsaved changes will be lost upon creation of new spreadsheet, are you sure you want to create a new spreadsheet?", "Yes", "Cancel");
                if (action)
                {
                    // Reset the cells
                    foreach(string names in spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        Entry entry = cells[names];
                        entry.Text = entry.StyleId;
                    }
                    // Set new spreadsheet
                    spreadsheet = new Spreadsheet(s => true, s => s, "six");
                    fileName = "";
                    currentSpreadsheet.Text = "No save name...";
                    unsavedChanges.Text = "";
                }
            }
            // If no unsaved changes do the same thing with no prompt.
            else
            {
                foreach (string names in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    Entry entry = cells[names];
                    entry.Text = entry.StyleId;
                }
                spreadsheet = new Spreadsheet(s => true, s => s, "six");
                fileName = "";
                currentSpreadsheet.Text = "No save name...";
                unsavedChanges.Text = "";
            }
        }
        #endregion

        #region - HELP MENU BAR -

        /// <summary>
        /// Displays an alert with help information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileMenuButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("File Menu Help",
                "New: The new button will create a new spreadsheet, it will reset all cells currently occupied, a clean slate. Open: The Open button will prompt the user to select a spreadsheet file, if the file is invalid it will revert back to the previous spreadsheet." +
                " Save: Save will prompt the user for a filename and then save the spreadsheet under that name on the desktop. Exit: The exit button will close the program.", "Ok");
        }

        /// <summary>
        /// Displays an alert with help information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CellMenuButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Cells Help", "When pressing enter on a cell the cell will calculate what was placed into it, it will also recalculate all other cells in case a dependency value was changed. If an error occurred it will place the error in string form into the cell.", "Ok");
        }

        /// <summary>
        /// Displays an alert with help information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExtraMenuButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Extras Help", "This program has a few unique extras that I have implemented myself. Next to the selected cell content widget there is a label for the currently loaded spreadsheet filename. Next to that is the unsaved changes indicator, it pops up if there are any unsaved changes.", "Ok");
        }

        #endregion
    }
}