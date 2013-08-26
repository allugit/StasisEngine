using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore.Models;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    public class DialogueManager
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private Dictionary<string, Dialogue> _dialogues;

        public DialogueManager(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _dialogues = new Dictionary<string, Dialogue>();
            createDialogues();
            loadStates();
        }

        private void createDialogues()
        {
            string dialogueUid = "";

            // Start switchvine quest dialogue
            dialogueUid = "start_quest_collect_switchvine";
            addDialogue(dialogueUid, "wake_up", () => { return true; });

            addDialogueNode(dialogueUid, "wake_up", "Wake up, [PLAYER_NAME]! We've got a lot of work to do today.");
            addDialogueNodeOption(
                dialogueUid,
                "wake_up",
                "...", 
                () => { return true; },
                () => { setCurrentDialogueNodeUid(dialogueUid, "get_switchvines"); });

            addDialogueNode(dialogueUid, "get_switchvines", "I'm running low on switchvine. I need you to run to the ravine and grab some more for me.");
            addDialogueNodeOption(
                dialogueUid,
                "get_switchvines",
                "Okay.",
                () => { return true; },
                () => 
                { 
                    DataManager.questManager.receiveQuest("collect_switchvine");
                    setCurrentDialogueNodeUid(dialogueUid, "unfinished");
                    closeDialogue();
                });

            addDialogueNode(dialogueUid, "unfinished", "Have you collected those vines yet?");
            addDialogueNodeOption(
                dialogueUid,
                "unfinished",
                "Yes, here.",
                () =>
                {
                    return DataManager.questManager.isQuestComplete("collect_switchvine");
                },
                () =>
                {
                    closeDialogue();
                });
            addDialogueNodeOption(
                dialogueUid,
                "unfinished",
                "No, not yet.",
                () =>
                {
                    return !DataManager.questManager.isQuestComplete("collect_switchvine");
                },
                () =>
                {
                    closeDialogue();
                });
        }

        private void loadStates()
        {
            XElement playerData = DataManager.playerData;

            foreach (XElement dialogueStateData in playerData.Elements("DialogueState"))
            {
                string dialogueUid = dialogueStateData.Attribute("dialogue_uid").Value;
                Dialogue dialogue = _dialogues[dialogueUid];

                dialogue.currentNodeUid = dialogueStateData.Attribute("current_node_uid").Value;
            }
        }

        public List<XElement> createData()
        {
            List<XElement> dialogueStateData = new List<XElement>();

            foreach (Dialogue dialogue in _dialogues.Values)
            {
                dialogueStateData.Add(new XElement("DialogueState", new XAttribute("dialogue_uid", dialogue.uid), new XAttribute("current_node_uid", dialogue.currentNodeUid)));
            }

            return dialogueStateData;
        }

        private void addDialogue(string dialogueUid, string initialNodeUid, Func<bool> conditionsToMeet)
        {
            _dialogues.Add(dialogueUid, new Dialogue(dialogueUid, initialNodeUid, conditionsToMeet));
        }

        private void addDialogueNode(string dialogueUid, string nodeUid, string message)
        {
            _dialogues[dialogueUid].dialogueNodes.Add(nodeUid, new DialogueNode(nodeUid, message));
        }

        private void addDialogueNodeOption(string dialogueUid, string nodeUid, string message, Func<bool> conditionsToMeet, Action action)
        {
            _dialogues[dialogueUid].dialogueNodes[nodeUid].options.Add(new DialogueOption(conditionsToMeet, message, action));
        }

        public string getText(string dialogueUid, string nodeUid)
        {
            return replaceSymbols(_dialogues[dialogueUid].dialogueNodes[nodeUid].message);
        }

        private string replaceSymbols(string text)
        {
            return text.Replace("[PLAYER_NAME]", DataManager.playerName);
        }

        private void setCurrentDialogueNodeUid(string dialogueUid, string nodeUid)
        {
            _dialogues[dialogueUid].currentNodeUid = nodeUid;
        }

        public string getCurrentDialogueNodeUid(string dialogueUid)
        {
            return _dialogues[dialogueUid].currentNodeUid;
        }

        public List<DialogueOption> getDialogueOptions(string dialogueUid, string nodeUid)
        {
            return _dialogues[dialogueUid].dialogueNodes[nodeUid].options;
        }

        private void closeDialogue()
        {
            DialogueSystem dialogueSystem = _systemManager.getSystem(SystemType.Dialogue) as DialogueSystem;
            string levelUid = LevelSystem.currentLevelUid;

            dialogueSystem.endDialogue(levelUid);
        }
    }
}
