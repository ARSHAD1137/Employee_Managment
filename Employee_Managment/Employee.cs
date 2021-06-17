using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Managment
{
    public class Employee
    {
        public float deduction { get; set; }
        public float basicPay { get; set; }
        public float taxablePay { get; set; }
        public float incomeTax { get; set; }
        public float netPay { get; set; }
        public int startDate { get; }
        public string name { get; set; }
        public char gender { get; set; }
        public int phoneNumber { get; set; }
        public string address { get; set; }
    }
}