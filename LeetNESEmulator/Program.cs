using System;
using System.Windows.Forms;
using Autofac;

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
                Application.Run(mainForm);
            }
        }

        private static IContainer ConfigureApplication(MainForm mainForm)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(mainForm).As<IPresenter>();
            return builder.Build();
        }
    }
}
