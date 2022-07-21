using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Csharp.Liqing.Utils;


namespace Csharp.Liqing.Net
{

    public class TcpServerBase
    {
        private Socket m_socket = null;
        private int m_backlog = 100;
        private int m_bufferSize = 1024;
        private bool m_isRun = false;

        private Pool<SocketAsyncEventArgs> acceptPool = new Pool<SocketAsyncEventArgs>();
        private Pool<SocketAsyncEventArgs> readWritePool = new Pool<SocketAsyncEventArgs>();

        #region 委托处理,所有委托都为同步处理
        public event EventHandler<SocketAsyncEventArgs> OnClientConnected = (a, b) => {
            Console.WriteLine("{0} connected",b.AcceptSocket.GetHashCode());
        };
        public event EventHandler<SocketAsyncEventArgs> OnReceiveSuccess = (a, b) => {
            string str = System.Text.Encoding.UTF8.GetString(b.Buffer,0,b.BytesTransferred);
            Console.WriteLine("{0} recieve: {1}", b.UserToken.GetHashCode(), str);
        };
        public event EventHandler<SocketAsyncEventArgs> OnReceiveError = (a, b) => { };
        public event EventHandler<SocketAsyncEventArgs> OnClientDisConnected = (a, b) => {
            Console.WriteLine("{0} DisConnected", b.UserToken.GetHashCode());
        }; 
        #endregion

        #region Geter
        public int GetBacklog() { return m_backlog; }
        public int GetBufferSize() { return m_bufferSize; } 
        #endregion

        #region Seter
        public void SetBacklog(int backlog) { this.m_backlog = backlog; }
        public void SetBufferSize(int size) { m_bufferSize = size; }  
        #endregion

        public void Start(int port,IPAddress ip = null) {
            if (m_isRun) { return; }
            m_isRun = true;
            m_socket = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
            if (null == ip) { ip = IPAddress.Any; }
            m_socket.Bind(new IPEndPoint(ip, port));
            m_socket.Listen(GetBacklog());

            for (int i = 0; i < GetBacklog(); i++)
            {
                BeginAcceptSocket(null);
            }
        }

        public void Close() {
            if (!m_isRun) { return; }
            m_isRun = false;

            if (null == m_socket) { return; }

            ReleaseAcceptPool();
            ReleaseReadWritePool();

            m_socket.Close();
        }

        private SocketAsyncEventArgs GetAcceptEventArgs() {
            return acceptPool.Pop();
        }

        private SocketAsyncEventArgs GetReadWriteEventArgs() {
            return readWritePool.Pop();
        }

        private void PushBackAcceptEventArgs(SocketAsyncEventArgs args) {
            acceptPool.Push(args);
        }

        private void PushBackReadWriteArgs(SocketAsyncEventArgs args) {
            readWritePool.Push(args);
        }

        private void ReleaseAcceptPool() {
            var list = acceptPool.GetList();
            foreach (var item in list)
            {
                item.Completed -= AcceptSocket;
                item.Dispose();
            }
            acceptPool.Clear();
        }

        private void ReleaseReadWritePool() {
            var list = readWritePool.GetList();
            foreach (var item in list)
            {
                item.Completed -= CompletedIO;
                CloseClientSocket(item);
                item.Dispose();
            }
            readWritePool.Clear();
        }

        private void ShutdownSocket(Socket socket, SocketAsyncEventArgs args) {
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    socket.Shutdown(SocketShutdown.Send);
                    break;
                case SocketAsyncOperation.Receive:
                    socket.Shutdown(SocketShutdown.Receive);
                    break;
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs args)
        {
            PushBackReadWriteArgs(args);
            Socket socket = args.UserToken as Socket;
            if (socket == null) { return; }
            args.UserToken = null;
            if (socket.Connected) { ShutdownSocket(socket, args); }
            socket.Close();
        }

        private void CompletedIO(object sender, SocketAsyncEventArgs args)
        {
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ReceiveSocket(sender, args);
                    break;
                case SocketAsyncOperation.Send:
                    break;
                default:
                    throw new NotSupportedException("Only Supported Send/Receive Operation!");
            }
        }

        private void InitReadWriteArgs(SocketAsyncEventArgs args)
        {
            if (args.Buffer != null) { return; }
            args.SetBuffer(new byte[GetBufferSize()], 0, GetBufferSize());
            args.Completed += CompletedIO;
        }

        private void BeginAcceptSocket(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = GetAcceptEventArgs();
                args.Completed += AcceptSocket;
            }

            if (false == m_socket.AcceptAsync(args))
            {
                AcceptSocket(this,args);
            }
        }

        private void AcceptSocket(Object sender, SocketAsyncEventArgs args) {
            OnClientConnected.Invoke(sender, args);

            Socket socket = args.AcceptSocket;
            SocketAsyncEventArgs readArgs = GetReadWriteEventArgs();
            InitReadWriteArgs(readArgs);
            readArgs.UserToken = socket;
            BeginReceiveSocket(readArgs);

            args.AcceptSocket = null;
            BeginAcceptSocket(args);
        }

        private void BeginReceiveSocket(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;
            if (false == socket.ReceiveAsync(args))
            {
                ReceiveSocket(this,args);
            }
        }

        private void ReceiveSocket(Object sender,SocketAsyncEventArgs args) {
            if (args.SocketError != SocketError.Success) {
                OnReceiveError.Invoke(sender, args);
                CloseClientSocket(args);
                return;
            }

            if (args.BytesTransferred <= 0) {
                OnClientDisConnected.Invoke(sender, args);
                CloseClientSocket(args);
                return;
            }

            OnReceiveSuccess.Invoke(sender, args);
            BeginReceiveSocket(args);
        }
    }
}
