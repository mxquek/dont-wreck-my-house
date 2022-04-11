using DontWreckMyHouse.Core.Models;

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
        public DateTime GetFutureDate(string message)
        {
            DateTime result = new DateTime();
            while (true)
            {
                result = _IO.ReadRequiredDate(message + " [MM/dd/yyyy]: ");
                if (result < DateTime.Now)
                {
                    _IO.Error("Date must be in the future.");
                }
                else { break; }
            }
            return result;
        }
        public DateTime? GetOptionalFutureDate(string message)
        {
            DateTime? result = new DateTime();
            while (true)
            {
                result = _IO.ReadOptionalDate(message + " [MM/dd/yyyy]: ");
                if(result == null)
                {
                    break;
                }
                else if (result < DateTime.Now)
                {
                    _IO.Error("Date must be in the future.");
                }
                else { break; }
            }
            return result;
        }

        public Result<Host> ChooseHost(List<Host> hosts)
        {
            DisplayHeader("Choose Host");
            Result<Host> result = new Result<Host>();
            if (hosts == null || hosts.Count == 0)
            {
                result.Success = false;
                result.Message = "No hosts found";
                return result;
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
                result.Message = "\nExiting Host Selection...";
                return result;
            }
            result.Data = hosts[index - 1];
            result.Success = true;
            result.Message = $"Host {result.Data.LastName} was chosen.";
            return result;
        }
        public Result<Guest> ChooseGuest(List<Guest> guests)
        {
            DisplayHeader("Choose Guest");
            Result<Guest> result = new Result<Guest>();

            if(guests.Count == 0 || guests == null)
            {
                result.Success = false;
                result.Message = "No guests found.";
                return result;
            }

            int index = 1;
            foreach(Guest guest in guests.Take(25))
            {
                _IO.PrintLine($"{index++:D2}: {guest.FirstName} {guest.LastName} {guest.Email}");
            }
            index--;

            if(guests.Count > 25)
            {
                _IO.PrintLine("More than 25 guests found. Showing first 25. Please refine your search.");
            }
            _IO.PrintLine("0. Exit");
            string message = $"Select a guest by their index [0-{index}]: ";

            index = _IO.ReadInt(message, 0, index);

            if(index == 0)
            {
                result.Success = false;
                result.Message = "\nExiting Guest Selection...";
                return result;
            }
            result.Data = guests[index - 1];
            result.Success = true;
            result.Message = $"Guest {result.Data.LastName} was chosen.";
            return result;
        }
        public void ChooseReservation(List<Reservation> reservations, Result<Reservation> result)
        {
            result.Success = true;

            if (reservations == null || reservations.Count == 0)
            {
                result.Success = false;
                result.Message = "No reservation found";
                return;
            }

            while (true)
            {
                _IO.PrintLine("0. Exit");
                int reservationID = _IO.ReadInt("Select a reservation by its ID: ");
                if (reservationID == 0)
                {
                    result.Success = false;
                    result.Message = "Exiting Reservation Selection...";
                    break;
                }
                result.Data = reservations.Where(reservation => reservation.ID == reservationID).FirstOrDefault();
                if (result.Data == null)
                {
                    _IO.Error("Invalid Reservation ID");
                }
                else
                {
                    result.Success = true;
                    break;
                }
            }
            return;
        }

        public bool ReservationConfirmation(Reservation reservation)
        {
            DisplayHeader("Summary");
            _IO.PrintLine($"Start: {reservation.StartDate:MM/dd/yyyy}");
            _IO.PrintLine($"End: {reservation.EndDate:MM/dd/yyyy}");
            _IO.PrintLine($"Total: {reservation.Total:C}");
            return _IO.ReadBool("Is this okay? [y/n]: ");
        }

        //Display Methods
        public void DisplayHeader(string message)
        {
            _IO.PrintLine("");
            _IO.PrintLine(message);
            _IO.PrintLine(new string('=', message.Length));
        }
        public void DisplayMessage(string message)
        {
            _IO.PrintLine(message);
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
        public void DisplayReservation(Reservation reservation, Guest guest)
        {
            _IO.PrintLine($"ID: {reservation.ID:D2}, {reservation.StartDate:MM/dd/yyyy} - {reservation.EndDate:MM/dd/yyyy}," +
                            $"Guest: {guest.LastName}, {guest.FirstName}, Email: {guest.Email}");
        }
    }
}
