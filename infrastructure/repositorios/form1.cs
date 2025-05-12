using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using sgi_App.infrastructure.mysql;
using sgi_App.domain.entities;

namespace sgi_App
{
    public partial class Form1 : Form
    {
        private List<Producto> mProductos;
        private Producto mProducto;
        private ProductoConsultas mProductoConsultas;

        public Form1()
        {
            InitializeComponent();

            mProductos = new List<Producto>();
            mProducto = new Producto();
            mProductoConsultas = new ProductoConsultas();

            CargarProductos();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Conexionmysql conexion = new Conexionmysql();
            conexion.ProbarConexion();
        }

        private void CargarProductos(string filtro = "")
        {
            dgvProductos.Rows.Clear();
            dgvProductos.Refresh();
            mProductos = mProductoConsultas.getProductos(filtro);

            foreach (var producto in mProductos)
            {
                dgvProductos.RowTemplate.Height = 50;
                dgvProductos.Rows.Add(
                    producto.id,
                    producto.nombre,
                    producto.precio,
                    producto.cantidad,
                    Image.FromStream(new MemoryStream(producto.imagen))
                );
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            CargarProductos(txtBusqueda.Text.Trim());
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!DatosCorrectos()) return;

            CargarDatosProducto();

            if (mProductoConsultas.agregarProducto(mProducto))
            {
                Console.WriteLine("Producto agregado");
                CargarProductos();
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtFolio.Text = "";
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            pbImage.Image = sgi_App.Resource.agregar_imagen; // Puedes quitar si no deseas imagen por defecto
        }

        private void CargarDatosProducto()
        {
            mProducto.id = GetFolioIfExist();
            mProducto.nombre = txtNombre.Text.Trim();
            mProducto.precio = float.Parse(txtPrecio.Text.Trim());
            mProducto.cantidad = int.Parse(txtCantidad.Text.Trim());
            mProducto.imagen = ImageToByteArray(pbImage.Image);
        }

        private int GetFolioIfExist()
        {
            if (!string.IsNullOrWhiteSpace(txtFolio.Text) && int.TryParse(txtFolio.Text.Trim(), out int folio))
            {
                return folio;
            }
            return -1;
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private bool DatosCorrectos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                Console.WriteLine("Ingrese el nombre");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                Console.WriteLine("Ingrese el precio");
                return false;
            }

            if (!float.TryParse(txtPrecio.Text.Trim(), out _))
            {
                Console.WriteLine("Ingrese un precio correcto");
                return false;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out _))
            {
                Console.WriteLine("Ingrese una cantidad correcta");
                return false;
            }

            return true;
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pbImage.ImageLocation = dlg.FileName;
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
            txtFolio.Text = fila.Cells["Folio"].Value.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
            txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
            txtCantidad.Text = fila.Cells["Cantidad"].Value.ToString();

            if (fila.Cells[4].Value is Image img)
            {
                pbImage.Image = img;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!DatosCorrectos()) return;

            CargarDatosProducto();

            if (mProductoConsultas.modificarProducto(mProducto))
            {
                Console.WriteLine("Producto modificado");
                CargarProductos();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (GetFolioIfExist() == -1) return;

            if (Console.WriteLine("¿Desea eliminar el producto?", "Eliminar producto", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CargarDatosProducto();

                if (mProductoConsultas.eliminarProducto(mProducto))
                {
                    Console.WriteLine("Producto eliminado");
                    CargarProductos();
                    LimpiarCampos();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
