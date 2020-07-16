using System.Windows;
using System.Windows.Input;
using Notebook;

namespace Notebook {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
           
        }

        ///// <summary>
        ///// TitleBar_MouseDown - Drag if single-click, resize if double-click
        ///// </summary>
        //private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e) {
        //    if (e.ChangedButton == MouseButton.Left)
        //        if (e.ClickCount == 2) {
        //            AdjustWindowSize();
        //        }
        //        else {
        //            Application.Current.MainWindow.DragMove();
        //        }
        //}

        ///// <summary>
        ///// CloseButton_Clicked
        ///// </summary>
        //private void CloseButton_Click(object sender, RoutedEventArgs e) {
        //    Application.Current.Shutdown();
        //}

        ///// <summary>
        ///// MaximizedButton_Clicked
        ///// </summary>
        //private void MaximizeButton_Click(object sender, RoutedEventArgs e) {
        //    AdjustWindowSize();
        //}

        ///// <summary>
        ///// Minimized Button_Clicked
        ///// </summary>
        //private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
        //    this.WindowState = WindowState.Minimized;
        //}

        ///// <summary>
        ///// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        ///// </summary>
        //private void AdjustWindowSize() {
        //    if (this.WindowState == WindowState.Maximized) {
        //        this.WindowState = WindowState.Normal;
                
        //    }
        //    else {
        //        this.WindowState = WindowState.Maximized;
        //    }

        //}
        
    }
}
