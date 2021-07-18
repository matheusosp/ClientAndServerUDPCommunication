using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Entities;
using Newtonsoft.Json;

namespace Server
{
    internal class Program
    {
        private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static Dictionary<string, string> dicionario = new Dictionary<string, string>();
        
        public static void Main(string[] args)
        {
            socket.Bind( new IPEndPoint(IPAddress.Any, 50000));
            Console.WriteLine("Servidor iniciado!");
            
            
            dicionario.Add("luz_guarita", "off");
            dicionario.Add("ar_guarita", "off");
            dicionario.Add("luz_estacionamento", "off");
            dicionario.Add("luz_galpao_externo", "off");
            dicionario.Add("luz_galpao_interno", "off");
            dicionario.Add("luz_escritorios", "off");
            dicionario.Add("ar_escritorios", "off");
            dicionario.Add("luz_sala_reunioes", "off");
            dicionario.Add("ar_sala_reunioes", "off");
            
            while (true)
            {
                Receive();
            }
        }
        private static void Receive()
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 50000);
            byte[] buffer = new byte[1000];
            var length = socket.ReceiveFrom(buffer, ref remoteEndPoint);
            String payload = Encoding.Unicode.GetString(buffer, 0, length);
            
            var deserializedDataReceived = JsonConvert.DeserializeObject<Model>(payload);
            
            if (deserializedDataReceived != null && deserializedDataReceived.command == "set")
            {
                dicionario[deserializedDataReceived.locate] = deserializedDataReceived.value;
            }
            SendResponse(deserializedDataReceived, remoteEndPoint);
        }
        private static void SendResponse(Model deserialized, EndPoint endPoint)
        {
            var response = new Response();
            response.locate = deserialized.locate;
            response.value = dicionario[deserialized.locate];
            
            string payloadResponse = JsonConvert.SerializeObject(response);
            Byte[] bufferResponse = Encoding.Unicode.GetBytes(payloadResponse.ToCharArray());
            socket.SendTo(bufferResponse, endPoint);
        }
    }
}