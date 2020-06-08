using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tioLogReplay
{
    public class TioConnection : Options
    {
        const string TIO_DEFAULT_SERVER = "localhost";
        const int TIO_DEFAULT_PORT = 2605;

        private TcpClient client { get; set; }
        private NetworkStream stream { get; set; }

        public TioConnection(string server = TIO_DEFAULT_SERVER, int port = TIO_DEFAULT_PORT)
        {
            this.client = new TcpClient(server, port);
            this.stream = client.GetStream();
        }

        public void SendCommand(string line)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes(line);
                stream.Write(data, 0, data.Length);
                Console.WriteLine(line);

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }
 
        public void Write(string line) {
            // Sends a command to the server 
            var data = Encoding.ASCII.GetBytes(line);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent {0}:", line);
            client.Close();
            stream.Close();
        }
    }
}

