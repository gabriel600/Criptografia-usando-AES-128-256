using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encrypt_Decrypt
{
    internal static class Program
    {
        private static Mutex _mutex;

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string appName = "MeuAplicativoUnico"; // Nome único para identificar o Mutex
            _mutex = new Mutex(true, appName, out bool isNewInstance);

            if (!isNewInstance)
            {
                // Se já existe outra instância, exibe uma mensagem e encerra
                MessageBox.Show("O aplicativo já está aberto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Process.GetCurrentProcess().Kill();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
