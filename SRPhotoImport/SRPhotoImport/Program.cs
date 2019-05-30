using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SRPhotoImport.Model;
using CsvHelper;

namespace SRPhotoImport
{
    class Program
    {
        static void Main(string[] args)
        {
            /* This program will take a file downloaded of StarRez students LSU ID and Entry ID,residents will
             * be searched against the Aux Services repository. If found with an 89# the student's photo will
             * be uploaded into StarRez. Program revised on 5-29-2019 using Visual Studio 2019 and C#
             * 
             * 
             * */

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 

            using (var sr = new StreamReader(@"C:\Users\ksplan2\Documents\StarRezPhotoImport\NoPhoto.csv"))
            {
                var reader = new CsvReader(sr);

                IEnumerable<Entry> records = reader.GetRecords<Entry>();

                int count = 0;

                Dictionary<string, int> openwith = new Dictionary<string, int>();

                foreach (Entry record in records)
                {
                    Console.WriteLine("EntryID:{0} LSUID:{1} ", record.EntryID, record.LSUID);
                    openwith.Add(record.LSUID, record.EntryID);
                    count++;

                }
                Console.WriteLine("The number of entries is {0}", count);

                string strTempID;
                string strTempID2;
                int entryID;
                string status;

                string[] dirs = Directory.GetFiles(@"Y:\", "*.JPG");

                Console.WriteLine("The number or jpeg files is  {0}.", dirs.Length);

                Dictionary<string, string> Pfile = new Dictionary<string, string>();

                foreach (string dir in dirs)
                {
                    entryID = 0;
                    status = "";
                    strTempID = dir.Replace(@"Y:\", "");
                    strTempID2 = strTempID.Replace(".jpg", "");

                    // Console.WriteLine("StrTempID: {0} strTempID2: {1} dir: {2} ", strTempID, strTempID2, dir);

                    Pfile.Add(strTempID2, dir);

                }

                int count1 = 0;
                foreach (KeyValuePair<string, string> fileData in Pfile)
                {
                    foreach (KeyValuePair<string, int> studentlist in openwith)
                    {
                        if (fileData.Key == studentlist.Key)
                        {
                            //  Console.WriteLine("This entryID:{0} is in the list and directory", studentlist.Value);
                            //   Console.WriteLine("This is fileData value:{0} this is studentlist value:{1}",fileData.Value,studentlist.Value);
                            Sub.Photoload(fileData.Value, studentlist.Value);
                            count1++;
                        }
                    }
                }

                Console.WriteLine("The number of files uploaded is {0}", count1);
            }

        }
    }
}
