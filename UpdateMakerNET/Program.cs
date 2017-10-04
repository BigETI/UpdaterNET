using System;
using System.Windows.Forms;

/// <summary>
/// Update maker .NET namespace
/// </summary>
namespace UpdateMakerNET
{
    /// <summary>
    /// Program class
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
