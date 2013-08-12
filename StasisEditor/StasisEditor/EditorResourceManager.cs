using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor
{
    public class EditorResourceManager
    {
        // Checks to see if a resource exists
        public static bool exists(string uid)
        {
            try
            {
                // Check to see if the resource is loaded already
                if (ResourceManager.isResourceLoaded(uid))
                    return true;

                // Check materials
                using (FileStream fs = new FileStream(ResourceManager.materialPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement materialData in data.Elements("Material"))
                    {
                        if (materialData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check items
                using (FileStream fs = new FileStream(ResourceManager.itemPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement itemData in data.Elements("Item"))
                    {
                        if (itemData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check blueprints
                using (FileStream fs = new FileStream(ResourceManager.blueprintPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement blueprintData in data.Elements("Blueprint"))
                    {
                        if (blueprintData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check characters
                using (FileStream fs = new FileStream(ResourceManager.characterPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement characterData in data.Elements("Character"))
                    {
                        if (characterData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check dialogue
                using (FileStream fs = new FileStream(ResourceManager.dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement dialogueData in data.Elements("Dialogue"))
                    {
                        if (dialogueData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check circuits
                using (FileStream fs = new FileStream(ResourceManager.dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement circuitData in data.Elements("Circuit"))
                    {
                        if (circuitData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check backgrounds
                using (FileStream fs = new FileStream(ResourceManager.backgroundPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement backgroundData in data.Elements("Background"))
                    {
                        if (backgroundData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check world maps
                using (FileStream fs = new FileStream(ResourceManager.worldMapPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement worldMapData in data.Elements("WorldMap"))
                    {
                        if (worldMapData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check quests
                using (FileStream fs = new FileStream(ResourceManager.questPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement questData in data.Elements("Quest"))
                    {
                        if (questData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check rope materials
                using (FileStream fs = new FileStream(ResourceManager.ropeMaterialPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement ropeMaterialData in data.Elements("RopeMaterial"))
                    {
                        if (ropeMaterialData.Attribute("uid").Value == uid)
                            return true;
                    }
                }
            }
            catch (XmlException e)
            {
                throw new InvalidResourceException();
            }
            catch (FileNotFoundException e)
            {
                throw new ResourceNotFoundException(uid);
            }

            return false;
        }

        // Push resources
        public static void pushResources()
        {
            string previousRootDirectory = ResourceManager.rootDirectory;
            ResourceManager.rootDirectory = "";

            for (int i = 0; i < 2; i++)
            {
                string resourceDestinationPath = i == 0 ? EditorController.debugGamePath : EditorController.releaseGamePath;

                if (!Directory.Exists(resourceDestinationPath))
                    continue;

                if (Directory.Exists(resourceDestinationPath + "\\data"))
                    Directory.Delete(resourceDestinationPath + "\\data", true);
                Directory.CreateDirectory(resourceDestinationPath + "\\data");

                // Textures
                string textureDestination = resourceDestinationPath + "\\" + ResourceManager.texturePath;
                string textureSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.texturePath;
                Directory.CreateDirectory(textureDestination);
                string[] texturePaths = Directory.GetFiles(textureSource);
                foreach (string texturePath in texturePaths)
                {
                    File.Copy(texturePath, textureDestination + "\\" + Path.GetFileName(texturePath));
                }

                // Materials
                string materialDestination = resourceDestinationPath + "\\" + ResourceManager.materialPath;
                string materialSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.materialPath;
                File.Copy(materialSource, materialDestination);

                // Blueprints
                string blueprintDestination = resourceDestinationPath + "\\" + ResourceManager.blueprintPath;
                string blueprintSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.blueprintPath;
                File.Copy(blueprintSource, blueprintDestination);

                // Circuits
                string circuitDestination = resourceDestinationPath + "\\" + ResourceManager.circuitPath;
                string circuitSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.circuitPath;
                File.Copy(circuitSource, circuitDestination);

                // Backgrounds
                string backgroundDestination = resourceDestinationPath + "\\" + ResourceManager.backgroundPath;
                string backgroundSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.backgroundPath;
                File.Copy(backgroundSource, backgroundDestination);

                // World maps
                string worldMapDestination = resourceDestinationPath + "\\" + ResourceManager.worldMapPath;
                string worldMapSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.worldMapPath;
                File.Copy(worldMapSource, worldMapDestination);

                // Quests
                string questDestination = resourceDestinationPath + "\\" + ResourceManager.questPath;
                string questSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.questPath;
                File.Copy(questSource, questDestination);

                // Rope materials
                string ropeMaterialDestination = resourceDestinationPath + "\\" + ResourceManager.ropeMaterialPath;
                string ropeMaterialSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.ropeMaterialPath;
                File.Copy(ropeMaterialSource, ropeMaterialDestination);

                // Characters
                string characterDestination = resourceDestinationPath + "\\" + ResourceManager.characterPath;
                string characterSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.characterPath;
                File.Copy(characterSource, characterDestination);

                // Dialogue
                string dialogueDestination = resourceDestinationPath + "\\" + ResourceManager.dialoguePath;
                string dialogueSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.dialoguePath;
                File.Copy(dialogueSource, dialogueDestination);

                // Items
                string itemDestination = resourceDestinationPath + "\\" + ResourceManager.itemPath;
                string itemSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.itemPath;
                File.Copy(itemSource, itemDestination);

                // Levels (and level scripts)
                string levelDestination = resourceDestinationPath + "\\" + ResourceManager.levelPath;
                string levelSource = EditorController.resourcesSourcePath + "\\" + ResourceManager.levelPath;
                Directory.CreateDirectory(levelDestination);
                string[] levelPaths = Directory.GetFiles(levelSource, "*.xml");
                foreach (string levelPath in levelPaths)
                {
                    string scriptFileName = levelPath.Replace(".xml", ".cs");

                    File.Copy(levelPath, levelDestination + "\\" + Path.GetFileName(levelPath));
                    if (File.Exists(scriptFileName))
                        File.Copy(scriptFileName, levelDestination + "\\" + Path.GetFileName(scriptFileName));
                }

                // Global script
                string globalScriptDestination = resourceDestinationPath + "\\data\\global_script.cs";
                string globalScriptSource = EditorController.resourcesSourcePath + "\\data\\global_script.cs";
                if (File.Exists(globalScriptSource))
                    File.Copy(globalScriptSource, globalScriptDestination);
            }
            ResourceManager.rootDirectory = previousRootDirectory;
        }

        // Save material resources
        public static void saveMaterialResources(List<Material> materials, bool backup)
        {
            // Backup materials
            if (backup)
            {
                string backupFile = ResourceManager.materialPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.materialPath, backupFile);
            }

            // Save materials
            XDocument doc = new XDocument(new XElement("Materials"));
            foreach (Material material in materials)
                doc.Element("Materials").Add(material.data);
            doc.Save(ResourceManager.materialPath);

            // Reload materials
            FileStream fs = new FileStream(ResourceManager.materialPath, FileMode.Open);
            ResourceManager.loadAllMaterials(fs);
        }

        // Save blueprint resources
        public static void saveBlueprintResources(List<Blueprint> blueprints, bool backup)
        {
            // Backup blueprints
            if (backup)
            {
                string backupFile = ResourceManager.blueprintPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.blueprintPath, backupFile);
            }

            // Save blueprints
            XDocument doc = new XDocument(new XElement("Blueprints"));
            foreach (Blueprint blueprint in blueprints)
                doc.Element("Blueprints").Add(blueprint.data);
            doc.Save(ResourceManager.blueprintPath);

            // Reload blueprints
            FileStream fs = new FileStream(ResourceManager.blueprintPath, FileMode.Open);
            ResourceManager.loadAllItems(fs);
        }

        // Save circuit resources
        public static void saveCircuitResources(List<Circuit> circuits, bool backup)
        {
            // Backup circuits
            if (backup)
            {
                string backupFile = ResourceManager.circuitPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.circuitPath, backupFile);
            }

            // Save circuits
            XDocument doc = new XDocument(new XElement("Circuits"));
            foreach (Circuit circuit in circuits)
                doc.Element("Circuits").Add(circuit.data);
            doc.Save(ResourceManager.circuitPath);

            // Reload circuits
            ResourceManager.loadAllCircuits(new FileStream(ResourceManager.circuitPath, FileMode.Open));
        }

        // Save background resources
        public static void saveBackgroundResources(XElement data, bool backup)
        {
            // Backup backgrounds
            if (backup)
            {
                string backupFile = ResourceManager.backgroundPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.backgroundPath, backupFile);
            }

            // Save backgrounds
            XDocument doc = new XDocument(data);
            doc.Save(ResourceManager.backgroundPath);

            // Reload background
            ResourceManager.loadAllBackgrounds(new FileStream(ResourceManager.backgroundPath, FileMode.Open));
        }

        // Save world map resources
        public static void saveWorldMapResources(XElement data, bool backup)
        {
            // Backup world maps
            if (backup)
            {
                string backupFile = ResourceManager.worldMapPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.worldMapPath, backupFile);
            }

            // Save world maps
            XDocument doc = new XDocument(data);
            doc.Save(ResourceManager.worldMapPath);

            // Reload world maps
            ResourceManager.loadAllWorldMaps(new FileStream(ResourceManager.worldMapPath, FileMode.Open));
        }

        // Save quest resources
        public static void saveQuestResources(XElement data, bool backup)
        {
            // Backup quests
            if (backup)
            {
                string backupFile = ResourceManager.questPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.questPath, backupFile);
            }

            // Save quests
            XDocument doc = new XDocument(data);
            doc.Save(ResourceManager.questPath);

            // Reload quests
            ResourceManager.loadAllQuests(new FileStream(ResourceManager.questPath, FileMode.Open));
        }

        // Save rope material resources
        public static void saveRopeMaterialResources(XElement data, bool backup)
        {
            // Backup rope materials
            if (backup)
            {
                string backupFile = ResourceManager.ropeMaterialPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.ropeMaterialPath, backupFile);
            }

            // Save rope materials
            XDocument doc = new XDocument(data);
            doc.Save(ResourceManager.ropeMaterialPath);

            // Reload rope materials
            ResourceManager.loadAllRopeMaterials(new FileStream(ResourceManager.ropeMaterialPath, FileMode.Open));
        }
    }
}
