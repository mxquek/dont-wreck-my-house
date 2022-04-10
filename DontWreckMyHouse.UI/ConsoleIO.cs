
namespace DontWreckMyHouse.UI
{
    public class ConsoleIO
    {
        private const string INVALID_NUMBER
            = "[INVALID] Enter a valid number.";
        private const string NUMBER_OUT_OF_RANGE
            = "[INVALID] Enter a number between {0} and {1}.";
        private const string REQUIRED
            = "[INVALID] Value is required.";
        private const string INVALID_DATE
            = "[INVALID] Enter a date in MM/dd/yyyy format.";
        private const string INVALID_BOOL
            = "[INVALID] Please enter 'y' or 'n'.";

        private DateTime? _ReadDate(string prompt, bool nullable)
        {
            DateTime? result = null;
            while (true)
            {
                string input = ReadString(prompt);
                if (nullable == true && string.IsNullOrEmpty(input))
                {
                    return result;
                }
                if (DateTime.TryParse(input, out DateTime temp))
                {
                    result = temp;
                    return result.Value.Date;
                }
                PrintLine(INVALID_DATE);
            }
        }

        public void Print(string message)
        {
            Console.Write(message);
        }
        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            PrintLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public string ReadString(string prompt)
        {
            Print(prompt);
            return Console.ReadLine();
        }
        public string ReadRequiredString(string prompt)
        {
            while (true)
            {
                string result = ReadString(prompt);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }
                PrintLine(REQUIRED);
            }
        }

        public decimal ReadDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                if (decimal.TryParse(ReadRequiredString(prompt), out result))
                {
                    return result;
                }

                PrintLine(INVALID_NUMBER);
            }
        }
        public decimal ReadDecimal(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                decimal result = ReadDecimal(prompt);
                if (result >= min && result <= max)
                {
                    return result;
                }
                PrintLine(string.Format(NUMBER_OUT_OF_RANGE, min, max));
            }
        }

        public int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                if (int.TryParse(ReadRequiredString(prompt), out result))
                {
                    return result;
                }

                PrintLine(INVALID_NUMBER);
            }
        }
        public int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                int result = ReadInt(prompt);
                if (result >= min && result <= max)
                {
                    return result;
                }
                PrintLine(string.Format(NUMBER_OUT_OF_RANGE, min, max));
            }
        }

        public bool ReadBool(string prompt)
        {
            while (true)
            {
                string input = ReadRequiredString(prompt).ToLower();
                if (input == "y")
                {
                    return true;
                }
                else if (input == "n")
                {
                    return false;
                }
                PrintLine(INVALID_BOOL);
            }
        }

        public DateTime ReadRequiredDate(string prompt)
        {
            return (DateTime)_ReadDate(prompt, false);
        }
        public DateTime? ReadOptionalDate(string prompt)
        {
            return _ReadDate(prompt, true);
        }
    }
}
