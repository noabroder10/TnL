using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TnL_DAL;

namespace TnL_BLL
{
    public class User
    {
        public enum UserType
        {
            student,
            teacher,
            manager
        }

        private string fname;
        private string lname;
        private string pass;
        private string username;
        private string pic;
        private UserType type; //UserType.student, UserType.teacher, UserType.manager

        public User (string fname, string lname, string pass, string username, string pic, UserType type)
        {
            this.fname = fname;
            this.lname = lname;
            this.pass = pass;
            this.username = username;
            this.pic = pic;
            this.type = type;
        }
        //public User (string username)
        //{
        //    this.
        //}




    }
}
