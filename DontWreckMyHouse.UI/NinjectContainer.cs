using Ninject;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.BLL;
using DontWreckMyHouse.DAL;

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

            Kernel.Bind<IReservationRepository>().To<ReservationRepository>().WithConstructorArgument(reservationFileDirectory);
            Kernel.Bind<IHostRepository>().To<HostRepository>().WithConstructorArgument(hostFileDirectory);
            Kernel.Bind<IGuestRepository>().To<GuestRepository>().WithConstructorArgument(guestFileDirectory);

            Kernel.Bind<ReservationService>().To<ReservationService>();
            Kernel.Bind<HostService>().To<HostService>();
            Kernel.Bind<GuestService>().To<GuestService>();
        }
    }
}
