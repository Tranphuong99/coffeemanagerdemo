﻿using PhanMemQuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;

        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
        public void CheckOut(int id)
        {
            string query = "UPDATE dbo.TableFood SET status ='Empty' where id = " +id;
            DataProvider.Instance.ExecuteNonQuery(query);
            
        }
        public void CheckIn(int id)
        {
            string query = "UPDATE dbo.TableFood SET status ='Occupied' where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);

        }
    }
}
