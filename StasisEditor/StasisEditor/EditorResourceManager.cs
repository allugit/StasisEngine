using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor
{
    public class EditorResourceManager
    {
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
            ResourceManager.loadAllMaterials();
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
            XDocument doc = new XDocument(new XElement("Items"));
            foreach (Blueprint blueprint in blueprints)
                doc.Element("Items").Add(blueprint.data);
            doc.Save(ResourceManager.blueprintPath);

            // Reload blueprints
            ResourceManager.loadAllItems();
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
            ResourceManager.loadAllCircuits();
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
            ResourceManager.loadAllBackgrounds();
        }
    }
}
