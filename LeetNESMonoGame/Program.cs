using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using LeetNES;
using LeetNES.ALU;
using LeetNES.ALU.Instructions;

namespace LeetNESMonoGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = ConfigureApplication();

            using (var game = (MyGame)container.Resolve<IPresenter>())
            {   
                game.Run();
            }
        }

        private static IContainer ConfigureApplication()
        {
            var builder = new ContainerBuilder();
       //     builder.RegisterType<InputSource>().As<IInputSource>().SingleInstance();
            builder.RegisterType<MyGame>().As<IPresenter>().As<IIO>().SingleInstance();

            builder.RegisterType<Cpu>().As<ICpu>().SingleInstance();
            builder.RegisterType<Emulator>().As<IEmulator>().SingleInstance();
            builder.RegisterType<Memory>().As<IMemory>().SingleInstance();
            builder.RegisterType<Ppu>().As<IPpu>().SingleInstance();
            builder.RegisterType<StolenPpu>();

            //var cartridge = new Cartridge("../../../LeetNES/roms/nestest.nes");

            var instructionTypes = typeof(IInstruction).Assembly.GetTypes().Where(f => typeof(IInstruction).IsAssignableFrom(f) && !f.IsAbstract).ToArray();
            builder.RegisterTypes(instructionTypes).As<IInstruction>();


            return builder.Build();
        }
    }
}
