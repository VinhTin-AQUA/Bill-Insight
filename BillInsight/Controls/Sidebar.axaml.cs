using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace BillInsight.Controls
{
    public partial class Sidebar : UserControl
    {
        private bool sidebarVisible = true;
        
        public Sidebar()
        {
            InitializeComponent();
        }
        private void Toggle_Sidebar_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sidebarVisible)
            {
                SidebarColumn.Width = 55;
                sidebarVisible = false;
                SidebarLabel.IsVisible = false;
                return;
            }

            SidebarColumn.Width = 200;
            sidebarVisible = true;
            SidebarLabel.IsVisible = true;
        }
    }
}