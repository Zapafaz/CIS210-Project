using System;
using System.IO;
using static AdamWightDSproject.Common_Code;

namespace AdamWightDSproject
{
    class Week_6_Class
    {
        // Instantiating pointers for use in the stack
        static Ptr stackBottom = new Ptr(0, 0, 0);
        static Ptr stackTop = new Ptr(0, 0, 0);
        static Ptr stackCurrent = new Ptr(0, 0, 0);
        static Ptr stackNext = new Ptr(0, 0, 0);

        // Setting some booleans so the program knows what it was doing last
        public static bool wasPushing = false;
        public static bool wasPopping = false;

        // Setting up some file name strings so my StreamReaders & StreamWriters can use different files
        private static string readFile;
        private static string logFile;
        private static string popLog;

        private static int[] startPoem = GetPoemStartPages();

        public static void VirtualStacks()
        {
            ShowHeader();

            // Initializes all virtualMemory locations to -99
            VirtualMemoryInit();

            // Reads poems into memory at their standard locations (page 9, page 29, page 399)
            Week_2_Class.PoemReader();

            // Tests if the last page (i.e. the stack area) is empty via serial search; writes first non-empty location and prompts user to close program if it's not empty        
            IsStackEmpty();

            // Setting this to true makes the VirtualStackPush method initialize stack pointers to appropriate pushing positions
            // Since the last page is stack area, that is: [499, 0, 0] for current, bottom, top, and [499, 0, 1] for next.
            wasPopping = true;

            // Calls the logging method and sends necessary args (logType, addToName) to name the log and determine whether to pop or push
            StackLogger("InitPush", "Stack");

            // Calls the logging method again, this time popping the stack clean
            StackLogger("Pop", "Stack");

            // Calls the logging method again, this time pushing the reversed poem first lines to the stack
            StackLogger("Push", "InverseStack");

            // Calls the logging method one last time, this time popping the stack clean - and setting the poem's first lines back in original order
            StackLogger("Pop", "InverseStack");
        }

        // Ensures the stack area is empty, if it's not, writes first non-empty location found and prompts user to close program
        public static void IsStackEmpty()
        {
            for (int row = 0; row < totalRow; row++)
            {
                for (int col = 0; col < totalCol; col++)
                {
                    if (virtualMemory[lastPage, row, col] != virtualNull)
                    {
                        Console.WriteLine("virtualMemory location: (Page: {0}, Row: {1}, Column: {2}) has {3} in it", lastPage, row, col, virtualMemory[lastPage, row, col]);
                        CloseProgram("The last page of virtual memory, where the stack should go, is not empty.");
                    }
                }
            }
        }

        // Calls Common_Code.LogNamer method and sends args in order to get a name for the logFile
        // Then determines what kind of stack operation to perform (push or pop)
        private static void StackLogger(string logType, string addToName)
        {
            logFile = GetLogFilePath(logType, addToName);
            using (var logger = new StreamWriter(logFile))
            {
                if (logType == "InitPush")
                {
                    // Ensures the stack area is empty, if it's not, writes first non-empty location found and prompts user to close program
                    IsStackEmpty();
                    // This is the initial push, so setting popLog to blank ("virtualNull") tells the PoemPush method that it should read from the Week_2_Class.textFiles array of filenames
                    popLog = blank;                   
                    PoemPush(logger);
                    Console.WriteLine("First lines pushed to stack from poem files!");
                    DisplayFooter();
                }

                else if (logType == "Pop")
                {
                    // popLog gets set to the logFile name for the pop log - the one with reversed text.
                    popLog = logFile;
                    PopClean(logger);
                    Console.WriteLine("Stack popped clean and logged!");
                    DisplayFooter();
                }

                else if (logType == "Push")
                {
                    // Ensures the stack area is empty, if it's not, writes first non-empty location found and prompts user to close program
                    IsStackEmpty();
                    // Since popLog was set to the logFile name for the pop log above, the PoemPush method will now read from the popLog file instead
                    PoemPush(logger);
                    Console.WriteLine("Pushed inverted poems from pop log!");
                    DisplayFooter();
                }
            }
        }

        /// <summary>
        /// Pushes the text from readFile (either the whole popLog or the first lines of each file in Week_2_Class.textFiles) to the stack, one (int)character at a time
        /// </summary>
        /// <param name="logger">The StreamWriter used to write the log</param>
        private static void PoemPush(StreamWriter logger)
        {
            try
            {
                for (int poem = 0; poem < startPoem.Length;)
                {
                    if (popLog == blank)
                    {
                        readFile = Week_2_Class.textFiles[poem];
                    }

                    else
                    {
                        readFile = popLog;
                    }
                    
                    using (var reader = new StreamReader(readFile))
                    {
                        for (int count = 0; count < totalRow * totalCol; count++)
                        {
                            // Implicit int cast of character the StreamReader is reading
                            int currentInt = reader.Read();

                            // If carriage return is found (i.e. the first line of the poem was read), pushes a newline to the stack and log for readability
                            // Then increments poem and breaks out to the top level for loop so it can run again with the next poem
                            if (currentInt == decCR)
                            {
                                logger.Write((char)decNL);
                                VirtualStackPush(decNL);
                                poem++;
                                break;
                            }

                            // If end of file is found, end method
                            else if (currentInt == -1)
                            {
                                return;
                            }

                            // Otherwise, push the current character (in ASCII decimal) to stack and log, then continue looping
                            else
                            {
                                logger.Write((char)currentInt);
                                VirtualStackPush(currentInt);
                            }                            
                        }
                    }
                }
            }

            catch (Exception e)
            {
                WhereAmI();
                Console.WriteLine("{0}", e);
                CloseProgram();
            }
        }

        /// <summary>
        /// Pops everything off the stack, writes what it pops off to log
        /// </summary>
        /// <param name="logger">The StreamWriter to write to the log with</param>
        private static void PopClean(StreamWriter logger)
        {
            var isStackClean = false;
            int gotPopped = 0;

            try
            {
                while (!isStackClean)
                {
                    // Calls VirtualStackPop to pop off the top of the stack and sets gotPopped to that value
                    gotPopped = VirtualStackPop();

                    // As long as gotPopped isn't virtualNull (-99), write it to the log
                    if (gotPopped != virtualNull)
                    {
                        logger.Write((char)gotPopped);
                    }

                    // If it is virtualNull (-99), the stack is clean, and the loop ends
                    else if (gotPopped == virtualNull)
                    {
                        isStackClean = true;
                    }
                }
            }
            catch (Exception e)
            {
                WhereAmI();
                Console.WriteLine("{0}", e);
                CloseProgram();
            }
        }

        /// <summary>
        /// Pushes the given int parameter to the stack and increments to the next location to push to
        /// </summary>
        /// <param name="pushMe">The integer to push to the stack</param>
        public static void VirtualStackPush(int pushMe)
        {
            // Checks if the program was popping before calling this method, sets pointers initial locations if so
            if (wasPopping)
            {                
                stackBottom.P = lastPage;
                stackBottom.R = 0;
                stackBottom.C = 0;
                
                stackTop = stackBottom;
                stackCurrent = stackBottom;
                stackNext = stackBottom;
                
                stackNext.C++;

                wasPopping = false;
            }

            // Sets virtualMemory at stackCurrent pointer to pushMe value
            virtualMemory[stackCurrent.P, stackCurrent.R, stackCurrent.C] = pushMe;

            // Sets stackTop to stackCurrent - the location that just got pushed to - for reference
            stackTop = stackCurrent;

            // If stackNext row equals the total number of rows (17) from previous incrementing, then close the program!
            // This means it's run out of stack space (i.e. it filled the last page of virtualMemory)
            if (stackNext.R == totalRow)
            {
                WhereAmI();
                CloseProgram("You have reached the end of the allotted stack space. This should never happen.");
            }

            // Set stackCurrent equal to stackNext in preparation for the next push
            stackCurrent = stackNext;

            // Increments stackNext.c (column), so the next push can get assigned correctly
            stackNext.C++;

            // If stackNext column equals the total number of columns (80) then increment the stackNext row and set stackNext column to 0
            if (stackNext.C == totalCol)
            {
                stackNext.R++;
                stackNext.C = 0;
            }

            wasPushing = true;
        }

        /// <summary>
        /// Pops the current stack location off the stack and increments to the next location to pop; returns what it popped off
        /// </summary>
        /// <returns>Returns the integer that was popped off of the stack</returns>
        public static int VirtualStackPop()
        {
            // Checks if the program was pushing before calling this method, sets pointers appropriately if so
            if (wasPushing)
            {
                // If the program last pushed to a 0 column, set next location to col: 79, decrement row
                if (stackTop.C == 0)
                {
                    stackNext.C = lastCol;
                    stackNext.R--;
                }

                // Otherwise, just set the column of the next location to top column - 1
                else
                {
                    stackNext.C = stackTop.C - 1;
                }

                // Set current stack location to top of the stack, so it can be popped
                stackCurrent = stackTop;

                wasPushing = false;
            }

            // If previous pop decremented row to -1 (i.e. if the stack is clean) then return virtualNull so the calling method knows it's clean
            if ((stackTop.C == stackBottom.C) && (stackTop.R == stackBottom.R))
            {
                return virtualNull;
            }

            // Set gotPopped to the location being popped so this method can return what it popped
            int gotPopped = virtualMemory[stackCurrent.P, stackCurrent.R, stackCurrent.C];

            // Set the top of the stack to the location that just got popped
            stackTop = stackCurrent;

            // Set the virtualMemory location that just got popped back to virtualNull (-99)
            virtualMemory[stackTop.P, stackTop.R, stackTop.C] = virtualNull;

            // Set stackCurrent equal to stackNext in preparation for the next pop
            stackCurrent = stackNext;

            // If the program last popped from a 0 column, set next location to col: 79, decrement row
            if (stackCurrent.C == 0)
            {
                stackNext.C = lastCol;
                stackNext.R--;
            }

            // Otherwise, just decrement the column of the next location
            else
            {
                stackNext.C--;
            }
            
            wasPopping = true;
            return gotPopped;
        }

        /// <summary>
        /// Returns location of each stack pointer and prompts user to continue, in case of error
        /// </summary>
        public static void WhereAmI()
        {
            Console.WriteLine("You are here:");

            Console.WriteLine("stackBottom: (Page: {0}; Row: {1}; Column: {2})", stackBottom.P, stackBottom.R, stackBottom.C);
            Console.WriteLine("stackTop: (Page: {0}; Row: {1}; Column: {2})", stackTop.P, stackTop.R, stackTop.C);
            Console.WriteLine("stackCurrent: (Page: {0}; Row: {1}; Column: {2})", stackCurrent.P, stackCurrent.R, stackCurrent.C);
            Console.WriteLine("stackNext: (Page: {0}; Row: {1}; Column: {2})", stackNext.P, stackNext.R, stackNext.C);

            DisplayFooter();
        }        
    }
}
