﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCafe.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int Status, int discount = 0)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
           
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];
            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "") 
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Status = (int)row["status"];
            this.Discount = (int)row["discount"];
            this.IDTable = (int)row["idTable"];
        }

        private int discount;

        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        private int status; 

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private DateTime? dateCheckOut;

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }
        private DateTime? dateCheckIn;

        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }

        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }

        }

        private int idTable;

        public int IDTable
        {
            get { return idTable; }
            set { idTable = value;}
        }
    }
}
