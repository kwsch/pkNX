using System;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace pkNX.Containers
{
    public sealed class ContainerHandler
    {
        public event EventHandler<FileCountDeterminedEventArgs> FileCountDetermined;
        public event EventHandler<FileProgressedEventArgs> FileProgressed;

        private int count;

        public void Initialize(int total)
        {
            count = total;
            var args = new FileCountDeterminedEventArgs {Total = total};
            FileCountDetermined?.Invoke(null, args);
        }

        public void StepFile(int ctr, int total = -1, string fileName = null)
        {
            if (total < 0)
                total = count;
            var args = new FileProgressedEventArgs {Current = ctr, Total = total, CurrentFile = fileName};
            FileProgressed?.Invoke(null, args);
        }
    }
}