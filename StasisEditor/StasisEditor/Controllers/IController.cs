using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StasisEditor.Controllers
{
    public interface IController
    {
        void setChangesMade(bool status);
        bool getChangesMade();
    }
}
