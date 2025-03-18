Imports Microsoft.VisualBasic
Imports System.Net
Imports AIMLbot
Namespace Chatbotclass

    Public Class CuteRobot
        Const UserId As String = "CityU.Scm.David"

        Private AimlBot As Bot
        Private myUser As User

        Public Sub New()
            AimlBot = New Bot()
            myUser = New User(UserId, AimlBot)
            Initialize()
        End Sub

        Public Sub Initialize()
            AimlBot.loadSettings()
            AimlBot.isAcceptingUserInput = False
            AimlBot.loadAIMLFromFiles()
            AimlBot.isAcceptingUserInput = True
        End Sub

        Public Function getOutput(ByVal input As String) As String
            Dim r As Request = New Request(input, myUser, AimlBot)
            Dim res As Result = AimlBot.Chat(r)
            Return (res.Output)
        End Function

    End Class
    Class Program

        Shared bot As CuteRobot

        Private Shared Sub Main(ByVal args As String())
            bot = New CuteRobot()
            Dim input As String = "Hello, what is your name"
            Dim output = bot.getOutput(input)
            Console.WriteLine(input)
            Console.WriteLine(output)
            Console.ReadKey()
        End Sub
    End Class
End Namespace


