using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OtusCSharpBasicFinalProject.Data;

namespace OtusCSharpBasicFinalProject.OpenWebNet
{
    public static class OpenWebNet
    {
        private static TcpClient Client { get; set; }

        private static NetworkStream Stream { get; set; }
        
        public static async Task StartWorkingAsync(string server, int port)
        {
            await OpenConnectionAsync(server,port);
            //StayConnected(server,port);
        }
        private static async Task OpenConnectionAsync(string server, int port)
        {
            try
            {
                Client = new TcpClient();
                await Client.ConnectAsync(server, port);
                await StayConnectedAsync();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
 
            Console.WriteLine("Запрос завершен...");
            Console.Read();
        }

        private static async Task StayConnectedAsync()
        {
            StringBuilder response = new StringBuilder();
            Stream = Client.GetStream();
            //var data1 = Encoding.ASCII.GetBytes("*#1*37##");
            var data2 = "*#1*37##";
            //stream.Write(data1);
            while (true)
            {
                // do
                // {
                //     int bytes = stream.Read(data, 0, data.Length);
                //     response.Clear();
                //     response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                // } while (stream.DataAvailable); // пока данные есть в потоке
                //
                // Console.WriteLine(response.ToString());
                // byte[] date2 = Encoding.ASCII.GetBytes(Console.ReadLine() ?? string.Empty);
                // try
                // {
                //     stream.Write(date2);
                // }
                // catch (Exception e)
                // {
                //     Console.WriteLine(e);
                //     OpenConnection(server,port);
                // }
                do
                {
                    await SendRequest(data2);
                    var responseText = await GetResponse(response);
                    if (responseText.Contains("*1*1*37") || responseText.Contains("*1*0*37"))
                        Console.WriteLine(responseText);
                    //Thread.Sleep(2000);
                    await Task.Delay(2000);

                } while (true);
            }
            // Закрываем потоки
            // stream.Close();
            // Client.Close();
        }
        
        private static async Task SendRequest(string sendCommand)
        {
            byte[] dateToSend = Encoding.ASCII.GetBytes(sendCommand);
            try
            {
                await Stream.WriteAsync(dateToSend);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task<string> GetResponse(StringBuilder response)
        {
            do
            {
                var data = new byte[256];
                var bytes = await Stream.ReadAsync(data, 0, data.Length);
                response.Clear();
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (Stream.DataAvailable); // пока данные есть в потоке

            // Console.WriteLine(response.ToString());
            return  response.ToString();
        }

        public static async Task<string> GetGroupLightState(LightGroup lightGroup)
        {
            do
            {
                StringBuilder response = new StringBuilder();
                await SendRequest(lightGroup.GetCommand);
                var responseText = await GetResponse(response);
                if (responseText.Contains(lightGroup.SendCommandOn) || responseText.Contains(lightGroup.SendCommandOff))
                {
                    if (responseText.Contains(lightGroup.SendCommandOn))
                        return "💡";
                    if (responseText.Contains(lightGroup.SendCommandOff))
                        return "🕯";
                }
                Thread.Sleep(100);
            } while (true);
        }
        public static async Task SetGroupLightStateOn(LightGroup lightGroup)
        {
            do
            {
                StringBuilder response = new StringBuilder();
                await SendRequest(lightGroup.SendCommandOn);
                var responseText = await GetResponse(response);
                if (responseText.Contains("*#*1##"))
                    return;
                Thread.Sleep(100);
            } while (true);
        }
        public static async Task SetGroupLightStateOff(LightGroup lightGroup)
        {
            do
            {
                StringBuilder response = new StringBuilder();
                await SendRequest(lightGroup.SendCommandOff);
                var responseText = await GetResponse(response);
                if (responseText.Contains("*#*1##"))
                    return;
                Thread.Sleep(100);
            } while (true);
        }
    }
}