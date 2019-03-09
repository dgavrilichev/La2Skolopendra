using System;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        internal MainWindow([NotNull] MainViewModel viewModel)
        {
            if(viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            viewModel.RequestActivateWindow += (sender, args) => Activate();
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
