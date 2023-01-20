using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace Practica
{
    public partial class frmDiscos : Form
    {

        private List<Disco> listaDisco;

        // LISTO
        public frmDiscos()
        {
            InitializeComponent();
        }      
        // LISTO
        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cbCampo.Items.Add("Titulo");
            cbCampo.Items.Add("Canciones");
            cbCampo.Items.Add("Estilo");
        }
        // LISTO
        private void cargar()
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                listaDisco = negocio.listar();
                dgvDisco.DataSource = listaDisco;
                ocultarColumnas();
                cargarImagen(listaDisco[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // LISTO
        private void ocultarColumnas()
        {
            dgvDisco.Columns["UrlImagen"].Visible = false;
            dgvDisco.Columns["Id"].Visible = false;
        }
        // LISTO
        private void cargarImagen(string imagen)
        {
            try
            {
                pbDisco.Load(imagen);
            }
            catch (Exception)
            {
                pbDisco.Load("https://thumb1.shutterstock.com/image-photo/stock-vector-vector-illustration-of-cd-or-dvd-s-in-gray-icon-250nw-574210795.jpg");
            }
        }
        // LISTO
        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            frmAltaDisco alta = new frmAltaDisco();
            alta.ShowDialog();
            cargar();
        }
        // LISTO
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Disco seleccionado;
            seleccionado = (Disco)dgvDisco.CurrentRow.DataBoundItem;

            frmAltaDisco modificar = new frmAltaDisco(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
        // LISTO
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }
        // LISTO
        private void eliminar(bool logico = false)
        {
            DiscoNegocio negocio = new DiscoNegocio();
            Disco seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estas seguro que lo queres eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Disco)dgvDisco.CurrentRow.DataBoundItem;

                    if (logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);

                    }
                    else
                    {
                        negocio.eliminar(seleccionado.Id);
                    }

                    cargar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        // LISTO
        private bool validarFiltro()
        {
            if (cbCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el campo a filtrar.");
                return true;
            }
            if (cbCriterio.SelectedItem.ToString() == "Canciones")
            {
                if (string.IsNullOrEmpty(tbFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar un numero.");
                    return true;
                }
                if (!(soloNumeros(tbFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Debes cargar solo numeros.");
                    return true;
                }
            }

            return false;
        }
        // LISTO
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }
        // LISTO
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                if (validarFiltro())
                {
                    return;
                }

                string campo = cbCampo.SelectedItem.ToString();
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtro = tbFiltroAvanzado.Text;
                dgvDisco.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // LISTO
        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            Disco seleccionado = (Disco)dgvDisco.CurrentRow.DataBoundItem;

            cargarImagen(seleccionado.UrlImagen);
        }
        // LISTO
        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString();

            if (opcion == "Canciones")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Mayor a");
                cbCriterio.Items.Add("Menor a");
                cbCriterio.Items.Add("Igual a");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Comienza con");
                cbCriterio.Items.Add("Termina con");
                cbCriterio.Items.Add("Contiene");
            }
        }
        // LISTO
        private void tbFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Disco> listaFiltrada;
            string filtro = tbFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaDisco.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper()) || x.Genero.Edicion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDisco;
            }

            dgvDisco.DataSource = null;
            dgvDisco.DataSource = listaFiltrada;
            ocultarColumnas();
        }
    }
}
