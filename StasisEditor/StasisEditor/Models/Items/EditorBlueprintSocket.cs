using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorBlueprintSocket
    {
        private BlueprintSocketResource _socketResource;
        private EditorBlueprintScrap _scrapA;
        private EditorBlueprintScrap _scrapB;
        private EditorBlueprintSocket _opposingSocket;

        public BlueprintSocketResource socketResource { get { return _socketResource; } set { _socketResource = value; } }
        public EditorBlueprintScrap scrapA { get { return _scrapA; } set { _scrapA = value; } }
        public EditorBlueprintScrap scrapB { get { return _scrapB; } set { _scrapB = value; } }
        public EditorBlueprintSocket opposingSocket { get { return _opposingSocket; } set { _opposingSocket = value; } }

        public EditorBlueprintSocket(EditorBlueprintScrap scrapA, EditorBlueprintScrap scrapB, BlueprintSocketResource resource)
        {
            _scrapA = scrapA;
            _scrapB = scrapB;
            _socketResource = resource;
        }

        // toXML
        public XElement toXML()
        {
            return _socketResource.toXML();
        }
    }
}
