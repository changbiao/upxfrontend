using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace UPXFrontEnd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBoxUPXPath.Text = Properties.Settings.Default.UPXPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog object.
            openFileDialog1 = new OpenFileDialog();

            // Check if the user selected a file from the OpenFileDialog.
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)

                // Load the contents of the file into a RichTextBox control.
                textBoxLog.Text = openFileDialog1.FileName;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBoxFile_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (a != null)
                {
                    string s = a.GetValue(0).ToString();
                    textBoxFile.Text = s;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in DragDrop function: " + ex.Message);
            }
        }

        private void textBoxFile_DragEnter(object sender, DragEventArgs e)
        {
            // If file is dragged, show cursor "Drop allowed"
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UPXPath.Length == 0)
            {
                MessageBox.Show("UPX path not configured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBoxFile.Text.Length == 0)
            {
                MessageBox.Show("Nothing to compress!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    //p.StartInfo.FileName = "upx.exe";
                    p.StartInfo.FileName = Properties.Settings.Default.UPXPath;
                    p.StartInfo.Arguments = "-v \"" + textBoxFile.Text.ToString() + "\"";
                    p.Start();
                    String output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    textBoxLog.AppendText(output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog object.
            openFileDialog1 = new OpenFileDialog();

            // Check if the user selected a file from the OpenFileDialog.
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.UPXPath = openFileDialog1.FileName;
                textBoxUPXPath.Text = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
            }
        }
    }
}
