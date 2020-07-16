using System.Windows;
using System.Windows.Input;

namespace Notebook {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window {
        #region variables.
        private LogIn_SignIn Page_logIn;
        private ShowAndEditNote page_Note;
        private int userID;
        private string username;
        #endregion variables.
        public MainPage() {
            InitializeComponent();
            Page_logIn = new LogIn_SignIn(this);
            Page_logIn.Show();
            this.Hide();
        }

        /// <summary>
        /// set username and password.
        /// </summary>
        public void SetUserIdAndUsername(int userID, string username) {
            this.userID = userID;
            this.username = username;
            textBlock_Welcome.Text = " خوش آمدید " + username + " سلام ";
        }

        #region titleBar Buttons
        /// <summary>
        /// Close Window.
        /// </summary>
        private void Button_Close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// call AdjustWindowSize() to change window size.
        /// </summary>
        private void Button_Maximize_Click(object sender, RoutedEventArgs e) {
            AdjustWindowSize();
        }
        /// <summary>
        /// Minimize Window.
        /// </summary>
        private void Button_Minimize_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Change window size from original size to maximum size and vice versa.
        /// </summary>
        private void AdjustWindowSize() {
            if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
            }
            else {
                this.WindowState = WindowState.Maximized;
            }
        }


        #endregion titleBar Buttons

        private void NoteCard_MouseUp(object sender, MouseButtonEventArgs e) {
            NoteCard card = (NoteCard)sender;
            if (card != null) {
                if (card.IsSelected) {
                    card.UnSelected();
                }
                else {
                    card.Selected();
                }
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e) {
            page_Note = new ShowAndEditNote();
            page_Note.Show();
            this.Hide();
        }
    }
}
