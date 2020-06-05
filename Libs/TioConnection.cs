using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace tioLogReplay
{
    public class TioConnection : Options
    {
        const string TIO_DEFAULT_SERVER = "localhost";
        const int TIO_DEFAULT_PORT = 6025;

        private TcpClient client { get; set; }
        private NetworkStream network { get; set; }

        public TioConnection(string server = TIO_DEFAULT_SERVER, int port = TIO_DEFAULT_PORT)
        {
            this.client = new TcpClient(server, port);
            this.network = client.GetStream();
        }

        public void Create(string name, string type)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes($"create {name} {type}");
                network.Write(data, 0, data.Length);
                
                // Gets response from server
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }

        public void Open(string name, string type)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes($"create {name} {type}");
                network.Write(data, 0, data.Length);
                
                // Gets response from server
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }

        public void PushFront(string line)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes(line);
                network.Write(data, 0, data.Length);
                
                // Gets response from server
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }

        public void PushBack(string line)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes(line);
                network.Write(data, 0, data.Length);
                
                // Gets response from server
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }

        public void Set(string line)
        {
            try
            {
                // Sends a command to the server 
                var data = Encoding.ASCII.GetBytes(line);
                network.Write(data, 0, data.Length);
                
                // Gets response from server
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }
        }

        public void Insert(string line)
        {
            try
            {
                // Sends a command
                var data = Encoding.ASCII.GetBytes(line);
                network.Write(data, 0, data.Length);
                
                // Gets response 
                var bytes = network.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);

                Console.WriteLine("Answer: {0}", responseData);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketExcepetion: {0}", e);
            }

        }

        public void PauseServer()
        {
            //TODO
        }

        public void ResumeServer()
        {
            //TODO
        }
    }
}
