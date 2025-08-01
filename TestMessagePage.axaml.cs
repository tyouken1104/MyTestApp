using Avalonia.Controls;
using MyTestApp.ViewModels;

namespace MyTestApp
{
    public partial class TestMessagePage : UserControl
    {
        public TestMessagePage()
        {
            InitializeComponent();
        }

        public TestMessagePage(TestMessageViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}