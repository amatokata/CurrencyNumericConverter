using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args)
        {
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening for client to send the dollar number...");
            listener.Start();

            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            while (true)
            {
                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                byte[] outBuffer = new byte[1000];

                //---read incoming stream---
                int bytesRead = 0;
                while (bytesRead == 0)
                    bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the dollar number received into a string---
                string number = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received dollar number from client: " + number);
                string convertedWords;

                if (number == "0")
                {
                    convertedWords = "zero dollars";
                }
                else if (number == "1")
                {
                    convertedWords = "one dollar";
                }
                else if (number.Contains("-"))
                {
                    convertedWords = "Dollar amount cannot be negative!";
                }
                else
                {
                    convertedWords = ConvertToWords(number);
                }

                //---send back the converted words to the client---
                Console.WriteLine("Sending back converted words: " + convertedWords);

                for (int i = 0; i < convertedWords.Length; i++)
                {
                    outBuffer[i] = (byte)(convertedWords[i]);
                }
                nwStream.Write(outBuffer, 0, convertedWords.Length);
            }
            client.Close();
            listener.Stop();
            Console.ReadLine();
        }

       
    }
}


