using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace IG_All_Feature.Helper
{
    public static class SoftBucketFileUtillity
    {
        private static int _bufferSize = 16384;

        public static List<string> ReadFile(string filename)
        {
            List<string> listFileContent = new List<string>();

            StringBuilder stringBuilder = new StringBuilder();
            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    char[] fileContents = new char[_bufferSize];
                    int charsRead = streamReader.Read(fileContents, 0, _bufferSize);

                    // Can't do much with 0 bytes
                    //if (charsRead == 0)
                    //    throw new Exception("File is 0 bytes");

                    while (charsRead > 0)
                    {
                        stringBuilder.Append(fileContents);
                        charsRead = streamReader.Read(fileContents, 0, _bufferSize);
                    }

                    string[] contentArray = stringBuilder.ToString().Split(new char[] { '\r', '\n' });
                    foreach (string line in contentArray)
                    {
                        listFileContent.Add(line.Replace("#", "").Replace("\0", string.Empty));
                    }
                    listFileContent.RemoveAll(s => string.IsNullOrEmpty(s));
                }
            }
            return listFileContent;
        }

        public static void AppendStringToTextfileNewLine(String content, string filepath)
        {

            StreamWriter writer = new StreamWriter(filepath, true);

            StringReader reader = new StringReader(content);

            string temptext = "";

            while ((temptext = reader.ReadLine()) != null)
            {
                writer.WriteLine(temptext);
            }

            writer.Close();
        }

        public static void WriteListtoTextfile(List<string> list, string filepath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    foreach (string listitem in list)
                    {
                        writer.WriteLine(listitem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
