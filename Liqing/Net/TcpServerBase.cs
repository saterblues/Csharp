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
        protected Socket _acceptSocket = null;
        protected int _backlog = 1000;
        protected int _bufferSize = 1024;
        protected bool _isRun = false;

        protected HashSet<TcpSessionBase> _sessions;
        protected Dictionary<Guid, TcpSessionBase> _sessionIds;
        protected Pool<SocketAsyncEventArgs> _acceptPool;
        protected Pool<SocketAsyncEventArgs> _readWritePool;

        public TcpServerBase() {
            _acceptSocket = null;
            _backlog = 1000;
            _bufferSize = 1024;
            _isRun = false;

            _sessions = new HashSet<TcpSessionBase>();
            _sessionIds = new Dictionary<Guid, TcpSessionBase>();
            _acceptPool = new Pool<SocketAsyncEventArgs>();
            _readWritePool = new Pool<SocketAsyncEventArgs>();
        }
      
        public bool OptionKeepAlive { get; set; }
        public bool OptionNoDelay { get; set; }

        public Action<Guid> OnConnected = (sessionGuid) => {
            Console.WriteLine("{0} connected", sessionGuid);
        };

        public Action<Guid> OnClose = (sessionGuid) =>
        {
            Console.WriteLine("{0} close", sessionGuid);
        };

        public Action<Guid, int> OnSend = (sessionGuid, length) =>
        {
            Console.WriteLine("{0} send:{1} bytes", sessionGuid, length);
        };

        public Action<Guid, byte[], int, int> OnReceive = (sessionGuid, buffer, offset, length) =>
        {
            string str = System.Text.Encoding.UTF8.GetString(buffer, offset, length);
            Console.WriteLine("{0} receive:{1}", sessionGuid, str);
        };
      
        public int GetBacklog() { return _backlog; }
        public int GetBufferSize() { return _bufferSize; } 

        public void SetBacklog(int backlog) { this._backlog = backlog; }
        public void SetBufferSize(int size) { _bufferSize = size; }  

        public void Start(int port,IPAddress ip = null) {
            if (_isRun) { return; }
            _isRun = true;
            _acceptSocket = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
            if (null == ip) { ip = IPAddress.Any; }
            _acceptSocket.Bind(new IPEndPoint(ip, port));
            _acceptSocket.Listen(GetBacklog());

            for (int i = 0; i < GetBacklog(); i++)
            {
                BeginAcceptSocket(null);
            }
        }

        public void Close() {
            if (!_isRun) { return; }
            _isRun = false;

            if (null == _acceptSocket) { return; }
            ReleaseAcceptPool();
            ReleaseReadWritePool();
            _acceptSocket.Close();
            _acceptSocket.Dispose();
            _acceptSocket = null;
            _sessionIds.Clear();
            _sessions.Clear();
        }

        protected SocketAsyncEventArgs GetAcceptEventArgs()
        {
            return _acceptPool.Pop();
        }

        protected SocketAsyncEventArgs GetReadWriteEventArgs()
        {
            return _readWritePool.Pop();
        }

        protected void PushBackAcceptEventArgs(SocketAsyncEventArgs args)
        {
            _acceptPool.Push(args);
        }

        protected void PushBackReadWriteArgs(SocketAsyncEventArgs args)
        {
            _readWritePool.Push(args);
        }

        protected void ReleaseAcceptPool()
        {
            var list = _acceptPool.GetList();
            foreach (var item in list)
            {
                item.Completed -= AcceptSocket;
                item.Dispose();
            }
            _acceptPool.Clear();
        }

        protected void ReleaseReadWritePool()
        {
            var list = _readWritePool.GetList();
            foreach (var item in list)
            {
                item.Dispose();
            }
            _readWritePool.Clear();
        }

        protected void InitReadWriteArgs(SocketAsyncEventArgs args)
        {
            if (args.Buffer != null) { return; }
            args.SetBuffer(new byte[GetBufferSize()], 0, GetBufferSize());
        }

        protected void BeginAcceptSocket(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = GetAcceptEventArgs();
                args.Completed += AcceptSocket;
            }

            if (false == _acceptSocket.AcceptAsync(args))
            {
                AcceptSocket(this,args);
            }
        }

        protected void AcceptSocket(Object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success) {
                args.AcceptSocket = null;
                BeginAcceptSocket(args);
                return;
            }

            Socket socket = args.AcceptSocket;

            if (OptionKeepAlive) {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, OptionKeepAlive);
            }

            if (OptionNoDelay) {
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, OptionNoDelay);
            }

            SocketAsyncEventArgs receive = GetReadWriteEventArgs();
            InitReadWriteArgs(receive);
            SocketAsyncEventArgs send = GetReadWriteEventArgs();
            InitReadWriteArgs(send);

            lock (_sessions) {
                TcpSessionBase session = new TcpSessionBase(socket, receive, send);
                session.OnReceive = this.OnSessionReceive;
                session.OnSend = this.OnSessionSend;
                session.OnClose = this.OnSessionClose;

                this.OnSessionConnected(session);

                session.Run();
                _sessions.Add(session);
                _sessionIds.Add(session.GetGuid(), session);
            }

            args.AcceptSocket = null;
            BeginAcceptSocket(args);
        }

        protected void OnSessionConnected(TcpSessionBase session)
        {
            OnConnected(session.GetGuid());
        }

        protected void OnSessionClose(TcpSessionBase session, SocketAsyncEventArgs recieveArgs, SocketAsyncEventArgs sendArgs)
        {
            this.OnClose(session.GetGuid());
            PushBackReadWriteArgs(recieveArgs);
            PushBackReadWriteArgs(sendArgs);
            _sessions.Remove(session);
            _sessionIds.Remove(session.GetGuid());
        }

        protected void OnSessionReceive(TcpSessionBase session, byte[] buffer, int offset, int length)
        {
            OnReceive(session.GetGuid(), buffer, offset, length);
        }

        protected void OnSessionSend(TcpSessionBase session, int length)
        {
            OnSend(session.GetGuid(), length);
        }

        public void SendAsync(Guid sessionId, byte[] buffer) {
            this.SendAsync(sessionId, buffer, 0, buffer.Length);  
        }

        public void SendAsync(Guid sessionId, byte[] buffer, int offset, int length)
        {
            TcpSessionBase session;
            if (false == _sessionIds.TryGetValue(sessionId, out session)) {
                return;
            }
            session.SendAsync(buffer, offset, length);
        }

        public void CloseSession(Guid sessionId) {
            TcpSessionBase session;
            if (_sessionIds.TryGetValue(sessionId, out session)) {
                session.ClearSession();
            }
        }
    }
}
