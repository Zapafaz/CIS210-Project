using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdamWightDSproject.Common_Code;

namespace AdamWightDSproject
{
    class Week_8_Class
    {
        // Creating queue index pointers
        static int queueRear;
        static int queueFront;

        static int queueMin;
        static int queueMax;

        // Declaring the queue size and name
        static string[] circularPoemQueue = new string[10];

        // Creating a clone of the decimal ASCII punctuation array in Common_Code for determining where a word ends/begins
        // Cloned because arrays can't be properly readonly in C#, as per CA2105: https://msdn.microsoft.com/en-us/library/ms182299.aspx
        static int[] decASCIIToSkip = GetASCIIPunctuation();

        public static void VirtualQueue()
        {
            ShowHeader();

            // If RC.txt isn't in the text directory, restart this method
            if (!CheckForPoem(richardCory))
            {
                Console.WriteLine("{0} was not found in {1}, please ensure it is there", richardCory, textDir);
                DisplayFooter();
                VirtualQueue();
            }

            // Initialize queue pointers
            queueMin = 0;
            queueFront = queueMin;
            queueRear = queueMin;
            queueMax = circularPoemQueue.GetLength(0);

            // Start reading Richard Cory with the queue
            ReadPoemWithQueue("RichardCory");
        }

        public static bool CheckForPoem(string fileNameToCheck)
        {
            // Check if Richard Cory is in the expected location and return true if it is
            if ((fileNameToCheck == richardCory) && (Week_2_Class.textFiles[1] == (textDir + richardCory)))
            {
                return true;
            }

            // could extend this method to check other poems but don't need to right now
            //else if (other poems)

            // Return false if the fileNameToCheck isn't in the expected location
            return false;
        }

        public static void ReadPoemWithQueue(string poemToRead)
        {
            Console.WriteLine("Reading from {0}", Week_2_Class.textFiles[1]);

            // Getting a log file name & path for use with StreamWriter
            string logFile = GetLogFilePath("PoemQueueLog", poemToRead);

            using (var logWriter = new StreamWriter(logFile))
            {
                using (var poemReader = new StreamReader(Week_2_Class.textFiles[1]))
                {
                    var wordBuilder = new StringBuilder();
                    string wordToLog;
                    string wordToQueue;
                    int currentInt = poemReader.Read();

                    // While currentInt is not -1 (i.e. stream reader has not reached end of file)
                    while (currentInt != -1)
                    {
                        // This if statement would catch all the words in RC.txt
                        // Since it includes the ASCII decimal values for every actual letter
                        // But I wrote all the punctuation out manually in Common_Code
                        // And am using that to check for punctuation instead
                        // if (currentInt >= 65 && =< 126)

                        // Check if the current value is punctuation
                        // In other words, is the currentInt part of a word?
                        if (IsValuePunctuation(currentInt))
                        {
                            // If currentInt is not part of a word, set the string to queue to the current string in the string builder
                            wordToQueue = wordBuilder.ToString();

                            // Then, clear the string builder so it can start a new word
                            wordBuilder.Clear();

                            // Checks if the string to queue is longer than 0
                            // If it is, queue and write
                            // Without this check, multiple sequential punctuation marks
                            // (e.g. the comma followed by double-quote in "Good-morning,")
                            // results in a blank line being written
                            if (wordToQueue.Length > 0)
                            {
                                // Then, assign the string to write to the log to the return value
                                // of PoemWordsQueue (i.e. the string that was removed from the queue)
                                // and send it the string to add to queue
                                wordToLog = PoemWordsQueue(wordToQueue);

                                // Lastly, write the string that came off the queue to the log
                                logWriter.WriteLine(wordToLog);
                            }
                        }

                        // If it wasn't punctuation
                        else
                        {
                            // Cast to char and append it to the current word being built
                            wordBuilder.Append((char)currentInt);
                        }

                        // Regardless, advance the streamreader to next char
                        currentInt = poemReader.Read();
                    }
                }
            }
        }

        // Checks all the values in clone of Common_Code.decimalASCIIPunctuation array
        // If it matches one of them, return true (i.e. the given value is punctuation)
        // Otherwise, return false (it is not punctuation)
        public static bool IsValuePunctuation(int decimalASCIIValue)
        {
            for (int i = 0; i < decASCIIToSkip.Length; i++)
            {
                if (decimalASCIIValue == decASCIIToSkip[i])
                {
                    return true;
                }
            }
            return false;
        }

        // Puts a word into the queue at the rear and returns one it takes off the front.
        // Could be made generic if necessary (i.e. accept/return any object)
        public static string PoemWordsQueue(string wordToAdd)
        {
            if (IsQueueFull())
            {
                CloseProgram("Queue is full! That's not supposed to happen...");
                return null;
            }
            else
            {
                AddToQueue(wordToAdd);
            }

            // Shortened if/else using ternary operator ?:
            // Returns null if the queue is empty, otherwise reads from queueFront
            return (IsQueueEmpty()) ? null : ReadFromQueue();

            // Expanded version of above ?: operation
            //if (IsQueueEmpty())
            //{
            //    CloseProgram("Queue is empty! That's not supposed to happen...");
            //    return null;
            //}
            //else
            //{
            //    // Returns the word that was at the front of the queue
            //    return ReadFromQueue();
            //}
        }

        public static void AddToQueue(string wordToAdd)
        {
            // Adds the given word at the rear of the queue
            circularPoemQueue[queueRear] = wordToAdd;

            // Advances the queue to the current rear location + 1 modulo the max size
            // Examples:
            // max = 10, rear = 5
            // 5 = ((5 + 1) % 10)
            //   = (6 % 10) = 6
            //
            // max = 10, rear = 7
            // 7 = ((7 + 1) % 10)
            //   = (8 % 10) = 8
            //
            // max = 10, rear = 9
            // 9 = ((9 + 1) % 10)
            //   = (10 % 10) = 0
            queueRear = ((queueRear + 1) % queueMax);
        }

        public static string ReadFromQueue()
        {
            // Sets the word to return to the location at the front of the queue
            string wordToReturn = circularPoemQueue[queueFront];

            // Advances the queue to the current front location + 1 modulo the max size
            // See AddToQueue above for examples; same idea just uses the front instead of the rear
            queueFront = ((queueFront + 1) % queueMax);

            // Returns the word that was set
            return wordToReturn;
        }

        public static bool IsQueueFull()
        {
            // Shortened if/else using the ternary operator ?:
            // Returns true if the queue is full
            // Queue will always leave one empty slot, otherwise IsQueueEmpty and
            // IsQueueFull could both return true on the same queue state
            return (queueFront == ((queueRear +1) % queueMax)) ? true : false;
        }

        public static bool IsQueueEmpty()
        {
            // Shortened if/else using the ternary operator ?:
            // Returns true if the queue is empty
            return (queueFront == queueRear) ? true : false;
        }

        //
        //
        //
        //
        //
        // (currently) Unused code for checking virtual memory pages
        // Could put queue in virtualMemory I suppose, but I don't think we're supposed to
        // and given a circular queue's length can fit on one row it doesn't seem necessary to practice
        public static int WhichPageToCheck()
        {
            var pageInput = 0;
            do
            {
                CenterWrite("Enter the page to check for data");
                var input = Console.ReadLine();
                int.TryParse(input, out pageInput);
            } while ((pageInput > lastPage) || (pageInput < 0));

            return pageInput;
        }

        public static void IsPageEmpty(int pageToCheck)
        {
            for (int row = 0; row < totalRow; row++)
            {
                for (int col = 0; col < totalCol; col++)
                {
                    if (virtualMemory[pageToCheck, row, col] != virtualNull)
                    {
                        Console.WriteLine("virtualMemory location: (Page: {0}, Row: {1}, Column: {2}) has {3} in it", pageToCheck, row, col, virtualMemory[pageToCheck, row, col]);
                        CloseProgram("The chosen page of virtual memory, where the queue should go, is not empty.");
                    }
                }
            }
        }
    }
}
