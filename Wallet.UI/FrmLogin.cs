using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Wallet.UI
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == string.Empty || textBoxUser.Text == string.Empty)
            {
                MessageBox.Show("Campos vacios", "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    Comprobacion();
                }
                else
                {
                    if (textBoxPassword.Text != textBoxPassword2.Text)
                    {
                        MessageBox.Show("Contrasenas no coinciden", "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        NuevaCuenta();
                    }
                }
            }
        }

        private void NuevaCuenta()
        {
            using(Wallet4Entities _repo = new Wallet4Entities())
            {
                Usuario entity = new Usuario()
                {
                    Email = textBoxUser.Text.Trim(),
                    Clave = textBoxPassword.Text.Trim(),
                    Estado = 0,
                    Saldo = 30.00M
                };
                _repo.Usuarios.Add(entity);
                _repo.SaveChanges();
                Usuario log = _repo.Usuarios.FirstOrDefault(x => x.Email == textBoxUser.Text && x.Clave == textBoxPassword.Text);
                Historial his = new Historial()
                {
                    UsuarioId = log.UsuarioId,
                    TipoTransaccion = "Inicio",
                    Descripcion = "Bono de bienvenida",
                    Monto = 30.00M,
                    Cargo = 0,
                    Total = 30.00M,
                    Fecha = DateTime.Now
                };
                _repo.Historials.Add(his);
                _repo.SaveChanges();
            }
            MessageBox.Show("Bienvenido, Por ser tu primera vez te hemos abonado $30.00 a tu cuenta");
            Comprobacion();
        }

        private void Comprobacion()
        {
            //pbLoading.Visible = true;
            using (Wallet4Entities _repo = new Wallet4Entities())
            {
                var log = _repo.Usuarios.FirstOrDefault(x => x.Email == textBoxUser.Text && x.Clave == textBoxPassword.Text);
                if (log != null)
                {
                    MessageBox.Show("Bienvenido!");
                    this.Hide();
                    LanzarProgra();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("¡Intente de nuevo!", "Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   // pbLoading.Visible = false;
                    textBoxUser.Text = string.Empty;
                    textBoxPassword.Text = string.Empty;
                    textBoxPassword2.Text = string.Empty;
                   // btnHidePassword_Click(null, null);
                }
            }
        }

        private void LanzarProgra()
        {
            FrmMain frm = new FrmMain(textBoxUser.Text, textBoxPassword.Text);
            frm.ShowDialog();
        }
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBoxPassword2.Enabled = false;
            }
            else
            {
                textBoxPassword2.Enabled = true;
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
