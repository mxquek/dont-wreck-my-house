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
                        ViewReservationsForHost();
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
        public void ViewReservationsForHost()
        {
            SearchOption option = _View.SelectSearchOption("Host");
            Result<Host> hostResult;// = new Result<Host>();
            switch (option)
            {
                case SearchOption.Exit:
                    return;
                default:
                    hostResult = GetHost(option);
                    break;
            }
            if(hostResult.Success == false)
            {
                return;
            }

            //foreach reservation
            Result<List<Reservation>> reservations = _ReservationService.GetReservationsByHostID(hostResult.Data.ID);
            if (reservations.Success == false)
            {
                _View.DisplayStatus(reservations.Success, reservations.Message);
                return;
            }

            _View.DisplayHeader($"{hostResult.Data.City}, {hostResult.Data.State}");
            foreach (Reservation reservation in reservations.Data)
            {
                Result<Guest> guestResult = _GuestService.FindByID(reservation.GuestID);
                _View.DisplayStatus(guestResult.Success, guestResult.Message);
                _View.DisplayReservation(reservation, guestResult.Data);
            }

        }

        public Result<Host> GetHost(SearchOption option)
        {
            Result<Host> hostResult = new Result<Host>();
            switch (option)
            {
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
                case SearchOption.SearchByEmail:
                    guestResult = _GuestService.FindByEmail(_View.GetEmail("Guest"));
                    break;
                case SearchOption.PickFromList:
                    break;
                case SearchOption.SearchByID:
                    break;
            }
            _View.DisplayStatus(guestResult.Success, guestResult.Message);
            return guestResult;
        }
        public void MakeReservation()
        {
            throw new NotImplementedException();
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
