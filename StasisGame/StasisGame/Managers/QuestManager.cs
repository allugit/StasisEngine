using System;
using System.Collections.Generic;
using StasisCore.Models;

namespace StasisGame.Managers
{
    public class QuestManager
    {
        private List<QuestDefinition> _questDefinitions;
        private Dictionary<string, QuestState> _questStates;

        public List<QuestDefinition> questDefinitions { get { return _questDefinitions; } }
        public Dictionary<string, QuestState> questStates { get { return _questStates; } set { _questStates = value; } }

        public QuestManager(List<QuestDefinition> questDefinitions)
        {
            _questDefinitions = questDefinitions;
            _questStates = new Dictionary<string, QuestState>();
        }

        public QuestDefinition getQuestDefinition(string questUid)
        {
            foreach (QuestDefinition questDefinition in _questDefinitions)
            {
                if (questDefinition.uid == questUid)
                {
                    return questDefinition;
                }
            }
            return null;
        }

        public ObjectiveDefinition getObjectiveDefinition(string questUid, string objectiveUid)
        {
            QuestDefinition questDefinition = getQuestDefinition(questUid);

            foreach (ObjectiveDefinition objectiveDefinition in questDefinition.objectiveDefinitions)
            {
                if (objectiveDefinition.uid == objectiveUid)
                {
                    return objectiveDefinition;
                }
            }
            return null;
        }

        public void addNewQuestState(string questUid)
        {
            QuestDefinition questDefinition = getQuestDefinition(questUid);
            QuestState questState = new QuestState(questDefinition);

            foreach (ObjectiveDefinition objectiveDefinition in questDefinition.objectiveDefinitions)
            {
                questState.objectiveStates.Add(new ObjectiveState(objectiveDefinition, objectiveDefinition.startingValue));
            }

            _questStates.Add(questUid, questState);
        }
    }
}
