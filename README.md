```
Author:     Brett Baxter
Partner:    None
Date:       1/12/2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  BrettBaxter1
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-BrettBaxter1
Date:       2-25-2023
Solution:   Spreadsheet
Copyright:  CS 3500 and Brett Baxter - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of:
    1. Evaluating mathematical expressions from user input from a console or other application.
    2. Showing the dependencies of two different variables.
    3. Evaluating and storing mathematical expressions as objects.
    4. Storing spreadsheets as objects.
    5. Saving spreadsheets.
    6. GUI

# Time Expenditures:

    1. Assignment One:   Predicted Hours:          10        Actual Hours:   7
    2. Assignment Two:   Predicted Hours:          8         Actual Hours:   7
    3. Assignment Three: Predicted Hours:          8         Actual Hours:   9
    4. Assignment Four:  Predicted Hours:          5         Actual Hours:   5
    5. Assignment Five:  Predicted Hours:          7         Actual Hours:   8
    6. Assignment Six:   Predicted Hours:          10        Actual Hours:   11

# Comments to Evaluators:

    1. Assignment One: This assignment was fairly straight forward for me. It took a little while to understand why the provided algorithm was the way it was,
    but after doing some work on paper and drawing it out it made sense. The console application was useful in quick testing, but I look forward to using
    actual C# unit tests.
    
    2. Assignment Two: Was very confused about this assignment until the lecture on Tuesday the 24th, the comments about using mirrored dictionary really helped steer me in the right direction.
    Spent a good hour fixing a git problem, fatal git error visual studio couldn't access my repo, had to edit windows credential manager because I have another personal github account.
    I don't know why it happened, everything was fine before this. Also wasted a lot of time on the replace functions, forgot I just wrote the add and remove methods that make
    these functions really easy.

    3. Assignment Three: Seemed fairly straight-forward. I liked completely checking whether a function was valid before doing any evaluation on it, it felt way more clean. Now I
    can remove most error checking inside the evaluate function! Also cleaned out a bunch of useless code thanks to the code coverage visualization from my old evaluator
    function.

    4. Assignment Four: Very satisfying being able to use my previously written methods from past assignment to tie this assignment together. Deceptively simple assignment.

    5. Assignment Five: Xml was interesting, I like using it. Being able to save stuff to an actual tangible file is really cool.

    6. Assignment Six: MAUI was very daunting, I wasn't sure how a project should be structured. It took some time going through lecture recording and lab to see how I should plug
    everything together.

# Assignment Writeups:

    1. Assignment One: There was no assignment specific writeup for this assignment.

    2. Assignment Two: There was no assignment specific writeup for this assignment.

    3. Assignment Three: There was no assignment specific writeup for this assignment.

    4. Assignment Four: There was no assignment specific writeup for this assignment.

    5. Assignment Five: Images are included.

    6. Assignment Six:
        Design Decisions:
            1. Added a dedicated exit button, it interacts with the unsaved changes pop-up to warn the user about closing the program before saving.
            2. Added some indicators to let the user know that they have not saved, up by the cell content entry widget at the top you can see the name of the
               file being currently used.
            3. If there are any unsaved changes in the spreadsheet there will be an indicator next to the file name indicator, this will go away after saving.

        Punt:
            1. Horizontal scroll view does not interact with the horizontal top bar.
            2. Variable recognition, could not figure out how to make the spreadsheet recognize A6 = a6.

        Time Tracking:
            My time estimates appear to be about correct, sometimes I underestimate, other times I overestimate. I definitely started this assignment too late, I did not have much
            knowledge on .net MAUI before this assignment so there was much more time spent researching than usual.
            I am fairly confident in my abilities, I'm not amazing at figuring out the best approach to the assignment but I am confident I can get them done and submitted on time.

# Examples of Good Software Practice (GSP):

    DRY:
        A3: Added 3 helper methods to check if a string is a double, variable, or operator rather than checking with regex in every instance.
            Added evaluate operation method that is called whenever an operation needs evaluating rather than checking what the operator is each time
            in every instance.
        A4: Added IsValidName helper method to check if the name fits the regex.
    Code Re-Use:
        A3: Reused most of my code from A1 with some modification.
            Reused the evaluate operator method.
            Reused the isDouble, isVariable, and isOperator methods.
    Well Named Methods:
        isDouble, isVariable, isOperator, isValidName, evaluateOperator.

# Peers/References:

    # Consulted Peers:

        Assignment One:
            None

        Assignment Two:
            None

        Assignment Three:
            Jack Marshall - Piazza @550 : Helped me understand returning FunctionErrors.

        Assignment Four:
            None

        Assignment Five:
            None

        Assignment Six:
            None   

    # References

        Assignment One:
            1. Stack Overflow - https://stackoverflow.com/questions/3560393/how-to-check-first-character-of-a-string-if-a-letter-any-letter-in-c-sharp: This source helped me figure out
               how to tell the first part of a string was a letter. This helped me detect variables.
            2. Geeks for Geeks - https://www.geeksforgeeks.org/expression-evaluation/: This source was used for formatting inspiration purposes only. No code was used from this source.
            3. Code Project - https://www.codeproject.com/Articles/9099/The-30-Minute-Regex-Tutorial: Helped me understand Regex better.
            4. Stack Overflow - https://stackoverflow.com/questions/8564001/how-to-check-if-a-string-contains-a-specific-format: Pointed me in the right direction.

        Assignment Two:
            1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0 : Used HashSets for the dictionaries to maintain O(1) time.

        Assignment Three:
            1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/is: Learned how to use the 'is' operator.
            2. C# Documentation - https://learn.microsoft.com/en-us/dotnet/api/system.object.referenceequals?view=net-7.0: Learned how to use the ReferenceEquals function.

        Assignment Four:
            1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/api/system.object.referenceequals?view=net-7.0: Learned how to use the ReferenceEquals function.

        Assignment Five:
            1. Stack Overflow - https://stackoverflow.com/questions/2476619/lambda-if-statement: Learned how to use If statements in lambda expressions.

        Assignment Six:
            1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-system-helpers?view=net-maui-7.0&tabs=windows - Helped me understand the file picker
            2. Stack Overflow - https://stackoverflow.com/questions/40781396/c-sharp-save-txt-file-on-desktop - Helped me figure out how to write to desktop
            3. C# Documentation - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/entry?view=net-maui-7.0 - Helped me understand the MAUI entry
            4. C# Documentation - https://learn.microsoft.com/en-us/dotnet/api/system.io.path?view=net-7.0 - Helped me understand file paths
            5. C# Documentation - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0 - Helped me understand pop-ups