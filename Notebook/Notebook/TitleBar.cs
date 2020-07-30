using System.Windows;
using System.Windows.Controls;

namespace Notebook {
    class TitleBar {
        private MainPage parentPage;
        private Window thisPage;
        public TitleBar(Window thisPage, MainPage parentPage = null) {
            this.parentPage = parentPage;
            this.thisPage = thisPage;
        }

        /// <summary>
        /// back to prev Page. 
        /// </summary>
        public void Back() {
            if (parentPage != null) {
                parentPage.Show();
            }
            thisPage.Close();
        }
        /// <summary>
        /// Close Window.
        /// </summary>
        public void Close() {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// call AdjustWindowSize() to change window size.
        /// </summary>
        public void Maximize(object sender) {
            AdjustWindowSize((Button)sender);
        }
        /// <summary>
        /// Minimize Window.
        /// </summary>
        public void Minimize() {
            thisPage.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Change window size from original size to maximum size and vice versa.
        /// </summary>
        public void AdjustWindowSize(Button sender) {
            sender.ToolTip = "Maximize";
            StackPanel panel = (StackPanel)sender.FindName("stackPanel");
            if (thisPage.WindowState == WindowState.Maximized) {
                thisPage.WindowState = WindowState.Normal;
                if (panel != null) {
                    Image maximize = (Image)panel.FindName("maximize");
                    if (maximize != null)
                        maximize.Visibility = Visibility.Visible;
                    Image normalSize = (Image)panel.FindName("normalSize");
                    if (normalSize != null)
                        normalSize.Visibility = Visibility.Collapsed;
                }
            }
            else {
                thisPage.WindowState = WindowState.Maximized;
                sender.ToolTip = "Normal Size";
                if (panel != null) {
                    Image maximize = (Image)panel.FindName("maximize");
                    if (maximize != null)
                        maximize.Visibility = Visibility.Collapsed;
                    Image normalSize = (Image)panel.FindName("normalSize");
                    if (normalSize != null)
                        normalSize.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
