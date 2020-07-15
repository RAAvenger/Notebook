using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notebook {
    /// <summary>
    /// Interaction logic for Edit_Note.xaml
    /// </summary>
    public partial class ShowAndEditNote : Window {
        private bool newNoteFlage = false;
        private bool noteTitleChangedFlag;
        private bool noteTextChangedFlag;
        private string noteTitle;
        private string noteText;
        public ShowAndEditNote() {
            InitializeComponent();
            this.noteTextChangedFlag = false;
            this.noteTitleChangedFlag = false;
            CheckNoteChanges();
        }

        #region titleBar Buttons
        /// <summary>
        /// Close Window.
        /// </summary>
        private void Button_Close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// back to prev Page. 
        /// </summary>
        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        #endregion titleBar Buttons
        #region Page Buttons
        /// <summary>
        /// save changes.
        /// </summary>
        private void Button_OK_Click(object sender, RoutedEventArgs e) {
            this.noteTextChangedFlag = false;
            this.noteTitleChangedFlag = false;
            this.noteTitle = textBox_Title.Text;
            this.noteText = GetTextFromRichTextBox(richTextBox_Note);
            CheckNoteChanges();
        }
        /// <summary>
        /// discard changes.
        /// </summary>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e) {
            textBox_Title.Text = this.noteTitle;
            richTextBox_Note.Document.Blocks.Clear();
            richTextBox_Note.Document.Blocks.Add(new Paragraph(new Run(this.noteText)));
            this.noteTextChangedFlag = false;
            this.noteTitleChangedFlag = false;
            CheckNoteChanges();
        }
        #endregion Page Buttons
        #region TextChanged Functions
        private void TextBox_Title_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.Compare(this.noteTitle, textBox_Title.Text) != 0) {
                this.noteTitleChangedFlag = true;
            }
            else {
                this.noteTitleChangedFlag = false;
            }
            CheckNoteChanges();
        }

        private void RichTextBox_Edit_Note_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.Compare(this.noteText, GetTextFromRichTextBox(richTextBox_Note)) != 0) {
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
                if (!this.noteTitleChangedFlag && !noteTextChangedFlag) {
                    button_OK.IsEnabled = false;
                    button_OK.Visibility = Visibility.Hidden;
                    button_Cancel.IsEnabled = false;
                    button_Cancel.Visibility = Visibility.Hidden;
                }
                else {
                    button_OK.IsEnabled = true;
                    button_OK.Visibility = Visibility.Visible;
                    button_Cancel.IsEnabled = true;
                    button_Cancel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception e) { }
        }
        #endregion User Functions

    }
}
