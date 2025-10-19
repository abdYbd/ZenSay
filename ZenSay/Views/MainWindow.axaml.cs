using Avalonia.Controls;
using Avalonia.Input;
using ZenSay.ViewModels;

namespace ZenSay.Views { 
    public partial class MainWindow : Window 
    { public MainWindow() 
        { 
            InitializeComponent(); 
            DataContext = new MainWindowViewModel(); 
        } 
        private void Grid_PointerPressed(object? sender, PointerPressedEventArgs e) 
        { if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) BeginMoveDrag(e); 
        } 
    } 
}