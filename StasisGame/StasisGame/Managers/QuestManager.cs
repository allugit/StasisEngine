using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Managers
{
    public class QuestManager
    {
        private Dictionary<string, Quest> _quests;

        public QuestManager()
        {
            XElement playerData = DataManager.playerData;
            List<XElement> allQuestData = ResourceManager.questResources;

            _quests = new Dictionary<string, Quest>();

            // Load quest definitions
            foreach (XElement questData in allQuestData)
            {
                string questUid = questData.Attribute("uid").Value;
                Quest quest = new Quest(questUid, questData.Attribute("title").Value, questData.Attribute("description").Value);

                _quests.Add(questUid, quest);

                foreach (XElement objectiveData in questData.Elements("Objective"))
                {
                    string objectiveUid = objectiveData.Attribute("uid").Value;
                    Objective objective = new Objective(
                        objectiveUid,
                        objectiveData.Attribute("label").Value,
                        Loader.loadInt(objectiveData.Attribute("starting_value"), 0),
                        Loader.loadInt(objectiveData.Attribute("end_value"), 1),
                        Loader.loadBool(objectiveData.Attribute("optional"), false));

                    quest.objectives.Add(objectiveUid, objective);
                }
            }

            // Load quest states from player data
            foreach (XElement questStateData in playerData.Elements("QuestState"))
            {
                string questUid = questStateData.Attribute("quest_uid").Value;
                Quest quest = _quests[questUid];

                quest.received = bool.Parse(questStateData.Attribute("received").Value);

                foreach (XElement objectiveStateData in questStateData.Elements("ObjectiveData"))
                {
                    string objectiveUid = objectiveStateData.Attribute("objective_uid").Value;
                    Objective objective = quest.objectives[objectiveUid];

                    objective.currentValue = int.Parse(objectiveStateData.Attribute("current_value").Value);
                }
            }
        }

        // Create data
        public List<XElement> createData()
        {
            List<XElement> data = new List<XElement>();

            foreach (Quest quest in _quests.Values)
            {
                XElement questData = new XElement(
                    "QuestState",
                    new XAttribute("quest_uid", quest.uid),
                    new XAttribute("received", quest.received));

                foreach (Objective objective in quest.objectives.Values)
                {
                    questData.Add(
                        new XElement(
                            "ObjectiveState",
                            new XAttribute("objective_uid", objective.uid),
                            new XAttribute("current_value", objective.currentValue)));
                }
            }

            return data;
        }

        public bool isQuestComplete(string questUid)
        {
            Quest quest = _quests[questUid];
            bool complete = true;

            foreach (Objective objective in quest.objectives.Values)
            {
                if (objective.currentValue != objective.endValue && !objective.optional)
                {
                    complete = false;
                    break;
                }
            }

            return complete;
        }

        public void receiveQuest(string questUid)
        {
            _quests[questUid].received = true;
        }
    }
}
