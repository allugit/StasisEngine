using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisEditor
{
    public class Input
    {
        public static KeyboardState newKey;
        public static KeyboardState oldKey;
        public static MouseState newMouse;
        public static MouseState oldMouse;
        public static int newScrollValue;
        public static int oldScrollValue;

        // update
        public static void update()
        {
            // Store input
            oldKey = newKey;
            oldMouse = newMouse;
            oldScrollValue = newScrollValue;

            // Update input
            newKey = Keyboard.GetState();
            newMouse = Mouse.GetState();
            newScrollValue = newMouse.ScrollWheelValue;
        }
    }
}
