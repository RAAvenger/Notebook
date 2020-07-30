using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Notebook {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window {
        #region variables.
        private int userID;
        private string username;
        private Database database;
        private LogIn_SignIn Page_logIn;
        private ShowAndEditNote page_Note;
        private TitleBar titleBar;
        #endregion variables.
        public MainPage() {
            InitializeComponent();
            WindowState = WindowState.Normal;
            this.titleBar = new TitleBar(this);
            this.database = new Database();
            Page_logIn = new LogIn_SignIn(this, this.database);
            Page_logIn.Show();
            Page_logIn.WindowState = this.WindowState;
            this.Hide();
        }
        #region titleBar Buttons
        /// <summary>
        /// Close Window.
        /// </summary>
        private void Button_Close_Click(object sender, RoutedEventArgs e) {
            titleBar.Close();
        }
        /// <summary>
        /// call AdjustWindowSize() to change window size.
        /// </summary>
        public void Button_Maximize_Click(object sender, RoutedEventArgs e) {
            titleBar.Maximize(sender);
        }
        /// <summary>
        /// Minimize Window.
        /// </summary>
        private void Button_Minimize_Click(object sender, RoutedEventArgs e) {
            titleBar.Minimize();
        }
        #endregion titleBar Buttons

        /// <summary>
        /// set username and userID.
        /// </summary>
        public void SetUserIdAndUsername(int userID, string username) {
            this.userID = userID;
            this.username = username;
            textBlock_Welcome.Text = " خوش آمدید " + username + " سلام ";
        }

        /// <summary>
        /// clear board and fill it again( get data from database ).
        /// </summary>
        public void Refresh() {
            DataSet t = database.GetUserNotes(this.userID);
            wrapPanel_NoteCards.Children.Clear();
            foreach (DataRow pRow in t.Tables[0].Rows) {
                NoteCard noteCard = new NoteCard(Int32.Parse(pRow.ItemArray.GetValue(0).ToString()),
                    pRow.ItemArray.GetValue(2).ToString(),
                    pRow.ItemArray.GetValue(3).ToString());
                noteCard.Padding = new Thickness(3);
                noteCard.Width = 280;
                noteCard.Height = 200;
                noteCard.MouseUp += NoteCard_MouseUp;
                noteCard.button_Delete.Click += Button_Delete_Click;
                wrapPanel_NoteCards.Children.Add(noteCard);
            }
        }

        /// <summary>
        /// delete Note function for delete button of cards.
        /// </summary>
        /// <param name="sender">
        /// the card's delete button.
        /// </param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e) {
            Button senderButton = (Button)sender;
            Grid parentGrid = (Grid)senderButton.Parent;
            NoteCard card = (NoteCard)parentGrid.Parent;
            this.database.DeleteNote(card.noteID);
            this.Refresh();
        }

        /// <summary>
        /// open card on mouse up.
        /// </summary>
        private void NoteCard_MouseUp(object sender, MouseButtonEventArgs e) {
            NoteCard card = (NoteCard)sender;
            if (card != null) {
                this.page_Note = card.GetEditePage(this, this.database);
                this.page_Note.Show();
                this.Hide();
            }
        }

        /// <summary>
        /// open a empty show and edit note page to add new note. 
        /// </summary>
        private void Button_Add_Click(object sender, RoutedEventArgs e) {
            page_Note = new ShowAndEditNote(this, this.database, userID);
            page_Note.Show();
            this.Hide();
        }

        /// <summary>
        /// Loging out and showing log in page.
        /// </summary>
        private void Button_LogOut_Click(object sender, RoutedEventArgs e) {
            this.userID = 0;
            this.username = null;
            wrapPanel_NoteCards.Children.Clear();
            Page_logIn = new LogIn_SignIn(this, this.database);
            Page_logIn.Show();
            this.Hide();
        }
    }
}
