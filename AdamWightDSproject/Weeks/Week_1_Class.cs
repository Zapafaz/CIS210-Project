using System;
using System.IO;

namespace AdamWightDSproject
{
    class Week_1_Class
    {
        public static void TextToASCIIdec()
        {
            Common_Code.ShowHeader();
            
            var textDir = Common_Code.textDir;

            // Calls TFileSelect method to find what text file they want to use (in this case for conversion)
            var fileToConv = TFileSelect();

            // Appends "ConV" to filename based on chosen file, assigns result to newFile
            var newFile = "ConV" + fileToConv;
            
            // Confirmation of chosen file, option to restart method to choose new file; if they choose no (YesNo method returns false), restarts method
            Console.WriteLine("The contents of {0} will be read to ASCII decimal values\nValues will output to new file {1}", fileToConv, newFile);
            if (!Common_Code.YesNo("Is this ok?"))
            {
                TextToASCIIdec();
            }

            // Uses StreamReader.Read method to read one character at a time from the text file
            // Then uses StreamReader.WriteLine method to write ASCII decimal value for each character
            // Writes to a file with newFile variable as name, in the textDir folder
            using (var readChar = new StreamReader(textDir + fileToConv))
            {
                var currentChar = readChar.Read();
                using (var write = new StreamWriter(textDir + newFile))
                {
                    while (currentChar != -1)
                    {
                        write.WriteLine("{0}", currentChar);
                        currentChar = readChar.Read();
                    }
                }
            }

            // Tells user what it did, asks if they'd like to convert another file
            // If YesNo method returns true, restarts TextToASCIIdec method
            Console.WriteLine("Converted {0} to {1}", fileToConv, newFile);
            if (Common_Code.YesNo("Would you like to convert another file?"))
            {
                TextToASCIIdec();
            }          
        }

        private static string TFileSelect()
        {
            var textDir = Common_Code.textDir;

            // Creates array of file names in textDir
            string[] textFiles = Directory.GetFiles(textDir);

            // Removes path from each string in textFiles array, based on textDir string length
            for (int i = 0; i < textFiles.Length; i++)
            {
                textFiles[i] = textFiles[i].Remove(0, textDir.Length);
            }

            // Displays filename for each file in textFiles array, numbered 1, 2, 3, etc, by count
            var count = 0;
            foreach (string file in textFiles)
            {
                count += 1;
                Console.WriteLine("{0}: {1}", count, file);
            }

            // Tests input for valid integer input, based on number of files in the textFiles array (i.e. the number of files in the textDir folder)
            // Then subtracts 1 from fileInput so the user's input matches array index
            var fileInput = 0;
            do
            {
                var request = string.Format("Enter text file to use (1 to {0})", textFiles.Length);
                Common_Code.CenterWrite(request);
                var input = Console.ReadLine();
                int.TryParse(input, out fileInput);
            } while ((fileInput <= 0) && (fileInput > textFiles.Length));
            fileInput -= 1;

            return textFiles[fileInput];
        }
    }
}
