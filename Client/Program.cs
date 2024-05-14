using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Networking;
using Service;

namespace Client
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      // Application.Run(new Form1());
      ICompetitionServices server = new CompetitionServicesRpcProxy("127.0.0.1", 44444);
      Application.Run(new LogIn(server));
    }
  }
}
