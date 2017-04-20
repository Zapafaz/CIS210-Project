/*
 * Student: Adam Wight
 * Class: CIS210M Data Structures and Elementary Algorithms
 * Instructor: Ed Cauthorn
 * Due date: Sunday, May 7th (FINAL)
 */

using System;

namespace AdamWightDSproject
{
    public static class Week_12_Class
    {
        private static int minValue = 0;
        private static int maxValue = 1500000;

        private static readonly int pageSize = Common_Code.totalRow * Common_Code.totalCol;

        /// <summary>
        /// Perform sorts (bucket, frequency, bubble) on a page of randomized integers in virtual memory and log their efficiency.
        /// </summary>
        public static void SortComparisons()
        {
            Common_Code.ShowHeader();

            Common_Code.VirtualMemoryInit();

            int pageToTwoDBucketSort = 200;
            int pageToFrequencySort = 205;
            int pageToBubbleSort = 210;

            // Use the default 0 and 1,500,000 as min and max to populate the pages with random numbers.
            // Not counting this towards the logCounter value since it's not part of the sorts being performed.
            PopulatePage(pageToTwoDBucketSort, minValue, maxValue);
            PopulatePage(pageToFrequencySort, minValue, maxValue);
            PopulatePage(pageToBubbleSort, minValue, maxValue);

            // Increment this value by 1 every time a variable (other than the logCount variable itself) changes.
            // Includes variable changes in iterators (e.g. for (int i = 0; i < 100; i++) should increase logCount by 1 for each iteration, for a total of 100).
            // Does not include variable declaration/initialization.
            int logCount = 0;

            // FindMinAndMax() Reassigns minValue and maxValue to whatever the lowest and highest numbers that were generated are (respectively).
            // Tracks how many iterations it takes (17 * 80 = 1360) to find the min and max values so it can be added to logCount.
            // Also, not every sort uses minValue and maxValue - only Frequency Sort and Bucket Sort, so I'm only increasing logCount by this for those sorts.
            int findMinAndMax = FindMinAndMax(pageToTwoDBucketSort);
            #region Sorts
            #region 2-Dimensional Bucket Sort
            // Set logCount to findMinAndMax since BucketSort uses them
            logCount += findMinAndMax;

            // Get the number of times to log based on operations performed during the bucket sort (i.e. total variable assignments it took to sort).
            logCount += TwoDBucketSortPage(pageToTwoDBucketSort);

            Week_10_Class.WriteEfficiencyLog(pageSize, logCount, Common_Code.arrayBucketSort);

            Console.WriteLine("After bucket sort on page {0}, is it sorted?: {1}", pageToTwoDBucketSort, IsPageSorted(pageToTwoDBucketSort));
            Common_Code.DisplayFooter();
            #endregion

            #region Frequency Sort
            // Reset logCount to findMinAndMax for frequency sort.
            logCount = findMinAndMax;

            logCount += FrequencySortPage(pageToFrequencySort);

            Week_10_Class.WriteEfficiencyLog(pageSize, logCount, Common_Code.frequencySort);

            Console.WriteLine("After frequency sort on page {0}, is it sorted?: {1}", pageToFrequencySort, IsPageSorted(pageToFrequencySort));
            Common_Code.DisplayFooter();
            #endregion

            #region Bubble Sort
            // Reset logCount to 0 for bubble sort.
            logCount = 0;

            // Get number of times to log based on bubble sort operations.
            logCount += LazyBubbleSortPage(pageToBubbleSort);

            Week_10_Class.WriteEfficiencyLog(pageSize, logCount, Common_Code.bubbleSort);

            Console.WriteLine("After bubble sort on page {0}, is it sorted?: {1}", pageToBubbleSort, IsPageSorted(pageToBubbleSort));
            Common_Code.DisplayFooter();
            #endregion
            #endregion

            // Writes locations in virtual memory to a log, excluding -99 (virtualNull), so the sorts can be manually checked for correctness.
            Common_Code.VirtualMemoryLog(12, false);
        }

        /// <summary>
        /// Bucket sort the given page in virtual memory, with a 2D array for the buckets (bubble sort to sort the buckets) and then sends that array to WriteToPage to be written back to the given page.
        /// </summary>
        /// <param name="pageToSort">The page in virtual memory to sort.</param>
        /// <returns>Returns the total number of variable changes performed by the sort.</returns>
        private static int TwoDBucketSortPage(int pageToSort)
        {
            var logCount = 0;
            int bucketSize = (maxValue - minValue) / pageSize;
            int bucketCount = (maxValue - minValue) / bucketSize + 1;
            var buckets = new int[bucketCount, bucketSize];
            var columnPointers = new int[bucketCount];
            var zeroPointers = new int[bucketCount];
            var bucketRow = 0;
            var tempExchange = 0;
            var nextBucket = false;

            // Fill buckets
            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    bucketRow = (Common_Code.virtualMemory[pageToSort, row, col] - minValue) / bucketSize;
                    buckets[bucketRow, columnPointers[bucketRow]] = Common_Code.virtualMemory[pageToSort, row, col];
                    columnPointers[bucketRow]++;

                    logCount += 3;
                }
            }

            // Increase by pageSize (1360) - i.e. once for each time row and col iterated
            logCount += pageSize;

            // Get the location of the first 0 in each bucket
            for (int bucket = 0; bucket < buckets.GetLength(0); bucket++)
            {
                logCount++;
                for (int slot = 0; slot < buckets.GetLength(1); slot++)
                {
                    logCount++;
                    if (buckets[bucket, 0] < minValue)
                    {
                        break;
                    }

                    if (buckets[bucket, slot + 1] < minValue)
                    {
                        zeroPointers[bucket] = slot + 1;
                        logCount++;
                        break;
                    }
                }
            }

            int remove;

            // Bubble sort each bucket
            for (int bucket = 0; bucket < buckets.GetLength(0); bucket++)
            {
                logCount++;
                for (int bubbler = zeroPointers[bucket] - 1; bubbler > 0; bubbler--)
                {
                    logCount++;
                    for (int slot = 0; slot < zeroPointers[bucket]; slot++)
                    {
                        logCount++;

                        // If the bucket is empty, go to next bucket by breaking out to outermost for loop.
                        if (buckets[bucket, 0] < minValue)
                        {
                            nextBucket = true;
                            logCount++;
                            break;
                        }

                        // If the next value is empty, bubble again.
                        if (buckets[bucket, slot + 1] < minValue)
                        {
                            break;
                        }

                        // If the current slot is greater than the next slot, exchange them.
                        if (buckets[bucket, slot] > buckets[bucket, (slot + 1)])
                        {
                            remove = buckets[bucket, (slot + 1)];
                            tempExchange = buckets[bucket, slot];
                            buckets[bucket, slot] = buckets[bucket, (slot + 1)];
                            buckets[bucket, (slot + 1)] = tempExchange;

                            logCount += 3;
                        }
                    }
                    if (nextBucket)
                    {
                        nextBucket = false;
                        logCount++;
                        break;
                    }
                }
            }

            logCount += WriteToPage(pageToSort, buckets);

            return logCount;
        }

        /// <summary>
        /// Sorts a given page in virtualMemory by creating a new array from that page (hence lazy) and bubble sorting that array, then sends that array to WriteToPage to be written back to the given page.
        /// </summary>
        /// <param name="page">The page to sort.</param>
        /// <returns>Returns the total number of variable changes performed by the sort.</returns>
        private static int LazyBubbleSortPage(int page)
        {
            var logCount = 0;
            int index = 0;
            int[] sort = new int[pageSize];

            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    sort[index] = Common_Code.virtualMemory[page, row, col];
                    index++;
                }
            }

            // Increase by pageSize (1360) times two - i.e. once for each time row and col iterated
            // once for each time we assign to the page and increment sorted index.
            logCount += (pageSize * 2) + 2;

            var week10 = new Week_10_Class();

            // Sort the array and tell the method not to log whats in the array after sorting (we write all of virtualmemory anyway)
            logCount += week10.BubbleSort(sort, false);

            logCount += WriteToPage(page, sort);

            return logCount;
        }

        /// <summary>
        /// Counts the number of occurrences of each value on the given page, then sends them to be written (sorted) to the given page.
        /// </summary>
        /// <param name="page">The page to count values on.</param>
        /// <param name="count">The total number of different values to check. Defaults to 1500000 (original max size).</param>
        /// <returns>Returns the total number of variable changes performed by the method.</returns>
        private static int FrequencySortPage(int page, int count = 1500000)
        {
            var logCount = 0;
            var frequencyCount = new int[count];

            for (int row = 0; row < Common_Code.virtualMemory.GetLength(1); row++)
            {
                for (int col = 0; col < Common_Code.virtualMemory.GetLength(2); col++)
                {
                    frequencyCount[Common_Code.virtualMemory[page, row, col]]++;
                    logCount++;
                }
            }

            // Increase by pageSize (1360) - i.e. once for each time row and col iterated (17 rows * 80 cols)
            logCount += pageSize;

            logCount += WriteFrequencyToPage(page, frequencyCount);

            return logCount;
        }

        /// <summary>
        /// Writes the given frequency counters array to the given page in virtual memory.
        /// </summary>
        /// <param name="page">The page to write to.</param>
        /// <param name="counters">The array to write.</param>
        /// <returns>Returns the total number of variable changes performed by the method.</returns>
        private static int WriteFrequencyToPage(int page, int[] counters)
        {
            var logCount = 0;
            var row = 0;
            var col = 0;

            for (int value = 0; value < counters.Length; value++)
            {
                logCount++;
                for (int count = 0; count < counters[value]; count++)
                {
                    Common_Code.virtualMemory[page, row, col] = value;
                    col++;
                    logCount += 3;
                    if (col >= Common_Code.totalCol)
                    {
                        row++;
                        logCount++;
                        if (row >= Common_Code.totalRow)
                        {
                            return logCount;
                        }
                        col = 0;
                        logCount++;
                    }
                }
            }

            return logCount;
        }

        /// <summary>
        /// Writes the sorted one-dimensional array to the given page in virtual memory.
        /// </summary>
        /// <param name="page">The page to write the array to.</param>
        /// <param name="sorted">The sorted one-dimensional array to write.</param>
        /// <returns>Returns the total number of variable changes performed by the write.</returns>
        private static int WriteToPage(int page, int[] sorted)
        {
            var logCount = 0;
            var index = 0;

            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    Common_Code.virtualMemory[page, row, col] = sorted[index];
                    index++;
                }
            }

            // Increase by pageSize (1360) * 3 - in other words, increment once for each time row and col iterated
            // and once for each time we assign to the page and increment sorted index.
            logCount += pageSize * 3;

            return logCount;
        }

        /// <summary>
        /// Writes the sorted two-dimensional array to the given page in virtual memory.
        /// </summary>
        /// <param name="page">The page to write the array to.</param>
        /// <param name="sorted">The sorted two-dimensional array to write.</param>
        /// <returns>Returns the total number of variable changes performed by the write.</returns>
        private static int WriteToPage(int page, int[,] sorted)
        {
            var logCount = 0;
            var sortedRow = 0;
            var sortedCol = 0;

            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    while (sorted[sortedRow, sortedCol] < minValue)
                    {
                        sortedRow++;
                        if (sortedRow >= sorted.GetLength(0))
                        {
                            return logCount;
                        }
                        sortedCol = 0;
                        logCount += 2;
                    }
                    Common_Code.virtualMemory[page, row, col] = sorted[sortedRow, sortedCol];
                    sortedCol++;
                    logCount += 2;
                }
            }

            // Increase by pageSize (1360) - i.e. once for each time row and col iterated
            logCount += pageSize;

            return logCount;
        }

        /// <summary>
        /// Get the minimum and maximum values on the page to be sorted.
        /// </summary>
        /// <param name="pageToSort">The page to be sorted.</param>
        /// <returns>Returns an approximate number of the operations performed.</returns>
        /// <remarks>Need to do this since Random is not guaranteed to generate a value at the given minimum (0) and maximum (1,500,000)
        /// it only guarantees that the numbers generated are somewhere between those values.</remarks>
        private static int FindMinAndMax(int pageToSort)
        {
            int logCount = 0;
            minValue = Common_Code.virtualMemory[pageToSort, 0, 0];
            maxValue = Common_Code.virtualMemory[pageToSort, 0, 0];
            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    if (minValue > Common_Code.virtualMemory[pageToSort, row, col])
                    {
                        minValue = Common_Code.virtualMemory[pageToSort, row, col];
                    }
                    else if (maxValue < Common_Code.virtualMemory[pageToSort, row, col])
                    {
                        maxValue = Common_Code.virtualMemory[pageToSort, row, col];
                    }
                    logCount++;
                }
            }
            return logCount;
        }

        /// <summary>
        /// Populate a page (17 row x 80 col = 1360 locations) in virtual memory with random numbers from min to max.
        /// </summary>
        /// <param name="pageToPopulate">The page in virtual memory to populate.</param>
        /// <param name="min">The minimum value that can be generated. Not guaranteed to be generated, but will not generate less than this.</param>
        /// <param name="max">The maximum value that can be generated. Not guaranteed to be generated, but will not generate greater than this.</param>
        /// <param name="seed">The seed value to use for the random number generator. Defaults to 1 for testing and debugging consistency.</param>
        private static void PopulatePage(int pageToPopulate, int min, int max, int seed = 1)
        {
            // Uses Random(seed) of 1 by default; makes it easier to test since it will always generate the same set of random numbers
            var rng = new Random(seed);

            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    Common_Code.virtualMemory[pageToPopulate, row, col] = rng.Next(min, max);
                }
            }
        }

        /// <summary>
        /// Checks if a given page is sorted in ascending order (i.e. index 0 should be the lowest value).
        /// </summary>
        /// <param name="page">The page in virtualMemory to check.</param>
        /// <returns>Returns true if the page is sorted; returns false at first unsorted value.</returns>
        private static bool IsPageSorted(int page)
        {
            // Declare and fill a temporary array with a given page from virtualMemory.
            var temp = new int[pageSize];
            var index = 0;
            for (int row = 0; row < Common_Code.totalRow; row++)
            {
                for (int col = 0; col < Common_Code.totalCol; col++)
                {
                    temp[index] = Common_Code.virtualMemory[page, row, col];
                    index++;
                }
            }

            // Check that temporary array for out-of-order values.
            for (int i = 1; i < temp.Length; i++)
            {
                if (temp[i - 1] > temp[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Unused - for debugging. Writes a given 2d int array to the console, except any 0 values.
        /// </summary>
        private static int WriteBucketsToConsole(int[,] buckets)
        {
            var logCount = 0;
            for (int bucketRow = 0; bucketRow < buckets.GetLength(0); bucketRow++)
            {
                for (int bucketCol = 0; bucketCol < buckets.GetLength(1); bucketCol++)
                {
                    if (buckets[bucketRow, bucketCol] != 0)
                    {
                        Console.Write("{0} ", buckets[bucketRow, bucketCol]);
                        logCount++;
                    }
                }
            }

            Console.WriteLine();

            return logCount;
        }
    }
}
