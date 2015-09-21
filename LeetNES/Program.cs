using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using LeetNES.ALU;
using LeetNES.ALU.Instructions;

namespace LeetNES
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<Cpu>().As<ICpu>().SingleInstance();
            containerBuilder.RegisterType<Emulator>().As<IEmulator>();
            containerBuilder.RegisterType<Memory>().As<IMemory>().SingleInstance();
            containerBuilder.RegisterType<Ppu>().As<IPpu>().SingleInstance();
            containerBuilder.RegisterType<StolenPpu>().SingleInstance();
            containerBuilder.RegisterType<Lazy<StolenPpu>>().SingleInstance();

            var instructionTypes = Assembly.GetExecutingAssembly().GetTypes().Where(f => typeof(IInstruction).IsAssignableFrom(f) && !f.IsAbstract).ToArray();

            containerBuilder.RegisterTypes(instructionTypes)
                .As<IInstruction>();

            //var cartridge = new Cartridge("../../roms/nestest.nes");
            var cartridge = new Cartridge("../../roms/Balloon Fight (JU) [!].nes");

            containerBuilder.RegisterInstance(cartridge).As<ICartridge>();

            var container = containerBuilder.Build();

            

            var emulator = container.Resolve<IEmulator>();
            var memory = container.Resolve<IMemory>();
            
            memory.SetCartridge(cartridge);
            emulator.Reset();
            for (;;)
            {
                emulator.Step();
            }
        }
    }
}
