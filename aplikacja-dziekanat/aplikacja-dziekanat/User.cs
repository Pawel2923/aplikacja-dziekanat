using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamarinFirebase.Model
{
    public class User
    {
        private string email;
        private bool isAdmin;
        private bool isTeacher;

        public string Email { get { return email; } set { email = value; } }
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; } }
        public bool IsTeacher { get { return isTeacher; } set { isTeacher = value; } }
    }
}