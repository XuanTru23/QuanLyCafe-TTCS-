using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource categoryList = new BindingSource();

        BindingSource foodList = new BindingSource();

        BindingSource tableList = new BindingSource();

        BindingSource accountList = new BindingSource();
        internal Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            dgvViewFood.DataSource = foodList;
            dgvViewCategory.DataSource = categoryList;
            dgvViewAccount.DataSource = accountList;
            dgvViewTable.DataSource = tableList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
            LoadListFood();
            LoadListCategory();
            LoadListTable();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddCategoryBinding();
            AddTableBinding();
            AddAccountBinding();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDay.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region eventsTable
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.GetListTable();
            btnLuuTable.Enabled = false;
            btnHuyTable.Enabled = false;
        }
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dgvViewTable.DataSource, "ID"));
            txbTableName.DataBindings.Add(new Binding("Text", dgvViewTable.DataSource, "Name"));
            cbTableSatus.DataBindings.Add(new Binding("Text", dgvViewTable.DataSource, "Status"));

        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
        }
        private void btnTL_Click(object sender, EventArgs e)
        {
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
        }
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            btnDeleteTable.Enabled = false;
            btnEditTable.Enabled = false;
            btnShowTable.Enabled = false;
            btnLuuTable.Enabled = true;
            btnHuyTable.Enabled = true;
            txbTableID.Text = "";
            txbTableName.Text = "";
        }
        private void btnLuuTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;


            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
            btnAddTable.Enabled = true;
            btnDeleteTable.Enabled = true;
            btnEditTable.Enabled = true;
            btnShowTable.Enabled = true;
            btnLuuTable.Enabled = false;
            btnHuyTable.Enabled = false;
            txbTableID.Text = "";
            txbTableName.Text = "";

        }
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadListTable();
                    if (deleteTable != null)
                        deleteTable(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn");
                }
            }
        }
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            int id = Convert.ToInt32(txbTableID.Text);
            string status = cbTableSatus.Text;
            if (TableDAO.Instance.UpdateTable(id, name, status))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadListTable();
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        #endregion
        

        #region EventFood 
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dgvViewFood.DataSource, "Name"));
            txbFoodID.DataBindings.Add(new Binding("Text", dgvViewFood.DataSource, "ID"));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvViewFood.DataSource, "Price"));
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
            dgvViewFood.Columns[0].HeaderText = "Giá";
            dgvViewFood.Columns[1].HeaderText = "ID danh mục";
            dgvViewFood.Columns[2].HeaderText = "Tên món ăn";
            dgvViewFood.Columns[3].HeaderText = "ID";
            txbFoodID.Enabled = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }
        private void ResetValueFood()
        {
            txbFoodName.Text = "";
            txbFoodID.Text = "";
            cbFoodCategory.Text = "";
            nmFoodPrice.Value = 0;
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dgvViewFood.SelectedCells.Count > 0)
            {
                int id = (int)dgvViewFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbFoodCategory.SelectedItem = cateogory;

                int index = -1;
                int i = 0;
                foreach (Category item in cbFoodCategory.Items)
                {
                    if (item.ID == cateogory.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbFoodCategory.SelectedIndex = index;
            }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            btnDeleteFood.Enabled = false;
            btnEditFood.Enabled = false;
            btnShowFood.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            ResetValueFood();


        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
            ResetValueFood();
            btnAddFood.Enabled = true;
            btnDeleteFood.Enabled = true;
            btnEditFood.Enabled = true;
            btnShowFood.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetValueFood();

            btnAddFood.Enabled = true;
            btnDeleteFood.Enabled = true;
            btnEditFood.Enabled = true;
            btnShowFood.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(txbFoodID.Text);
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (FoodDAO.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công");
                    LoadListFood();
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa thức ăn");
                }
            }
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        #endregion

        #region eventsCategory
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dgvViewCategory.DataSource, "ID"));
            txbCategoryName.DataBindings.Add(new Binding("Text", dgvViewCategory.DataSource, "Name"));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
            txbCategoryID.Enabled = false;
            btnHuyCategory.Enabled = false;
            btnLuuCategory.Enabled = false;
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            txbCategoryName.Text = "";
            btnDeleteCategory.Enabled = false;
            btnEditCategory.Enabled = false;
            btnShowCategory.Enabled = false;
            btnHuyCategory.Enabled = true;
            btnLuuCategory.Enabled = true;

            
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa món thành công");
                    LoadListCategory();
                    if (deleteCategory != null)
                        deleteCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa thức ăn");
                }
            }
            
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadListFood();
                if (updateCategory != null)
                    updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục");
            }
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        private void btnLuuCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadListCategory();
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
            LoadListFood();
            btnHuyCategory.Enabled = false;
            btnLuuCategory.Enabled = false;
            btnShowCategory.Enabled = true;
            btnDeleteCategory.Enabled = true;
            btnEditCategory.Enabled = true;
            btnAddAccount.Enabled = true;
            txbCategoryID.Text = "";
            txbCategoryName.Text = "";
            LoadListCategory();
        }
        private void btnHuyCategory_Click(object sender, EventArgs e)
        {
            txbCategoryName.Text = "";
            btnLuuCategory.Enabled = false;
            btnHuyCategory.Enabled = false;
            btnEditCategory.Enabled = true;
            btnDeleteCategory.Enabled = true;
            btnShowCategory.Enabled = true;

        }

        #endregion

        #region eventsAccount
        void AddAccountBinding()
        {
            txbUsername.DataBindings.Add(new Binding("Text", dgvViewAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dgvViewAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nudTypeAccount.DataBindings.Add(new Binding("Value", dgvViewAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
            button3.Enabled = false;
            button4.Enabled = false;
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }
        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nudTypeAccount.Value;

            AddAccount(userName, displayName, type);
            btnDeleteAccount.Enabled = true;
            btnEditAccount.Enabled = true;
            btnShowAccount.Enabled = true;
            btnResetPassWord.Enabled = true;
            btnAddAccount.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            txbUsername.Text = "";
            txbDisplayName.Text = "";
            nudTypeAccount.Value = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            btnDeleteAccount.Enabled = true;
            btnEditAccount.Enabled = true;
            btnShowAccount.Enabled = true;
            btnResetPassWord.Enabled = true;
            btnAddAccount.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            txbUsername.Text = "";
            txbDisplayName.Text = "";
            nudTypeAccount.Value = 0;
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;

            DeleteAccount(userName);
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nudTypeAccount.Value;

            EditAccount(userName, displayName, type);
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            btnDeleteAccount.Enabled = false;
            btnEditAccount.Enabled = false;
            btnShowAccount.Enabled = false;
            btnResetPassWord.Enabled = false;
            btnAddAccount.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            txbUsername.Text = "";
            txbDisplayName.Text = "";
            nudTypeAccount.Value = 0;
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;

            ResetPass(userName);
        }

        #endregion

        private void btnHuyTable_Click(object sender, EventArgs e)
        {
            btnAddTable.Enabled = true;
            btnDeleteTable.Enabled = true;
            btnEditTable.Enabled = true;
            btnShowTable.Enabled = true;
            btnHuyTable.Enabled = false;
            btnLuuTable.Enabled = false;
            txbTableID.Text = "";
            txbTableName.Text = "";
            cbTableSatus.Text = "";
        }

        
    }
}

