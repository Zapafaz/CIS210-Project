using System;
using static AdamWightDSproject.Common_Code;

namespace AdamWightDSproject
{
    class Week_5_Class
    {        
        public static void FindUppercaseT()
        {
            ShowHeader();

            // Initializes all virtualMemory locations to -99
            VirtualMemoryInit();

            // Reads the poems into memory, but does not write them to console - that was split into Week_2_Class.PoemWriter()
            Week_2_Class.PoemReader();

            // Finds a blank page and puts my name into memory on that page
            RememberMe();

            // Calls CreateSearchLog method with appropriate args for searchType, searchQuery, and addToName strings
            // Searches virtualMemory and writes log file based on search performed, creates "C:\devel\Logs\" directory if it does not exist
            // Since I'm sending "T" for searchQuery, will find all instances of 'T' and set pointers for their locations
            // Then sends total number of 'T' found to ConvertTtoQ() method
            CreateSearchLog(serializedSearch, "T", "find_T");
        }


        // Adds my name (Adam F. Wight) to virtualMemory and sets a pointer for where it starts
        public static void RememberMe()
        {
            // Not sure where else I should/could use this pointer so for now it's a local variable
            var nameStart = new Ptr(FindBlankPage(), 0, 0);

            // Converts myName string to an array of characters so it can be read into memory
            char[] nameChars = myName.ToCharArray();

            // Reads my name into memory, using the page value from the pointer
            for (int col = 0; col < nameChars.GetLength(0); col++)
            {
                // Roslyn tells me I don't need the explicit (int) cast - keeping it here anyway for clarity. There's a couple implicit (int) casts later on.
                virtualMemory[nameStart.P, 0, col] = (int)nameChars[col];
            }

            Console.Write("'");
            // Writes my name from memory, using the page value from the pointer
            for (int col = 0; col < nameChars.GetLength(0); col++)
            {
                Console.Write((char)virtualMemory[nameStart.P, 0, col]);
            }
            Console.Write("'");

            Console.WriteLine(" was added to virtual memory on page {0}", nameStart.P);
            DisplayFooter();
        }

        // Returns the first blank page number (int) in virtualMemory
        public static int FindBlankPage()
        {
            // Using two separate variables to improve readability. Pretty sure it could be done with one variable.
            var blankPage = 0;
            var page = 0;

            while (page <= virtualMemory.GetLength(0))
            {
                if (virtualMemory[page, 0, 0] == virtualNull)
                {
                    blankPage = page;
                    break;
                }

                else if (page < virtualMemory.GetLength(0))
                {
                    page++;
                }

                // Just in case there aren't any blank pages somehow, the program will close rather than overwriting data. Should never occur.
                else if (page == virtualMemory.GetLength(0))
                {
                    CloseProgram("There are no blank pages in virtual memory. This should never happen.");
                    break;
                }
            }
            return blankPage;
        }

        // Now that pointers are set for each 'T' in virtualMemory, change each of them to 'Q'
        // Tell user when it converts and where each 'T' was in virtualMemory
        public static void ConvertTtoQ(int foundAmount)
        {
            var counter = 0;
            while (counter < foundAmount)
            {
                Console.WriteLine("Converting 'T' at pointer #{0} (page: {1}, row: {2}, col: {3}) to 'Q'", counter+1, memLoc[counter].P, memLoc[counter].R, memLoc[counter].C);
                // 'Q' gets implicit (int) cast
                virtualMemory[memLoc[counter].P, memLoc[counter].R, memLoc[counter].C] = 'Q';
                counter++;
            }
            // Creates a log from memory for confirming conversion
            CreateSearchLog(serializedSearch, blank, "T_to_Q");
            
            DisplayFooter();

            // Converts back to 'T'
            ConvertQtoT(foundAmount);
        }

        // Basically the same as ConvertTtoQ, just converts the other way.
        public static void ConvertQtoT(int foundAmount)
        {
            var counter = 0;
            while (counter < foundAmount)
            {
                Console.WriteLine("Converting 'Q' at pointer {0} (page: {1}, row: {2}, col: {3}) to 'T'", counter+1, memLoc[counter].P, memLoc[counter].R, memLoc[counter].C);
                // 'T' gets implicit (int) cast
                virtualMemory[memLoc[counter].P, memLoc[counter].R, memLoc[counter].C] = 'T';
                counter++;
            }
            // Creates a log from memory for confirming conversion
            CreateSearchLog(serializedSearch, blank, "Q_to_T");            
        }        
    }
}
