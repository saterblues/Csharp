using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Csharp.Liqing.Net
{
    public class TcpSessionBase
    {
        #region 待发送数据包结构体
        private struct WaitingForSendPackage
        {
            public WaitingForSendPackage(byte[] buffer = null, int offset = 0, int length = 0)
            {
                this.buffer = buffer;
                this.offset = offset;
                this.waitingForSendLength = length;
            }
            public byte[] buffer;
            public int offset;
            public int waitingForSendLength;

            public void CleanUp()
            {
                buffer = null;
                offset = 0;
                waitingForSendLength = 0;
            }
        } 
        #endregion

        private int _sendBufferSize = 1024;
        private Guid _guid;
        private Socket _socket = null;
        private SocketAsyncEventArgs _receiveArgs = null;
        private SocketAsyncEventArgs _sendArgs = null;
        private WaitingForSendPackage _waitingForSendPackage = new WaitingForSendPackage();
        private SocketError _lastError = SocketError.SocketError;
        public Action<TcpSessionBase, byte[], int, int> OnReceive = (session,buffer,offset,length) => { };
        public Action<TcpSessionBase, int> OnSend = (session, length) => { };
        public Action<TcpSessionBase, SocketAsyncEventArgs, SocketAsyncEventArgs> OnClose = (session, receiveArgs, sendArgs) => { };

        public TcpSessionBase(Socket socket, SocketAsyncEventArgs receive, SocketAsyncEventArgs send)
        {
            _guid = Guid.NewGuid();
            _socket = socket;
            _receiveArgs = receive;
            _sendArgs = send;
            _sendBufferSize = _sendArgs.Buffer.Length;
            _receiveArgs.Completed += SocketAsyncComplete;
            _sendArgs.Completed += SocketAsyncComplete;
        }

        public Guid GetGuid() {
            return _guid;
        }

        public void Run(){
            BeginReceive();
        }

        public void SocketAsyncComplete(Object sender, SocketAsyncEventArgs args)
        {
            switch (args.LastOperation) { 
                case SocketAsyncOperation.Send:
                    SendComplete();
                    break;
                case SocketAsyncOperation.Receive:
                    ReceiveComplete();
                    break;
            }
        }

        private void BeginReceive() {
            if (_socket == null || _receiveArgs == null) { return; }
            if (false == _socket.ReceiveAsync(_receiveArgs))
            {
                ReceiveComplete();
            }
        }

        private void BeginSend() {
            if (_socket == null || _socket == null) { return; }
            if (false == _socket.SendAsync(_sendArgs)) {
                SendComplete();
            }
        }

        /// <summary>
        /// 发送数据的大小,可能比_sendArgs的缓存大；当发送数据大时，使用将无法一次发送的数据分割
        /// 将暂时无法发送的数据WaitingForSendPackage存储在_sendArgs.UserToken中
        /// 待SendAsync调用完成后，将未发送的数据包再进行发送
        /// </summary>
        /// <param name="buffer">需要发送的原始数据</param>
        /// <param name="offset">原始数据发送的起始位置</param>
        /// <param name="length">原始数据需要发送总数据的长度</param>
        /// <returns>分割后，本次发送字节的长度</returns>
        private int SendDataSplit(byte[] buffer, int offset, int length)
        {
            int len = length;
            if (length > _sendBufferSize)
            {
                len = _sendBufferSize;
                _waitingForSendPackage.buffer = buffer;
                _waitingForSendPackage.offset = offset + len;
                _waitingForSendPackage.waitingForSendLength = length - len;
            }
            else
            {
                _waitingForSendPackage.CleanUp();
            }
            return len;
        }

        private void ReceiveComplete(){
            if (_receiveArgs == null) { return; }
            if (_receiveArgs.SocketError != SocketError.Success)
            {
                _lastError = _receiveArgs.SocketError;
                ClearSession();
                return;
            }

            if (_receiveArgs.BytesTransferred <= 0) {
                ClearSession();
                return;
            }

            OnReceive(this, _receiveArgs.Buffer, _receiveArgs.Offset, _receiveArgs.BytesTransferred);
            BeginReceive();
        }

        private void SendComplete() {
            if (_sendArgs == null) { return; }

            if (_sendArgs.SocketError != SocketError.Success) {
                _lastError = _sendArgs.SocketError;
                ClearSession();
                return;
            }

            OnSend(this, _sendArgs.BytesTransferred);

            if (_waitingForSendPackage.waitingForSendLength != 0) {
                SendAsync(
                    _waitingForSendPackage.buffer,
                    _waitingForSendPackage.offset,
                    _waitingForSendPackage.waitingForSendLength);
            }
        }

        private void ClearSocket() {
            if (_socket.Connected) { _socket.Shutdown(SocketShutdown.Both); }
            _socket.Close();
            _socket.Dispose();
            _socket = null;
        }

        public SocketError GetLastSocketError() {
            return _lastError;
        }

        public void ClearSession() {
            if (_socket == null) { return; }
            ClearSocket();
            OnClose(this, _receiveArgs, _sendArgs);

            _waitingForSendPackage.CleanUp();
            _receiveArgs.Completed -= SocketAsyncComplete;
            _receiveArgs = null;
            _sendArgs.Completed -= SocketAsyncComplete;
            _sendArgs = null;
        }

        public void SendAsync(byte[] buffer,int offset,int length)
        {
            if (_sendArgs == null) { return; }
            int len = SendDataSplit(buffer, offset, length);
            Array.Copy(buffer, offset, _sendArgs.Buffer, 0, len);
            try
            {
                _sendArgs.SetBuffer(0, len);
                BeginSend();
            }
            catch (Exception)
            {
                //似乎有时会重复发送，然后报错，提示已在操作
            }
        }

        public void SendAsync(byte[] buffer) 
        {
            SendAsync(buffer, 0, buffer.Length);
        }
    }
}
