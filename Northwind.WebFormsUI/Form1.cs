using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.DependencyResolvers.Ninject;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.DataAccess.Concrete.NHibernate;
using Northwind.Entities.Concrete;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = InstanceFactory.GetInstance<IProductService>();
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();
        }

        IProductService _productService;
        ICategoryService _categoryService;
        private void Form1_Load(object sender, EventArgs e)
        {

            LoadProducts();
            LoadCategories();
            
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategory2.DataSource = _categoryService.GetAll();
            cbxCategory2.DisplayMember = "CategoryName";
            cbxCategory2.ValueMember = "CategoryId";

            cbxCategoryUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryUpdate.DisplayMember = "CategoryName";
            cbxCategoryUpdate.ValueMember = "CategoryId";
        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch
            { 

            }
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbxSearch.Text))
            {
                dgwProduct.DataSource = _productService.GetAll();
            }
            else
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxSearch.Text);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    ProductName = tbxProductName2.Text,
                    CategoryId = Convert.ToInt32(cbxCategory2.SelectedValue.ToString()),
                    QuantityPerUnit = tbxQuantityPerUnit.Text,
                    UnitsInStock = Convert.ToInt16(tbxStock.Text),
                    UnitPrice = Convert.ToDecimal(tbxUnitPrice.Text),



                });

                MessageBox.Show("Product Has Been Added!");
                LoadProducts();
            }
            catch (Exception)
            {

                MessageBox.Show("Inputs are not meet the rules! Please meet the rules!");
            }
            
           
        }

        private void cbxCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Update(new Product
                {
                    ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                    ProductName = tbxProductNameUpdate.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryUpdate.SelectedValue.ToString()),
                    QuantityPerUnit = tbxQuantityPerUnitUpdate.Text,
                    UnitsInStock = Convert.ToInt16(tbxStockUpdate.Text),
                    UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),
                });
                MessageBox.Show("Selected Product Has Been Updated !");
                LoadProducts();
            }
            catch 
            {

                MessageBox.Show("Inputs are not meet the rules! Please meet the rules!");
            }

        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            tbxProductNameUpdate.Text = (string)row.Cells[2].Value;
            cbxCategoryUpdate.SelectedIndex = Convert.ToInt32(row.Cells[1].Value)-1;
            tbxQuantityPerUnitUpdate.Text= Convert.ToString(row.Cells[5].Value);
            tbxStockUpdate.Text = Convert.ToString(row.Cells[4].Value);
            tbxUnitPriceUpdate.Text = Convert.ToString(row.Cells[3].Value);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var row = dgwProduct.CurrentRow;
            if(row != null)
            {
                try
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(row.Cells[0].Value)
                    });
                    MessageBox.Show("Selected Product Has Been Removed!");
                    LoadProducts();
                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                }
                
            }
            
            
            

        }
    }
}
