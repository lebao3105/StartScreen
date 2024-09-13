using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    public class Program
    {
        [System.STAThreadAttribute()]
        public static void Main()
        {
            using (new StartScreenUWP.App())
            {
                var app = new StartScreen.App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}
