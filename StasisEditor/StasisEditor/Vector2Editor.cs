using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using StasisEditor.Views.Controls;

namespace StasisEditor
{
    using Vector2 = Microsoft.Xna.Framework.Vector2;

    public class Vector2Editor : UITypeEditor
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
                Vector2 vector2 = (Vector2)value;
                Vector2EditorForm form = new Vector2EditorForm();
                form.value = vector2;
                form.ShowDialog();
                value = form.value;

                /*
                XNAColor color = (XNAColor)value;
                ColorDialog selectionControl = new ColorDialog();
                selectionControl.Color = Color.FromArgb(BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, color.A }, 0));
                selectionControl.ShowDialog();
                value = new XNAColor((int)selectionControl.Color.R, (int)selectionControl.Color.G, (int)selectionControl.Color.B, (int)color.A);
                */
            }

            return value;
        }
    }
}
