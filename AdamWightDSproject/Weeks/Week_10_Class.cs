/*
 * Student: Adam Wight
 * Class: CIS210M Data Structures and Elementary Algorithms
 * Instructor: Ed Cauthorn
 * Due date: Sunday, April 2nd
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamWightDSproject
{
    /// <summary>
    /// Bubble sort and logging to test algorithm efficiency
    /// </summary>
    class Week_10_Class
    {
        private int temporaryExchange;

        /// <summary>
        /// Show header, initialize virtualMemory, bubble sort poems by incremented segments and log number of exchanges used by each sort
        /// </summary>
        public void BubbleSortAndLog()
        {
            // Shows a header in the console similar to the comment at the top of this file
            Common_Code.ShowHeader();

            // Initializes all virtualMemory locations to -99
            Common_Code.VirtualMemoryInit();

            // Reads all three of the poems (TCOTLB.txt, RC.txt, GEAH.txt) in textDir (C:\devel\TFiles\)
            Week_2_Class.PoemReader();

            // Search virtualMemory for the text of the poems and put it in an array
            int[] allPoems = GetPoemsInVirtualMemory();
            
            int totalPoemsLength = allPoems.Length;

            // Variable to track the number of exchanges used in a sort
            int exchanges;

            // Sort a segment of allPoems of size 10
            exchanges = BubbleSortSegment(allPoems, 10);

            // Write the log from the size 10 sort
            WriteEfficiencyLog(10, exchanges, Common_Code.bubbleSort);

            // Sort and log segments of the array starting at 100 and incrementing by 100 every time (100, 200, 300, etc) up to 2000
            for (int i = 100; i <= 2000; i += 100)
            {
                exchanges = BubbleSortSegment(allPoems, i);
                WriteEfficiencyLog(i, exchanges, Common_Code.bubbleSort);
            }

            bool keepSorting = Common_Code.YesNo("Keep sorting segments from size 2000 to 5927 (may take a few minutes)");

            if (keepSorting)
            {
                // Sort and log segments of the array starting at 2000 and incrementing by 100 every time (2000, 2100, 2200, etc) up to 5900
                for (int i = 2000; i <= totalPoemsLength; i += 100)
                {
                    exchanges = BubbleSortSegment(allPoems, i);
                    WriteEfficiencyLog(i, exchanges, Common_Code.bubbleSort);
                }

                // Sort the entire array
                exchanges = BubbleSort(allPoems);

                // Write the log for the full sort
                WriteEfficiencyLog(totalPoemsLength, exchanges, Common_Code.bubbleSort);
            }

            // Write all locations in the sortable source array to log file for debugging
            Common_Code.IntArrayLog(allPoems, "AllPoems");

            // Write all locations in virtual memory to log file for debugging
            Common_Code.VirtualMemoryLog(10, true);
        }

        /// <summary>
        /// Sorts the values of the array by stepping through it repeatedly and comparing each value to the next.
        /// Returns the total number of exchanges made by the sort.
        /// </summary>
        /// <param name="toSort">The array to be sorted.</param>
        /// <returns>Total number of exchanges performed by the sort.</returns>
        public int BubbleSort(int[] toSort, bool log = true)
        {
            int totalExchanges = 0;

            for (int number = toSort.Length - 1; number > 0; number--)
            {
                for (int i = 0; i < number; i++)
                {
                    if ((toSort[i]) > (toSort[i + 1]))
                    {
                        ExchangeValues(toSort, i, (i + 1));
                        totalExchanges += 3;
                    }
                }
            }

            if (log)
            {
                // Write all locations in the sorted array to log for debugging
                Common_Code.IntArrayLog(toSort, "Size" + toSort.Length.ToString());
            }

            return totalExchanges;
        }

        /// <summary>
        /// Exchange two given values in the given array
        /// </summary>
        /// <param name="exchangable">The array to exchange the values in</param>
        /// <param name="swapOne">The first index to exchange</param>
        /// <param name="swapTwo">The second index to exchange</param>
        private void ExchangeValues(int[] exchangable, int swapOne, int swapTwo)
        {
            temporaryExchange = exchangable[swapOne];
            exchangable[swapOne] = exchangable[swapTwo];
            exchangable[swapTwo] = temporaryExchange;
        }

        /// <summary>
        /// Sort a segment of segmentSize from the source array (without altering source array) using the BubbleSort method.
        /// Returns the number of exchanges used to sort the segment.
        /// </summary>
        /// <param name="source">The source array to take a segment of</param>
        /// <param name="segmentSize">The size of the segment to sort</param>
        /// <returns>Returns the number of exchanges used by the sort in order to sort the segment</returns>
        private int BubbleSortSegment(int[] source, int segmentSize)
        {
            var segmentToSort = new int[segmentSize];

            Array.Copy(source, segmentToSort, segmentSize);
            
            return BubbleSort(segmentToSort);
        }

        /// <summary>
        /// Writes a log file with a line of "1" listed for the value in logAmount.
        /// </summary>
        /// <param name="segmentSize">The size of the segment that the algorithm was performed on.</param>
        /// <param name="logAmount">The number of ones to log; a measure of operations that were performed by the algorithm.</param>
        /// <param name="algorithmType">The type of algorithm that was performed.</param>
        public static void WriteEfficiencyLog(int segmentSize, int logAmount, string algorithmType)
        {
            string logFilePath = Common_Code.GetLogFilePath("Efficiency_" + algorithmType, ("Size" + segmentSize.ToString()));

            using (var logger = new StreamWriter(logFilePath))
            {
                for (int i = 0; i <= logAmount; i++)
                {
                    logger.WriteLine(1);
                }
            }
        }

        /// <summary>
        /// Performs serialized search on pages that contain poem text, returns it in int array
        /// </summary>
        /// <returns>Returns a one dimensional int array of the text of the poems</returns>
        public static int[] GetPoemsInVirtualMemory()
        {
            int[] startPoem = Common_Code.GetPoemStartPages();
            var count = 0;
            var temp = new List<int>();

        NEXT_POEM:
            if (count < startPoem.Length)
            {
                for (int page = startPoem[count]; page < Common_Code.virtualMemory.GetLength(0); page++)
                {
                    for (int row = 0; row < Common_Code.virtualMemory.GetLength(1); row++)
                    {
                        for (int col = 0; col < Common_Code.virtualMemory.GetLength(2); col++)
                        {
                            if (Common_Code.virtualMemory[page, row, col] != Common_Code.virtualNull)
                            {
                                temp.Add(Common_Code.virtualMemory[page, row, col]);
                            }

                            else
                            {
                                count++;
                                goto NEXT_POEM;
                            }
                        }
                    }
                }
            }    

            return temp.ToArray();
        }
    }
}
