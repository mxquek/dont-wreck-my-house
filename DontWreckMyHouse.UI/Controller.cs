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
            //_View.DisplayHeader("Welcome to Dont Wreck My House!");
            RunAppLoop();
            //_View.DisplayHeader("Exiting");
        }

        public void RunAppLoop()
        {
            string option = ""; //_View.GetMainMenuOption();
            bool running = true;
            while (running)
            {
                switch (option)
                {
                    case "0":
                        running = false;
                        break;
                    case "1":
                        ViewReservationsForHost();
                        break;
                    case "2":
                        MakeReservation();
                        break;
                    case "3":
                        EditReservation();
                        break;
                    case "4":
                        CancelReservation();
                        break;
                }
            }
        }

        private void CancelReservation()
        {
            throw new NotImplementedException();
        }

        private void EditReservation()
        {
            throw new NotImplementedException();
        }

        private void MakeReservation()
        {
            throw new NotImplementedException();
        }

        public void ViewReservationsForHost()
        {
            throw new NotImplementedException();
        }
    }
}
