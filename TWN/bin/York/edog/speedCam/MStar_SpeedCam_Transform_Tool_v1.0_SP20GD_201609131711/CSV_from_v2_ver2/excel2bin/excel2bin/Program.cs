using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace excel2bin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()                           ,the original  
        static void Main(string[] args)
        {
            /*
            for (int i = 0; i < args.Length; i++)
                MessageBox.Show(args[i]);            

            MessageBox.Show("parameter counts: " + args.Length.ToString());
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
                Application.Run(new Form1(args));
            else
                Application.Exit();
        }
    }
}
