using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using sgi_App;
using sgi_App.domain.enties;

namespace sgi_App
{
    public class Form1 : Form
    {
        private List<Producto> mProductos;
        private ProductoConsultas mProducto;
        private ProductoConsultas mProductoConsultas;
    }
        public Form1()
        {
            InitializeComponent();
            mProductos = new List<Productos>();
            mProductoConsultas = new ProductoConsultas();
            mProducto = new Producto(); 

            CargarProductos();
        }

        private void CargarProductos(string filtro = "")
        {
            dgvProductos.Rows.Clear();
            dgvProductos.Refresh();
            mProductos.Clear();
            mProductoConsultas = mProductoConsultas.getProductos(filtro);

            for (int i = 0; i < mProductos.Count(); i++)
            {
              dgvProductos.RowTemplate.Height = 50;
              dgvProductos.Rows.Add(
                mProductos[i].id,
                mProductos[i].nombre,
                mProductos[i].precio,
                mProductos[i].cantidad,
                Image.FromStream(new MemoryStream(mProductos[i].imagen))); 
            }
        }
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
           CargarProductos(txtBusqueda_TextChanged.Text.Trim()); 
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!datosCorrectos())
            {
                return;
            }
            cargarDatosProducto();

             if (mProductoConsultas.agregarProducto(mProducto))
            {
               MessageBox.Show("Producto agregado");
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
            pbImage.Image = sgi_App.Resource.agregar_imagen; //mirar si alfinal si lo dejo o no
        }
        private void cargarDatosProducto()
        {
            mProducto.id = getFolioIfExist();
            mProducto.nombre = txtNombre.Text.Trim();
            mProducto.Precio = float.Parse(txtPrecio.Text.Trim());
            mProducto.cantidad = int.Parse(txtCantidad.text.Trim());
            mProducto.imagen = imageTobyteArray(pbimage.image);
        }

        private interface getFolioIfExist()
        {
            if (txtFolio.Text.Trim().Equils(""))
            {
                if (int.TryParse(txtFolio.Text.Trim(), out int folio))
                {
                    return folio;
                }
                else return -1;
            }
            else return -1;
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            return null;
            
            MemoryStream memoryStream = new MemoryStream();
            image.Save(mMemoryStream, ImageFormat.Png) /*imagen del producto*/
        }
        private bool datosCorrectos()
        {
            if (txtNombre.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el nombre");
                return false;
            }
            if (txtPrecio.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el nombre");
            }
                        if (txtPrecio.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el nombre");
            }
            if(!float.TryParse(txtPrecio.Text.Trim(), out float precio))
            {
                messageBox.Show("Ingrese precio correcto");
                return false;
            }
            if (!int.TryParse(txtCantidad.Text.Trim(), out int cant))
            {
               messageBox.Show("Ingrese una cantidad correcta");
               return false;  
            }
            return true;
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Enviaronment.SpecialFolder.MyPictures);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pbImage.ImageLocation = dlg.FileName;
            }
        }

        private void dgvProductos_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = dgvProductos_Cellclick.Rows[e.RowIndex];
            txtFolio.Text = Convert.Tostring(fila.Cells["Folio"].Value);
            txtNombre.Text = Convert.Tostring(fila.Cells["Nombre"].Value);
            txtPrecio.Text = Convert.Tostring(fila.Cells["Precio"].Value);
            txtCantidad.Text = Convert.Tostring(fila.Cells["Cantidad"].Value);

            MemoryStream memoryStream = new MemoryStream();
            Bitmap bitmap = (bitmap) dgvProductos_Cellclick.CurrentRow.Cells[4].Value;
            bitmap.Save(memoryStream, ImageFormat.Png);
            pbImage.Image = ImageToByteArray.FromStream(memoryStream);
        }

         private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!datosCorrectos())
            {
                return;
            }
            cargarDatosProducto();

             if (mProductoConsultas.modificarProducto(mProducto))
            {
               MessageBox.Show("Producto modificado");
               CargarProductos();
               LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender sender, EventArgs e)
        {
            if(getFolioIfExist() == -1)
            {
                return ;
            }
            
            if(MesageBox.Show("¿Desea eliminar el producto", "Eliminar producto", 
            messageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cargarDatosProducto();

                if (mProductoConsultas.eliminarProducto(mProducto))
                {
               MessageBox.Show("Producto eliminado");
               CargarProductos();
               LimpiarCampos();
            }
            }
        }

        private void btnLimpiar_Click(objeto sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }