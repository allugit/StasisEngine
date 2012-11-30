using System;
using System.Collections.Generic;
using System.Drawing;
using StasisEditor.Controller;

namespace StasisEditor.View
{
    public interface IEditorView
    {
        void setController(EditorController controller);
        void enableNewLevel(bool enabled);
        Point getLevelContainerSize();
        void addLevelView(ILevelView levelView);
        void removeLevelView(ILevelView levelView);
    }
}
