using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanoramaRename {
    static class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _form1 = new Form1();
            Application.Run(_form1);
        }

        // インスタンス化
        static public Form1 _form1 = null;
        static public Form1 GetInstance() { return _form1; }
    }
}
