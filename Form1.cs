using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decrypt_GB;
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Encrypt_Decrypt
{
    public partial class Form1 : Form
    {
        private bool mousdown;
        private Point lastlocation;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousdown = true;
            lastlocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousdown)
            {
                Location = new Point(Location.X - lastlocation.X + e.X, Location.Y - lastlocation.Y + e.Y);
                Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousdown = false;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        public string x08()
        {
            string chave = "";
            try
            {
                // URL do script PHP para recuperar a chave
                string url = "https://suaurlaqui/AES/x3.php";  // Substitua com a URL do script PHP

                // Cria o cliente HTTP
                using (HttpClient client = new HttpClient())
                {
                    // Envia a requisição GET de forma síncrona usando Result
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Verifica se a resposta foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        // Verifica se o cabeçalho "X-Custom-Key" está presente na resposta
                        if (response.Headers.Contains("X-Custom-Key"))
                        {
                            // Obtém o valor do cabeçalho "X-Custom-Key" (chave codificada)
                            var chaveBase64Values = response.Headers.GetValues("X-Custom-Key");

                            // Aqui, extraímos o primeiro valor da coleção de cabeçalhos (assumindo que há apenas um valor)
                            string chaveBase64 = chaveBase64Values.FirstOrDefault();

                            if (!string.IsNullOrEmpty(chaveBase64))
                            {
                                // Decodifica a chave de Base64 para obter a chave original
                                byte[] chaveBytes = Convert.FromBase64String(chaveBase64);
                                string chaveOriginal = Encoding.UTF8.GetString(chaveBytes);
                                return chave = chaveOriginal;
                            }
                            else
                            {
                                MessageBox.Show("A chave Base64 está vazia ou inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cabeçalho 'X-Custom-Key' não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Falha ao acessar a URL. Código de resposta: " + response.StatusCode, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return chave;
        }

        private void EncryptBtn_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(NormalText.Text) || NormalText.Text == string.Empty)
            {
                return;
            }
            try
            {
                // Guarde essa chave em local seguro!
                EscureCryptoHelper cryptoHelper = new EscureCryptoHelper(x08());

                string textoOriginal = NormalText.Text;
                
                string textoEncriptado = cryptoHelper.Encrypt(textoOriginal);// Criptografar
                EncryptText.Text = textoEncriptado;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void DencryptBtn_Click(object sender, EventArgs e)
        {
            try
            {
                EscureCryptoHelper cryptoHelper = new EscureCryptoHelper(x08());
                                                                                                                 
                string textoDesencriptado = cryptoHelper.Decrypt(EncryptText.Text); // Descriptografar
                DencryptText.Text = textoDesencriptado;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CleanBtn_Click(object sender, EventArgs e)
        {
            NormalText.Text = string.Empty;
            EncryptText.Text = string.Empty;
            DencryptText.Text = string.Empty;
            NormalText.Focus();
        }
    }
}
