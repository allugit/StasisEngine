using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StasisEditor.Models;

namespace StasisEditor.Views.Controls
{
    public class HackyListBox : ListBox
    {
        public HackyListBox()
            : base()
        {
        }

        public void setItems(IEnumerable<object> items)
        {
            Items.Clear();
            foreach (object item in items)
                Items.Add(item);
        }
    }
}
