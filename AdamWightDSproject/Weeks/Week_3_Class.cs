using static AdamWightDSproject.Common_Code;

namespace AdamWightDSproject
{
    class Week_3_Class
    {
        public static void SerializedSearch()
        {
            ShowHeader();

            // Initializes all virtualMemory locations to -99
            VirtualMemoryInit();

            // Reads the poems into memory, but does not write them to console - that was split into Week_2_Class.PoemWriter()
            Week_2_Class.PoemReader();
            
            // Searches and writes log file based on search performed, creates "C:\devel\Logs\" directory if it does not exist
            CreateSearchLog(serializedSearch, blank, "find_virtualNulls");

            // Asks if they'd like to search again, sends back to search choice method if yes
            if (YesNo("Would you like to search again"))
            {
                ChooseSearch();
            }
        }

        public static void IndexSearch()
        {
            // Calls ShowHeader method and sends current class name string for arg
            ShowHeader();

            // Initializes all virtualMemory locations to -99
            VirtualMemoryInit();

            // Reads the poems into memory, but does not write them to console - that was split into Week_2_Class.PoemWriter()
            Week_2_Class.PoemReader();
            
            // Searches and writes log file based on search performed, creates "C:\devel\Logs\" directory if it does not exist
            CreateSearchLog(indexSearch, blank, "find_virtualNulls");

            // Asks if they'd like to search again, sends back to search choice method if yes
            if (YesNo("Would you like to search again"))
            {
                ChooseSearch();
            }
        }        
    }
}
