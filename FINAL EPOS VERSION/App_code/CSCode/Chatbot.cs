using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class Chatbot
{
    private readonly List<TrainingData> _trainingData;

    public Chatbot(string trainingDataPath)
    {
        _trainingData = LoadTrainingData(trainingDataPath) ?? new List<TrainingData>();
    }

    public string GetResponse(string userInput)
    {
        var intent = ClassifyIntent(userInput);
        var matchingData = _trainingData.FirstOrDefault(data => data.Intent == intent);

        return matchingData != null ? matchingData.Response : "Sorry, I don't understand.";
    }

    private List<TrainingData> LoadTrainingData(string filePath)
    {
        string jsonData = "";
        try
        {
            jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<TrainingData>>(jsonData);
            WriteToResponse("If good jsonData=" + jsonData);
        }
        catch (Exception ex)
        {
            WriteToResponse("##If Error jsonData=" + jsonData);
            return null; // Handle the exception or log it as needed
        }
    }

    private string ClassifyIntent(string userInput)
    {
        try
        {
            if (_trainingData == null || !_trainingData.Any())
            {
                return "Unknown";
            }

            foreach (var data in _trainingData)
            {
                if (userInput.ToLower().Contains(data.Query.ToLower()))
                {
                    return data.Intent;
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return "### error occurred: " + ex.Message + "userInput" + userInput;
           
        }

        return "Unknown";
    }
    public void WriteToResponse(string message)
    {
        HttpContext.Current.Response.Write(message);
    }
}

public class TrainingData
{
    public string Query
    {
        get; set;
    }
    public string Intent
    {
        get; set;
    }
    public string Response
    {
        get; set;
    }
}


// usage code example follows
//protected void Page_Load(object sender, EventArgs e)
//{
//    Chatbot chatbot = new Chatbot(Server.MapPath("~/App_Data/trainingData.json"));
//    string userInput = "Tell me a joke."; // Example user input
//    string response = chatbot.GetResponse(userInput);
//    Response.Write(response);
// }

