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
        private Socket _acceptSocket = null;
        private int _backlog = 1000;
        private int _bufferSize = 1024;
        private bool _isRun = false;

        private HashSet<TcpSessionBase> _sessions;
        private Dictionary<Guid, TcpSessionBase> _sessionIds;
        private Pool<SocketAsyncEventArgs> _acceptPool;
        private Pool<SocketAsyncEventArgs> _readWritePool;

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
        
        public Action<Guid> OnConnected = (a) => {
            Console.WriteLine("{0} connected",a);
        };

        public Action<Guid> OnClose = (a) => {
            Console.WriteLine("{0} close", a);
        };

        public Action<Guid, int> OnSend = (a,b) => {
            Console.WriteLine("{0} send:{1} bytes",a,b);
        };

        public Action<Guid, byte[], int, int> OnReceive = (a, b, c, d) => {
            string str = System.Text.Encoding.UTF8.GetString(b, c, d);
            Console.WriteLine("{0} receive:{1}", a, str);
        };

        #region Geter
        public int GetBacklog() { return _backlog; }
        public int GetBufferSize() { return _bufferSize; } 
        #endregion

        #region Seter
        public void SetBacklog(int backlog) { this._backlog = backlog; }
        public void SetBufferSize(int size) { _bufferSize = size; }  
        #endregion

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

        private SocketAsyncEventArgs GetAcceptEventArgs() {
            return _acceptPool.Pop();
        }

        private SocketAsyncEventArgs GetReadWriteEventArgs() {
            return _readWritePool.Pop();
        }

        private void PushBackAcceptEventArgs(SocketAsyncEventArgs args) {
            _acceptPool.Push(args);
        }

        private void PushBackReadWriteArgs(SocketAsyncEventArgs args) {
            _readWritePool.Push(args);
        }

        private void ReleaseAcceptPool() {
            var list = _acceptPool.GetList();
            foreach (var item in list)
            {
                item.Completed -= AcceptSocket;
                item.Dispose();
            }
            _acceptPool.Clear();
        }

        private void ReleaseReadWritePool() {
            var list = _readWritePool.GetList();
            foreach (var item in list)
            {
                item.Dispose();
            }
            _readWritePool.Clear();
        }

        private void InitReadWriteArgs(SocketAsyncEventArgs args)
        {
            if (args.Buffer != null) { return; }
            args.SetBuffer(new byte[GetBufferSize()], 0, GetBufferSize());
        }

        private void BeginAcceptSocket(SocketAsyncEventArgs args)
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

        private void AcceptSocket(Object sender, SocketAsyncEventArgs args) {

            if (args.SocketError != SocketError.Success) {
                args.AcceptSocket = null;
                BeginAcceptSocket(args);
                return;
            }

            Socket socket = args.AcceptSocket;
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

        private void OnSessionConnected(TcpSessionBase session) {
            OnConnected(session.GetGuid());
        }

        private void OnSessionClose(TcpSessionBase session,SocketAsyncEventArgs recieveArgs,SocketAsyncEventArgs sendArgs) {
            this.OnClose(session.GetGuid());
            PushBackReadWriteArgs(recieveArgs);
            PushBackReadWriteArgs(sendArgs);
            _sessions.Remove(session);
            _sessionIds.Remove(session.GetGuid());
        }

        private void OnSessionReceive(TcpSessionBase session, byte[] buffer, int offset, int length) {
            OnReceive(session.GetGuid(), buffer, offset, length);
        }

        private void OnSessionSend(TcpSessionBase session, int length) {
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

    }
}
