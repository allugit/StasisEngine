using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StasisEditor.Models
{
    public abstract class Material
    {
        // copyFrom -- clones a list
        public static List<Material> copyFrom(ReadOnlyCollection<Material> list)
        {
            List<Material> copy = new List<Material>(list.Count);

            foreach (Material material in list)
                copy.Add(material.clone());

            return copy;
        }

        // clone
        public abstract Material clone();
    }
}
