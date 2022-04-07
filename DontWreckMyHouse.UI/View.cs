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


        //Display Methods
        public void DisplayHeader(string message)
        {
            _IO.PrintLine("");
            _IO.PrintLine(message);
            _IO.PrintLine(new string('=', message.Length));
        }
    }
}
