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
            this.IsSelected = false;
        }
        public NoteCard(int id, string noteTitle, string noteText) {
            InitializeComponent();
            this.noteID = id;
            this.noteTitle = noteTitle;
            this.noteText = noteText;
            textBlock_Title.Text = this.noteTitle;
            textBlock_ShortNote.Text = SmallText(this.noteText);
            this.IsSelected = false;
        }

        private string SmallText(string text) {
            text.Replace("/n", " ");
            text.Replace("/t", " ");
            if (text.Length > 6 * 50) {
                text.Remove(6 * 50);
                text = text.Insert(6 * 50, "...");
            }
            return text;
        }

        public ShowAndEditNote GetEditePage(MainPage homePage, Database database) {
            return new ShowAndEditNote(homePage, database, noteID, noteTitle, noteText);
        }
        private void SetTextBlocksText() {
            textBlock_Title.Text = this.noteTitle;
            textBlock_ShortNote.Text = this.noteText;
        }
        public void Selected() {
            grid_Contaner.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xC4, 0x25));
            this.IsSelected = true;
        }
        public void UnSelected() {
            grid_Contaner.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
            this.IsSelected = false;
        }

    }
}
