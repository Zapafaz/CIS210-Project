using System;
using System.IO;

namespace AdamWightDSproject
{
    class Week_2_Class
    {
        // Declare array of files in textDir ("C:\devel\TFiles\")  
        public static string textDir = Common_Code.textDir;
        public static string[] textFiles = Directory.GetFiles(textDir);

        // Declare maximums for each dimension of the array
        public const int maxPage = 500;
        public const int maxRow = 17;
        public const int maxCol = 80;

        // Declare an int far out of bounds of maxPage, to designate poems as already written
        public const int nilPage = 999999;

        // Declare a string for missing & wrong files error
        public static string missingFiles = string.Format("GEAH.txt, RC.txt, and TCOTLB.txt should be the only files in {0}", textDir);

        public static void PoemReadWrite()
        {
            Common_Code.ShowHeader();
            // Calls the rest of the Week_2_Class methods
            PoemReader();
            PoemWriter();
        }

        /// <summary>
        /// Reads all three of the poems (TCOTLB.txt, RC.txt, GEAH.txt) in textDir (C:\devel\TFiles\)
        /// </summary>
        public static void PoemReader()
        {
            string poemOneFilePath = string.Format(Common_Code.textDir + Common_Code.theChargeOfTheLightBrigade);
            string poemTwoFilePath = string.Format(Common_Code.textDir + Common_Code.richardCory);
            string poemThreeFilePath = string.Format(Common_Code.textDir + Common_Code.greenEggsAndHam);

            // Reverse array if it hasn't already been reversed
            // Order before reverse: GEAH, RC, TCOTLB; After reverse: TCOTLB, RC, GEAH
            // Since TCOTLB comes at page 10, and GEAH comes at page 400, it makes more sense to me reversed
            if (textFiles[0] == (poemThreeFilePath))
            {
                Array.Reverse(textFiles);
            }

            // Test textFiles array elements to ensure they are the correct poems
            try
            {
                if ((textFiles[0] != poemOneFilePath) || (textFiles[1] != poemTwoFilePath) || (textFiles[2] != poemThreeFilePath))
                {
                    Console.WriteLine("{0}", missingFiles);
                    // Calls method to display "Press enter to continue..." prompt
                    Common_Code.DisplayFooter();
                    PoemReader();
                }
            }
            
            // Spit out an error if the files are missing, then restart PoemReader method
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("{0}", e);
                Console.WriteLine("{0}", missingFiles);
                Common_Code.DisplayFooter();
                PoemReader();
            }

            // Creates 3D virtualMemory array and initializes all locations to -99
            Common_Code.VirtualMemoryInit();

            // Declaring counters
            int pageCount = 0;
            int rowCount = 0;
            int colCount = 0;

            // Declare an int array so loop can tell at what page to start reading each poem
            int[] startPoem = Common_Code.GetPoemStartPages();

            // Main block for reading poems into memory
            try
            {
                for (int poem = 0; poem <= startPoem.Length;)
                {
                    using (StreamReader readPoem = new StreamReader(textFiles[poem]))
                    {
                        while (pageCount < maxPage)
                        {
                            // If pageCount is greater than or equal to array element designating appropriate starting page, assign to virtual memory
                            // Also checks if current startPoem value is not nilPage (already read)                          
                            if ((pageCount >= startPoem[poem]) && (colCount < maxCol) && (rowCount < maxRow) && (startPoem[poem] != nilPage))
                            {
                                // Assigns to virtualMemory integer array then reads it with (char) cast so they are written as characters                    
                                var currentChar = readPoem.Read();
                                if (currentChar != -1)
                                {
                                    Common_Code.virtualMemory[pageCount, rowCount, colCount] = currentChar;
                                }

                                // If end of file is reached, set current poem to nilPage to designate it as already read, so it does not get read again
                                // Then, increment poem by 1 and break out of while loop so StreamReader can be set with next poem in textFiles array                            
                                if (currentChar == -1)
                                {
                                    startPoem[poem] = nilPage;
                                    ++poem;
                                    break;
                                }
                            }

                            // Increments to next element in the array
                            if ((colCount < maxCol) && (rowCount < maxRow))
                            {                                
                                ++colCount;
                            }

                            else if (rowCount < maxRow)
                            {
                                ++rowCount;
                                colCount = 0;
                            }

                            else if (rowCount == maxRow)
                            {
                                ++pageCount;
                                rowCount = 0;
                                colCount = 0;
                            }

                            // Increments pageCount if nothing else is possible to get to the next poem
                            else if (pageCount <= startPoem[poem])
                            {
                                ++pageCount;
                            }
                        }
                        // If poem was incremented so textFiles[poem] would be out of range, break out of for loop
                        if (poem >= textFiles.Length)
                        {
                            break;
                        }
                    }
                }
            }

            // Catch exceptions from above try block and report some probably useful stuff
            // Shouldn't be possible to reach in normal operation, but useful for debugging!
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
                Console.WriteLine("Page: {0}, Row: {1}, Col: {2}", pageCount, rowCount, colCount);
                Console.WriteLine("virtualMemory.Length: {0}", Common_Code.virtualMemory.Length);
            }         
        }

        public static void PoemWriter()
        {
            // Declaring counters
            int pageCount = 0;
            int rowCount = 0;
            int colCount = 0;

            // Declare an int array so loop can tell at what page to start writing each poem
            int[] startPoem = new int[] { 9, 29, 399 };


            // Main block for writing poems to console
            try
            {
                for (int poem = 0; poem <= startPoem.Length;)
                {
                    while (pageCount < maxPage)
                    {
                        // If pageCount is greater than or equal to array element designating appropriate starting page, write character to console
                        // Also checks if current startPoem value is not nilPage (already read)                          
                        if ((pageCount >= startPoem[poem]) && (colCount < maxCol) && (rowCount < maxRow) && (startPoem[poem] != nilPage))
                        {
                            // Writes virtualMemory integer with (char) cast                              
                            var currentChar = Common_Code.virtualMemory[pageCount, rowCount, colCount];
                            if (currentChar != -99)
                            {
                                Console.Write("{0}", (char)currentChar);
                                ++colCount;                              
                            }
                            
                            // If end of poem was reached, set current poem to nilPage to designate it as already written, so it does not get written again
                            // Then, increment poem by 1 to start writing the next poem in the process                         
                            else if (currentChar == -99)
                            {
                                startPoem[poem] = nilPage;
                                ++poem;

                                // Breaks out of while loop if necessary
                                if (poem >= startPoem.Length)
                                {
                                    break;
                                }
                            }
                        }

                        // Increments as needed
                        else if (colCount < maxCol)
                        {
                            Console.Write(' ');
                            ++colCount;
                        }

                        else if (rowCount < maxRow)
                        {
                            ++rowCount;
                            colCount = 0;
                        }

                        else if (rowCount == maxRow)
                        {
                            ++pageCount;
                            rowCount = 0;
                            colCount = 0;
                            Console.Write("Page {0}", pageCount);
                            Common_Code.DisplayFooter();
                        }
                    }

                    // Breaks out of for loop if necessary
                    if (poem >= startPoem.Length)
                    {
                        break;
                    }
                }
            }

            // Catch exceptions from above try block and report some probably useful stuff
            // Shouldn't be possible to reach in normal operation, but useful for debugging!
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
                Console.WriteLine("Page: {0}, Row: {1}, Col: {2}", pageCount, rowCount, colCount);
                Console.WriteLine("virtualMemory.Length: {0}", Common_Code.virtualMemory.Length);
            }

            // This is where we end up after the last poem in textFiles array is read
            // Run through pages with user prompt at each page, until page 500, where program gets sent back to Main(), prompting exit
            // Seems somewhat redundant but not sure how to write it better, maybe write a method that does this and use it in both places (here and above, in the main poem loop)
            try
            {
                while (pageCount < maxPage)
                {
                    if (colCount < maxCol)
                    {
                        Console.Write(' ');
                        ++colCount;
                    }

                    else if (rowCount < maxRow)
                    {
                        ++rowCount;
                        colCount = 0;
                    }

                    else if (rowCount == maxRow)
                    {
                        ++pageCount;
                        rowCount = 0;
                        colCount = 0;
                        Console.Write("Page {0}", pageCount);
                        Common_Code.DisplayFooter();
                    }
                }
            }

            // Catch exceptions from above try block and report some probably useful stuff
            // Shouldn't be possible to reach in normal operation
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
                Console.WriteLine("Page: {0}, Row: {1}, Col: {2}", pageCount, rowCount, colCount);
                Console.WriteLine("virtualMemory.Length: {0}", Common_Code.virtualMemory.Length);
            }
        }                       
    }
}
