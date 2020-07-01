using PhanMemQuanLyQuanCafe.DAO;
using PhanMemQuanLyQuanCafe.DTO;
using QuanLyQuanCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = PhanMemQuanLyQuanCafe.DTO.Menu;

namespace PhanMemQuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private int selectedTable = 0;
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
            LoadCategory();
            
        }

        #region Method

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            cbSwitchTable.Items.Clear();
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Empty":
                        btn.BackColor = Color.Aqua;
                        cbSwitchTable.Items.Add(item.ID);
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill (int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Menu item in listBillInfo)
            {   
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem); 
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            Thread.CurrentThread.CurrentCulture = culture;
            txbTotalPrice.Text = totalPrice.ToString("c", culture);

        }
        #endregion

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;  
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
            selectedTable = tableID;
        }
        private void fTableManager_Load(object sender, EventArgs e)
        {

        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mnFoodCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbFood_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nmDiscount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            this.Close();
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)mnFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                TableDAO.Instance.CheckIn(table.ID);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
            {
                Table table = lsvBill.Tag as Table;

                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int discount = (int)nmDiscount.Value;

                if (idBill > -1)
                {
                if (MessageBox.Show("Thanh toán hóa đơn?"+ table.Name, "Thông báo",MessageBoxButtons.OKCancel)== System.Windows.Forms.DialogResult.OK)
                    {
                    
                    BillDAO.Instance.CheckOut(idBill, discount);
                    TableDAO.Instance.CheckOut(selectedTable);
                    lsvBill.Items.Clear();
                    LoadTable();
                    }
                }
            }
            private void btnSwitchTable_Click(object sender, EventArgs e)
                    {
                        
                    //Đổi tất cả bill có trong tableID cũ thành tableID của bàn cần chuyển đến

                        Bill bill = BillDAO.Instance.GetUncheckBillDTOByTableID(selectedTable);
                        bill.IDTable = (int)cbSwitchTable.SelectedItem;
                        BillDAO.Instance.UpdateBill(bill);

                    //Set table cũ thành empty
                    TableDAO.Instance.CheckOut(selectedTable);
                    //Set table mới thành occupied
                    TableDAO.Instance.CheckIn(bill.IDTable);
            ShowBill(selectedTable);
            LoadTable();

        }
        #endregion
        
    }
}
