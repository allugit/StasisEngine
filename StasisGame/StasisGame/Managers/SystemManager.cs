using System;
using System.Diagnostics;
using StasisGame.Systems;

namespace StasisGame.Managers
{
    public class SystemManager
    {
        private SystemNode _head;

        public SystemNode head { get { return _head; } set { _head = value; } }

        public int count
        {
            get
            {
                int i = 0;
                SystemNode current = _head;
                while (current != null)
                {
                    i++;
                    current = current.next;
                }
                return i;
            }
        }

        public SystemManager()
        {
        }

        public ISystem getSystem(SystemType type)
        {
            SystemNode current = _head;
            while (current != null)
            {
                if (current.system.systemType == type)
                {
                    return current.system;
                }
                current = current.next;
            }
            return null;
        }

        public void add(ISystem system, int priority)
        {
            priority = priority == -1 ? system.defaultPriority : priority;
            Debug.Assert(priority != -1);

            SystemNode newNode = new SystemNode(system, priority);
            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                int countBefore = count;

                SystemNode current = _head;
                while (current.next != null)
                {
                    if (current.next.priority > priority)
                    {
                        break;
                    }
                    current = current.next;
                }
                current.insert(newNode);

                int countAfter = count;
                Debug.Assert(countAfter == countBefore + 1);
            }
        }

        public void remove(SystemType systemType)
        {
            remove(getSystem(systemType));
        }

        public void remove(ISystem system)
        {
            SystemNode current = _head;
            while (current != null)
            {
                if (current.system == system)
                {
                    current.remove();
                    return;
                }
                current = current.next;
            }
        }

        public void process()
        {
            SystemNode current = _head;
            while (current != null)
            {
                current.system.update();
                current = current.next;
            }
        }
    }
}
