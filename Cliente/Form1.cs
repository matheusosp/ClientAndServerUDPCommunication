using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entities;
using Newtonsoft.Json;

namespace Cliente
{
    public partial class Form1 : Form
    {
        static EndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 50000);
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public Form1()
        {
            InitializeComponent();
        }
        private void btnLigar_Click(object sender, EventArgs e)
        {
            var model = new Model();
            model.command = "set";
            model.locate = Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked)?.Text ?? string.Empty;
            model.value = "on";
            SendAndGetResponse(model);
        }
        private void btnDesligar_Click(object sender, EventArgs e)
        {
            var model = new Model();
            model.command = "set";
            model.locate = Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked)?.Text ?? string.Empty;
            model.value = "off";
            SendAndGetResponse(model);
        }
        private void btnGetState_Click(object sender, EventArgs e)
        {
            var model = new Model();
            model.command = "get";
            model.locate = Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked)?.Text ?? string.Empty;
            SendAndGetResponse(model);
        }
        private void SendAndGetResponse(Model model)
        {
            string payload = JsonConvert.SerializeObject(model);
            Byte[] buffer = Encoding.Unicode.GetBytes(payload.ToCharArray());
            socket.SendTo(buffer, ipEndPoint);

            byte[] serverStringResponse = new byte[1000];
            socket.ReceiveFrom(serverStringResponse, ref ipEndPoint);
            var strData = Encoding.Unicode.GetString(serverStringResponse);
            MessageBox.Show(strData);
        }
        
    }
}