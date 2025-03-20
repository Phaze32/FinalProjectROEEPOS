Imports Microsoft.VisualBasic
Imports Microsoft.ML
Imports Microsoft.ML.Data

Namespace AiClass
    Public Class usingML

        ' Method for performing linear regression.
        Public Function LinearRegression(ByVal xValues As List(Of Double), ByVal yValues As List(Of Double)) As Double()
            Dim n As Integer = xValues.Count
            Dim sumX As Double = 0, sumY As Double = 0
            Dim sumXx As Double = 0, sumXy As Double = 0

            For i As Integer = 0 To n - 1
                sumX += xValues(i)
                sumY += yValues(i)
                sumXx += xValues(i) * xValues(i)
                sumXy += xValues(i) * yValues(i)
            Next

            Dim slope As Double = (n * sumXy - sumX * sumY) / (n * sumXx - sumX * sumX)
            Dim intercept As Double = (sumY - slope * sumX) / n

            Return New Double() {slope, intercept}
        End Function

        ' Add more methods for different machine learning algorithms here.


        Shared Function usingML(hourOfDay As Double, xValues As List(Of Double), yValues As List(Of Double)) As String
            '   Using above code, SimpleMachineLearning

            ' Instantiate the machine learning class
            Dim ml As New usingML()

            ' Sample data: xValues represent the hour of the day (0-23), and yValues represent the average conversation duration in minutes
            xValues = New List(Of Double) From {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23}
            yValues = New List(Of Double) From {5, 5, 5, 5, 5, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95}

            ' Perform linear regression to get the slope and intercept
            Dim regressionResult As Double() = ml.LinearRegression(xValues, yValues)

            ' Predict the conversation duration for a given hour of the day
            'Dim hourOfDay As Double = 14 ' 2 PM
            Dim predictedDuration As Double = regressionResult(0) * hourOfDay + regressionResult(1)
            Dim rv As String = "The predicted conversation duration at " & hourOfDay & " is: " & predictedDuration & " minutes."
            'Console.WriteLine()
            Return rv
        End Function
    End Class
End Namespace
