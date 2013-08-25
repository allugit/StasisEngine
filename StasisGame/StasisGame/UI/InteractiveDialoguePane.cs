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
        private DialogueNode _currentNode;
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
            setCurrentNode(_dialogueManager.getCurrentDialogueNode(dialogueComponent.dialogueUid));
        }

        private void setCurrentNode(DialogueNode node)
        {
            float optionYOffset = 32;
            _currentNode = node;
            _selectedOptionIndex = 0;
            _optionButtons.Clear();
            foreach (DialogueOption option in node.options)
            {
                string text = Screen.wrapText(_optionFont, option.message, _maxLineWidth);
                Vector2 size = _optionFont.MeasureString(text);

                _optionButtons.Add(new TextButton(_screen, _optionFont, UIAlignment.TopLeft, x + _width - (int)_margin, y + _height - (int)optionYOffset, 0, TextAlignment.Right, option.message, 1, Color.White, Color.Gray, option.action));
                optionYOffset += size.Y;
            }
        }

        public override void update()
        {
            DialogueNode currentNode = _dialogueManager.getCurrentDialogueNode(dialogueComponent.dialogueUid);

            base.update();

            if (_currentNode != currentNode)
            {
                setCurrentNode(currentNode);
            }
        }

        public override void draw()
        {
            string message;

            base.draw();
            message = Screen.wrapText(_dialogueFont, _dialogueManager.getText(_dialogueComponent.dialogueUid, _currentNode.uid), _maxLineWidth);

            _spriteBatch.DrawString(_dialogueFont, message, new Vector2(x + _margin, y + _margin), Color.White);

            for (int i = 0; i < _optionButtons.Count; i++)
            {
                _optionButtons[i].draw();
            }
        }
    }
}
