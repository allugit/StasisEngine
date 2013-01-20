using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace StasisEditor.Views.Controls
{
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
                selectionControl.Color = Color.FromArgb(color.GetARGB());
                selectionControl.ShowDialog();
                value = new XNAColor(selectionControl.Color.ToArgb());
            }

            return value;
        }
    }
}
