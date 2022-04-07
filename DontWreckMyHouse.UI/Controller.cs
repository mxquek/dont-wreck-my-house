using DontWreckMyHouse.BLL;
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
            MainMenuOption option = _View.SelectMainMenuOption();
            bool running = true;
            while (running)
            {
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
            switch (option)
            {
                case SearchOption.Exit:
                    return;
                case SearchOption.SearchByEmail:
                    //_HostService.FindByEmail();
                    break;
                case SearchOption.PickFromList:
                    //HostService.FindByLastName();
                    break;
            }
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
