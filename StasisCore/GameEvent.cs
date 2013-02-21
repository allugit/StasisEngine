using System;

namespace StasisCore
{
    public enum GameEventType
    {
        None,
        OnLowerLimitReached,
        EnableMotor,
        DisableMotor,
        ReverseMotor
    };

    public class GameEvent
    {
        private GameEventType _type;
        private int _originEntityId;

        public GameEventType type { get { return _type; } }
        public int originEntityId { get { return _originEntityId; } }

        public GameEvent(GameEventType type, int originEntityId)
        {
            _type = type;
            _originEntityId = originEntityId;
        }
    }
}