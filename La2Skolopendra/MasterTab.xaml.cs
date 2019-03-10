using System;
using System.Windows.Controls;

namespace La2Skolopendra
{
    /// <summary>
    /// Interaction logic for MasterTab.xaml
    /// </summary>
    public partial class MasterTab
    {
        public MasterTab()
        {
            InitializeComponent();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            LogBox.ScrollToEnd();
        }
    }
}
