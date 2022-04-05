using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.IO;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.BLL;

namespace DontWreckMyHouse.UI
{
    internal class NinjectContainer
    {
        public static StandardKernel Kernel { get; private set; }

        public static void Configure()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<ConsoleIO>().To<ConsoleIO>();
            Kernel.Bind<View>().To<View>();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string reservationFileDirectory = Path.Combine(projectDirectory, "data", "reservations");
            string hostFileDirectory = Path.Combine(projectDirectory, "data", "hosts.csv");
            string guestFileDirectory = Path.Combine(projectDirectory, "data", "guests.csv");

            Kernel.Bind<IReservationRepository>().To<IReservationRepository>().WithConstructorArgument(reservationFileDirectory);
            Kernel.Bind<IHostRepository>().To<IHostRepository>().WithConstructorArgument(hostFileDirectory);
            Kernel.Bind<IGuestRepository>().To<IGuestRepository>().WithConstructorArgument(guestFileDirectory);

            Kernel.Bind<ReservationService>().To<ReservationService>();
            Kernel.Bind<HostService>().To<HostService>();
            Kernel.Bind<GuestService>().To<GuestService>();
        }
    }
}
