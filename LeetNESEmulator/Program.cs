using System;
using System.Windows.Forms;
using Autofac;
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
                // Test code.
                var input = container.Resolve<IInputSource>();

                Application.Run(mainForm);
            }
        }

        private static IContainer ConfigureApplication(MainForm mainForm)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<InputSource>().As<IInputSource>().SingleInstance();
            builder.RegisterInstance(mainForm).As<IPresenter>();
            return builder.Build();
        }
    }
}
