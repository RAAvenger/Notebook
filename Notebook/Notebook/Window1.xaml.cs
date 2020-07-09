using System;
using System.Windows;
using System.Windows.Controls;

namespace Notebook {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
            Button b = (Button)this.FindName("b");
            b.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)this.FindName("test");
            tb.Text = "hi";
        }

        private void Test_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tb = (TextBox)this.FindName("test");
            TextBox tb2 = (TextBox)this.FindName("test2");
            Button b = (Button)this.FindName("b");
            if (tb2 != null && tb != null && b != null) {
                if (!String.IsNullOrWhiteSpace(tb.Text))
                    b.IsEnabled = true;
                else
                    b.IsEnabled = false;

                tb2.Text = tb.Text;
            }
        }
    }
}
