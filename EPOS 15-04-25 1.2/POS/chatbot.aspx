<%@ Page Language="C#" %>
<!DOCTYPE html>
<%@Import Namespace="System"%>
<%@Import Namespace="System.Collections.Generic"%>
<%@Import Namespace="System.Linq"%>
<%@Import Namespace="System.Text"%>
<%@Import Namespace="System.Threading.Tasks"%>
   <script runat="server">
//namespace BasicAIChatBot {
    protected void page_load (object sender, EventArgs e) {
      Response.Write("helllooooooo") ;
       }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Basic AI Chat Bot. How may I assist you today?");

            while (true)
            {
                string userMessage = Console.ReadLine().ToLower();

                if (userMessage.Contains("hello") || userMessage.Contains("hi") || userMessage.Contains("hey"))
                {
                    Console.WriteLine("Hello there! How can I help you today?");
                }
                else if (userMessage.Contains("how are you"))
                {
                    Console.WriteLine("I'm a computer program, so I don't have emotions. But thanks for asking!");
                }
                else if (userMessage.Contains("what's your name") || userMessage.Contains("who are you"))
                {
                    Console.WriteLine("My name is Basic AI Chat Bot. I'm here to help you!");
                }
                else if (userMessage.Contains("thank you") || userMessage.Contains("thanks"))
                {
                    Console.WriteLine("You're welcome! Have a nice day.");
                }
                else if (userMessage.Contains("bye"))
                {
                    Console.WriteLine("Goodbye! Come back soon.");
                    break;
                }
                else
                {
                    Console.WriteLine("I'm sorry, I didn't understand what you said. Can you please try again?");
                }
            }
        }
    }
//};
</script>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title></title>    
</head>
<body>
    <form id="form1" runat="server"> 
        
        hello 
    </form>
</body>
</html>
