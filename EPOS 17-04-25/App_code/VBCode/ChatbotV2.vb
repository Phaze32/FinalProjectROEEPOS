Imports Microsoft.VisualBasic
Imports System.Web.Script.Serialization
Imports System.IO
Imports System.Web.Services
Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic

Partial Class ChatBotV2
    Inherits System.Web.UI.Page

    Private Shared trainingData As List(Of ChatData)
    Private Shared model As NaiveBayes
    Private Shared predictions_logfilepath As String = "~/App_Data/predictions_log.txt"
    Private Shared predictiondata_filepath As String = "predictiondata.json"
    Private Shared trainingdatafilepath As String = "~/App_Data/trainingdata.json"
    Private Shared predictions_filepath As String = "~/App_Data/predictions.json"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadTrainingData()
            TrainModel()
        End If
    End Sub

    Protected Sub SendMessage(ByVal sender As Object, ByVal e As EventArgs)
        Dim userMessage As String = sender.userInput.Text.Trim()
        If userMessage <> String.Empty Then
            Dim botResponse As String = GetResponse(userMessage)
            Response.Write("userMessage=" & userMessage & "botResponse=" & botResponse)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AddMessage", $"addMessage('You', '{userMessage}'); addMessage('Bot', '{botResponse}');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AddMessage", "addMessage('You', '" & userMessage & "'); addMessage('Bot', '" & botResponse & "');", True)

            sender.userInput.Text = String.Empty
        End If
    End Sub

    Private Sub LoadTrainingData()
        Dim json As String = File.ReadAllText(Server.MapPath(predictiondata_filepath))
        Dim serializer As New JavaScriptSerializer()
        trainingData = serializer.Deserialize(Of List(Of ChatData))(json)
        'Response.Write(" trainingData=" & trainingData.ToString)
    End Sub

    Private Sub TrainModel()
        model = New NaiveBayes()
        Dim messages As New List(Of String)()
        Dim labels As New List(Of String)()

        For Each item In trainingData
            messages.Add(item.message)
            labels.Add(item.label)
        Next

        model.Train(messages.ToArray(), labels.ToArray())
    End Sub

    Private Function GetResponse(ByVal userInput As String) As String
        Try
            Dim label As String = model.Predict(userInput)
            Dim prediction As String = GetPrediction(userInput)
            Dim UserIntention As String = PredictUserIntention(userInput)
            If label = prediction Then
                Response.Write("prediction good=" & prediction & "=" & label & " & UserIntention =" & UserIntention & "<br>")
            Else
                Response.Write("prediction bad=" & prediction & "=" & label & " & UserIntention =" & UserIntention & "<br>")
            End If
            For Each item In trainingData
                If item.label = label Then
                    Return item.response
                End If
            Next
        Catch ex As Exception
            Response.Write("ln70 GetResponse err=" & vbCrLf & ex.Message)
        End Try
        Return "I didnt understand that."
    End Function
    Function GetPrediction(userInput As String) As String
        ' Load prediction data from JSON file
        Dim predictionData As Dictionary(Of String, List(Of String)) = LoadPredictionData()

        ' Simple matching based on keywords in user input (can be improved)
        Dim bestMatchCategory As String = Nothing
        Dim highestMatchCount As Integer = 0
        For Each category In predictionData.Keys
            Dim matchingKeywords As Integer = 0
            For Each keyword In predictionData(category)
                If userInput.ToLower().Contains(keyword.ToLower()) Then
                    matchingKeywords += 1
                End If
            Next
            If matchingKeywords > highestMatchCount Then
                bestMatchCategory = category
                highestMatchCount = matchingKeywords
            End If
        Next

        ' Return the predicted category or "No Match" if no good match found
        If bestMatchCategory Is Nothing Then
            Return "No Match"
        Else
            Return bestMatchCategory
        End If
    End Function
    Public Function PredictUserIntention(ByVal userStatement As String) As String
        ' Load JSON data from file (replace with your actual file path)
        Dim filePath As String = Server.MapPath(predictions_filepath)
        Dim jsonData As String = My.Computer.FileSystem.ReadAllText(filePath)

        ' Deserialize JSON data
        Dim predictionCategories As List(Of Dictionary(Of String, Object)) = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(jsonData)
        Dim categoryName As String = "Unknown"
        ' Loop through categories and subcategories (if applicable)
        For Each category In predictionCategories
            categoryName = category("category").ToString()

            ' Check for subcategories (modify if using subcategories)
            If category.ContainsKey("subcategories") Then
                For Each subcategory In category("subcategories").Cast(Of List(Of Dictionary(Of String, Object))).ToList()
                    Dim subcategoryExamples As List(Of String) = subcategory("examples").Cast(Of List(Of String)).ToList()

                    For Each example In subcategoryExamples
                        If userStatement.IndexOf(example, StringComparison.OrdinalIgnoreCase) > -1 Then
                            Return categoryName & " - " & subcategory("name").ToString() ' Include subcategory name
                        End If
                    Next
                Next
            Else
                ' Check for top-level category examples
                Dim examples As List(Of String) = category("examples").Cast(Of List(Of String)).ToList()

                For Each example In examples
                    If userStatement.IndexOf(example, StringComparison.OrdinalIgnoreCase) > -1 Then
                        Return categoryName
                    End If
                Next
            End If
        Next
        ' Return "Unknown" if no match found
        If categoryName Is Nothing Then
            Return "Unknown"
        Else
            Return categoryName
        End If


    End Function

    Private Function LoadPredictionData() As Dictionary(Of String, List(Of String))
        Dim predictionData As New Dictionary(Of String, List(Of String))
        Try
            ' Read JSON file content
            Dim jsonData As String = System.IO.File.ReadAllText(predictiondata_filepath)

            ' Deserialize JSON data using Newtonsoft.Json
            predictionData = JsonConvert.DeserializeObject(Of Dictionary(Of String, List(Of String)))(jsonData)
        Catch ex As Exception
            Console.WriteLine("Error loading prediction data: " & ex.Message)
        End Try
        Return predictionData
    End Function
    Private Sub SavePrediction(ByVal userMessage As String, ByVal prediction As String)
        'Dim logEntry As String = $"{DateTime.Now}: User Message: '{userMessage}', Prediction: '{prediction}'{Environment.NewLine}"

        Dim logEntry As String = String.Format("{0}: User Message: '{1}', Prediction: '{2}{3}", DateTime.Now, userMessage, prediction, Environment.NewLine)

        File.AppendAllText(Server.MapPath(predictions_logfilepath), logEntry)
    End Sub
    Private Class ChatData
        Public Property message As String
        Public Property label As String
        Public Property response As String
    End Class

    Private Class NaiveBayes
        ' This is a placeholder for a simple Naive Bayes implementation
        Private messageToLabel As New Dictionary(Of String, String)()

        Public Sub Train(ByVal messages As String(), ByVal labels As String())
            For i As Integer = 0 To messages.Length - 1
                messageToLabel(messages(i)) = labels(i)
            Next
        End Sub

        Public Function Predict(ByVal message As String) As String
            For Each kvp In messageToLabel
                If message.ToLower().Contains(kvp.Key.ToLower()) Then
                    Return kvp.Value
                End If
            Next
            Return "unknown"
        End Function
    End Class
End Class


