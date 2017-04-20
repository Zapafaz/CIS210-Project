using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamWightDSproject
{
    class Week_9_Class
    {
        //Pushes the poems to the stack from the reversed log (from PopClean method), using logName to find the file
        //private static void PoemStackPush(StreamWriter logger)
        //{
        //    var pushed = false;
        //    try
        //    {
        //        using (StreamReader reader = new StreamReader(popLogName))
        //        {
        //            for (int row = 0; row < totalRow; row++)
        //            {
        //                for (int col = 0; col < totalCol; col++)
        //                {
        //                    // Implicit int cast of character the StreamReader is reading
        //                    int currentInt = reader.Read();

        //                    // If -1 is found (i.e. end of the file was reached), pushes a newline to the stack and log for readability
        //                    // Then sets pushed to true (because that means it pushed everything) and breaks out of the loops
        //                    if (currentInt == -1)
        //                    {
        //                        logger.Write((char)decNL);
        //                        VirtualStackPush(decNL);
        //                        pushed = true;
        //                        break;
        //                    }

        //                    // If it doesn't find a newline, push the current character (ASCII decimal) to stack and log, then continues looping
        //                    else
        //                    {
        //                        logger.Write((char)currentInt);
        //                        VirtualStackPush(currentInt);
        //                    }
        //                }

        //                // If it found a -1 (i.e. end of the file was reached), continues breaking out
        //                if (pushed)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        WhereAmI();
        //        Console.WriteLine("{0}", e);
        //        CloseProgram(genericError);
        //    }
        //}
    }
}
