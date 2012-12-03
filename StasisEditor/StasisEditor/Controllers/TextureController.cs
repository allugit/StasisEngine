using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public class TextureController : ITextureController
    {
        private IController _controller;
        private ITextureView _textureView;
        private List<TextureResource> _textureResources;

        public TextureController(IController controller)
        {
            _controller = controller;
            _textureResources = new List<TextureResource>();
        }

        // getTextureResources
        public ReadOnlyCollection<TextureResource> getTextureResources() { return _textureResources.AsReadOnly(); }

        // openView
        public void openView()
        {
            if (_textureView == null)
            {
                // Load texture resources
                loadTextureResources();

                // Create view
                _textureView = new TextureView();
                _textureView.setController(this);
                _textureView.refreshGrid();
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

        // loadTextureResources
        public void loadTextureResources()
        {
            // Clear already loaded texture resources
            _textureResources.Clear();
            _textureResources = TextureResource.loadFromDirectory(EditorController.TEXTURE_RESOURCE_DIRECTORY);
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
                    if (MessageBox.Show(String.Format("The file {{...{0}\\{1}}} already exists. Overwrite it?", tempResource.category, tempResource.fileName), "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        File.Copy(tempResource.sourcePath, fileDestination, true);
                }
                else
                    File.Copy(tempResource.sourcePath, fileDestination);
            }

            // Reload texture resources
            loadTextureResources();
            _textureView.refreshGrid();
        }
    }
}
