using System;
using System.Collections.Generic;
using System.Drawing;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public interface IEditorView
    {
        void setController(EditorController controller);
        void enableNewLevel(bool enabled);
        void addLevelView(ILevelView levelView);
        void removeLevelView();
        void addLevelSettings(Level level);
        void removeLevelSettings();
    }
}
