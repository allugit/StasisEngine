using System;
using System.Collections.Generic;
using System.Drawing;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public interface IEditorView
    {
        void setController(EditorController controller);
        IMaterialView getMaterialView();
        ITextureView getTextureView();
        ILevelView getLevelView();
        
        void enableNewLevel(bool enabled);
        void enableCloseLevel(bool enabled);
        void enableLoadLevel(bool enabled);
        void enableSaveLevel(bool enabled);
        
        void addLevelView(ILevelView levelView);
        void removeLevelView();
        
        void addLevelSettings(LevelResource level);
        void removeLevelSettings();

        void addActorToolbar(ILevelController controller);
        void removeActorToolbar();
    }
}
