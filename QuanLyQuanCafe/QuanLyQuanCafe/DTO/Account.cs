using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    internal class Account
    {
        public Account(string userName, string displayName, int type, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.Type = (int)row["type"];
            this.Password = row["password"].ToString();
        }

        private int type;

        private string password;
    
        private string displayName;

        private string userName;

        public int Type 
        {
            get { return type; }
            set { type = value; }
        }

        public string Password 
        {
            get { return password; }
            set { password = value; }
        }

        public string DisplayName 
        {
            get { return displayName; }
            set { displayName = value; 
            }
        }

        public string UserName 
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
