namespace db
{
    public class User
    {
        private string uid;
        private string email;
        private bool isAdmin;
        private bool isTeacher;
        private string classId;

        public User()
        {
            uid = null;
            email = null;
            isAdmin = false;
            isTeacher = false;
            classId = null;
        }

        public string Uid { get { return uid; } set { uid = value; } }
        public string Email { get { return email; } set { email = value; } }
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; } }
        public bool IsTeacher { get { return isTeacher; } set { isTeacher = value; } }
        public string ClassId { get { return classId; } set { classId = value; } }
    }
}