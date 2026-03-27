using BVS2.Database;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BVS2.Forms
{
    public class MainForm : Form
    {
        private TabControl tabControl = new TabControl();
        private TabPage tabCustomers = new TabPage();
        private TabPage tabVideos = new TabPage();
        private TabPage tabRentals = new TabPage();
        private TabPage tabReports = new TabPage();

        // Customer controls
        private DataGridView dgvCustomers = new DataGridView();
        private TextBox txtCustFirstName = new TextBox();
        private TextBox txtCustLastName = new TextBox();
        private TextBox txtCustAddress = new TextBox();
        private TextBox txtCustPhone = new TextBox();
        private TextBox txtCustEmail = new TextBox();
        private TextBox txtCustSearch = new TextBox();
        private Button btnCustAdd = new Button();
        private Button btnCustUpdate = new Button();
        private Button btnCustClear = new Button();
        private int selectedCustomerID = -1;

        // Video controls
        private DataGridView dgvVideos = new DataGridView();
        private TextBox txtVideoTitle = new TextBox();
        private TextBox txtVideoQty = new TextBox();
        private TextBox txtVideoSearch = new TextBox();
        private ComboBox cmbVideoCategory = new ComboBox();
        private ComboBox cmbVideoRentalDays = new ComboBox();
        private Label lblVideoPrice = new Label();
        private Button btnVideoAdd = new Button();
        private Button btnVideoUpdate = new Button();
        private Button btnVideoDelete = new Button();
        private Button btnVideoClear = new Button();
        private int selectedVideoID = -1;

        // Rental controls
        private ComboBox cmbRentCustomer = new ComboBox();
        private ComboBox cmbRentVideo = new ComboBox();
        private Label lblRentInfo = new Label();
        private Label lblRentSummary = new Label();
        private Button btnConfirmRent = new Button();
        private DataGridView dgvCurrentRentals = new DataGridView();

        // Return controls
        private DataGridView dgvReturnRentals = new DataGridView();
        private Label lblReturnInfo = new Label();
        private Button btnProcessReturn = new Button();
        private TextBox txtReturnSearch = new TextBox();
        private int selectedRentalID = -1;

        // Report controls
        private DataGridView dgvReportInventory = new DataGridView();
        private DataGridView dgvReportCustomer = new DataGridView();
        private ComboBox cmbReportCustomer = new ComboBox();

        public MainForm()
        {
            Text = "BVS - Bogsy Video Store | Buhangin, Davao City";
            Size = new Size(1100, 700);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9f);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 102, 204)
            };

            var lblHeader = new Label
            {
                Text = "BVS - Bogsy Video Store",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true
            };

            var lblSubHeader = new Label
            {
                Text = "Buhangin, Davao City",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(200, 230, 255),
                Location = new Point(22, 38),
                AutoSize = true
            };

            pnlHeader.Controls.AddRange(new Control[] { lblHeader, lblSubHeader });

            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            tabControl.ItemSize = new Size(150, 35);
            tabControl.SizeMode = TabSizeMode.Fixed;

            tabCustomers.Text = "  Customers";
            tabVideos.Text = "  Videos";
            tabRentals.Text = "  Rentals";
            tabReports.Text = "  Reports";

            tabControl.TabPages.AddRange(new TabPage[] { tabCustomers, tabVideos, tabRentals, tabReports });

            var statusStrip = new StatusStrip { BackColor = Color.FromArgb(0, 102, 204) };
            var lblStatus = new ToolStripStatusLabel("Ready  |  BVS 2024  |  Buhangin, Davao City") { ForeColor = Color.White };
            statusStrip.Items.Add(lblStatus);

            Controls.Add(tabControl);
            Controls.Add(pnlHeader);
            Controls.Add(statusStrip);

            SetupCustomerTab();
            SetupVideoTab();
            SetupRentalTab();
            SetupReportTab();
        }

        private DataGridView MakeGrid()
        {
            var dgv = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f)
            };
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 102, 204);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 30;
            return dgv;
        }

        private Button MakeButton(string text, Color color, Point loc, Size? size = null)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size ?? new Size(130, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Panel MakeFormPanel(int left, int top, int width, int height, string title)
        {
            var pnl = new Panel
            {
                Location = new Point(left, top),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(245, 248, 255)
            };
            pnl.Paint += (s, e) => e.Graphics.DrawRectangle(
                new Pen(Color.FromArgb(0, 102, 204), 2), 0, 0, pnl.Width - 1, pnl.Height - 1);
            pnl.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(10, 10),
                AutoSize = true
            });
            return pnl;
        }

        private void AddFormField(Panel pnl, string labelText, Control field, int top)
        {
            pnl.Controls.Add(new Label
            {
                Text = labelText,
                Location = new Point(10, top),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 60)
            });
            field.Location = new Point(10, top + 18);
            if (field is TextBox tb) { tb.Size = new Size(280, 25); tb.BorderStyle = BorderStyle.FixedSingle; }
            if (field is ComboBox cb) { cb.Size = new Size(280, 25); cb.DropDownStyle = ComboBoxStyle.DropDownList; }
            pnl.Controls.Add(field);
        }

        // ── TAB 1: CUSTOMERS ─────────────────────────────────────────
        private void SetupCustomerTab()
        {
            tabCustomers.BackColor = Color.White;
            tabCustomers.Padding = new Padding(10, 10, 10, 10);

            var lblTitle = new Label
            {
                Text = "Customer Library",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var lblSearch = new Label { Text = "Search:", Location = new Point(10, 50), AutoSize = true };
            txtCustSearch.Location = new Point(65, 47);
            txtCustSearch.Size = new Size(220, 25);
            txtCustSearch.BorderStyle = BorderStyle.FixedSingle;
            txtCustSearch.TextChanged += (s, e) => LoadCustomers(txtCustSearch.Text);

            dgvCustomers = MakeGrid();
            dgvCustomers.Location = new Point(10, 80);
            dgvCustomers.Size = new Size(680, 490);
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;

            var pnlCust = MakeFormPanel(705, 10, 340, 490, "Customer Details");

            string[] labels = { "First Name *", "Last Name *", "Address", "Phone", "Email" };
            TextBox[] fields = { txtCustFirstName, txtCustLastName, txtCustAddress, txtCustPhone, txtCustEmail };
            int top = 45;
            for (int i = 0; i < labels.Length; i++)
            {
                AddFormField(pnlCust, labels[i], fields[i], top);
                top += 55;
            }

            btnCustAdd = MakeButton("Add Customer", Color.FromArgb(0, 153, 76), new Point(10, top + 5));
            btnCustAdd.Click += BtnCustAdd_Click;

            btnCustUpdate = MakeButton("Update", Color.FromArgb(0, 102, 204), new Point(10, top + 50));
            btnCustUpdate.Enabled = false;
            btnCustUpdate.Click += BtnCustUpdate_Click;

            btnCustClear = MakeButton("Clear", Color.FromArgb(150, 150, 150), new Point(155, top + 5));
            btnCustClear.Click += (s, e) => ClearCustomerForm();

            pnlCust.Controls.AddRange(new Control[] { btnCustAdd, btnCustUpdate, btnCustClear });
            tabCustomers.Controls.AddRange(new Control[] { lblTitle, lblSearch, txtCustSearch, dgvCustomers, pnlCust });
            LoadCustomers();
        }

        private void LoadCustomers(string search = "")
        {
            string sql = string.IsNullOrWhiteSpace(search)
                ? "SELECT CustomerID, FirstName, LastName, Phone, Email, Address FROM Customers WHERE IsActive=1 ORDER BY LastName, FirstName"
                : $"SELECT CustomerID, FirstName, LastName, Phone, Email, Address FROM Customers WHERE IsActive=1 AND (FirstName+' '+LastName LIKE '%{search}%' OR Phone LIKE '%{search}%') ORDER BY LastName, FirstName";
            dgvCustomers.DataSource = DatabaseHelper.ExecuteRawQuery(sql);
            if (dgvCustomers.Columns.Contains("CustomerID"))
                dgvCustomers.Columns["CustomerID"]!.Visible = false;
        }

        private void DgvCustomers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0) return;
            var row = dgvCustomers.SelectedRows[0];
            selectedCustomerID = Convert.ToInt32(row.Cells["CustomerID"].Value);
            txtCustFirstName.Text = row.Cells["FirstName"].Value?.ToString() ?? "";
            txtCustLastName.Text = row.Cells["LastName"].Value?.ToString() ?? "";
            txtCustPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
            txtCustEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtCustAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
            btnCustUpdate.Enabled = true;
            btnCustAdd.Enabled = false;
        }

        private void BtnCustAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustFirstName.Text) || string.IsNullOrWhiteSpace(txtCustLastName.Text))
            { MessageBox.Show("First Name and Last Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            DatabaseHelper.ExecuteNonQuery("sp_AddCustomer", new[] {
                new SqlParameter("@FirstName", txtCustFirstName.Text.Trim()),
                new SqlParameter("@LastName",  txtCustLastName.Text.Trim()),
                new SqlParameter("@Address",   txtCustAddress.Text.Trim()),
                new SqlParameter("@Phone",     txtCustPhone.Text.Trim()),
                new SqlParameter("@Email",     txtCustEmail.Text.Trim())
            });
            MessageBox.Show("Customer added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearCustomerForm(); LoadCustomers();
        }

        private void BtnCustUpdate_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustFirstName.Text) || string.IsNullOrWhiteSpace(txtCustLastName.Text))
            { MessageBox.Show("First Name and Last Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            DatabaseHelper.ExecuteNonQuery("sp_UpdateCustomer", new[] {
                new SqlParameter("@CustomerID", selectedCustomerID),
                new SqlParameter("@FirstName",  txtCustFirstName.Text.Trim()),
                new SqlParameter("@LastName",   txtCustLastName.Text.Trim()),
                new SqlParameter("@Address",    txtCustAddress.Text.Trim()),
                new SqlParameter("@Phone",      txtCustPhone.Text.Trim()),
                new SqlParameter("@Email",      txtCustEmail.Text.Trim())
            });
            MessageBox.Show("Customer updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearCustomerForm(); LoadCustomers();
        }

        private void ClearCustomerForm()
        {
            txtCustFirstName.Text = txtCustLastName.Text = txtCustAddress.Text = txtCustPhone.Text = txtCustEmail.Text = "";
            selectedCustomerID = -1;
            btnCustUpdate.Enabled = false;
            btnCustAdd.Enabled = true;
            dgvCustomers.ClearSelection();
        }

        // ── TAB 2: VIDEOS ────────────────────────────────────────────
        private void SetupVideoTab()
        {
            tabVideos.BackColor = Color.White;

            var lblTitle = new Label
            {
                Text = "Video Library",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var lblSearch = new Label { Text = "Search:", Location = new Point(10, 50), AutoSize = true };
            txtVideoSearch.Location = new Point(65, 47);
            txtVideoSearch.Size = new Size(220, 25);
            txtVideoSearch.BorderStyle = BorderStyle.FixedSingle;
            txtVideoSearch.TextChanged += (s, e) => LoadVideos(txtVideoSearch.Text);

            dgvVideos = MakeGrid();
            dgvVideos.Location = new Point(10, 80);
            dgvVideos.Size = new Size(680, 490);
            dgvVideos.SelectionChanged += DgvVideos_SelectionChanged;

            var pnlVideo = MakeFormPanel(705, 10, 340, 520, "Video Details");

            AddFormField(pnlVideo, "Title *", txtVideoTitle, 45);
            AddFormField(pnlVideo, "Category *", cmbVideoCategory, 100);
            cmbVideoCategory.Items.AddRange(new object[] { "VCD", "DVD" });
            cmbVideoCategory.SelectedIndexChanged += (s, e) => {
                if (cmbVideoCategory.SelectedItem == null) return;
                lblVideoPrice.Text = cmbVideoCategory.SelectedItem.ToString() == "VCD"
                    ? "Rental Price: P25.00" : "Rental Price: P50.00";
            };

            AddFormField(pnlVideo, "Rental Days (1-3) *", cmbVideoRentalDays, 155);
            cmbVideoRentalDays.Items.AddRange(new object[] { 1, 2, 3 });

            AddFormField(pnlVideo, "Total Quantity *", txtVideoQty, 210);
            txtVideoQty.Text = "1";

            lblVideoPrice.Text = "Rental Price: --";
            lblVideoPrice.Location = new Point(10, 255);
            lblVideoPrice.Size = new Size(280, 25);
            lblVideoPrice.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblVideoPrice.ForeColor = Color.FromArgb(0, 102, 204);
            pnlVideo.Controls.Add(lblVideoPrice);

            btnVideoAdd = MakeButton("Add Video", Color.FromArgb(0, 153, 76), new Point(10, 290));
            btnVideoAdd.Click += BtnVideoAdd_Click;

            btnVideoUpdate = MakeButton("Update", Color.FromArgb(0, 102, 204), new Point(10, 335));
            btnVideoUpdate.Enabled = false;
            btnVideoUpdate.Click += BtnVideoUpdate_Click;

            btnVideoDelete = MakeButton("Delete", Color.FromArgb(204, 0, 0), new Point(155, 290));
            btnVideoDelete.Enabled = false;
            btnVideoDelete.Click += BtnVideoDelete_Click;

            btnVideoClear = MakeButton("Clear", Color.FromArgb(150, 150, 150), new Point(155, 335));
            btnVideoClear.Click += (s, e) => ClearVideoForm();

            pnlVideo.Controls.AddRange(new Control[] { btnVideoAdd, btnVideoUpdate, btnVideoDelete, btnVideoClear });
            tabVideos.Controls.AddRange(new Control[] { lblTitle, lblSearch, txtVideoSearch, dgvVideos, pnlVideo });
            LoadVideos();
        }

        private void LoadVideos(string search = "")
        {
            string sql = string.IsNullOrWhiteSpace(search)
                ? "SELECT VideoID, Title, Category, TotalQuantity AS [Total], RentalDays AS [Days], RentalPrice AS [Price], QuantityIn AS [In], QuantityOut AS [Out] FROM vw_VideoInventory ORDER BY Title"
                : $"SELECT VideoID, Title, Category, TotalQuantity AS [Total], RentalDays AS [Days], RentalPrice AS [Price], QuantityIn AS [In], QuantityOut AS [Out] FROM vw_VideoInventory WHERE Title LIKE '%{search}%' ORDER BY Title";
            dgvVideos.DataSource = DatabaseHelper.ExecuteRawQuery(sql);
            if (dgvVideos.Columns.Contains("VideoID"))
                dgvVideos.Columns["VideoID"]!.Visible = false;
        }

        private void DgvVideos_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvVideos.SelectedRows.Count == 0) return;
            var row = dgvVideos.SelectedRows[0];
            selectedVideoID = Convert.ToInt32(row.Cells["VideoID"].Value);
            txtVideoTitle.Text = row.Cells["Title"].Value?.ToString() ?? "";
            cmbVideoCategory.SelectedItem = row.Cells["Category"].Value?.ToString();
            cmbVideoRentalDays.SelectedItem = Convert.ToInt32(row.Cells["Days"].Value);
            txtVideoQty.Text = row.Cells["Total"].Value?.ToString() ?? "";
            btnVideoUpdate.Enabled = btnVideoDelete.Enabled = true;
            btnVideoAdd.Enabled = false;
        }

        private void BtnVideoAdd_Click(object? sender, EventArgs e)
        {
            if (!ValidateVideo()) return;
            DatabaseHelper.ExecuteNonQuery("sp_AddVideo", new[] {
                new SqlParameter("@Title",         txtVideoTitle.Text.Trim()),
                new SqlParameter("@Category",      cmbVideoCategory.SelectedItem!.ToString()),
                new SqlParameter("@TotalQuantity", int.Parse(txtVideoQty.Text)),
                new SqlParameter("@RentalDays",    int.Parse(cmbVideoRentalDays.SelectedItem!.ToString()!))
            });
            MessageBox.Show("Video added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearVideoForm(); LoadVideos();
        }

        private void BtnVideoUpdate_Click(object? sender, EventArgs e)
        {
            if (!ValidateVideo()) return;
            DatabaseHelper.ExecuteNonQuery("sp_UpdateVideo", new[] {
                new SqlParameter("@VideoID",       selectedVideoID),
                new SqlParameter("@Title",         txtVideoTitle.Text.Trim()),
                new SqlParameter("@Category",      cmbVideoCategory.SelectedItem!.ToString()),
                new SqlParameter("@TotalQuantity", int.Parse(txtVideoQty.Text)),
                new SqlParameter("@RentalDays",    int.Parse(cmbVideoRentalDays.SelectedItem!.ToString()!))
            });
            MessageBox.Show("Video updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearVideoForm(); LoadVideos();
        }

        private void BtnVideoDelete_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this video?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("sp_DeleteVideo", new[] { new SqlParameter("@VideoID", selectedVideoID) });
                MessageBox.Show("Video deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearVideoForm(); LoadVideos();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool ValidateVideo()
        {
            if (string.IsNullOrWhiteSpace(txtVideoTitle.Text)) { MessageBox.Show("Title is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (cmbVideoCategory.SelectedItem == null) { MessageBox.Show("Select a category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (cmbVideoRentalDays.SelectedItem == null) { MessageBox.Show("Select rental days.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (!int.TryParse(txtVideoQty.Text, out int q) || q < 1) { MessageBox.Show("Quantity must be at least 1.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearVideoForm()
        {
            txtVideoTitle.Text = txtVideoQty.Text = "";
            cmbVideoCategory.SelectedIndex = cmbVideoRentalDays.SelectedIndex = -1;
            lblVideoPrice.Text = "Rental Price: --";
            selectedVideoID = -1;
            btnVideoUpdate.Enabled = btnVideoDelete.Enabled = false;
            btnVideoAdd.Enabled = true;
            dgvVideos.ClearSelection();
        }

        // ── TAB 3: RENTALS ───────────────────────────────────────────
        private void SetupRentalTab()
        {
            tabRentals.BackColor = Color.White;

            var pnlRent = MakeFormPanel(10, 10, 1040, 180, "Rent a Video");

            pnlRent.Controls.Add(new Label { Text = "Customer:", Location = new Point(10, 45), AutoSize = true });
            cmbRentCustomer.Location = new Point(10, 63);
            cmbRentCustomer.Size = new Size(300, 25);
            cmbRentCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRentCustomer.SelectedIndexChanged += CmbRentCustomer_Changed;
            pnlRent.Controls.Add(cmbRentCustomer);

            pnlRent.Controls.Add(new Label { Text = "Video:", Location = new Point(330, 45), AutoSize = true });
            cmbRentVideo.Location = new Point(330, 63);
            cmbRentVideo.Size = new Size(350, 25);
            cmbRentVideo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRentVideo.SelectedIndexChanged += CmbRentVideo_Changed;
            pnlRent.Controls.Add(cmbRentVideo);

            lblRentInfo.Location = new Point(700, 45);
            lblRentInfo.Size = new Size(320, 60);
            lblRentInfo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblRentInfo.ForeColor = Color.FromArgb(0, 102, 204);
            pnlRent.Controls.Add(lblRentInfo);

            btnConfirmRent = MakeButton("Confirm Rental", Color.FromArgb(0, 153, 76), new Point(10, 120), new Size(160, 40));
            btnConfirmRent.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnConfirmRent.Click += BtnConfirmRent_Click;
            pnlRent.Controls.Add(btnConfirmRent);

            lblRentSummary.Location = new Point(200, 115);
            lblRentSummary.Size = new Size(500, 50);
            lblRentSummary.ForeColor = Color.FromArgb(60, 60, 60);
            pnlRent.Controls.Add(lblRentSummary);

            var pnlReturn = MakeFormPanel(10, 200, 1040, 55, "Return a Video");

            pnlReturn.Controls.Add(new Label { Text = "Search:", Location = new Point(10, 25), AutoSize = true });
            txtReturnSearch.Location = new Point(65, 22);
            txtReturnSearch.Size = new Size(200, 25);
            txtReturnSearch.BorderStyle = BorderStyle.FixedSingle;
            txtReturnSearch.TextChanged += (s, e) => LoadReturnRentals(txtReturnSearch.Text);

            lblReturnInfo.Location = new Point(290, 22);
            lblReturnInfo.Size = new Size(500, 25);
            lblReturnInfo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblReturnInfo.ForeColor = Color.DarkRed;

            btnProcessReturn = MakeButton("Process Return", Color.FromArgb(0, 102, 204), new Point(840, 18), new Size(160, 35));
            btnProcessReturn.Enabled = false;
            btnProcessReturn.Click += BtnProcessReturn_Click;

            pnlReturn.Controls.AddRange(new Control[] { txtReturnSearch, lblReturnInfo, btnProcessReturn });

            var lblGrid = new Label
            {
                Text = "Currently Rented Videos (click a row to return)",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(10, 262),
                AutoSize = true
            };

            dgvCurrentRentals = MakeGrid();
            dgvCurrentRentals.Location = new Point(10, 285);
            dgvCurrentRentals.Size = new Size(1040, 250);
            dgvCurrentRentals.SelectionChanged += DgvCurrentRentals_SelectionChanged;

            tabRentals.Controls.AddRange(new Control[] { pnlRent, pnlReturn, lblGrid, dgvCurrentRentals });
            LoadRentalDropdowns();
            LoadReturnRentals();
        }

        private void LoadRentalDropdowns()
        {
            var custDT = DatabaseHelper.ExecuteRawQuery("SELECT CustomerID, FirstName+' '+LastName AS Name, Phone FROM Customers WHERE IsActive=1 ORDER BY LastName, FirstName");
            cmbRentCustomer.DisplayMember = "Name";
            cmbRentCustomer.ValueMember = "CustomerID";
            cmbRentCustomer.DataSource = custDT;
            cmbRentCustomer.SelectedIndex = -1;

            var vidDT = DatabaseHelper.ExecuteRawQuery("SELECT VideoID, Title+' ['+Category+']' AS Display, Category, RentalPrice, RentalDays, QuantityIn FROM vw_VideoInventory WHERE QuantityIn > 0 ORDER BY Title");
            cmbRentVideo.DisplayMember = "Display";
            cmbRentVideo.ValueMember = "VideoID";
            cmbRentVideo.DataSource = vidDT;
            cmbRentVideo.SelectedIndex = -1;
        }

        private void CmbRentCustomer_Changed(object? sender, EventArgs e) => UpdateRentSummary();

        private void CmbRentVideo_Changed(object? sender, EventArgs e)
        {
            if (cmbRentVideo.SelectedItem is not DataRowView rowView) return;
            var row = rowView.Row;
            lblRentInfo.Text = $"Category: {row["Category"]}  |  Price: P{Convert.ToDecimal(row["RentalPrice"]):0.00}\nDays: {row["RentalDays"]}  |  Available: {row["QuantityIn"]}";
            UpdateRentSummary();
        }

        private void UpdateRentSummary()
        {
            if (cmbRentCustomer.SelectedIndex < 0 || cmbRentVideo.SelectedIndex < 0) { lblRentSummary.Text = ""; return; }
            if (cmbRentVideo.SelectedItem is not DataRowView rv) return;
            decimal price = Convert.ToDecimal(rv.Row["RentalPrice"]);
            int days = Convert.ToInt32(rv.Row["RentalDays"]);
            lblRentSummary.Text = $"Fee: P{price:0.00}  |  Duration: {days} day(s)  |  Due: {DateTime.Now.AddDays(days):MM/dd/yyyy}  |  Overdue: P5.00/day";
        }

        private void BtnConfirmRent_Click(object? sender, EventArgs e)
        {
            if (cmbRentCustomer.SelectedIndex < 0 || cmbRentVideo.SelectedIndex < 0)
            { MessageBox.Show("Please select a customer and a video.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int custID = Convert.ToInt32(cmbRentCustomer.SelectedValue);
            int videoID = Convert.ToInt32(cmbRentVideo.SelectedValue);
            string custName = ((DataRowView)cmbRentCustomer.SelectedItem!).Row["Name"].ToString()!;
            decimal price = Convert.ToDecimal(((DataRowView)cmbRentVideo.SelectedItem!).Row["RentalPrice"]);

            if (MessageBox.Show($"Rent to {custName}?\nFee: P{price:0.00}", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DatabaseHelper.ExecuteNonQuery("sp_RentVideo", new[] {
                    new SqlParameter("@CustomerID", custID),
                    new SqlParameter("@VideoID",    videoID)
                });
                MessageBox.Show("Rental confirmed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentalDropdowns();
                LoadReturnRentals();
                lblRentSummary.Text = lblRentInfo.Text = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadReturnRentals(string search = "")
        {
            string sql = string.IsNullOrWhiteSpace(search)
                ? "SELECT RentalID, CustomerName, VideoTitle, Category, RentalDate, DueDate, RentalFee AS [Fee], DaysOverdue AS [Days Overdue], Status FROM vw_CustomerRentals WHERE Status='Rented' ORDER BY DueDate"
                : $"SELECT RentalID, CustomerName, VideoTitle, Category, RentalDate, DueDate, RentalFee AS [Fee], DaysOverdue AS [Days Overdue], Status FROM vw_CustomerRentals WHERE Status='Rented' AND (CustomerName LIKE '%{search}%' OR VideoTitle LIKE '%{search}%') ORDER BY DueDate";

            dgvCurrentRentals.DataSource = DatabaseHelper.ExecuteRawQuery(sql);
            if (dgvCurrentRentals.Columns.Contains("RentalID"))
                dgvCurrentRentals.Columns["RentalID"]!.Visible = false;

            foreach (DataGridViewRow row in dgvCurrentRentals.Rows)
                if (Convert.ToInt32(row.Cells["Days Overdue"].Value) > 0)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);

            selectedRentalID = -1;
            btnProcessReturn.Enabled = false;
            lblReturnInfo.Text = "";
        }

        private void DgvCurrentRentals_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCurrentRentals.SelectedRows.Count == 0) return;
            var row = dgvCurrentRentals.SelectedRows[0];
            selectedRentalID = Convert.ToInt32(row.Cells["RentalID"].Value);
            int overdue = Convert.ToInt32(row.Cells["Days Overdue"].Value);
            decimal fee = Convert.ToDecimal(row.Cells["Fee"].Value);
            decimal overdueFee = overdue * 5m;
            decimal total = fee + overdueFee;
            lblReturnInfo.Text = overdue > 0
                ? $"OVERDUE by {overdue} day(s)! Overdue Fee: P{overdueFee:0.00} | Total: P{total:0.00}"
                : $"On time. Total Fee: P{total:0.00}";
            btnProcessReturn.Enabled = true;
        }

        private void BtnProcessReturn_Click(object? sender, EventArgs e)
        {
            if (selectedRentalID < 0) return;
            var row = dgvCurrentRentals.SelectedRows[0];
            string cust = row.Cells["CustomerName"].Value.ToString()!;
            string vid = row.Cells["VideoTitle"].Value.ToString()!;
            if (MessageBox.Show($"Return '{vid}' from {cust}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("sp_ReturnVideo", new[] { new SqlParameter("@RentalID", selectedRentalID) });
                decimal overdueFee = 0, totalFee = 0;
                if (dt.Rows.Count > 0) { overdueFee = Convert.ToDecimal(dt.Rows[0]["OverdueFee"]); totalFee = Convert.ToDecimal(dt.Rows[0]["TotalFee"]); }
                string msg = overdueFee > 0 ? $"Returned!\nOverdue Fee: P{overdueFee:0.00}\nTotal: P{totalFee:0.00}" : $"Returned!\nTotal: P{totalFee:0.00}";
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentalDropdowns();
                LoadReturnRentals();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── TAB 4: REPORTS ───────────────────────────────────────────
        private void SetupReportTab()
        {
            tabReports.BackColor = Color.White;

            var lblInv = new Label
            {
                Text = "Video Inventory Report (Alphabetical)",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var btnRefreshInv = MakeButton("Refresh", Color.FromArgb(0, 102, 204), new Point(10, 35), new Size(100, 30));
            btnRefreshInv.Click += (s, e) => LoadInventoryReport();

            var btnPrintInv = MakeButton("Print", Color.FromArgb(108, 117, 125), new Point(120, 35), new Size(100, 30));
            btnPrintInv.Click += BtnPrintInventory_Click;

            dgvReportInventory = MakeGrid();
            dgvReportInventory.Location = new Point(10, 75);
            dgvReportInventory.Size = new Size(500, 470);

            var lblCust = new Label
            {
                Text = "Customer Rentals Report",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(525, 10),
                AutoSize = true
            };

            var lblFilter = new Label { Text = "Filter:", Location = new Point(525, 40), AutoSize = true };
            cmbReportCustomer.Location = new Point(567, 37);
            cmbReportCustomer.Size = new Size(220, 25);
            cmbReportCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReportCustomer.SelectedIndexChanged += (s, e) => LoadCustomerRentalsReport();

            var btnPrintCust = MakeButton("Print", Color.FromArgb(108, 117, 125), new Point(800, 35), new Size(100, 30));
            btnPrintCust.Click += BtnPrintCustomerRentals_Click;

            dgvReportCustomer = MakeGrid();
            dgvReportCustomer.Location = new Point(525, 75);
            dgvReportCustomer.Size = new Size(520, 470);

            tabReports.Controls.AddRange(new Control[] { lblInv, btnRefreshInv, btnPrintInv, dgvReportInventory, lblCust, lblFilter, cmbReportCustomer, btnPrintCust, dgvReportCustomer });

            LoadInventoryReport();
            LoadReportCustomers();
            LoadCustomerRentalsReport();
        }

        private void LoadInventoryReport()
        {
            var dt = DatabaseHelper.ExecuteRawQuery("SELECT Title, Category, TotalQuantity AS [Total], QuantityIn AS [In], QuantityOut AS [Out], RentalPrice AS [Price], RentalDays AS [Days] FROM vw_VideoInventory ORDER BY Title ASC");
            dgvReportInventory.DataSource = dt;
            foreach (DataGridViewRow row in dgvReportInventory.Rows)
            {
                int inQty = Convert.ToInt32(row.Cells["In"].Value);
                int outQty = Convert.ToInt32(row.Cells["Out"].Value);
                if (inQty == 0) row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                else if (outQty > 0) row.DefaultCellStyle.BackColor = Color.FromArgb(210, 230, 255);
            }
        }

        private void LoadReportCustomers()
        {
            var dt = new DataTable();
            dt.Columns.Add("CustomerID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Rows.Add(0, "-- All Customers --");
            foreach (DataRow row in DatabaseHelper.ExecuteRawQuery("SELECT CustomerID, FirstName+' '+LastName AS Name FROM Customers WHERE IsActive=1 ORDER BY LastName, FirstName").Rows)
                dt.Rows.Add(row["CustomerID"], row["Name"]);
            cmbReportCustomer.DisplayMember = "Name";
            cmbReportCustomer.ValueMember = "CustomerID";
            cmbReportCustomer.DataSource = dt;
            cmbReportCustomer.SelectedIndex = 0;
        }

        private void LoadCustomerRentalsReport()
        {
            string where = "";
            if (cmbReportCustomer.SelectedValue != null && Convert.ToInt32(cmbReportCustomer.SelectedValue) > 0)
                where = $"AND CustomerID = {cmbReportCustomer.SelectedValue}";
            var dt = DatabaseHelper.ExecuteRawQuery($"SELECT CustomerName AS [Customer], VideoTitle AS [Video], Category, RentalDate AS [Rented], DueDate AS [Due], RentalFee AS [Fee], DaysOverdue AS [Days Overdue], Status FROM vw_CustomerRentals WHERE Status='Rented' {where} ORDER BY CustomerName");
            dgvReportCustomer.DataSource = dt;
            foreach (DataGridViewRow row in dgvReportCustomer.Rows)
                if (Convert.ToInt32(row.Cells["Days Overdue"].Value) > 0)
                { row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220); row.DefaultCellStyle.ForeColor = Color.DarkRed; }
        }

        private void BtnPrintInventory_Click(object? sender, EventArgs e)
        {
            var pd = new PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                var g = ev.Graphics!;
                var font = new Font("Courier New", 10f);
                var bold = new Font("Courier New", 10f, FontStyle.Bold);
                int y = 40;
                g.DrawString("BVS - Bogsy Video Store", new Font("Courier New", 14f, FontStyle.Bold), Brushes.Black, 180, y); y += 30;
                g.DrawString("Video Inventory Report", new Font("Courier New", 11f), Brushes.Black, 200, y); y += 20;
                g.DrawString($"Generated: {DateTime.Now:MM/dd/yyyy hh:mm tt}", font, Brushes.Gray, 200, y); y += 30;
                g.DrawLine(Pens.Black, 40, y, 760, y); y += 10;
                g.DrawString($"{"Title",-35}{"Category",-12}{"Total",-8}{"In",-8}{"Out",-8}", bold, Brushes.Black, 40, y); y += 20;
                g.DrawLine(Pens.Black, 40, y, 760, y); y += 8;
                foreach (DataGridViewRow row in dgvReportInventory.Rows)
                {
                    string title = row.Cells["Title"].Value.ToString()!;
                    if (title.Length > 33) title = title[..30] + "...";
                    g.DrawString($"{title,-35}{row.Cells["Category"].Value,-12}{row.Cells["Total"].Value,-8}{row.Cells["In"].Value,-8}{row.Cells["Out"].Value,-8}", font, Brushes.Black, 40, y);
                    y += 18;
                }
            };
            using var dlg = new PrintPreviewDialog { Document = pd, Width = 900, Height = 700 };
            dlg.ShowDialog();
        }

        private void BtnPrintCustomerRentals_Click(object? sender, EventArgs e)
        {
            var pd = new PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                var g = ev.Graphics!;
                var font = new Font("Courier New", 9f);
                var bold = new Font("Courier New", 9f, FontStyle.Bold);
                int y = 40;
                g.DrawString("BVS - Bogsy Video Store", new Font("Courier New", 14f, FontStyle.Bold), Brushes.Black, 160, y); y += 28;
                g.DrawString("Customer Rentals Report", new Font("Courier New", 11f), Brushes.Black, 180, y); y += 22;
                g.DrawString($"Generated: {DateTime.Now:MM/dd/yyyy hh:mm tt}", font, Brushes.Gray, 200, y); y += 28;
                g.DrawLine(Pens.Black, 40, y, 760, y); y += 10;
                g.DrawString($"{"Customer",-22}{"Video",-28}{"Cat",-6}{"Due",-12}{"Fee",-8}{"Status"}", bold, Brushes.Black, 40, y); y += 20;
                g.DrawLine(Pens.Black, 40, y, 760, y); y += 8;
                foreach (DataGridViewRow row in dgvReportCustomer.Rows)
                {
                    string cust = row.Cells["Customer"].Value.ToString()!; if (cust.Length > 20) cust = cust[..17] + "...";
                    string vid = row.Cells["Video"].Value.ToString()!; if (vid.Length > 26) vid = vid[..23] + "...";
                    string due = Convert.ToDateTime(row.Cells["Due"].Value).ToString("MM/dd/yy");
                    string fee = "P" + Convert.ToDecimal(row.Cells["Fee"].Value).ToString("0.00");
                    int overdue = Convert.ToInt32(row.Cells["Days Overdue"].Value);
                    string status = row.Cells["Status"].Value.ToString()!;
                    if (overdue > 0) status += $"({overdue}d late)";
                    var brush = overdue > 0 ? Brushes.DarkRed : Brushes.Black;
                    g.DrawString($"{cust,-22}{vid,-28}{row.Cells["Category"].Value,-6}{due,-12}{fee,-8}{status}", font, brush, 40, y);
                    y += 18;
                }
            };
            using var dlg = new PrintPreviewDialog { Document = pd, Width = 900, Height = 700 };
            dlg.ShowDialog();
        }
    }
}