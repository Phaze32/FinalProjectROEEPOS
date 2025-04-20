using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TextProcessClass
/// </summary>
public class TextProcessClass
{
    public TextProcessClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static Dictionary<string, string> ReadKeyValuePairsFromFile(string filePath)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        try
        {
            // Open the text file for reading
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;

                // Read each line of the file
                while ((line = reader.ReadLine()) != null)
                {
                    // Skip empty lines or lines starting with comments (optional)
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        continue;
                    }

                    // Split the line based on a delimiter (e.g., colon)
                    string[] keyValue = line.Split(':');

                    // Check for valid key-value pairs (key and value should exist)
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();

                        // Add the key-value pair to the dictionary
                        dictionary.Add(key, value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., file not found, invalid format)
            Console.WriteLine("Error reading file: {0}", ex.Message);
        }

        return dictionary;
    }
}