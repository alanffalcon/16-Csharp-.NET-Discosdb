using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;


namespace Practica
{
    public partial class frmAltaDisco : Form
    {
        private Disco disco = null;
        private OpenFileDialog archivo = null;

        // LISTO
        public frmAltaDisco()
        {
            InitializeComponent();
        }
        // LISTO
        public frmAltaDisco(Disco disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar Disco";
        }
        // LISTO
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }




        // PRUEBA 3
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Disco disc = new Disco();
            DiscoNegocio negocio = new DiscoNegocio();

            try
            {
                if(disco == null)
                {
                    disco = new Disco();
                }

                disco.Titulo = tbTitulo.Text;
                disco.FechaLanzamiento = DateTime.Parse(dtpFechaLanzamiento.Text);
                disco.CantidadCanciones = int.Parse(tbCanciones.Text);
                disco.UrlImagen = tbUrlImagen.Text;
                disco.Genero = (Genero)cbEstilo.SelectedItem;
                disco.Edicion = (Edicion)cbEdicion.SelectedItem;

                if(disco.Id != 0)
                {
                    negocio.modificar(disco);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(disco);
                    MessageBox.Show("Agregado exitosamente");
                }
       
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EdicionNegocio edicionNegocio = new EdicionNegocio();
            GeneroNegocio generoNegocio = new GeneroNegocio();
            try
            {
                cbEdicion.DataSource = edicionNegocio.listar();
                cbEdicion.ValueMember = "Id";
                cbEdicion.DisplayMember = "Formato";

                cbEstilo.DataSource = generoNegocio.listar();
                cbEstilo.ValueMember = "Id";
                cbEstilo.DisplayMember = "Edicion";

                if (disco != null)
                {
                    tbTitulo.Text = disco.Titulo;
                    dtpFechaLanzamiento.Value = disco.FechaLanzamiento;
                    tbCanciones.Text = disco.CantidadCanciones.ToString();
                    tbUrlImagen.Text = disco.UrlImagen;

                    cargarImagen(disco.UrlImagen);

                    cbEdicion.SelectedValue = disco.Edicion.Id;
                    cbEstilo.SelectedValue = disco.Genero.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tbUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(tbUrlImagen.Text);
        }

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

        // PRUEBA 2 ANDA BIEN
        /*
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Disco disc = new Disco();
            DiscoNegocio negocio = new DiscoNegocio();

            try
            {
                disc.Titulo = tbTitulo.Text;
                disc.FechaLanzamiento = DateTime.Parse(dtpFechaLanzamiento.Text);
                disc.CantidadCanciones = int.Parse(tbCanciones.Text);
                disc.Genero = (Genero)cbEstilo.SelectedItem;
                disc.Edicion = (Edicion)cbEdicion.SelectedItem;

                negocio.agregar(disc);
                MessageBox.Show("Agregado exitosamente");
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EdicionNegocio edicionNegocio = new EdicionNegocio();
            GeneroNegocio generoNegocio= new GeneroNegocio();
            try
            {
                cbEdicion.DataSource = edicionNegocio.listar();
                cbEstilo.DataSource = generoNegocio.listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        */




        // PRUEBA 1
        /*
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DiscoNegocio negocio = new DiscoNegocio();
            try
            {
                if (disco == null)
                {
                    disco = new Disco();
                }

                disco.Titulo = tbTitulo.Text;
                disco.FechaLanzamiento = DateTime.Parse(dtpFechaLanzamiento.Text);
                disco.CantidadCanciones = int.Parse(tbCanciones.Text);
                disco.UrlImagen = tbUrlImagen.Text;                
                disco.Genero = cbEstilo.SelectedItem as Genero;
                disco.Edicion = cbEdicion.SelectedItem as Edicion;

                if (disco.Id != 0)
                {
                    negocio.modificar(disco);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(disco);
                    MessageBox.Show("Agregado exitosamente");

                }

                if(archivo != null && !(tbUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["imgaes-folder"] + archivo.SafeFileName);
                }

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            DiscoNegocio discoNegocio= new DiscoNegocio();
            try
            {
                cbEstilo.DataSource = discoNegocio.listar();
                cbEstilo.ValueMember = "Id";
                cbEstilo.DisplayMember = "Genero";
                
                cbEdicion.DataSource = discoNegocio.listar();
                cbEdicion.ValueMember = "Id";
                cbEdicion.DisplayMember = "Edicion";

                if(disco != null)
                {
                    tbTitulo.Text = disco.Titulo;
                    dtpFechaLanzamiento.Value = disco.FechaLanzamiento;
                    tbCanciones.Text = disco.CantidadCanciones.ToString();
                    tbUrlImagen.Text = disco.UrlImagen;

                    cargarImagen(disco.UrlImagen);

                    cbEstilo.SelectedValue = disco.Genero.Id;
                    cbEdicion.SelectedValue = disco.Edicion.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbDisco.Load(imagen);
            }
            catch (Exception)
            {
                pbDisco.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
            
        }


        */
    }
}
