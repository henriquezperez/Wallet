using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wallet.UI
{
    public partial class FrmMain : Form
    {
        public string usuarioTemp = string.Empty;
        public string claveTemp = string.Empty;
        public int usuarioId = 0;

        public FrmMain()
        {
            InitializeComponent();
        }

        public FrmMain(string user, string password)
        {
            InitializeComponent();
            //labelNameUser.Text = entity.Email;
            labelNameUser.Text = user.ToString();
            usuarioTemp = user.ToString();
            claveTemp = password.ToString();
            UpdateData();
        }

        private void UpdateData()
        {
            using (Wallet4Entities _repo = new Wallet4Entities())
            {
                var log = _repo.Usuarios.FirstOrDefault(x => x.Email == usuarioTemp && x.Clave == claveTemp);
                if (log != null)
                {
                    labelMoney.Text = '$' + log.Saldo.ToString();
                    usuarioId = log.UsuarioId;
                }
            }
        }

        private void buttonMoneyList_Click(object sender, EventArgs e)
        {
            FrmMonedas frm = new FrmMonedas();
            frm.ShowDialog();
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            FrmCompraVenta frm = new FrmCompraVenta(usuarioId);
            frm.ShowDialog();
            UpdateDataGrid();
            UpdateData();
        }

        private void UpdateDataGrid()
        {
            using (Wallet4Entities _repo = new Wallet4Entities())
            {
                var query = (from h in _repo.Historials.Where(x => x.UsuarioId == usuarioId).ToList()
                             select new
                             {
                                 h.TipoTransaccion,
                                 h.Descripcion,
                                 h.Monto,
                                 h.Cargo,
                                 h.Total,
                                 h.Fecha
                             }
                             ).ToList();


                //dataGridView1.DataSource = _repo.Historials.Where(x=>x.UsuarioId == usuarioId).ToList();
                dataGridView1.DataSource = query;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            CerrarSesion();
        }

        private void CerrarSesion()
        {
            FrmLogin frm = new FrmLogin();
            frm.Show();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateDataGrid();
            UpdateData();
        }
    }
}
