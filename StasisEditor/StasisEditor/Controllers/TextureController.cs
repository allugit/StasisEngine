﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using StasisEditor.Views;
using StasisCore.Models;

namespace StasisEditor.Controllers
{
    public class TextureController : Controller
    {
        private EditorController _editorController;
        private TextureView _textureView;
        private BindingList<TextureResource> _textureResources;

        public BindingList<TextureResource> textureResources { get { return _textureResources; } }

        public TextureController(EditorController editorController, TextureView textureView)
        {
            _editorController = editorController;
            _textureView = textureView;

            // Initialize texture view
            _textureView.setController(this);
            _textureView.bindTextureResources();
        }

        // loadResources
        protected override void loadResources()
        {
            // Load texture resources
            _textureResources = new BindingList<TextureResource>(TextureResource.loadAll(EditorController.TEXTURE_RESOURCE_DIRECTORY));

            // Initialize core texture controller
            StasisCore.Controllers.TextureController.textureDirectory = EditorController.TEXTURE_RESOURCE_DIRECTORY; // Use the absolute path, since the core uses a relative path by default.
            StasisCore.Controllers.TextureController.graphicsDevice = XNAResources.graphicsDevice;
            StasisCore.Controllers.TextureController.addResources(new List<TextureResource>(_textureResources));
        }

        // viewClosed
        public void viewClosed()
        {
            _textureView = null;
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

            // Create texture resource
            TextureResource resource = TextureResource.loadFromFile(filePath);

            // Copy to a temporary directory
            copyToTemporaryDirectory(resource);

            // Clear initial values
            resource.tag = "";
            resource.category = "";
            _textureResources.Add(resource);
            StasisCore.Controllers.TextureController.addResource(resource);
        }

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

        // copyToTemporaryDirectory
        private void copyToTemporaryDirectory(TextureResource resource)
        {
            // Make sure temporary directory exists
            string temporaryDirectory = EditorController.TEMPORARY_TEXTURE_DIRECTORY;
            if (!Directory.Exists(temporaryDirectory))
                Directory.CreateDirectory(temporaryDirectory);

            // Make sure subdirectory exists
            string subDirectory = Path.GetDirectoryName(resource.getFullPath(temporaryDirectory));
            if (!Directory.Exists(subDirectory))
                Directory.CreateDirectory(subDirectory);

            // Copy file
            string destination = resource.getFullPath(temporaryDirectory);
            File.Copy(resource.currentPath, destination);

            // Update current path
            resource.currentPath = destination;
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
                    File.Move(resource.currentPath, fileDestination);
            }
            else
                File.Move(resource.currentPath, fileDestination);

            // Delete empty directories
            string sourceDirectory = Path.GetDirectoryName(resource.currentPath);
            if (Directory.GetFiles(sourceDirectory).Length == 0)
                Directory.Delete(sourceDirectory);

            // Update loadedFrom
            resource.currentPath = fileDestination;
        }
    }
}
