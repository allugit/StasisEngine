using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Views.Controls
{
    public class LayersTreeView : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            // Filter WM_LBUTTONDBLCLK
            if (m.Msg != 0x203) base.WndProc(ref m);
        }
    }
}
