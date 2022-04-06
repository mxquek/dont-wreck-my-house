// See https://aka.ms/new-console-template for more information

using DontWreckMyHouse.UI;
using Ninject;

NinjectContainer.Configure();
Controller controller = NinjectContainer.Kernel.Get<Controller>();
controller.Run();

