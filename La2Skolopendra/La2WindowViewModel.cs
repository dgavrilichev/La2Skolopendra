using System;
using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class La2WindowViewModel : ViewModelBase
    {
        public BitmapSource Image { get; }

        internal La2WindowViewModel([NotNull] BitmapSource bitmapSource)
        {
            Image = bitmapSource ?? throw new ArgumentNullException(nameof(bitmapSource));
        }
    }
}
