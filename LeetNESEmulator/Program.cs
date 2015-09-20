using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using LeetNES;
using LeetNES.ALU;
using LeetNES.ALU.Instructions;
using LeetNESEmulator.Input;

namespace LeetNESEmulator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            using (var container = ConfigureApplication(mainForm))
            {
                Task.Factory.StartNew(() =>
                {
                    // ReSharper disable AccessToDisposedClosure
                    var emulator = container.Resolve<IEmulator>();
                    var memory = container.Resolve<IMemory>();
                    // ReSharper restore AccessToDisposedClosure
                    memory.SetCartridge(new Cartridge("../../../LeetNES/roms/nestest.nes"));
                    emulator.Reset();
                    for (;;)
                    {
                        emulator.Step();
                    }    
                }, TaskCreationOptions.LongRunning);
                Application.Run(mainForm);
            }
        }

        private static IContainer ConfigureApplication(MainForm mainForm)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<InputSource>().As<IInputSource>().SingleInstance();
            builder.RegisterInstance(mainForm).As<IPresenter>();

            builder.RegisterType<Cpu>().As<ICpu>().SingleInstance();
            builder.RegisterType<Emulator>().As<IEmulator>();
            builder.RegisterType<Memory>().As<IMemory>().SingleInstance();
            builder.RegisterType<Ppu>().As<IPpu>();
            builder.RegisterType<StolenPpu>();

            var instructionTypes = Assembly.GetExecutingAssembly().GetTypes().Where(f => typeof(IInstruction).IsAssignableFrom(f) && !f.IsAbstract).ToArray();
            builder.RegisterTypes(instructionTypes).As<IInstruction>();
                

            return builder.Build();
        }
    }
}
