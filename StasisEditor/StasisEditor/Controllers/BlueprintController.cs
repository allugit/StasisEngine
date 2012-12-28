using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private BlueprintView _view;
        private BindingList<Blueprint> _blueprints;

        public BlueprintView view { get { return _view; } }
        public BindingList<Blueprint> blueprints { get { return _blueprints; } }

        public BlueprintController(EditorController controller, BlueprintView blueprintView)
            : base()
        {
            _editorController = controller;
            _blueprints = new BindingList<Blueprint>();
            _view = blueprintView;
            _view.setController(this);

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
                foreach (XElement childData in resource.data.Elements("Socket"))
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
                _blueprints.Add(new Blueprint(resource.data, scraps, sockets));
            }
        }

        // saveBlueprints
        public void saveBlueprints()
        {
            ResourceController.saveBlueprintResources(new List<Blueprint>(_blueprints));
        }

        // checkUnsavedBlueprints
        private bool isUnsavedResourceUsed(string uid)
        {
            // Check unsaved materials
            foreach (Blueprint b in _blueprints)
            {
                if (b.uid == uid)
                    return true;
                else
                {
                    foreach (BlueprintScrap scrap in b.scraps)
                    {
                        if (scrap.uid == uid)
                            return true;
                    }
                }
            }

            return false;
        }

        // createBlueprint
        public Blueprint createBlueprint(string uid)
        {
            // Check unsaved resources
            if (isUnsavedResourceUsed(uid))
            {
                System.Windows.Forms.MessageBox.Show(String.Format("An unsaved resource with the uid [{0}] already exists.", uid), "Blueprint Error", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }

            Blueprint blueprint = new Blueprint(uid);
            _blueprints.Add(blueprint);
            return blueprint;
        }

        // createBlueprintScrap
        public BlueprintScrap createBlueprintScrap(Blueprint blueprint, string uid)
        {
            // Check unsaved resources
            if (isUnsavedResourceUsed(uid))
            {
                System.Windows.Forms.MessageBox.Show(String.Format("An unsaved resource with the uid [{0}] already exists.", uid), "Blueprint Error", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }

            BlueprintScrap scrap = new BlueprintScrap(uid);
            blueprint.scraps.Add(scrap);
            return scrap;
        }

        // removeBlueprint
        public void removeBlueprint(string uid, bool destroy = true)
        {
            Blueprint blueprintToRemove = null;
            foreach (Blueprint blueprint in _blueprints)
            {
                if (blueprint.uid == uid)
                {
                    blueprintToRemove = blueprint;
                    break;
                }
            }

            System.Diagnostics.Debug.Assert(blueprintToRemove != null);

            _blueprints.Remove(blueprintToRemove);

            try
            {
                if (destroy)
                    ResourceController.destroy(uid);
            }
            catch (ResourceNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show(String.Format("Could not destroy resource.\n{0}", e.Message), "Resource Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        // removeBlueprintScrap
        public void removeBlueprintScrap(Blueprint blueprint, string uid)
        {
            BlueprintScrap scrapToRemove = null;
            foreach (BlueprintScrap scrap in blueprint.scraps)
            {
                if (scrap.uid == uid)
                {
                    scrapToRemove = scrap;
                    break;
                }
            }

            System.Diagnostics.Debug.Assert(scrapToRemove != null);

            blueprint.scraps.Remove(scrapToRemove);
        }
    }
}
