using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.ExternalProgram.Commands
{
    public interface ICommand
    {
        void execute();
        string getDescription();
    }
}
