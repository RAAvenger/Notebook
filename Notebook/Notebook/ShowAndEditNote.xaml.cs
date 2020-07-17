using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notebook {
    /// <summary>
    /// Interaction logic for Edit_Note.xaml
    /// </summary>
    public partial class ShowAndEditNote : Window {

        #region variables
        private bool newNoteFlage = false;
        private bool noteTitleChangedFlag;
        private bool noteTextChangedFlag;
        private int userID;
        private int noteID;
        private string noteTitle;
        private string noteText;
        private Database database;
        private MainPage homePage;
        #endregion variables

        public ShowAndEditNote(MainPage homePage, Database database, int userID) {
            InitializeComponent();
            this.homePage = homePage;
            this.database = database;
            this.userID = userID;
            this.newNoteFlage = true;
            textBox_Title.Text = "تیتر را وارد کنید";
            this.noteTextChangedFlag = false;
            this.noteTitleChangedFlag = false;
            CheckNoteChanges();
        }
        public ShowAndEditNote(MainPage homePage, Database database, int noteID, string noteTitle, string noteText) {
            InitializeComponent();
            this.homePage = homePage;
            this.database = database;
            this.noteID = noteID;
            this.noteTitle = noteTitle == "null" ? "تیتر را وارد کنید" : noteTitle;
            this.noteText = noteText == "null" ? null : noteText;
            this.newNoteFlage = false;
            textBox_Title.Text = this.noteTitle;
            richTextBox_Note.Document.Blocks.Clear();
            richTextBox_Note.Document.Blocks.Add(new Paragraph(new Run(this.noteText)));
            this.noteTextChangedFlag = false;
            this.noteTitleChangedFlag = false;
            CheckNoteChanges();
        }

        #region titleBar Buttons
        /// <summary>
        /// back to prev Page. 
        /// </summary>
        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            this.homePage.Refresh();
            this.homePage.Show();
            this.Close();
        }
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

        #region save and discard Functions
        /// <summary>
        /// save changes.
        /// </summary>
        private void Button_OK_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox_Title.Text.Trim()) && string.IsNullOrEmpty(GetTextFromRichTextBox(richTextBox_Note).Trim())) {
                MessageBox.Show("Note is Empty");
            }
            else {
                if (this.newNoteFlage) {
                    database.AddNewNote(this.userID, textBox_Title.Text.Trim() == "تیتر را وارد کنید" ? "" : textBox_Title.Text.Trim(), GetTextFromRichTextBox(richTextBox_Note).Trim());
                    this.homePage.Refresh();
                    this.homePage.Show();
                    this.Close();
                }
                else {
                    this.noteTextChangedFlag = false;
                    this.noteTitleChangedFlag = false;
                    this.noteTitle = textBox_Title.Text.Trim();
                    this.noteText = GetTextFromRichTextBox(richTextBox_Note).Trim();
                    this.database.EditNote(this.noteID, this.noteTitle, this.noteText);
                    CheckNoteChanges();
                }
            }
        }
        /// <summary>
        /// discard changes.
        /// </summary>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e) {
            if (this.newNoteFlage) {
                this.homePage.Show();
                this.Close();
            }
            else {
                textBox_Title.Text = this.noteTitle;
                richTextBox_Note.Document.Blocks.Clear();
                richTextBox_Note.Document.Blocks.Add(new Paragraph(new Run(this.noteText)));
                this.noteTextChangedFlag = false;
                this.noteTitleChangedFlag = false;
                CheckNoteChanges();
            }
        }
        #endregion save and discard Functions

        #region TextChanged Functions
        private void TextBox_Title_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.Compare(this.noteTitle, textBox_Title.Text.Trim()) != 0) {
                this.noteTitleChangedFlag = true;
            }
            else {
                this.noteTitleChangedFlag = false;
            }
            CheckNoteChanges();
        }

        private void RichTextBox_Edit_Note_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.Compare(this.noteText, GetTextFromRichTextBox(richTextBox_Note).Trim()) != 0) {
                this.noteTextChangedFlag = true;
            }
            else {
                this.noteTextChangedFlag = false;
            }
            CheckNoteChanges();
        }
        #endregion TextChanged Functions

        #region User Functions
        /// <summary>
        /// get text from rich text box.
        /// </summary>
        /// <param name="richtextbox"> the rich text box that you want its text</param>
        /// <returns> rich text box's text as string </returns>
        private string GetTextFromRichTextBox(RichTextBox richtextbox) {
            try {
                TextRange textRange = new TextRange(richtextbox.Document.ContentStart, richtextbox.Document.ContentEnd);
                return textRange.Text;
            }
            catch (Exception e) {
                return null;
            }
        }

        private void CheckNoteChanges() {
            try {
                if (string.IsNullOrEmpty(textBox_Title.Text.Trim()) && string.IsNullOrEmpty(GetTextFromRichTextBox(richTextBox_Note).Trim())) {
                    button_OK.IsEnabled = false;
                    button_OK.Visibility = Visibility.Hidden;
                    button_Cancel.IsEnabled = false;
                    button_Cancel.Visibility = Visibility.Hidden;
                }
                else if (this.noteTitleChangedFlag || noteTextChangedFlag) {
                    button_OK.IsEnabled = true;
                    button_OK.Visibility = Visibility.Visible;
                    button_Cancel.IsEnabled = true;
                    button_Cancel.Visibility = Visibility.Visible;
                }
                else {
                    button_OK.IsEnabled = false;
                    button_OK.Visibility = Visibility.Hidden;
                    button_Cancel.IsEnabled = false;
                    button_Cancel.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception e) { }
        }

        #endregion User Functions

        private void TextBox_Title_GotFocus(object sender, RoutedEventArgs e) {
            if (textBox_Title.Text.Trim() == "تیتر را وارد کنید") {
                textBox_Title.Text = null;
                this.noteTitleChangedFlag = false;
                this.noteTextChangedFlag = false;
                CheckNoteChanges();
            }
        }

        private void TextBox_Title_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox_Title.Text.Trim())) {
                textBox_Title.Text = "تیتر را وارد کنید";
                this.noteTitleChangedFlag = false;
                this.noteTextChangedFlag = false;
                CheckNoteChanges();
            }
        }
    }
}
