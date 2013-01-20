using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace StasisEditor
{
    using XNAColor = Microsoft.Xna.Framework.Color;

    public class XNAColorEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null)
            {
                XNAColor color = (XNAColor)value;
                ColorDialog selectionControl = new ColorDialog();
                selectionControl.Color = Color.FromArgb(BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, color.A }, 0));
                selectionControl.ShowDialog();
                value = new XNAColor((int)selectionControl.Color.R, (int)selectionControl.Color.G, (int)selectionControl.Color.B);
            }

            return value;
        }
    }
}
