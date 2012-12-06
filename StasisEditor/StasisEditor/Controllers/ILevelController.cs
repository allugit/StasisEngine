﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers
{
    public interface ILevelController : IController
    {
        void resizeGraphicsDevice(int width, int height);
        void createNewLevel();
        void closeLevel();
        void mouseMove(MouseEventArgs e);
        void mouseLeave();
        void mouseEnter();
        Vector2 getWorldOffset();
        Vector2 getWorldMouse();
        bool getIsMouseOverView();
        float getScale();
    }
}
