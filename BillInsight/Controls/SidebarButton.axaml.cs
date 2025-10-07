using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace BillInsight.Controls
{
    public partial class SidebarButton : UserControl
    {
        #region properties
        
        public static readonly StyledProperty<string> MyDataPropertyProperty =
            AvaloniaProperty.Register<SidebarButton, string>(nameof(MyDataProperty), "Default Value");

        public string MyDataProperty
        {
            get => GetValue(MyDataPropertyProperty);
            set => SetValue(MyDataPropertyProperty, value);
        }
        
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<SidebarButton, string>(nameof(Text), "");
        
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public static readonly StyledProperty<string> IconProperty =
            AvaloniaProperty.Register<SidebarButton, string>(nameof(Icon), "");
        
        public string Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<SidebarButton, ICommand>(nameof(Command));
        
        public ICommand Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        
        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<SidebarButton, object?>(nameof(CommandParameter));

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public new static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<SidebarButton, IBrush>(nameof(Foreground), Brushes.Black);
        
        public new IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        
        public new static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<SidebarButton, bool>(nameof(IsActive), false);
        
        public new bool IsActive
        {
            get => GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        
        #endregion
        
        public SidebarButton()
        {
            InitializeComponent();
            // ButtonSidebar.Classes.Add("active");
            
            // Gắn sự kiện Click
            ButtonSidebar.Click += OnButtonClick;
        }
        
        private void OnButtonClick(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine(IsActive);
            if (Command?.CanExecute(CommandParameter) == true)
                Command.Execute(CommandParameter);
        }
    }
}