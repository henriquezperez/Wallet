using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Wallet.UI
{
    public partial class FrmCompraVenta : Form
    {
        public int userId = 0;
        public decimal saldo = 0;
        public decimal saldo1 = 0;
        List<Divisa> lista;
        public decimal ValorDivisa = 0;
        public decimal totalC = 0;
        public decimal cargo = 0;

        public decimal conversion = 0;
        public decimal monto = 0;

        public FrmCompraVenta()
        {
            InitializeComponent();
        }

        public FrmCompraVenta(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            using (Wallet4Entities _repo = new Wallet4Entities())
            {
                var log = _repo.Usuarios.FirstOrDefault(x => x.UsuarioId == userId);
                if (log != null)
                {
                    label12.Text = '$' + log.Saldo.ToString();
                    label13.Text = log.UsuarioId.ToString() + ", " + log.Email;
                    saldo = log.Saldo;
                    saldo1 = log.Saldo;
                }
            }
        }

        private void FrmCompraVenta_Load(object sender, EventArgs e)
        {
            using (Wallet4Entities _repo = new Wallet4Entities())
            {
                lista = _repo.Divisas.ToList();
                comboBoxDivisas.DataSource = lista;
                comboBoxDivisas.DisplayMember = "Nombre";
                comboBoxDivisas.ValueMember = "DivisaId";
            }
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            decimal monto = 0;
            if(saldo >= 0 && numericUpDown1.Value <= saldo)
            {
                saldo = saldo - numericUpDown1.Value;
                monto = monto + numericUpDown1.Value;
            }
            else
            {
                MessageBox.Show("Sobrepasa el saldo");
            }
            label12.Text = '$' + saldo.ToString();
            //labelConversion.Text = '$' + monto.ToString();
           // numericUpDown1.Value = 0;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            saldo = saldo1;
            numericUpDown1.Value = 0;
            label12.Text = '$' + saldo.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //decimal conversion = 0;
            //decimal monto = 0;
            monto += numericUpDown1.Value;
            labelConversion.Text = '$' + monto.ToString();
            // numericUpDown1.Value = 0;
            conversion = monto * ValorDivisa;
            labelConversion.Text = "$" + monto.ToString() + " X " + ValorDivisa.ToString() + " = " + conversion.ToString();
            label6.Text = '$' + (monto * 0.04M).ToString();
            label9.Text = '$' + ((monto * 0.04M) + monto).ToString();
            totalC = (monto * 0.04M) + monto;
            cargo = monto * 0.04M;
        }

        private void comboBoxDivisas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Divisa query = lista.FirstOrDefault(x => x.DivisaId.Equals(comboBoxDivisas.SelectedValue));
            if (query != null)
            {
                conversion = monto * ValorDivisa;
                labelPrecioDivisa.Text = "->" + query.Valor.ToString();
                label7.Text = '$' + query.Valor.ToString();
                ValorDivisa = query.Valor;
                labelConversion.Text = "$" + monto.ToString() + " X " + ValorDivisa.ToString() + " = " + conversion.ToString();
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Confirmar transaccion", "¡Confirmar Transaccion!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                bool result = false;
                using (Wallet4Entities _repo = new Wallet4Entities())
                {
                    Usuario es = _repo.Usuarios.FirstOrDefault(x => x.UsuarioId == userId);
                    es.Saldo = es.Saldo - totalC;
                    result = _repo.SaveChanges() > 0;
                    Historial historial = new Historial()
                    {
                        UsuarioId = userId,
                        TipoTransaccion = "Cambio",
                        Descripcion = "Conversion",
                        Monto = totalC,
                        Cargo = cargo,
                        Total = totalC + cargo,
                        Fecha = DateTime.Now
                    };
                    _repo.Historials.Add(historial);
                    _repo.SaveChanges();
                }
                this.Close();
            }
        }
    }
}
