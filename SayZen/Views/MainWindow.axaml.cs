using System;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Runtime.InteropServices; // ��� RuntimeInformation

namespace SayZen.Views
{
    public partial class MainWindow : Window
    {
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        public MainWindow()
        {
            InitializeComponent();
            this.Opened += MainWindow_Opened;
        }

        // ���� XAML-���������� �� �����-�� ������� �� ������������ InitializeComponent �
        // ���� ����� �������� XAML ������� � ����� ��������.
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MainWindow_Opened(object? sender, EventArgs e)
        {
            // ��������� ������ �� Windows
            if (!RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                return;

            // ������� �������� �������� HWND (��� ������� ������ � ����������� ������ Avalonia)
            var handle = this.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;

            if (handle != IntPtr.Zero)
            {
                // �������� ���� � ����� ��� Z-������� (��� ������ ����)
                SetWindowPos(handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
        }

        // ��� ���������� ��� ����������� ���� ��� ������� ����� ������ ����
        private void Grid_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }
    }
}
