using DontWreckMyHouse.BLL;
using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        ViewReservationsForHost(GetHost(GetSearchOption("Host")).Data,new DateTime());
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
        public Result<List<Reservation>> ViewReservationsForHost(Host host,DateTime startingViewDate, Guest guest = null)
        {
            Result<List<Reservation>> result = new Result<List<Reservation>>();
            if (host== null)
            {
                result.Success = false;
                result.Message = "Valid host required.";
                return result;
            }

            //foreach reservation
            result = _ReservationService.GetReservationsByHostID(host.ID);
            if (result.Success == false)
            {
                _View.DisplayStatus(result.Success, result.Message);
                return result;
            }

            _View.DisplayHeader($"{host.LastName}: {host.City}, {host.State}");

            result.Data = result.Data.OrderBy(reservation => reservation.StartDate)
                                                    .Where(reservation => reservation.StartDate >= startingViewDate).ToList();
            if(guest != null)
            {
                result.Data = result.Data.Where(reservation => reservation.GuestID == guest.ID).ToList();
            }

            if(startingViewDate > DateTime.MinValue && result.Data.Count() == 0)
            {
                result.Success = false;
                result.Message = $"{host.LastName} has no future reservations.";
                _View.DisplayMessage(result.Message);
                return result;
            }
            foreach (Reservation reservation in result.Data)
            {
                Result<Guest> guestResult = _GuestService.FindByID(reservation.GuestID);
//                _View.DisplayStatus(guestResult.Success, guestResult.Message);
                _View.DisplayReservation(reservation, guestResult.Data);
            }
            return result;
        }

        public SearchOption GetSearchOption(string person)
        {
            SearchOption option = _View.SelectSearchOption(person);
            return option;
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
        public void MakeReservation()
        {
            Host host = GetHost(GetSearchOption("Host")).Data;
            ViewReservationsForHost(host,DateTime.Now);
            Guest guest = GetGuest(GetSearchOption("Guest")).Data;
            DateTime startDate = _View.GetFutureDate("Start Date");
            DateTime endDate = _View.GetFutureDate("End Date");

            Reservation reservation = _ReservationService.Make(host, guest, startDate, endDate);
            if(_View.ReservationConfirmation(reservation))
            {
                _ReservationService.Add(reservation,host.ID);
            }
            else
            {
                return;
            }

        }
        public void EditReservation()
        {
            throw new NotImplementedException();
        }
        public void CancelReservation()
        {
            Host host = GetHost(GetSearchOption("Host")).Data;
            Guest guest = GetGuest(GetSearchOption("Guest")).Data;
            Result<List<Reservation>> reservations = ViewReservationsForHost(host, DateTime.Now, guest);
            if(reservations.Success == false) { return; }
            Result<Reservation> result = _ReservationService.Remove(_View.ChooseReservation(reservations.Data, guest).Data, host.ID);
            _View.DisplayStatus(result.Success, result.Message);
        }

        

        
    }
}
