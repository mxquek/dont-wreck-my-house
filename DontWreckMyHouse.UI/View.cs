using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.UI
{
    public class View
    {
        private readonly ConsoleIO _IO;
        public View(ConsoleIO io)
        {
            _IO = io;
        }
        public MainMenuOption SelectMainMenuOption()
        {
            DisplayHeader("Main Menu");
            
            MainMenuOption[] options = Enum.GetValues<MainMenuOption>();
            int max = options.Length-1;
            for (int index = 0; index <= max; index++)
            {
                MainMenuOption option = options[index];
                _IO.PrintLine($"{index}. {option.ToLabel()}");
            }

            string message = $"Select [0 - {max}]: ";
            return options[_IO.ReadInt(message, 0, max)];
        }
        public SearchOption SelectSearchOption(string item)
        {
            DisplayHeader($"Search {item} Options");
            SearchOption[] options = Enum.GetValues<SearchOption>();
            int max = options.Length - 1;
            for (int index = 0; index <= max; index++)
            {
                SearchOption option = options[index];
                _IO.PrintLine($"{index}. {option.ToLabel()}");
            }

            string message = $"Select [0 - {max}]: ";
            return options[_IO.ReadInt(message, 0, max)];
        }

        public string GetEmail(string person)
        {
            return _IO.ReadRequiredString($"{person} Email: ");
        }
        public string GetNamePrefix(string person)
        {
            return _IO.ReadRequiredString($"{person} last name starts with: ");
        }

        public Result<Host> ChooseHost(List<Host> hosts)
        {
            Result<Host> result = new Result<Host>();
            if (hosts == null || hosts.Count == 0)
            {
                _IO.Error("No hosts found");
                return null;
            }

            int index = 1;
            foreach (Host host in hosts.Take(25))
            {
                _IO.PrintLine($"{index++:D2}: {host.LastName} {host.Email}");
            }
            index--;

            if (hosts.Count > 25)
            {
                _IO.PrintLine("More than 25 hosts found. Showing first 25. Please refine your search.");
            }
            _IO.PrintLine("0: Exit");
            string message = $"Select a host by their index [0-{index}]: ";

            index = _IO.ReadInt(message, 0, index);
            if (index <= 0)
            {
                result.Success = false;
                result.Message = "Exiting Host Selection...";
                return result;
            }
            result.Data = hosts[index - 1];
            result.Success = true;
            result.Message = $"Host {result.Data.LastName} was chosen.";
            return result;
        }

        //Display Methods
        public void DisplayHeader(string message)
        {
            _IO.PrintLine("");
            _IO.PrintLine(message);
            _IO.PrintLine(new string('=', message.Length));
        }

        public void DisplayStatus(bool success, string message)
        {
            if (success)
            {
                _IO.Success(message);
            }
            else
            {
                _IO.Error(message);
            }
        }

    }
}
