using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notebook {
    /// <summary>
    /// Interaction logic for NoteCard.xaml
    /// </summary>
    public partial class NoteCard : UserControl {
        public int noteID { get; }
        private string noteTitle;
        private string noteText;
        public bool IsSelected { get; set; }

        public NoteCard() {
            InitializeComponent();
            IsSelected = false;
        }
        public NoteCard(int id, string noteTitle, string noteText) {
            InitializeComponent();
            noteID = id;
            this.noteTitle = noteTitle;
            this.noteText = noteText;
            IsSelected = false;
            ShowTitleAndText();
        }
        private string SmallText(string text, bool isTitle) {
            text = text.Replace("\r\n", " ");
            text = text.Replace("\r\t", " ");
            if (isTitle && text.Length > 30) {
                text = text.Remove(27);
                text = text.Insert(27, "...");

            }
            else if (!isTitle && text.Length > 6 * 30) {
                text = text.Remove(6 * 30);
                text = text.Insert(6 * 30, "...");
            }
            return text;
        }
        public ShowAndEditNote GetEditePage(MainPage homePage, Database database) {
            return new ShowAndEditNote(homePage, database, noteID, noteTitle, noteText, homePage.windowProperties);
        }
        private void SetTextBlocksText() {
            textBlock_Title.Text = noteTitle;
            textBlock_ShortNote.Text = noteText;
        }
        public void Selected() {
            grid_Contaner.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xC4, 0x25));
            IsSelected = true;
        }
        public void UnSelected() {
            grid_Contaner.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
            IsSelected = false;
        }
        private void ShowTitleAndText() {
            if (noteTitle == "null") {
                textBlock_Title.Visibility = Visibility.Collapsed;
                line_betwinTitleAndText.Visibility = Visibility.Collapsed;
                textBlock_ShortNote.SetValue(Grid.RowProperty, 1);
                textBlock_ShortNote.SetValue(Grid.RowSpanProperty, 4);
            }
            else {
                textBlock_Title.Text = SmallText(noteTitle, true);
            }
            if (noteText == "null") {
                textBlock_ShortNote.Text = "خالی";
                textBlock_ShortNote.Padding = new Thickness(10);
                textBlock_ShortNote.FontSize = 25;
            }
            else {
                textBlock_ShortNote.Text = SmallText(noteText, false);
            }
        }
    }
}
