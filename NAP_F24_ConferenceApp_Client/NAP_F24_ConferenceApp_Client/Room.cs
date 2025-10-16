using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NAP_F24_ConferenceApp_Client
{
    public class Room
    {
        public string Name { get; set; }
        public int MemberCount { get; set; } = 0;
        public int MaxMembers { get; set; } = 10;
        public List<TcpClientWrapper> Clients { get; set; } = new List<TcpClientWrapper>();
    }

    public class TcpClientWrapper
    {
        public TcpClient TcpClient { get; set; }
        public IPEndPoint UdpEndPoint { get; set; } // لتخزين عنوان UDP الخاص بالعميل
        public string UserName { get; set; }
    }
}
