using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StasisEditor.Views.Controls
{
    public partial class EditPathsForm : Form
    {
        public EditPathsForm()
        {
            InitializeComponent();
            loadSettings();
        }

        private void loadSettings()
        {
            if (File.Exists("settings.xml"))
            {
                XDocument doc = XDocument.Load("settings.xml");
                XElement settings = doc.Element("Settings");
                debugFolderPath.Text = settings.Attribute("game_debug_path").Value;
                releaseFolderPath.Text = settings.Attribute("game_release_path").Value;
                resourcesSourcePath.Text = settings.Attribute("resources_source_path").Value;
            }
        }

        public void saveSettings()
        {
            XDocument doc = new XDocument(
                new XElement("Settings",
                    new XAttribute("game_debug_path", debugFolderPath.Text),
                    new XAttribute("game_release_path", releaseFolderPath.Text),
                    new XAttribute("resources_source_path", resourcesSourcePath.Text)));
            doc.Save("settings.xml");
        }

        private void debugFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();

            if (folderBrowseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                debugFolderPath.Text = folderBrowseDialog.SelectedPath;
        }

        private void releaseFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();

            if (folderBrowseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                releaseFolderPath.Text = folderBrowseDialog.SelectedPath;
        }

        private void resourcesSourceBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();

            if (folderBrowseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                resourcesSourcePath.Text = folderBrowseDialog.SelectedPath;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
