using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MyTestApp.Views
{
    public partial class MessageTestView : UserControl
    {
        public MessageTestView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}