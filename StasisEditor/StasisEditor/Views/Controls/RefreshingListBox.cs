using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Views.Controls
{
    public class RefreshingListBox : ListBox
    {
        public RefreshingListBox()
            : base()
        {
        }

        public new void RefreshItems()
        {
            base.RefreshItems();
        }
    }
}
