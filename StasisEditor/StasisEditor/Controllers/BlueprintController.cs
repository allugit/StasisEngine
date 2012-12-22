using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using StasisEditor.Views;
using StasisEditor.Models;
using StasisCore.Models;
using StasisCore.Controllers;
using StasisCore.Resources;

namespace StasisEditor.Controllers
{
    public class BlueprintController : Controller
    {
        private EditorController _editorController;
        private BlueprintView _blueprintView;
        private List<Blueprint> _blueprints;

        public BlueprintController(EditorController controller, BlueprintView blueprintView)
            : base()
        {
            _editorController = controller;
            _blueprintView = blueprintView;
            _blueprintView.setController(this);
            _blueprints = new List<Blueprint>();

            // Load blueprints
            List<ResourceObject> resources = ResourceController.loadItems("blueprint");
            foreach (ResourceObject resource in resources)
            {
                // Create scraps
                List<BlueprintScrap> scraps = new List<BlueprintScrap>();
                foreach (XElement childData in resource.data.Elements("Item"))
                {
                    if (childData.Attribute("type").Value == "blueprint_scrap")
                        scraps.Add(new BlueprintScrap(childData));
                }

                // Create sockets
                List<BlueprintSocket> sockets = new List<BlueprintSocket>();
                foreach (XElement childData in resource.data.Elements("Item"))
                {
                    // Find associated scraps
                    BlueprintScrap scrapA = (from scrap in scraps
                                            where scrap.uid == childData.Attribute("scrap_a_uid").Value
                                            select scrap).First();
                    BlueprintScrap scrapB = (from scrap in scraps
                                             where scrap.uid == childData.Attribute("scrap_b_uid").Value
                                             select scrap).First();
                    
                    if (childData.Attribute("type").Value == "blueprint_socket")
                        sockets.Add(new BlueprintSocket(childData, scrapA, scrapB));
                }

                // Create blueprint
                Blueprint blueprint = new Blueprint(resource.data, scraps, sockets);
            }
        }

        // createBlueprint
        public void createBlueprint(string uid)
        {
            // Check unsaved materials
            foreach (Blueprint blueprint in _blueprints)
            {
                if (blueprint.uid == uid)
                {
                    System.Windows.Forms.MessageBox.Show(String.Format("An unsaved resource material with the uid [{0}] already exists.", uid), "Material Error", System.Windows.Forms.MessageBoxButtons.OK);
                    return;
                }
            }
        }
    }
}
