﻿using Core8.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Core8
{
    public class PaperTape : IReader, IPunch
    {
        private readonly ManualResetEvent readerFlag = new ManualResetEvent(false);

        private volatile uint buffer;

        private ConcurrentQueue<uint> queue = new ConcurrentQueue<uint>();

        public uint Buffer => buffer & Masks.READER_BUFFER_MASK;

        public uint ReaderFlag => readerFlag.WaitOne(TimeSpan.Zero) ? 1u : 0u;

        public bool IsReaderFlagSet => ReaderFlag == 1u;

        public bool IsTapeLoaded => !queue.IsEmpty;

        public void Tick()
        {
            if (!IsReaderFlagSet && queue.TryDequeue(out var item))
            {
                buffer = item;

                readerFlag.Set();
            }
        }

        public void ClearReaderFlag()
        {
            readerFlag.Reset();
        }

        public void Load(byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            foreach (var item in data)
            {
                queue.Enqueue(item);
            }
        }
    }
}