﻿using NetCoreServer;
using NetMQ;
using NetMQ.Sockets;
using Serilog;
using System;
using System.Net.Sockets;

namespace Core8
{
    internal class TelnetSession : TcpSession
    {
        private PublisherSocket publisher;

        public TelnetSession(TelnetServer server, PublisherSocket publisher) : base(server)
        {
            this.publisher = publisher;
        }

        protected override void OnConnected()
        {
            SendAsync("WELCOME TO THE PDP-8 TERMINAL SERVER\r\n");

        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var frame = new byte[size];

            Array.Copy(buffer, offset, frame, 0, size);

            publisher.TrySendFrame(frame);

            //SendAsync(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            Log.Error($"Error caught in session: {error}");
        }
    }
}