using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TextFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string selectedPath;
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            CommonOpenFileDialog browse = new CommonOpenFileDialog();
            browse.IsFolderPicker = true;
            if (browse.ShowDialog() == CommonFileDialogResult.Ok)
            {
                selectedPath = browse.FileName;
                DirectoryInfo dir = new DirectoryInfo(selectedPath);
                foreach (FileInfo file in dir.GetFiles("*.txt"))
                {
                    string[] row = { file.Name, file.Length.ToString(), file.Extension };
                    ListViewItem item = new ListViewItem(row);
                    listView1.Items.Add(item);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            if (string.IsNullOrEmpty(searchText)) return;

            listView1.Items.Clear(); // clear previous search results

            string[] files = Directory.GetFiles(selectedPath, "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string fileContent = File.ReadAllText(file);
                string pattern = @"\b" + searchText + @"\b"; // regex pattern to match whole words only
                if (Regex.IsMatch(fileContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    ListViewItem item = new ListViewItem(new[] { fileName, new FileInfo(file).Length.ToString(), Path.GetExtension(file) });
                    listView1.Items.Add(item);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                
                string filePath = Path.Combine(selectedPath, listView1.SelectedItems[0].Text);
                textBox1.Text = filePath;
                if (!string.IsNullOrEmpty(filePath))
                {
                    string arguments = $@"/select, {filePath}";
                    Process.Start("explorer.exe", arguments);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            listView1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(selectedPath);
            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                string[] row = { file.Name, file.Length.ToString(), file.Extension };
                ListViewItem item = new ListViewItem(row);
                listView1.Items.Add(item);
            }

        }
    }
}
