using System;

namespace AdamWightDSproject
{
    /// <summary>
    /// Class for Main method with intro, week selection, and outro for the program
    /// </summary>
    class Intro_Outro
    {
        static void Main(string[] args)
        {
            Common_Code.ShowHeader();
            WeekSelect();

            // Calls CenterWrite method and sends it the given string to display centered in console
            Common_Code.CenterWrite("Press Enter to open the log folder and exit the program...");
            Console.ReadLine();
            Common_Code.OpenLogFolder();
        }

        /// <summary>
        /// Prompts the user to select which section (week) to run
        /// </summary>
        /// <remarks>Week input do-while loop had a bug almost the whole semester where it wasn't actually doing anything because I used && (AND) instead of || (OR). Whoops.</remarks>
        private static void WeekSelect()
        {
            // Tests input for valid integers (1 through 16), asks again if input is invalid
            var weekInput = 0;
            do
            {
                Common_Code.CenterWrite("Enter class week (1 to 16)");
                var input = Console.ReadLine();
                int.TryParse(input, out weekInput);
            } while ((weekInput <= 0) || (weekInput > 16));
            
            var labWeek = string.Format("Week {0} was a lab week. Please choose a different week.", weekInput);

            var futureWeek = string.Format("We aren't at week {0} yet. Please choose a different week.", weekInput);

            // Sends user to Week_#_Class.Method based on input, restarts WeekSelect method if incorrect input
            // Defaults to restarting WeekSelect method
            switch (weekInput)
            {
                case 1:
                    Week_1_Class.TextToASCIIdec();
                    break;
                case 2:
                    Week_2_Class.PoemReadWrite();
                    break;
                case 3:
                    // Sends user to choose which search method to use
                    // Figure we might implement more searches later on down the line
                    // Can just point back to this case for those weeks
                    Common_Code.ChooseSearch();
                    break;
                case 4:
                    Console.WriteLine(labWeek);
                    WeekSelect();
                    break;
                case 5:
                    Week_5_Class.FindUppercaseT();
                    break;
                case 6:
                    Week_6_Class.VirtualStacks();
                    break;
                case 7:
                    goto case 4;
                case 8:
                    Week_8_Class.VirtualQueue();
                    break;
                case 9:
                    goto case 4;
                case 10:
                    // Create new instance of Week_10_Class and start the BubbleSortAndLog method from that class
                    var week_10 = new Week_10_Class();
                    week_10.BubbleSortAndLog();
                    break;
                case 11:
                    Week_11_Class.FrequencySortAndLog();
                    break;
                case 12:
                    goto case 16;
                case 13:
                    goto case 16;
                case 14:
                    goto case 16;
                case 15:
                    goto case 16;
                case 16:
                    Week_12_Class.SortComparisons();
                    break;
                default:
                    Console.WriteLine("Entered {0} week, ended in default case - please select a valid week", weekInput);
                    WeekSelect();
                    break;
            }
        }
    }
}
