using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private BindingList<TextureResource> _textureResources;

        public TextureController(IController controller)
        {
            _controller = controller;
            _textureResources = new BindingList<TextureResource>();
        }

        // getTextureResources
        public BindingList<TextureResource> getTextureResources() { return _textureResources; }

        // openView
        public void openView()
        {
            if (_textureView == null)
            {
                // Load texture resources
                loadTextureResources();

                // Create view
                _textureView = new NewTextureResource();
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

        // loadTextureResources
        public void loadTextureResources()
        {
            // Clear already loaded texture resources
            _textureResources.Clear();
            _textureResources = new BindingList<TextureResource>(TextureResource.loadAll(EditorController.TEXTURE_RESOURCE_DIRECTORY));
        }

        // addTextureResources
        public void addTextureResources(string[] fileNames)
        {
            foreach (string filePath in fileNames)
                addTextureResource(filePath);
        }
        public void addTextureResource(string filePath)
        {
            // Make sure only unique files get added
            foreach (TextureResource tr in _textureResources)
            {
                if (tr.fileName == Path.GetFileName(filePath))
                    return;
            }

            // Create texture resource, and clear its tag and category properties
            TextureResource resource = TextureResource.loadFromFile(filePath);
            resource.tag = "";
            resource.category = "";
            _textureResources.Add(resource);
        }

        /*
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
        }*/

        // removeTextureResource
        public void removeTextureResource(List<TextureResource> resources)
        {
            foreach (TextureResource resource in resources)
                removeTextureResource(resource);
        }
        public void removeTextureResource(TextureResource resource)
        {
            // Remove file
            string textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY;
            string filePath = String.Format("{0}\\{1}", textureDirectory, resource.relativePath);
            File.Delete(filePath);

            // Remove directory if empty
            string categoryDirectory = String.Format("{0}\\{1}", textureDirectory, resource.category);
            if (Directory.GetFiles(categoryDirectory).Length == 0)
                Directory.Delete(categoryDirectory);

            // Remove from list
            _textureResources.Remove(resource);
        }

        // relocateTextureResource
        public void relocateTextureResource(TextureResource resource)
        {
            // Check to make sure category directory exists
            string textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY;
            string categoryDirectory = String.Format("{0}\\{1}", textureDirectory, resource.category);
            if (!Directory.Exists(categoryDirectory))
                Directory.CreateDirectory(categoryDirectory);

            // Copy file to texture_directory\category\tag.extension
            string fileDestination = String.Format("{0}\\{1}", categoryDirectory, resource.fileName);
            if (File.Exists(fileDestination))
            {
                if (MessageBox.Show(String.Format("The file {{...{0}\\{1}}} already exists. Overwrite it?", resource.category, resource.fileName), "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    File.Copy(resource.loadedFrom, fileDestination, true);
            }
            else
                File.Copy(resource.loadedFrom, fileDestination);

            // Update loadedFrom
            resource.loadedFrom = fileDestination;
        }
    }
}
