using System.Windows;

namespace Notebook {
    public class WindowProperties {
        double top { get; set; }
        double left { get; set; }
        double height { get; set; }
        double width { get; set; }
        WindowState windowState { get; set; }
        public WindowProperties(double top, double left, double height, double width, WindowState windowState) {
            this.top = top;
            this.left = left;
            this.height = height;
            this.width = width;
            this.windowState = windowState;
        }
    }
}
