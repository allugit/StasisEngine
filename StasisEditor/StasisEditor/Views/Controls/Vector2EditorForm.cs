using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StasisEditor.Views.Controls
{
    using Vector2 = Microsoft.Xna.Framework.Vector2;

    public partial class Vector2EditorForm : Form
    {
        public Vector2 value
        {
            get
            {
                Vector2 result = Vector2.Zero;
                if (!float.TryParse(x.Text, out result.X))
                    result.X = 0;
                if (!float.TryParse(y.Text, out result.Y))
                    result.Y = 0;
                return result;
            }

            set
            {
                x.Text = value.X.ToString();
                y.Text = value.Y.ToString();
            }
        }

        public Vector2EditorForm()
        {
            InitializeComponent();
        }

        private void x_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void y_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
