using AIMLbot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Chatbot_AIML
{
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

    public class Chatbot_AIML
    {
        private Bot bot;
        private User user;
        private Dictionary<string, string> responses;

        public Chatbot_AIML()
        {
            try
            {
                bot = new Bot();
                Log("Bot object initialized.");

                string settingsPath = HttpContext.Current.Server.MapPath("~/config/Settings.xml");
                if (File.Exists(settingsPath))
                {
                    Log("Settings file found at path: " + settingsPath);

                    try
                    {
                        // Read and log Settings.xml content for debugging
                        string settingsContent = File.ReadAllText(settingsPath);
                        Log("Settings file content: " + settingsContent);

                        bot.loadSettings(settingsPath);
                        Log("Settings loaded.");
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error loading settings: " + ex.Message, ex);
                    }
                }
                else
                {
                    throw new FileNotFoundException("Settings.xml file not found at path: " + settingsPath);
                }

                user = new User("User", bot);
                Log("User object initialized.");

                bot.isAcceptingUserInput = false;
                bot.loadAIMLFromFiles();
                bot.isAcceptingUserInput = true;
                Log("AIML files loaded.");

                responses = LoadResponses();
                Log("Custom training data loaded.");

                if (bot == null || user == null)
                {
                    throw new NullReferenceException("Bot or User initialization failed.");
                }
            }
            catch (Exception ex)
            {
                Log("Error initializing Chatbot: " + ex.Message);
                Log(ex.StackTrace);
            }
        }

        public string GetResponse(string userInput)
        {
            if (bot == null || user == null)
            {
                return "Chatbot is not properly initialized.";
            }

            // Use AIML first
            Request request = new Request(userInput, user, bot);
            Result result = bot.Chat(request);
            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                return result.Output;
            }

            // If no AIML response, use custom training data
            string bestMatch = "";
            double bestMatchScore = 0.0;

            foreach (KeyValuePair<string, string> entry in responses)
            {
                double score = CalculateMatchScore(userInput, entry.Key);
                if (score > bestMatchScore)
                {
                    bestMatchScore = score;
                    bestMatch = entry.Value;
                }
            }

            return bestMatchScore == 0.0 ? "Sorry, I don't understand." : bestMatch;
        }

        private Dictionary<string, string> LoadResponses()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/trainingdata.json");
            Dictionary<string, string> responseMap = new Dictionary<string, string>();

            try
            {
                string jsonData = File.ReadAllText(filePath);
                var trainingData = JsonConvert.DeserializeObject<List<TrainingData>>(jsonData);
                foreach (var data in trainingData)
                {
                    responseMap[data.Query.ToLower()] = data.Response;
                }
                Log("Responses loaded from trainingdata.json.");
            }
            catch (Exception ex)
            {
                Log("Error loading training data: " + ex.Message);
            }

            return responseMap;
        }

        private double CalculateMatchScore(string userText, string keyword)
        {
            string[] userWords = userText.Split(' ');
            int matchingWords = 0;

            foreach (string word in userWords)
            {
                if (keyword.Contains(word))
                {
                    matchingWords++;
                }
            }

            return matchingWords;
        }

        private void Log(string message)
        {
            string logFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Logs/ChatbotLog.txt");
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
             //   writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
