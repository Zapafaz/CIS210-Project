/*
 * Student: Adam Wight
 * Class: CIS210M Data Structures and Elementary Algorithms
 * Instructor: Ed Cauthorn
 * Due date: Sunday, April 9th
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
    /// Frequency sort and algorithm efficiency
    /// </summary>
    class Week_11_Class
    {
        /// <summary>
        /// Use frequency sort to sort the poems and log the efficiency of the algorithm.
        /// </summary>
        public static void FrequencySortAndLog()
        {
            // Shows a header in the console similar to the comment at the top of this file
            Common_Code.ShowHeader();

            // Initializes all virtualMemory locations to -99
            Common_Code.VirtualMemoryInit();

            // Reads all three of the poems (TCOTLB.txt, RC.txt, GEAH.txt) in textDir (C:\devel\TFiles\)
            Week_2_Class.PoemReader();

            // Search virtualMemory for the text of the poems and put it in an array
            int[] allPoems = Week_10_Class.GetPoemsInVirtualMemory();

            // Set log count to total poem array length, since part of this algorithm is loading the poems into the array.
            int logOneCount = allPoems.Length;

            // Round the length of allPoems down to nearest 100
            // Integer division always rounds down so by dividing by 100 then multiplying by 100 we get the value we want
            // Examples:
            // 5927 / 100 = 59.27; C# rounds int to 59; 59 * 100 = 5900
            // 5977 / 100 = 59.77; C# rounds int to 59; 59 * 100 = 5900
            int poemLengthDownToHundred = allPoems.Length / 100 * 100;

            // Frequency sort segment of size i
            // Increase log count by the number of operations performed by a segment sort of size 10
            // 255 represents the size of the frequency counters array (i.e. ASCII values 0-255); Parameter exists in case I want to frequency sort things other than ASCII
            logOneCount += FrequencySortSegment(allPoems, 10, 255);

            // Write a log file with a 1 for the value of logOneCount
            Week_10_Class.WriteEfficiencyLog(10, logOneCount, Common_Code.frequencySort);

            // Repeat above processes for segments of size 100, 200, 300...5900, then size allPoems.Length
            for (int i = 100;
                i <= allPoems.Length;
                i = i < poemLengthDownToHundred ? (i + 100) : allPoems.Length)
            {
                // Set log count to total poem array length, since part of this algorithm is loading the poems into the array.
                logOneCount = allPoems.Length;

                // Frequency sort a segment of allPoems, segment size: i (100, 200, 300, etc)
                // Increase log count by the number of operations performed by that sort
                logOneCount += FrequencySortSegment(allPoems, i, 255);

                // Write a log file with a 1 for the value of logOneCount
                Week_10_Class.WriteEfficiencyLog(i, logOneCount, Common_Code.frequencySort);
                
                // Break out of loop if i == allPoems.Length, otherwise infinite loop
                if (i == allPoems.Length)
                {
                    break;
                }

                // Expanded version of above ? : ternary operation (in for() loop iterator)
                //if (i < poemLengthDownToHundred)
                //{
                //    i += 100;
                //}
                //else
                //{
                //    i = allPoems.Length;
                //}
            }

            // Open the log folder path given in Common_Code.logDir in explorer
            Common_Code.OpenLogFolder();
        }

        /// <summary>
        /// Get an array segment to sort from the given array.
        /// </summary>
        /// <param name="source">The source array to copy from.</param>
        /// <param name="segmentSize">The size of the segment to copy.</param>
        /// <param name="counterSize">The number of different values in the array; e.g. ASCII is 0-255, so counterSize = 255.</param>
        /// <returns>Returns a measure of the operations performed by the sort.</returns>
        private static int FrequencySortSegment(int[] source, int segmentSize, int counterSize)
        {
            var segmentToSort = new int[segmentSize];

            Array.Copy(source, segmentToSort, segmentSize);

            return FrequencySortCount(segmentToSort, counterSize);
        }

        /// <summary>
        /// Perform a frequency sort on the given array, with the given value being the number of different items in that array
        /// </summary>
        /// <param name="toSort">The array to be sorted.</param>
        /// <param name="counterSize">The number of different values in the array; e.g. ASCII is 0-255, so counterSize = 255.</param>
        /// <returns>Returns a measure of the operations performed by the sort.</returns>
        private static int FrequencySortCount(int[] toSort, int counterSize)
        {
            int logCount = 0;
            int[] frequencyCount = new int[counterSize];

            Common_Code.Populate(frequencyCount, 0);

            // Increment the counter at the index of the ASCII value by 1 and increment log count by 1 since we performed an operation
            for(int i = 0; i < toSort.Length; i++)
            {
                frequencyCount[toSort[i]]++;
                logCount++;
            }

            // I think this should work, test and stuff
            logCount += FrequencySortToFile(frequencyCount, toSort.Length);

            return logCount;
        }

        /// <summary>
        /// Write the given frequency sort array to a log file with the given value appended as "Size".
        /// </summary>
        /// <param name="counters">The an array of the count of each value to be written.</param>
        /// <param name="segmentSize">The size of the original array that was sorted.</param>
        public static int FrequencySortToFile(int[] counters, int segmentSize)
        {
            string path = Common_Code.GetLogFilePath(Common_Code.frequencySort, "SegmentSize" + segmentSize.ToString() + "_Counters" + counters.ToString());
	        int logCount = 1;
            using (var writer = new StreamWriter(path))
            {
                for (int i = 0; i < counters.Length; i++)
                {
                    for (int j = 0; j < counters[i]; j++)
                    {
                        writer.WriteLine(i);
                        logCount++;
                    }
                }
            }
	    return logCount;
        }
    }
}
