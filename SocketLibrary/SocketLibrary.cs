using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace SocketLibrary
{
    public class SocketLibrary
    {
        Socket s = null;
        static ManualResetEvent done = new ManualResetEvent(false);

        public bool EstablishTCPConnection(string host, int port)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = new DnsEndPoint(host, port);
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object o, SocketAsyncEventArgs e)
                {
                    done.Set();
                });
            done.Reset();
            s.ConnectAsync(socketEventArg);
            return done.WaitOne(10000);
        }

        public bool Send(string data)
        {
            if (s != null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = s.RemoteEndPoint;
                socketEventArg.UserToken = null;

                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object o, SocketAsyncEventArgs e)
                    {
                        done.Set();
                    });

                socketEventArg.SetBuffer(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                done.Reset();
                s.SendAsync(socketEventArg);
                return done.WaitOne(10000);
            }
            return false;
        }

        public string Receive()
        {
            string received = "";
            if (s != null)
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = s.RemoteEndPoint;

                socketEventArg.SetBuffer(new byte[1024], 0, 1024);
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object o, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        received = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred).Trim('\0');
                    }
                    done.Set();
                });

                
                done.Reset();
                s.ReceiveAsync(socketEventArg);
                done.WaitOne(10000);
            }
            return received;
        }
        public void CloseSocket()
        {
            if (s != null)
            {
                s.Close();
            }
        }
    }
}