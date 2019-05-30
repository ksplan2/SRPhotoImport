﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net;
using System.Web;
using System.Media;
using System.Drawing.Imaging;

namespace SRPhotoImport.Model
{
   public class Sub
    {
        public static void Photoload(string path, int EID)
        {
            string base64String;

            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    base64String = Convert.ToBase64String(imageBytes);

                }

            }
            Console.WriteLine("EntryID:{0} was uploaded", EID);
            // Console.WriteLine("This is base64String:{0}",base64String);
            // Create a request using a URL that can receive a post.
            //Fill in URL Server information
            WebRequest request = WebRequest.Create("/starrezrest/services/setphoto/entry/" + EID);

            // Set the Method property to of the request to POST.
            request.Method = "POST";
            // Add username 
            request.Headers.Add("StarRezusername", "username");
            //Add password
            request.Headers.Add("StarRezPassword", "password");

            // Create POST data and convert it to a byte array.
            // string postData =
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(base64String);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x=www-form-urlencoded";

            // Get the request stream
            Stream dataStream = request.GetRequestStream();

            Console.WriteLine("DataStream:{0}");
            //Write the data to the request stream
            dataStream.Write(byteArray, 0, byteArray.Length);
            Console.WriteLine("EntryID:{0} was uploaded", EID);
            //Close the stream object
            dataStream.Close();

            //Get the response
            WebResponse response = request.GetResponse();

            //Display the status
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server
            dataStream = response.GetResponseStream();

            //Open the stream using a StreamReader for easy access
            StreamReader reader = new StreamReader(dataStream);

            //Read the content.
            string reponseFromServer = reader.ReadToEnd();

            //Display the content.
            Console.WriteLine(reponseFromServer);

            //Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();


        }
    }
}
