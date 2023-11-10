using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamarinFirebase.Model
{
    public class User
    {
        private int uid;
        private string email;
        private bool isAdmin;
        private bool isLecturer;

        public int Uid { get { return uid; } set { uid = value; } }
        public string Email { get { return email; } set { email = value; } }
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; } }
        public bool IsLecturer { get { return isLecturer; } set { isLecturer = value; } }
    }
}