using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.ExternalProgram.Commands
{
    public class Invoker
    {
        ICommand command;
        ICommand[] commands;
        string description;
        public Invoker(ICommand command)
        {
            this.command = command;
        }
        public Invoker(ICommand[] commands)
        {
            this.commands = commands;
        }
        public void Action()
        {
            if (commands == null)
            {
                command.execute();
                description = command.getDescription();
            }
            else
            {
                for(int i = 0; i < commands.Count(); i++)
                {
                    commands[i].execute();
                    description += commands[i].getDescription();
                    description += "&";
                }
            }
        }
        public string getDescription()
        {
            return description;
        }

    }
}