using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StasisEditor.Views.Controls
{
    public class FileSafeTextBox : TextBox
    {
        public bool IsValid { get { return Text.Length > 0; } }

        public FileSafeTextBox()
            : base()
        {
            KeyDown += new KeyEventHandler(FileSafeTextBox_KeyDown);
            CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
        }

        // Validate input
        void FileSafeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Insert || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Tab)
                return;

            if (e.Shift && (e.KeyCode == Keys.Home || e.KeyCode == Keys.End))
                return;

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                return;

            if ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z) ||
                (!e.Shift && (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)) ||
                (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
                (e.KeyCode == Keys.OemMinus && e.Shift))
            {
                string character = e.KeyCode.ToString().ToLower();
            }
            else
                e.SuppressKeyPress = true;
        }
    }
}
