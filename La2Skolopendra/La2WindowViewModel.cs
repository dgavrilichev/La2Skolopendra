using System;
using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class La2WindowViewModel : ViewModelBase
    {
        public BitmapSource Image { get; }
        public string ProcessId { get; }

        internal La2WindowViewModel([NotNull] BitmapSource bitmapSource, string processId)
        {
            if(string.IsNullOrEmpty(processId)) throw new ArgumentNullException(processId);
            Image = bitmapSource;// ?? throw new ArgumentNullException(nameof(bitmapSource));
            ProcessId = processId;
        }
    }
}
