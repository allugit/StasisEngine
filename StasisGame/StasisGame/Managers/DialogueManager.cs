using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Managers
{
    public class DialogueManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private List<DialogueDefinition> _dialogueDefinitions;
        private Dictionary<string, DialogueState> _dialogueStates;

        public Dictionary<string, DialogueState> dialogueStates { get { return _dialogueStates; } set { _dialogueStates = value; } }

        public DialogueManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _dialogueDefinitions = new List<DialogueDefinition>();
            createDialogueDefinitions();
        }

        private List<DialogueDefinition> createDialogueDefinitions()
        {
            List<DialogueDefinition> definitions = new List<DialogueDefinition>();
            string dialogueUid = "";

            // Start switchvine quest dialogue
            dialogueUid = "start_quest_collect_switchvine";
            addDialogueDefinition(dialogueUid, () => { return true; });

            addDialogueNode(dialogueUid, "wake_up", string.Format("Wake up, {0}!", DataManager.playerName));
            addDialogueNodeOption(
                dialogueUid,
                "wake_up",
                "...", 
                () => { return true; },
                () => { DataManager.setCustomString(dialogueUid + "_current_dialogue_node", "get_switchvines"); });

            addDialogueNode(dialogueUid, "get_switchvines", "I'm running low on switchvine. I need you to run to the ravine and grab some more for me.");
            addDialogueNodeOption(dialogueUid, "get_switchvines", "Okay.", () => { return true; }, () => { DataManager.questManager.addNewQuestState("helping_dagny_1"); });

            return definitions;
        }

        public DialogueDefinition getDialogueDefinition(string dialogueUid)
        {
            foreach (DialogueDefinition definition in _dialogueDefinitions)
            {
                if (definition.uid == dialogueUid)
                {
                    return definition;
                }
            }
            return null;
        }

        public DialogueNode getDialogueNode(string dialogueUid, string nodeUid)
        {
            DialogueDefinition definition = getDialogueDefinition(dialogueUid);

            foreach (DialogueNode node in definition.dialogueNodes)
            {
                if (node.uid == nodeUid)
                {
                    return node;
                }
            }
            return null;
        }

        private void addDialogueDefinition(string dialogueUid, Func<bool> conditionsToMeet)
        {
            _dialogueDefinitions.Add(new DialogueDefinition(dialogueUid, conditionsToMeet));
        }

        private void addDialogueNode(string dialogueUid, string nodeUid, string message)
        {
            DialogueDefinition dialogueDefinition = getDialogueDefinition(dialogueUid);

            dialogueDefinition.dialogueNodes.Add(new DialogueNode(dialogueDefinition, nodeUid, message));
        }

        private void addDialogueNodeOption(string dialogueUid, string nodeUid, string message, Func<bool> conditionsToMeet, Action action)
        {
            DialogueNode node = getDialogueNode(dialogueUid, nodeUid);

            node.options.Add(new DialogueOption(node, conditionsToMeet, message, action));
        }
    }
}
