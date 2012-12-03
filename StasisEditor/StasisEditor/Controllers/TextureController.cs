using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisEditor.Views;
using StasisEditor.Models;

namespace StasisEditor.Controllers
{
    public class TextureController : ITextureController
    {
        private IController _controller;
        private ITextureView _textureView;

        public TextureController(IController controller)
        {
            _controller = controller;
        }

        // openView
        public void openView()
        {
            if (_textureView == null)
            {
                _textureView = new TextureView();
                _textureView.setController(this);
                _textureView.Show();
            }
            else
            {
                _textureView.Focus();
            }
        }

        // viewClosed
        public void viewClosed()
        {
            _textureView = null;
        }

        // createNewTextureResources
        public void createNewTextureResources(List<TemporaryTextureResource> temporaryResources)
        {
            foreach (TemporaryTextureResource tempResource in temporaryResources)
            {
                // Prepare temporary resource for copy
                tempResource.prepareForCopy();

                // Check to make sure texture resource directory exists
                string textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY;
                if (!Directory.Exists(textureDirectory))
                    Directory.CreateDirectory(textureDirectory);

                // Check to make sure category directory exists
                string categoryDirectory = String.Format("{0}\\{1}", textureDirectory, tempResource.category);
                if (!Directory.Exists(categoryDirectory))
                    Directory.CreateDirectory(categoryDirectory);

                // Copy file to texture_directory\category\tag.extension
                string fileDestination = String.Format("{0}\\{1}", categoryDirectory, tempResource.fileName);
                if (File.Exists(fileDestination))
                {
                    if (MessageBox.Show(String.Format("The file {..{0}\\{1}} already exists. Overwrite it?", tempResource.category, tempResource.fileName), "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        File.Copy(tempResource.sourcePath, fileDestination);
                }
                else
                    File.Copy(tempResource.sourcePath, fileDestination);
            }
        }
    }
}
