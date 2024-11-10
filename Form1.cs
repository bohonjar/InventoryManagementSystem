using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class MainForm : Form
    {
        // Controls for the product section
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblProductCategory;
        private ComboBox cmbProductCategory;
        private Label lblProductPrice;
        private TextBox txtProductPrice;
        private Label lblProductQuantity;
        private TextBox txtProductQuantity;
        private Button btnAddProduct;
        private Button btnUpdateProduct;
        private Button btnDeleteProduct;
        private DataGridView dgvProducts;

        // Controls for the company section
        private Label lblCompanyName;
        private TextBox txtCompanyName;
        private Label lblCompanyLocation;
        private TextBox txtCompanyLocation;
        private Button btnAddCompany;
        private Button btnUpdateCompany;
        private Button btnDeleteCompany;
        private DataGridView dgvCompanies;

        // Controls for the inventory section
        private Label lblInventoryLevel;
        private TextBox txtInventoryLevel;
        private Button btnCheckInventory;
        private DataGridView dgvInventory;

        // Add Company Section Controls
        private Label lblCompany;
        private ComboBox cmbCompany;

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            CustomizeUI();  // Call to apply styles to controls
            LoadData();  // Ensure data is loaded when the form initializes
            LoadCategories(); // Load categories for the ComboBox
 
            dgvProducts.ReadOnly = true;
            dgvCompanies.ReadOnly = true;
            dgvInventory.ReadOnly = true;
            // Add this to the constructor to subscribe to the CellClick event of the DataGridView
            dgvProducts.CellClick += dgvProducts_CellClick;
            dgvCompanies.SelectionChanged += dgvCompanies_SelectionChanged;

            // Subscribe to the TextChanged event of the Inventory Level TextBox
            txtInventoryLevel.TextChanged += txtInventoryLevel_TextChanged;



            // Set ComboBox to be non-editable
            cmbProductCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void CustomizeUI()
        {
            // Customize TextBox (Product and Inventory Levels)
            txtProductName.BackColor = Color.LightGray;
            txtProductName.ForeColor = Color.Black;
            txtProductName.Font = new Font("Arial", 10, FontStyle.Regular);
            txtProductName.BorderStyle = BorderStyle.FixedSingle;

            txtProductPrice.BackColor = Color.LightGray;
            txtProductPrice.ForeColor = Color.Black;
            txtProductPrice.Font = new Font("Arial", 10, FontStyle.Regular);
            txtProductPrice.BorderStyle = BorderStyle.FixedSingle;

            txtProductQuantity.BackColor = Color.LightGray;
            txtProductQuantity.ForeColor = Color.Black;
            txtProductQuantity.Font = new Font("Arial", 10, FontStyle.Regular);
            txtProductQuantity.BorderStyle = BorderStyle.FixedSingle;

            txtInventoryLevel.BackColor = Color.LightGray;
            txtInventoryLevel.ForeColor = Color.Black;
            txtInventoryLevel.Font = new Font("Arial", 10, FontStyle.Regular);
            txtInventoryLevel.BorderStyle = BorderStyle.FixedSingle;

            // Customize ComboBox (Product Category and Company)
            cmbProductCategory.BackColor = Color.WhiteSmoke;
            cmbProductCategory.ForeColor = Color.DarkSlateGray;
            cmbProductCategory.Font = new Font("Arial", 10, FontStyle.Bold);
            cmbProductCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbCompany.BackColor = Color.WhiteSmoke;
            cmbCompany.ForeColor = Color.DarkSlateGray;
            cmbCompany.Font = new Font("Arial", 10, FontStyle.Bold);
            cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList;

            // Customize DataGridView (Products, Companies, Inventory)
            dgvProducts.BackgroundColor = Color.LightBlue;
            dgvProducts.DefaultCellStyle.BackColor = Color.White;
            dgvProducts.DefaultCellStyle.ForeColor = Color.Black;
            dgvProducts.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvProducts.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
            dgvProducts.RowsDefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvProducts.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            dgvCompanies.BackgroundColor = Color.LightBlue;
            dgvCompanies.DefaultCellStyle.BackColor = Color.White;
            dgvCompanies.DefaultCellStyle.ForeColor = Color.Black;
            dgvCompanies.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvCompanies.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvCompanies.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
            dgvCompanies.RowsDefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvCompanies.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            dgvInventory.BackgroundColor = Color.LightBlue;
            dgvInventory.DefaultCellStyle.BackColor = Color.White;
            dgvInventory.DefaultCellStyle.ForeColor = Color.Black;
            dgvInventory.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Regular);
            dgvInventory.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
            dgvInventory.RowsDefaultCellStyle.SelectionBackColor = Color.DarkOrange;
            dgvInventory.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Customize Buttons (Add/Update/Delete Product and Company)
            btnAddProduct.BackColor = Color.Green;
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.Font = new Font("Arial", 12, FontStyle.Bold);
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.FlatAppearance.BorderSize = 0;

            btnUpdateProduct.BackColor = Color.Orange;
            btnUpdateProduct.ForeColor = Color.White;
            btnUpdateProduct.Font = new Font("Arial", 12, FontStyle.Bold);
            btnUpdateProduct.FlatStyle = FlatStyle.Flat;
            btnUpdateProduct.FlatAppearance.BorderSize = 0;

            btnDeleteProduct.BackColor = Color.Red;
            btnDeleteProduct.ForeColor = Color.White;
            btnDeleteProduct.Font = new Font("Arial", 12, FontStyle.Bold);
            btnDeleteProduct.FlatStyle = FlatStyle.Flat;
            btnDeleteProduct.FlatAppearance.BorderSize = 0;

            btnAddCompany.BackColor = Color.Green;
            btnAddCompany.ForeColor = Color.White;
            btnAddCompany.Font = new Font("Arial", 12, FontStyle.Bold);
            btnAddCompany.FlatStyle = FlatStyle.Flat;
            btnAddCompany.FlatAppearance.BorderSize = 0;

            btnUpdateCompany.BackColor = Color.Orange;
            btnUpdateCompany.ForeColor = Color.White;
            btnUpdateCompany.Font = new Font("Arial", 12, FontStyle.Bold);
            btnUpdateCompany.FlatStyle = FlatStyle.Flat;
            btnUpdateCompany.FlatAppearance.BorderSize = 0;

            btnDeleteCompany.BackColor = Color.Red;
            btnDeleteCompany.ForeColor = Color.White;
            btnDeleteCompany.Font = new Font("Arial", 12, FontStyle.Bold);
            btnDeleteCompany.FlatStyle = FlatStyle.Flat;
            btnDeleteCompany.FlatAppearance.BorderSize = 0;
        }

        private void InitializeControls()
        {
            // Product Section
            lblProductName = new Label { Text = "Product Name", Location = new Point(20, 20), Width = 120 };
            txtProductName = new TextBox { Location = new Point(150, 20), Width = 200 };

            // Company Section
            lblCompany = new Label { Text = "Company", Location = new Point(20, 60), Width = 120 };
            cmbCompany = new ComboBox { Location = new Point(150, 60), Width = 200 };

            lblProductCategory = new Label { Text = "Category", Location = new Point(20, 100), Width = 120 };
            cmbProductCategory = new ComboBox { Location = new Point(150, 100), Width = 200 };

            lblProductPrice = new Label { Text = "Price", Location = new Point(20, 140), Width = 120 };
            txtProductPrice = new TextBox { Location = new Point(150, 140), Width = 200 };

            lblProductQuantity = new Label { Text = "Quantity", Location = new Point(20, 180), Width = 120 };
            txtProductQuantity = new TextBox { Location = new Point(150, 180), Width = 200 };

            btnAddProduct = new Button { Text = "Add Product", Location = new Point(20, 240), Width = 100 };
            btnUpdateProduct = new Button { Text = "Update Product", Location = new Point(130, 240), Width = 100 };
            btnDeleteProduct = new Button { Text = "Delete Product", Location = new Point(240, 240), Width = 100 };

            dgvProducts = new DataGridView { Location = new Point(20, 320), Width = 500, Height = 200 };

            // Company Section (Continued)
            lblCompanyName = new Label { Text = "Company Name", Location = new Point(550, 20), Width = 120 };
            txtCompanyName = new TextBox { Location = new Point(700, 20), Width = 200 };

            lblCompanyLocation = new Label { Text = "Location", Location = new Point(550, 60), Width = 120 };
            txtCompanyLocation = new TextBox { Location = new Point(700, 60), Width = 200 };

            btnAddCompany = new Button { Text = "Add Company", Location = new Point(550, 100), Width = 100 };
            btnUpdateCompany = new Button { Text = "Update Company", Location = new Point(660, 100), Width = 100 };
            btnDeleteCompany = new Button { Text = "Delete Company", Location = new Point(770, 100), Width = 100 };
            
            dgvCompanies = new DataGridView { Location = new Point(550, 140), Width = 500, Height = 200 };

            // Inventory Section
            lblInventoryLevel = new Label { Text = "Inventory Level", Location = new Point(550, 350), Width = 120 };
            txtInventoryLevel = new TextBox { Location = new Point(700, 350), Width = 100 };

            dgvInventory = new DataGridView { Location = new Point(550, 380), Width = 500, Height = 170 };

            // Add controls to the form
            this.Controls.Add(lblProductName);
            this.Controls.Add(txtProductName);
            this.Controls.Add(lblProductCategory);
            this.Controls.Add(cmbProductCategory);
            this.Controls.Add(lblProductPrice);
            this.Controls.Add(txtProductPrice);
            this.Controls.Add(lblProductQuantity);
            this.Controls.Add(txtProductQuantity);
            this.Controls.Add(lblCompany);  // New company label
            this.Controls.Add(cmbCompany);  // New company combo box
            this.Controls.Add(btnAddProduct);
            this.Controls.Add(btnUpdateProduct);
            this.Controls.Add(btnDeleteProduct);
            this.Controls.Add(dgvProducts);

            this.Controls.Add(lblCompanyName);
            this.Controls.Add(txtCompanyName);
            this.Controls.Add(lblCompanyLocation);
            this.Controls.Add(txtCompanyLocation);
            this.Controls.Add(btnAddCompany);
            this.Controls.Add(btnUpdateCompany);
            this.Controls.Add(btnDeleteCompany);
            this.Controls.Add(dgvCompanies);

            this.Controls.Add(lblInventoryLevel);
            this.Controls.Add(txtInventoryLevel);
            this.Controls.Add(dgvInventory);

            // Set form properties
            this.Text = "Inventory Management System";
            this.Size = new Size(1080, 600);

            btnAddProduct.Click += btnAddProduct_Click;
            btnUpdateProduct.Click += btnUpdateProduct_Click;
            btnDeleteProduct.Click += btnDeleteProduct_Click;
            btnAddCompany.Click += btnAddCompany_Click;
            btnUpdateCompany.Click += btnUpdateCompany_Click;
            btnDeleteCompany.Click += btnDeleteCompany_Click;
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Ensure a valid row is clicked
                {
                    // Get the selected row
                    DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

                    // Populate the fields with the selected product's information
                    txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                    cmbProductCategory.SelectedItem = row.Cells["Category"].Value.ToString(); // assuming "Category" column exists in the grid
                    txtProductPrice.Text = row.Cells["Price"].Value.ToString();
                    txtProductQuantity.Text = row.Cells["Quantity"].Value.ToString();

                    // Optionally, if you have a Company column, you can populate cmbCompany
                    int companyId = (int)row.Cells["CompanyID"].Value; // assuming "CompanyID" column exists
                    cmbCompany.SelectedValue = companyId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading product data: {ex.Message}");
            }

        }
        private void dgvCompanies_SelectionChanged(object sender, EventArgs e)
        {
            // Check if there is at least one row selected
            if (dgvCompanies.SelectedRows.Count > 0)
            {
                // Get the selected company row
                DataGridViewRow selectedRow = dgvCompanies.SelectedRows[0];

                // Retrieve the company ID
                int companyId = Convert.ToInt32(selectedRow.Cells["CompanyID"].Value);

                // Populate the text boxes with the selected company's data
                txtCompanyName.Text = selectedRow.Cells["CompanyName"].Value.ToString();
                txtCompanyLocation.Text = selectedRow.Cells["Location"].Value.ToString();

                // Now load the inventory data for the selected company
                LoadInventory(companyId);
            }
        }

        private void LoadInventory(int companyId)
        {
            string query = "SELECT ProductName, Quantity, Price FROM Products WHERE CompanyID = @CompanyID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@CompanyID", companyId);

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the data to the dgvInventory DataGridView
                dgvInventory.DataSource = dt;
            }
        }


        private void LoadData()
        {
            LoadProducts();
            LoadCompanies();
        }

        private void LoadProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", connection);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dgvProducts.DataSource = dt;
            }
        }

        private void LoadCompanies()
        {
            string query = "SELECT CompanyID, CompanyName, Location FROM Companies";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the companies to the ComboBox
                cmbCompany.DisplayMember = "CompanyName";  // Display company name
                cmbCompany.ValueMember = "CompanyID";  // Use company ID for selection
                cmbCompany.DataSource = dt;

                // Bind the companies to the DataGridView
                dgvCompanies.DataSource = dt;
            }
        }


        private void LoadCategories()
        {
            string query = "SELECT DISTINCT Category FROM Products"; // Assuming "Category" is the column in the Products table that stores the product categories

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the categories to the ComboBox
                cmbProductCategory.DisplayMember = "Category"; // This is the column name in the DataTable
                cmbProductCategory.ValueMember = "Category";   // You can bind it to any value you want to store for the selected item
                cmbProductCategory.DataSource = dt;
            }
        }

        // Add Product
        // Add Product
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtProductName.Text;
                string category = cmbProductCategory.Text;
                decimal price = decimal.Parse(txtProductPrice.Text);
                int quantity = int.Parse(txtProductQuantity.Text);
                int companyId = (int)cmbCompany.SelectedValue;  // Get the selected company ID

                string query = "INSERT INTO Products (ProductName, Category, Price, Quantity, CompanyID) VALUES (@name, @category, @price, @quantity, @companyId)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@companyId", companyId);  // Insert company ID
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                LoadProducts();  // Reload products after adding
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the product: {ex.Message}");
            }
        }

        // Update Product
        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                int productId = (int)dgvProducts.SelectedRows[0].Cells[0].Value;
                string name = txtProductName.Text;
                string category = cmbProductCategory.Text;

                if (!decimal.TryParse(txtProductPrice.Text, out decimal price))
                {
                    MessageBox.Show("Please enter a valid price.");
                    return;
                }

                if (!int.TryParse(txtProductQuantity.Text, out int quantity))
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                string query = "UPDATE Products SET ProductName = @name, Category = @category, Price = @price, Quantity = @quantity WHERE ProductID = @productId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand(query, connection, transaction);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@category", category);
                            command.Parameters.AddWithValue("@price", price);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.Parameters.AddWithValue("@productId", productId);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Product updated successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }

                LoadProducts();  // Reload products after updating
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the product: {ex.Message}");
            }
        }

        // Delete Product
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                int productId = (int)dgvProducts.SelectedRows[0].Cells[0].Value;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @productId";
                            SqlCommand deleteProductCommand = new SqlCommand(deleteProductQuery, connection, transaction);
                            deleteProductCommand.Parameters.AddWithValue("@productId", productId);
                            deleteProductCommand.ExecuteNonQuery();

                            transaction.Commit();
                            LoadProducts();  // Reload products after deletion
                            MessageBox.Show("Product deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred while deleting the product: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the product: {ex.Message}");
            }
        }

        // Add Company
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCompanyName.Text;
                string location = txtCompanyLocation.Text;

                string query = "INSERT INTO Companies (CompanyName, Location) VALUES (@name, @location)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@location", location);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                LoadCompanies();  // Reload companies after adding
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the company: {ex.Message}");
            }
        }

        // Update Company
        private void btnUpdateCompany_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCompanies.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a company to update.");
                    return;
                }

                int companyId = (int)dgvCompanies.SelectedRows[0].Cells[0].Value;
                string name = txtCompanyName.Text;
                string location = txtCompanyLocation.Text;

                string query = "UPDATE Companies SET CompanyName = @name, Location = @location WHERE CompanyID = @companyId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand(query, connection, transaction);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@location", location);
                            command.Parameters.AddWithValue("@companyId", companyId);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            LoadCompanies();  // Reload companies after updating
                            MessageBox.Show("Company updated successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred while updating the company: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the company: {ex.Message}");
            }
        }

        // Delete Company
        private void btnDeleteCompany_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCompanies.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a company to delete.");
                    return;
                }

                int companyId = (int)dgvCompanies.SelectedRows[0].Cells[0].Value;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string deleteCompanyQuery = "DELETE FROM Companies WHERE CompanyID = @companyId";
                            SqlCommand deleteCompanyCommand = new SqlCommand(deleteCompanyQuery, connection, transaction);
                            deleteCompanyCommand.Parameters.AddWithValue("@companyId", companyId);
                            deleteCompanyCommand.ExecuteNonQuery();

                            transaction.Commit();
                            LoadCompanies();  // Reload companies after deletion
                            MessageBox.Show("Company deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred while deleting the company: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the company: {ex.Message}");
            }
        }

        private void LoadInventoryWithSearch(string searchTerm)
        {
            // Query to search products based on the search term
            string query = "SELECT ProductName, Quantity, Price FROM Products WHERE CompanyID = @CompanyID";

            // If search term is not empty, add a filter for ProductName or Quantity
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " AND (ProductName LIKE @searchTerm OR Quantity LIKE @searchTerm)";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@CompanyID", cmbCompany.SelectedValue);

                // If there's a search term, add it to the parameters
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                // Bind the filtered data to the DataGridView
                dgvInventory.DataSource = dt;
            }
        }


        private void txtInventoryLevel_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtInventoryLevel.Text;

            // Reload inventory with the filter applied
            LoadInventoryWithSearch(searchTerm);
        }

    }
}