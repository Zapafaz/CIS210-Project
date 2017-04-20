using System;
using System.Diagnostics;
using System.IO;

namespace AdamWightDSproject
{
    /// <summary>
    /// Commonly used readonly values, constants, types, and methods
    /// </summary>
    class Common_Code
    {
        #region Constants
        public const string genericError = "Something has gone wrong... ";

        public const string serializedSearch = "SerializedSearch";
        public const string indexSearch = "IndexSearch";

        public const string bubbleSort = "BubbleSort";
        public const string frequencySort = "FrequencySort";
        public const string arrayBucketSort = "ArrayBucketSort";
        public const string listBucketSort = "ListBucketSort";

        public const string blank = "virtualNull";

        public const string myName = "Adam F. Wight";
        public const string dataStructures = "CIS210M Data Structures and Elementary Algorithms";
        public const string currentInstructor = "Ed Cauthorn";

        public const string theChargeOfTheLightBrigade = "TCOTLB.txt";
        public const string richardCory = "RC.txt";
        public const string greenEggsAndHam = "GEAH.txt";

        public const int decCR = 13;
        public const int decNL = 10;

        public const int virtualNull = -99;
        #endregion

        #region Readonly
        // One set for at school (C:\), one set for at home (H:\).
        /// <summary>
        /// Text file directory, on home PC: @"H:\Projects\School\TextFiles\"
        /// </summary>
        public static readonly string textDir = @"H:\Projects\School\TextFiles\";
        /// <summary>
        /// Log file directory, on home PC: @"H:\Projects\School\Logs\"
        /// </summary>
        public static readonly string logDir = @"H:\Projects\School\Logs\";
        /// <summary>
        /// Text file directory, on school PC: @"C:\AW\TextFiles\""
        /// </summary>
        //public static readonly string textDir = @"C:\AW\TextFiles\";
        /// <summary>
        /// Log file directory, on school PC: @"C:\AW\Logs\""
        /// </summary>
        //public static readonly string logDir = @"C:\AW\Logs\";

        // These next 6 variables are initialized in the static constructor for Common_Code.
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(0)
        /// </summary>
        public static readonly int totalPage;
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(1)
        /// </summary>
        public static readonly int totalRow;
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(2)
        /// </summary>
        public static readonly int totalCol;

        // These 3 variables are really not necessary, just included because I don't feel like fixing the code that uses them.
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(0) - 1
        /// </summary>
        public static readonly int lastPage;
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(1) - 1
        /// </summary>
        public static readonly int lastRow;
        /// <summary>
        /// Shortcut for Common_Code.virtualMemory.GetLength(2) - 1
        /// </summary>
        public static readonly int lastCol;
        #endregion

        // Declares an array for the starting page of each poem
        private static int[] startPoem = { 9, 29, 399 };

        private static int[] decimalASCIIPunctuation
            = new int[]
        {
            10, // Line Feed / New Line
            13, // Carriage Return
            32, // Space
            33, // Exclamation mark !
            34, // Double quote "
            39, // Single quote '
            40, // Left parentheses (
            41, // Right parentheses )
            44, // Comma ,
            45, // Hyphen -
            46, // Period .
            59  // Semicolon ;
        };

        /// <summary>
        /// Get the ASCII punctuation values.
        /// </summary>
        /// <returns>Returns an int array of common decimal ASCII punctuation.</returns>
        /// <remarks>Clone method because array fields can't be properly readonly in C#, as per CA2105: https://msdn.microsoft.com/en-us/library/ms182299.aspx </remarks>
        public static int[] GetASCIIPunctuation()
        {
            return (int[])decimalASCIIPunctuation.Clone();
        }

        /// <summary>
        /// Get the start page of each poem.
        /// </summary>
        /// <returns>Returns an int array containing the page on which each poem starts.</returns>
        /// <remarks>Clone method because array fields can't be properly readonly in C#, as per CA2105: https://msdn.microsoft.com/en-us/library/ms182299.aspx </remarks>
        public static int[] GetPoemStartPages()
        {
            return (int[])startPoem.Clone();
        }

        /// <summary>
        /// The 3D array we're using to represent memory. Initialized in the Common_Code static constructor to: new int[500,17,80];
        /// </summary>
        public static int[,,] virtualMemory;

        // Declares an array of pointers with page, row, col values for pointing to virtualMemory locations
        public static Ptr[] memLoc = new Ptr[100];

        /// <summary>
        /// Class for pointers, for pointing at virtualMemory locations
        /// </summary>
        public class Ptr
        {
            // Added setters so pointers can be reassigned
            public int P { get; set; }
            public int R { get; set; }
            public int C { get; set; }

            /// <summary>
            /// The first, second, and third locations to point at in the 3D array
            /// </summary>
            /// <param name="page">The page to point at in the 3D array</param>
            /// <param name="row">The row to point at in the 3D array</param>
            /// <param name="col">The column to point at in the 3D array</param>
            public Ptr(int page, int row, int col)
            {
                P = page;
                R = row;
                C = col;
            }
        }

        /// <summary>
        /// Static constructor for this class (Common_Code) because the order of initialization of these static variables matters.
        /// </summary>
        /// <remarks>For example, we can't get info about virtualMemory (virtualMemory.GetLength(0)) if virtualMemory doesn't exist yet!
        /// Program crashes on first call to Common_Code with TypeInitializationException without this constructor.</remarks>
        static Common_Code()
        {
            // 3D array initialization: 500 pages, 17 rows per page, 80 columns per row
            // Remember, arrays are 0 indexed. Upper bound of each index of this array is one less than each value listed here
            virtualMemory = new int[500, 17, 80];

            // Sets total (page/row/col) values for each dimension of the array
            totalPage = virtualMemory.GetLength(0);
            totalRow = virtualMemory.GetLength(1);
            totalCol = virtualMemory.GetLength(2);

            // Sets last valid indices for each dimension of the array
            lastPage = totalPage - 1;
            lastRow = totalRow - 1;
            lastCol = totalCol - 1;
        }

        /// <summary>
        /// Opens the log folder path given in Common_Code.logDir in explorer
        /// </summary>
        public static void OpenLogFolder()
        {
            try
            {
                Process.Start(logDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Closes the program and gives the user an idea of why (what happened?)
        /// </summary>
        /// <param name="whatHappened">Summarize what might have caused the program to close</param>
        public static void CloseProgram(string whatHappened = genericError)
        {
            Console.WriteLine("{0}", whatHappened);
            CenterWrite("Press Enter to exit the program...");
            Console.ReadLine();
            Environment.Exit(0);
        }

        /// <summary>
        /// Displays a header with the calling class, instructor, and current date
        /// </summary>
        /// <remarks>Unfinished. Should be able to use this while scrolling through pages (e.g. in Week_2_Class.PoemWriter method)
        /// Not sure how to make it work like I want without erasing or overwriting text, though</remarks>
        public static void ShowHeader()
        {
            Console.Clear();

            // Sets currentDate variable to long form date string for current date (e.g. "Saturday, January 28, 2017")
            var currentDate = DateTime.Today.ToString("D");

            // Sets amount of spaces needed for other info to appear right aligned on same line as other info
            // Based on current console window width & length of other variables, minus two because otherwise it automatically adds a newline
            var line1Pad = (Console.WindowWidth - currentInstructor.Length) - 2;
            var line2Pad = (Console.WindowWidth - currentDate.Length) - 2;

            // Writes the header. One space added before each line of the header to match right side (e.g. | stuff   stuff | instead of |stuff   stuff |)   
            Console.WriteLine(" {0}{1}\n {2}{3}\n", myName.PadRight(line1Pad), currentInstructor, dataStructures.PadRight(line2Pad), currentDate);
            Console.SetCursorPosition(0, 3);
        }

        /// <summary>
        /// Prompts the user to continue by pressing Enter at the horizontal center of the console window
        /// </summary>
        /// <remarks>Unfinished. Should always display at bottom of console window without overwriting other text that might be there</remarks>
        public static void DisplayFooter()
        {
            CenterWrite("Press Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays given string at the center of the console instead of at the left
        /// </summary>
        /// <param name="output">The string to output at the center of the console</param>
        public static void CenterWrite(string output)
        {
            var centered = ((Console.WindowWidth / 2) + (output.Length / 2));
            Console.Write("{0}: ", output.PadLeft(centered));
        }

        /// <summary>
        /// Method for yes/no choices, displays prompt string with appended " (yes/no)?: " at center, returns true if yes or y
        /// </summary>
        /// <param name="prompt">The yes or no question to prompt the user with</param>
        /// <returns>Returns true if the user chooses yes or y, otherwise returns false</returns>
        public static bool YesNo(string prompt)
        {
            prompt = prompt + " (yes/no)?";
            CenterWrite(prompt);
            var choice = Console.ReadLine();
            if ((choice == "y") || (choice == "yes"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes all virtualMemory array locations to -99
        /// </summary>
        public static void VirtualMemoryInit()
        {
            for (int page = 0; page < virtualMemory.GetLength(0); page++)
            {
                for (int row = 0; row < virtualMemory.GetLength(1); row++)
                {
                    for (int col = 0; col < virtualMemory.GetLength(2); col++)
                    {
                        virtualMemory[page, row, col] = virtualNull;
                    }
                }
            }
        }

        /// <summary>
        /// Lets user choose Week_#_Class.Method() search type
        /// </summary>
        public static void ChooseSearch()
        {
            // Declares an array with valid search names and what week of class they're from
            string[] searches = { "serialized (week 3)", "index (week 3)" };

            // Writes and numbers valid search names
            Console.WriteLine("Possible searches: ");
            for (int search = 0; search < searches.Length; search++)
            {
                Console.WriteLine("{0}: {1}", search+1, searches[search]);
            }
            
            // Asks user to input which search to perform, converts input to lowercase
            // Includes cases for some possible alternate inputs with equivalent meaning
            CenterWrite("Enter search type to perform");
            var searchType = Console.ReadLine();
            var type = searchType.ToLower();
            switch (type)
            {
                case "1":
                case "serialized":
                case "linear":
                case "linear search":
                case "serialized search":
                case "lin":
                case "ser":
                    Week_3_Class.SerializedSearch();
                    break;
                case "2":
                case "index":
                case "page":
                case "index search":
                case "page search":
                case "ind":
                case "pag":
                    Week_3_Class.IndexSearch();
                    break;
                default:
                    Console.WriteLine("\nInvalid search type input: {0}\n", searchType);
                    ChooseSearch();
                    break;
            }
        }

        /// <summary>
        /// Searches and creates a log that tracks how many iterations the search takes
        /// </summary>
        /// <param name="searchType">The type of search to be performed</param>
        /// <param name="searchQuery">The query to search for</param>
        /// <param name="addToName">An additional item to add to the log file name</param>
        public static void CreateSearchLog(string searchType, string searchQuery, string addToName)
        {
            // Searches and writes log based on search type and informs user of what it created
            try
            {
                string fileName = GetLogFilePath(searchType, addToName);

                var counter = 0;

                // Writes to log file for every index searched
                using (var logger = new StreamWriter(fileName))
                {    
                    // Searches every location of the array, in sequence
                    if (searchType == serializedSearch)
                    {
                        for (int page = 0; page < virtualMemory.GetLength(0); page++)
                        {
                            for (int row = 0; row < virtualMemory.GetLength(1); row++)
                            {
                                for (int col = 0; col < virtualMemory.GetLength(2); col++)
                                {
                                    // if searchQuery is blank, write a 1 for every virtualNull (-99) found, and write the character for everything else
                                    if (searchQuery == blank)
                                    { 
                                        if (virtualMemory[page, row, col] == virtualNull)
                                        {
                                            logger.WriteLine("1");
                                        }

                                        else
                                        {
                                            logger.WriteLine("{0}", (char)virtualMemory[page, row, col]);
                                        }
                                    }

                                    // if searchQuery is anything besides blank, write when it finds an instance of searchQuery
                                    // Then add a pointer for the location where it found searchQuery
                                    // also writes a 0 for other characters and a 1 for virtualNull
                                    else if (searchQuery != blank)
                                    {
                                        char[] searchChars = searchQuery.ToCharArray();
                                        var intQuery = (int)searchChars[0];

                                        if (virtualMemory[page, row, col] == virtualNull)
                                        {
                                            logger.WriteLine("1");
                                        }

                                        else if (virtualMemory[page, row, col] == intQuery)
                                        {
                                            logger.WriteLine("{0}", searchQuery);
                                            memLoc[counter] = new Ptr(page, row, col);
                                            counter++;
                                        }

                                        else
                                        {
                                            logger.WriteLine("0");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Searches only the "upper left" location of every page (i.e. [page, 0, 0]), in sequence
                    else if (searchType == indexSearch)
                    {
                        for (int page = 0; page < virtualMemory.GetLength(0); page++)
                        {
                            // Writes a 1 for every blank page, by checking the "upper left" location of each page (i.e. [page, 0, 0])
                            if (virtualMemory[page, 0, 0] == virtualNull)
                            {
                                logger.WriteLine("1");
                            }

                            // Writes the character at "upper left" of non-blank pages (e.g. [10, 0, 0] will have some TCOTLB character printed)
                            else if (searchQuery == blank)
                            {
                                logger.WriteLine("{0}", (char)virtualMemory[page, 0, 0]);
                            }
                        }
                    }
                }

                // If the query was 'T', sends total number of searchQuery's (i.e. 'T's) found to T to Q conversion method in week 5
                if (searchQuery == "T")
                {
                    Week_5_Class.ConvertTtoQ(counter);
                }
            }

            // Writes an error if something goes wrong creating the log (e.g. wrong permissions)
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}", genericError, e);
            }
        }

        /// <summary>
        /// Method for naming log files. Returns a string: file path at Common_Code.logDir
        /// </summary>
        /// <param name="logType">The type of log that will be created</param>
        /// <param name="addToName">An optional string to add to the name of the log</param>
        /// <returns>Returns a file path string at Common_Code.logDir, based on the given parameters</returns>
        public static string GetLogFilePath(string logType, string addToName = "DEFAULT")
        {
            string logName;
            try
            {
                // Sets fileName based on log directory, search type, identifier(addToName), and counter
                // Increments counter while fileName already exists so multiple logs of the same type can be stored
                // If they already have over 950 logs of that search type for some reason, requests archive or delete and prompts continue
                var counter = 0;
                logName = logDir + logType + "_" + addToName + "_log" + counter.ToString("D3") + ".log";
                while (File.Exists(logName))
                {
                    ++counter;
                    if (counter >= 950)
                    {
                        Console.WriteLine("You have {0} {1} logs in {2}, please archive or delete them", counter, logType, logDir);
                        DisplayFooter();
                    }
                    logName = logDir + logType + "_" + addToName + "_log" + counter.ToString("D3") + ".log";
                }

                // Creates log directory if it doesn't already exist
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                    Console.WriteLine("Directory created: {0}", logDir);
                }
                Console.WriteLine("Creating log at: {0}", logName);

                return logName;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
                CloseProgram();
                return logName = "ERROR";
            }
        }

        /// <summary>
        /// Writes locations in virtualMemory to a log file for debugging
        /// </summary>
        /// <param name="weekOfClass">Which week of class is this virtual memory dump being called from?</param>
        /// <param name="writeNull">Should the log write a 1 for virtualNull (-99) locations, or ignore them?</param>
        public static void VirtualMemoryLog(int weekOfClass, bool writeNull)
        {
            try
            {
                string style = writeNull ? "WriteNull" : "IgnoreNull";
                string fileName = GetLogFilePath("VirtualMemoryDump", ("Week_" + weekOfClass + style));
                int current;

                // Writes virtualMemory to log: writes '1' for each -99, otherwise writes char
                using (var logger = new StreamWriter(fileName))
                {
                    for (int page = 0; page < virtualMemory.GetLength(0); page++)
                    {
                        switch (page)
                        {
                            case 200:
                                logger.WriteLine("ARRAY_BUCKET_SORT");
                                break;
                            case 205:
                                logger.WriteLine("FREQUENCY_SORT");
                                break;
                            case 210:
                                logger.WriteLine("BUBBLE_SORT");
                                break;
                        }
                        for (int row = 0; row < virtualMemory.GetLength(1); row++)
                        {
                            for (int col = 0; col < virtualMemory.GetLength(2); col++)
                            {
                                current = virtualMemory[page, row, col];

                                if (writeNull && current == virtualNull)
                                {
                                    logger.WriteLine('1');
                                }
                                else if (!writeNull && current != virtualNull)
                                {
                                    logger.WriteLine(current);
                                }
                                else if (writeNull)
                                {
                                    logger.WriteLine(current);
                                }
                            }
                        }
                    }
                }
            }

            // Writes an error if something goes wrong creating the log (e.g. wrong permissions)
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }
        }

        /// <summary>
        /// Writes all locations in the source array to a log file for debugging
        /// </summary>
        /// <param name="toLog">The source array to log</param>
        /// <param name="arrayID">The name or other identifier of the array to log</param>
        public static void IntArrayLog(int[] toLog, string arrayID)
        {
            try
            {
                string fileName = GetLogFilePath("OneDimensionalIntArrayLog", arrayID);

                // Writes array to log: writes '1' for -99, otherwise writes (char) cast of the int at that location
                using (var logger = new StreamWriter(fileName))
                {
                    for (int i = 0; i < toLog.Length; i++)
                    {
                        if (toLog[i] == virtualNull)
                        {
                            logger.WriteLine('1');
                        }
                        else
                        {
                            logger.WriteLine(toLog[i]);
                        }
                    }
                }
            }

            // Writes an error if something goes wrong creating the log (e.g. wrong permissions)
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}", genericError, e);
            }
        }

        /// <summary>
        /// Fills every location in the given array with the given object.
        /// </summary>
        /// <typeparam name="T">The type of the array and object.</typeparam>
        /// <param name="array">The array to be filled.</param>
        /// <param name="populateWith">The object to fill the array with.</param>
        public static void Populate<T>(T[] array, T populateWith)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = populateWith;
            }
        }
    }
}
