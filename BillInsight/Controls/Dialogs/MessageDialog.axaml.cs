using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace BillInsight.Controls.Dialogs
{
    public partial class MessageDialog : UserControl
    {
        public MessageDialog()
        {
            InitializeComponent();
        }
        
        public MessageDialog(string title, string mesage, bool isSucces) 
        {
            InitializeComponent();

            if (isSucces)
            {
                Border.Background = Brush.Parse("#4ade80");
            }
            else
            {
                Border.Background = Brush.Parse("#ef4444");
            }
            
            Title.Text = title;
            Message.Text = mesage;
        }
    }
}