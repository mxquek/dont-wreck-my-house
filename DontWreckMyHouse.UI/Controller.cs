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
        public void ViewReservationsForHost(Host host,DateTime startingViewDate)
        {
            if (host== null)
            {
                return;
            }

            //foreach reservation
            Result<List<Reservation>> reservations = _ReservationService.GetReservationsByHostID(host.ID);
            if (reservations.Success == false)
            {
                _View.DisplayStatus(reservations.Success, reservations.Message);
                return;
            }

            _View.DisplayHeader($"{host.LastName}: {host.City}, {host.State}");

            var targetReservations = reservations.Data.OrderBy(reservation => reservation.StartDate)
                                                    .Where(reservation => reservation.StartDate > startingViewDate);

            foreach (Reservation reservation in targetReservations)
            {
                Result<Guest> guestResult = _GuestService.FindByID(reservation.GuestID);
                _View.DisplayStatus(guestResult.Success, guestResult.Message);
                _View.DisplayReservation(reservation, guestResult.Data);
            }

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
                case SearchOption.SearchByID:
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
            DateTime endDate = _View.GetFutureDate("Start Date");
            Reservation reservation = _ReservationService.Make(host, guest, startDate, endDate);
        }
        public void EditReservation()
        {
            throw new NotImplementedException();
        }
        public void CancelReservation()
        {
            throw new NotImplementedException();
        }

        

        
    }
}
