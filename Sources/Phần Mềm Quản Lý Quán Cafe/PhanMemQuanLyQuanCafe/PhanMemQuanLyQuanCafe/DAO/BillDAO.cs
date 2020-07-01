﻿using PhanMemQuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.Bill where idTable="+ id + " and status = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            else
            return -1;
        }

        public void CheckOut (int id, int discount)
        {
            string query = "UPDATE dbo.Bill SET status =1 , discount = " +discount  +" where id =" +id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
        public Bill GetUncheckBillDTOByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.Bill where idTable=" + id + " and status = 0");

            if (data.Rows.Count > 0)
            {
                return new Bill(data.Rows[0]);
            }
            else
                return null ;
        }

        public int UpdateBill(Bill bill)
        {
            return DataProvider.Instance.ExecuteNonQuery("Update dbo.Bill SET idTable= " + bill.IDTable + " where id = " + bill.ID);
        }
    }
}
