using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore.Models;

namespace StasisGame.UI
{
    public class InteractiveDialoguePane : BluePane
    {
        private DialogueManager _dialogueManager;
        private SpriteFont _dialogueFont;
        private SpriteFont _optionFont;
        private CharacterDialogueComponent _dialogueComponent;
        private string _currentNodeUid;
        private List<TextButton> _optionButtons;
        private int _selectedOptionIndex;
        private float _margin = 16f;
        private float _optionMargin = 8f;
        private float _maxLineWidth;

        public CharacterDialogueComponent dialogueComponent { get { return _dialogueComponent; } }

        public InteractiveDialoguePane(Screen screen, UIAlignment alignment, int x, int y, int width, int height, SpriteFont dialogueFont, SpriteFont optionFont, CharacterDialogueComponent dialogueComponent)
            : base(screen, alignment, x, y, width, height)
        {
            _dialogueComponent = dialogueComponent;
            _dialogueFont = dialogueFont;
            _optionFont = optionFont;
            _dialogueManager = DataManager.dialogueManager;
            _maxLineWidth = (float)_width - _margin * 2f;
            _optionButtons = new List<TextButton>();
            setCurrentNodeUid(_dialogueManager.getCurrentDialogueNodeUid(dialogueComponent.dialogueUid));
        }

        private void setCurrentNodeUid(string nodeUid)
        {
            float optionYOffset = 32;
            _currentNodeUid = nodeUid;
            _selectedOptionIndex = 0;
            _optionButtons.Clear();
            foreach (DialogueOption option in _dialogueManager.getDialogueOptions(dialogueComponent.dialogueUid, _currentNodeUid))
            {
                if (option.conditionsToMeet())
                {
                    string text = Screen.wrapText(_optionFont, option.message, _maxLineWidth);
                    Vector2 size = _optionFont.MeasureString(text);

                    _optionButtons.Add(new TextButton(_screen, _optionFont, UIAlignment.TopLeft, x + _width - (int)_margin, y + _height - (int)optionYOffset, 0, TextAlignment.Right, option.message, 1, Color.Yellow, Color.Gray, option.action));
                    optionYOffset += size.Y;
                }
            }
            _optionButtons[0].select();
        }

        public override void update()
        {
            string currentNodeUid = _dialogueManager.getCurrentDialogueNodeUid(dialogueComponent.dialogueUid);
            Vector2 mousePosition = new Vector2(_screen.newMouseState.X, _screen.newMouseState.Y);

            base.update();

            // Update current node
            if (_currentNodeUid != currentNodeUid)
            {
                setCurrentNodeUid(currentNodeUid);
            }

            // Handle button input
            for (int i = 0; i < _optionButtons.Count; i++)
            {
                if (_optionButtons[i].hitTest(mousePosition))
                {
                    if (_selectedOptionIndex != i)
                    {
                        _selectedOptionIndex = i;
                        _optionButtons[i].select();
                    }

                    if (_screen.newMouseState.LeftButton == ButtonState.Pressed && _screen.oldMouseState.LeftButton == ButtonState.Released)
                    {
                        _optionButtons[_selectedOptionIndex].activate();
                    }
                }
            }
        }

        public override void draw()
        {
            string message;

            base.draw();
            message = Screen.wrapText(_dialogueFont, _dialogueManager.getText(_dialogueComponent.dialogueUid, _currentNodeUid), _maxLineWidth);

            _spriteBatch.DrawString(_dialogueFont, message, new Vector2(x + _margin, y + _margin), Color.White);

            for (int i = 0; i < _optionButtons.Count; i++)
            {
                _optionButtons[i].draw();
            }
        }
    }
}
