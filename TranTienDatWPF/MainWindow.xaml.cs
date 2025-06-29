using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessObjects.Models;
using Services;

namespace TranTienDatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductService iProductService;
        private readonly ICategoryService iCategoryService;
        public MainWindow()
        {
            InitializeComponent();
            iProductService = new ProductService();
            iCategoryService = new CategoryService();
        }

        private void LoadCategoryList()
        {
            try
            {
                var catList = iCategoryService.GetCategories();
                cboCategory.ItemsSource = catList;
                cboCategory.DisplayMemberPath = "CategoryName";
                cboCategory.SelectedValuePath = "CategoryId";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of categories: " );
            }   
        }

        public void LoadProductList()
        {
            try
            {
                var productList = iProductService.GetProducts()
                    .Select(p => new ProductDisplay
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice ?? 0,
                        UnitsInStock = p.UnitsInStock ?? 0,
                        CategoryName = p.Category?.CategoryName ?? "N/A"
                    }).ToList();
                dgData.ItemsSource = productList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of products: ");
            }
            finally
            {
                resetInput();
            }
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    ProductName = txtProductName.Text,
                    UnitPrice = Decimal.Parse(txtPrice.Text),
                    UnitsInStock = short.Parse(txtUnitsInStock.Text),
                    CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString())
                };

                iProductService.SaveProduct(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadProductList();
                resetInput();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtProductID.Text.Length > 0)
                {
                    Product product = new Product
                    {
                        ProductId = Int32.Parse(txtProductID.Text),
                        ProductName = txtProductName.Text,
                        UnitPrice = Decimal.Parse(txtPrice.Text),
                        UnitsInStock = short.Parse(txtUnitsInStock.Text),
                        CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString())
                    };

                    iProductService.UpdateProduct(product);
                }
                else
                {
                    MessageBox.Show("You must select a Product !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadProductList();
                resetInput();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtProductID.Text.Length > 0)
                {
                    Product product = new Product
                    {
                        ProductId = Int32.Parse(txtProductID.Text),
                        ProductName = txtProductName.Text,
                        UnitPrice = Decimal.Parse(txtPrice.Text),
                        UnitsInStock = short.Parse(txtUnitsInStock.Text),
                        CategoryId = Int32.Parse(cboCategory.SelectedValue.ToString())
                    };

                    iProductService.DeleteProduct(product);
                }
                else
                {
                    MessageBox.Show("You must select a Product !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadProductList();
                resetInput();
            }
        }
        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            string id = ((TextBlock)RowColumn.Content).Text;

            Product product = iProductService.GetProductById(Int32.Parse(id));
            txtProductID.Text = product.ProductId.ToString();
            txtProductName.Text = product.ProductName;
            txtPrice.Text = product.UnitPrice.ToString();
            txtUnitsInStock.Text = product.UnitsInStock.ToString();
            cboCategory.SelectedValue = product.CategoryId;*/
            if (dgData.SelectedItem is ProductDisplay display)
            {
                txtProductID.Text = display.ProductId.ToString();
                txtProductName.Text = display.ProductName;
                txtPrice.Text = display.UnitPrice.ToString();
                txtUnitsInStock.Text = display.UnitsInStock.ToString();

                var category = iCategoryService.GetCategories()
                                .FirstOrDefault(c => c.CategoryName == display.CategoryName);
                if (category != null)
                {
                    cboCategory.SelectedValue = category.CategoryId;
                }
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void resetInput()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtUnitsInStock.Text = "";
            cboCategory.SelectedValue = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProductList();
            LoadCategoryList();
        }



    }
}