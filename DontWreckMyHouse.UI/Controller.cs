using DontWreckMyHouse.BLL;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.UI
{
    public class Controller
    {
        private readonly ReservationService _ReservationService;
        private readonly HostService _HostService;
        private readonly GuestService _GuestService;
        private readonly View _View;

        public Controller(ReservationService reservationService, HostService hostService, GuestService guestService, View view)
        {
            _ReservationService = reservationService;
            _HostService = hostService;
            _GuestService = guestService;
            _View = view;
        }

        //Running Application
        public void Run()
        {
            _View.DisplayHeader("Welcome to Dont Wreck My House!");
            RunAppLoop();
            _View.DisplayHeader("Exiting");
        }
        public void RunAppLoop()
        {
            MainMenuOption option;
            bool running = true;
            while (running)
            {
                option = _View.SelectMainMenuOption();
                switch (option)
                {
                    case MainMenuOption.Exit:
                        running = false;
                        break;
                    case MainMenuOption.ViewReservationsForHost:
                        ViewReservationsForHost(GetHost(GetSearchOption("Host")).Data);
                        break;
                    case MainMenuOption.MakeReservation:
                        MakeReservation();
                        break;
                    case MainMenuOption.EditReservation:
                        EditReservation();
                        break;
                    case MainMenuOption.CancelReservation:
                        CancelReservation();
                        break;
                }
            }
        }

        //Main Menu Options
        public void ViewReservationsForHost(Host host, DateTime startingViewDate = new DateTime(), Guest guest = null, Result < List<Reservation>> result = null)
        {
            if(result == null)
            {
                result = new Result<List<Reservation>>();
                result.Data = new List<Reservation>();
            }
            if(host == null)
            {
                result.Success = false;
                result.Message = "No Valid Host";
                return;
            }

            _ReservationService.GetReservationsByHostID(result,host.ID);
            if(result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
                return;
            }

            DisplayHostReservations(host, result, startingViewDate, guest);
            if (result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
                return;
            }

            return;
        }
        public void MakeReservation()
        {
            Host host = PromptForHost();
            if(host == null) { return; }

            DateTime future = DateTime.Now.AddDays(1);
            ViewReservationsForHost(host,future);

            Guest guest = PromptForGuest();
            if (guest == null) { return; }

            DateTime startDate = _View.GetFutureDate("Start Date");
            DateTime endDate = _View.GetFutureDate("End Date");

            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation();

            result.Data.ID = _ReservationService.GetNextReservationID(host.ID);
            result.Data.StartDate = startDate;
            result.Data.EndDate = endDate;
            result.Data.GuestID = guest.ID;
            result.Data.Total = _ReservationService.CalculateTotal(host, startDate, endDate);
            
            _ReservationService.ValidateReservation(result, host);
            if(result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
                return;
            }

            if(_View.ReservationConfirmation(result.Data))
            {
                _ReservationService.Add(result, host);
            }
            else
            {
                result.Success = false;
                result.Message = "Reservation abandoned. Returning to Main Menu...";
            }

            _View.DisplayStatus(result.Success, result.Message);
        }
        public void EditReservation()
        {
            Host host = PromptForHost();
            if (host == null) {return;}
            Guest guest = PromptForGuest();
            if (guest == null) {return;}

            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            DateTime future = DateTime.Now.AddDays(1);

            ViewReservationsForHost(host, future, guest, reservations);
            if (reservations.Success == false) {return;}

            Result<Reservation> oldReservation = new Result<Reservation>();
            oldReservation.Data = new Reservation();
            _View.ChooseReservation(reservations.Data,oldReservation);
            if (oldReservation.Success == false)
            {
                _View.DisplayStatus(oldReservation.Success, oldReservation.Message);
                return;
            }

            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(oldReservation.Data);

            DateTime? newStartDate = _View.GetOptionalFutureDate($"Start ({oldReservation.Data.StartDate:MM/dd/yyyy}) ");
            DateTime? newEndDate = _View.GetOptionalFutureDate($"End ({oldReservation.Data.EndDate:MM/dd/yyyy}) ");
            if (newStartDate != null)
            {
                result.Data.StartDate = (DateTime)newStartDate;
            }
            if(newEndDate != null)
            {
                result.Data.EndDate = (DateTime)newEndDate;
            }

            _ReservationService.ValidateReservation(result, host);
            if (result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
                return;
            }

            if (_View.ReservationConfirmation(result.Data))
            {
                _ReservationService.Edit(result, host);
            }
            else
            {
                result.Success = false;
                result.Message = "Changes abandoned. Returning to Main Menu...";
            }
            _View.DisplayStatus(result.Success, result.Message);
        }
        public void CancelReservation()
        {
            Host host = PromptForHost();
            if (host == null) { return; }
            Guest guest = PromptForGuest();
            if (guest == null) { return; }

            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            DateTime future = DateTime.Now.AddDays(1);

            ViewReservationsForHost(host, future, guest, reservations);
            if (reservations.Success == false) {return;}

            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation();
            _View.ChooseReservation(reservations.Data, result);
            if(result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
            }

            _ReservationService.Remove(result, host);
            _View.DisplayStatus(result.Success, result.Message);
        }

        //Get Component Methods
        public SearchOption GetSearchOption(string person)
        {
            SearchOption option = _View.SelectSearchOption(person);
            return option;
        }

        public Host PromptForHost()
        {
            Host host = new Host();
            SearchOption option;
            while (true)
            {
                option = GetSearchOption("Host");
                if (option == SearchOption.Exit) 
                {
                    _View.DisplayStatus(false,"\nRETURNING TO MAIN MENU...");
                    break;
                }

                host = GetHost(option).Data;
                if (host != null) { break; }
            }
            return host;
        }
        public Result<Host> GetHost(SearchOption option)
        {
            Result<Host> hostResult = new Result<Host>();

            switch (option)
            {
                case SearchOption.Exit:
                    hostResult.Success = false;
                    hostResult.Message = "Exiting Select Host Menu...";
                    break;
                case SearchOption.SearchByEmail:
                    hostResult = _HostService.FindByEmail(_View.GetEmail("Host"));
                    break;
                case SearchOption.PickFromList:
                    Result<List<Host>> hosts = _HostService.FindByLastName(_View.GetNamePrefix("Host"));
                    hostResult = _View.ChooseHost(hosts.Data);
                    break;
            }

            _View.DisplayStatus(hostResult.Success, hostResult.Message);

            return hostResult;
        }


        public Guest PromptForGuest()
        {
            Guest guest = new Guest();
            SearchOption option;
            while (true)
            {
                option = GetSearchOption("Guest");
                if (option == SearchOption.Exit)
                {
                    _View.DisplayStatus(false, "\nRETURNING TO MAIN MENU...");
                    break;
                }

                guest = GetGuest(option).Data;
                if (guest != null) { break; }
            }
            return guest;
        }
        public Result<Guest> GetGuest(SearchOption option)
        {
            Result<Guest> guestResult = new Result<Guest>();

            switch (option)
            {
                case SearchOption.Exit:
                    guestResult.Success = false;
                    guestResult.Message = "Exiting Select Guest Menu...";
                    break;
                case SearchOption.SearchByEmail:
                    guestResult = _GuestService.FindByEmail(_View.GetEmail("Guest"));
                    break;
                case SearchOption.PickFromList:
                    Result<List<Guest>> guests = _GuestService.FindByLastName(_View.GetNamePrefix("Guest"));
                    guestResult = _View.ChooseGuest(guests.Data);
                    break;
            }

            _View.DisplayStatus(guestResult.Success, guestResult.Message);
            return guestResult;
        }

        //Helper Methods
        public void DisplayHostReservations(Host host, Result<List<Reservation>> result, DateTime startingViewDate = new DateTime(), Guest guest = null)
        {
            if (host == null)
            {
                result.Success = false;
                result.Message = "Valid host required.";
                return;
            }

            _View.DisplayHeader($"{host.LastName}: {host.City}, {host.State}");
            result.Data = result.Data.OrderBy(reservation => reservation.StartDate)
                                                    .Where(reservation => reservation.StartDate >= startingViewDate).ToList();
            if (guest != null)
            {
                result.Data = result.Data.Where(reservation => reservation.GuestID == guest.ID).ToList();
            }

            if (startingViewDate > DateTime.MinValue && result.Data.Count() == 0)
            {
                result.Success = false;
                result.Message = $"{host.LastName} has no reservations on or after {startingViewDate:MM/dd/yyyy}.";
                return;
            }
            
            foreach (Reservation reservation in result.Data)
            {
                Result<Guest> guestResult = _GuestService.FindByID(reservation.GuestID);
                _View.DisplayReservation(reservation, guestResult.Data);
            }
            result.Success = true;
        }
    }
}
