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
        private TitleBar titleBar;
        private WindowProperties windowProperties;
        #endregion variables

        public ShowAndEditNote(MainPage homePage, Database database, int userID, WindowProperties windowProperties) {
            InitializeComponent();
            this.windowProperties = windowProperties;
            titleBar = new TitleBar(this, homePage);
            this.homePage = homePage;
            this.database = database;
            this.userID = userID;
            newNoteFlage = true;
            textBox_Title.Text = "تیتر را وارد کنید";
            noteTextChangedFlag = false;
            noteTitleChangedFlag = false;
            CheckNoteChanges();
        }
        public ShowAndEditNote(MainPage homePage, Database database, int noteID, string noteTitle, string noteText, WindowProperties windowProperties) {
            InitializeComponent();
            this.windowProperties = windowProperties;
            titleBar = new TitleBar(this, homePage);
            this.homePage = homePage;
            this.database = database;
            this.noteID = noteID;
            this.noteTitle = noteTitle == "null" ? "تیتر را وارد کنید" : noteTitle;
            this.noteText = noteText == "null" ? null : noteText;
            newNoteFlage = false;
            textBox_Title.Text = this.noteTitle;
            richTextBox_Note.Document.Blocks.Clear();
            richTextBox_Note.Document.Blocks.Add(new Paragraph(new Run(this.noteText)));
            noteTextChangedFlag = false;
            noteTitleChangedFlag = false;
            CheckNoteChanges();
        }

        #region titleBar
        /// <summary>
        /// back to prev Page. 
        /// </summary>
        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            titleBar.Back();
        }
        /// <summary>
        /// Close Window.
        /// </summary>
        private void Button_Close_Click(object sender, RoutedEventArgs e) {
            titleBar.Close();
        }
        /// <summary>
        /// call AdjustWindowSize() to change window size.
        /// </summary>
        private void Button_Maximize_Click(object sender, RoutedEventArgs e) {
            titleBar.Maximize(sender);
        }
        /// <summary>
        /// Minimize Window.
        /// </summary>
        private void Button_Minimize_Click(object sender, RoutedEventArgs e) {
            titleBar.Minimize();
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
                if (newNoteFlage) {
                    database.AddNewNote(userID, textBox_Title.Text.Trim() == "تیتر را وارد کنید" ? "" : textBox_Title.Text.Trim(), GetTextFromRichTextBox(richTextBox_Note).Trim());
                    homePage.Refresh();
                    homePage.Show();
                    Close();
                }
                else {
                    noteTextChangedFlag = false;
                    noteTitleChangedFlag = false;
                    noteTitle = textBox_Title.Text.Trim();
                    noteText = GetTextFromRichTextBox(richTextBox_Note).Trim();
                    database.EditNote(noteID, noteTitle == "تیتر را وارد کنید" ? "" : noteTitle, noteText);
                    CheckNoteChanges();
                }
            }
        }
        /// <summary>
        /// discard changes.
        /// </summary>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e) {
            if (newNoteFlage) {
                homePage.Show();
                Close();
            }
            else {
                textBox_Title.Text = noteTitle;
                richTextBox_Note.Document.Blocks.Clear();
                richTextBox_Note.Document.Blocks.Add(new Paragraph(new Run(noteText)));
                noteTextChangedFlag = false;
                noteTitleChangedFlag = false;
                CheckNoteChanges();
            }
        }
        #endregion save and discard Functions

        #region TextChanged Functions
        private void TextBox_Title_TextChanged(object sender, TextChangedEventArgs e) {
            noteTitleChangedFlag = string.Compare(noteTitle, textBox_Title.Text.Trim()) != 0 ? true : false;
            CheckNoteChanges();
        }

        private void RichTextBox_Edit_Note_TextChanged(object sender, TextChangedEventArgs e) {
            noteTextChangedFlag = string.Compare(noteText, GetTextFromRichTextBox(richTextBox_Note).Trim()) != 0 ? true : false;
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
                else if (noteTitleChangedFlag || noteTextChangedFlag) {
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
                noteTitleChangedFlag = false;
                noteTextChangedFlag = false;
                CheckNoteChanges();
            }
        }

        private void TextBox_Title_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox_Title.Text.Trim())) {
                textBox_Title.Text = "تیتر را وارد کنید";
                noteTitleChangedFlag = false;
                noteTextChangedFlag = false;
                CheckNoteChanges();
            }
        }
    }
}
