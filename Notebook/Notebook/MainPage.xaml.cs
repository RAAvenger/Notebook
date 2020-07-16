﻿using System;
using System.Data;
using System.Windows;
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
        #endregion variables.
        public MainPage() {
            InitializeComponent();
            this.database = new Database();
            Page_logIn = new LogIn_SignIn(this, this.database);
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

        public void Refresh() {
            DataSet t = database.GetUserNotes(this.userID);
            NoteCards.Children.Clear();
            foreach (DataRow pRow in t.Tables[0].Rows) {
                NoteCard noteCard = new NoteCard(Int32.Parse(pRow.ItemArray.GetValue(0).ToString()),
                    pRow.ItemArray.GetValue(2).ToString(),
                    pRow.ItemArray.GetValue(3).ToString());
                noteCard.Padding = new Thickness(3);
                noteCard.Width = 280;
                noteCard.Height = 200;
                noteCard.MouseUp += NoteCard_MouseUp;
                NoteCards.Children.Add(noteCard);
            }
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
                this.page_Note = card.GetEditePage(this, this.database);
                this.page_Note.Show();
                this.Hide();
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e) {
            page_Note = new ShowAndEditNote(this, this.database, userID);
            page_Note.Show();
            this.Hide();
        }
    }
}
