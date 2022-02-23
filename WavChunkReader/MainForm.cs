namespace WavChunkReader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                OpenTargetFile(files[0]);
            }
        }

        private void OpenTargetFile(string filePath)
        {
            if (string.Compare(Path.GetExtension(filePath).ToUpper(), ".WAV") != 0) return;

            // ì«Ç›èoÇµ
            var reader = new RiffReader(filePath);
            _textBoxResult.Text = reader.ToString();
        }
    }
}