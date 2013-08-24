using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Managers
{
    public class DialogueManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private List<Dialogue> _dialogueDefinitions;

        public DialogueManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _dialogueDefinitions = createDialogueDefinitions();
        }

        private List<Dialogue> createDialogueDefinitions()
        {
            List<Dialogue> definitions = new List<Dialogue>();
            string dialogueUid = "";

            // Start switchvine quest dialogue
            dialogueUid = "quest_start_switchvine";
            addDialogueDefinition(dialogueUid, "wake_up", () => { return true; });

            addDialogueNode(dialogueUid, "wake_up", string.Format("Wake up, {0}!", DataManager.playerName));
            addDialogueNodeOption(
                dialogueUid,
                "wake_up",
                "...", 
                () => { return true; },
                () => { DataManager.set });

            addDialogueNode(dialogueUid, "get_switchvines", "I'm running low on switchvine. I need you to run to the ravine and grab some more for me.");
            addDialogueNodeOption(dialogueUid, "get_switchvines", "Okay.", () => { return true; }, () => { DataManager.questManager.addNewQuestState("helping_dagny_1"); });

            return definitions;
        }

        public Dialogue getDialogueDefinition(string dialogueUid)
        {
            foreach (Dialogue definition in _dialogueDefinitions)
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
            Dialogue definition = getDialogueDefinition(dialogueUid);

            foreach (DialogueNode node in definition.dialogueNodes)
            {
                if (node.uid == nodeUid)
                {
                    return node;
                }
            }
            return null;
        }

        private void addDialogueDefinition(string dialogueUid, string headNodeUid, Func<bool> conditionsToMeet)
        {
            _dialogueDefinitions.Add(new Dialogue(dialogueUid, headNodeUid, conditionsToMeet));
        }

        private void addDialogueNode(string dialogueUid, string nodeUid, string message)
        {
            Dialogue dialogueDefinition = getDialogueDefinition(dialogueUid);

            dialogueDefinition.dialogueNodes.Add(new DialogueNode(dialogueDefinition, nodeUid, message));
        }

        private void addDialogueNodeOption(string dialogueUid, string nodeUid, string message, Func<bool> conditionsToMeet, Action action)
        {
            DialogueNode node = getDialogueNode(dialogueUid, nodeUid);

            node.options.Add(new DialogueOption(node, conditionsToMeet, message, action));
        }
    }
}
