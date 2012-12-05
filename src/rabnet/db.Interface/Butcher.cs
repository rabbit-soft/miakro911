using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class ButcherDate : IData
    {
        public DateTime Date;
        public int Victims;
        public int Products;
        public ButcherDate(DateTime Date, int victims, int products)
        {
            this.Date = Date;
            this.Victims = victims;
            this.Products = products;
        }
    }

    public class sMeat
    {
        public int Id;
        public DateTime Date;
        public string ProductType;
        public float Amount;
        public string Units;
        public bool Today;
        public string User;

        public sMeat(int id, DateTime date, string prodType, float amount, string unit, bool today, string user)
        {
            this.Id = id;
            this.Date = date;
            this.ProductType = prodType;
            this.Amount = amount;
            this.Units = unit;
            this.Today = today;
            this.User = user;
        }
    }

    //public class ScalePLUSummary
    //{
    //    private int _id;
    //    private int _prodId;
    //    public int Id { get { return _id; } }
    //    public int ProdId
    //    {
    //        get { return _prodId; }
    //    }
    //    public DateTime Date;
    //    public string ProdName;
    //    public int TotalSell;
    //    public int TotalSumm;
    //    public int TotalWeight;
    //    public DateTime Cleared;

    //    public ScalePLUSummary(int id, DateTime date, int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime clear)
    //    {
    //        this._id = id;
    //        this._prodId = prodid;
    //        this.Date = date;
    //        this.ProdName = prodname;
    //        this.TotalSell = tsell;
    //        this.TotalSumm = tsumm;
    //        this.TotalWeight = tweight;
    //        this.Cleared = clear;
    //    }
    //}
}
