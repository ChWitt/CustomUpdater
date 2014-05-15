using System;
using System.Windows.Forms;

namespace CustomUpdater
{
  internal static class Updater
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The arguments. (Arugment 1: SettingsXMLPath, Argument 2: ProcessID)</param>
    [STAThread]
    private static void Main(string[] args)
    {
      if (args.Length != 2)
      {
        System.Console.WriteLine("Fehler bei den übergebenden Daten.");
      }

      string settingsXMLPath = args[0];
      int processID = Convert.ToInt32(args[1]);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new CustomerUpdater.UI.MainForm(settingsXMLPath, processID));
    }
  }
}