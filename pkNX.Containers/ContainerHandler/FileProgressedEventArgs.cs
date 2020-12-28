using System;

namespace pkNX.Containers
{
    public class FileProgressedEventArgs : EventArgs
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public string? CurrentFile { get; set; }
    }
}