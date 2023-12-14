namespace db
{
    public class Profile
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string address;
        private string city;
        private string zipCode;
        private string albumNumber;
        private string studyStatus;
        private string[] groups;

        public Profile()
        {
            string[] noGroup = { "Brak grup" };
            firstName = "Brak";
            lastName = "Brak";
            phoneNumber = "Brak";
            address = "Brak";
            city = "Brak";
            zipCode = "00-000";
            albumNumber = "Brak";
            studyStatus = "Nieznany";
            groups = noGroup;
        }

        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string City { get { return city; } set { city = value; } }
        public string ZipCode { get { return zipCode; } set { zipCode = value; } }
        public string AlbumNumber { get { return albumNumber; } set { albumNumber = value; } }
        public string StudyStatus { get { return studyStatus; } set { studyStatus = value; } }
        public string[] Groups { get { return groups; } set { groups = value; } }
    }

    public class User
    {
        private string uid;
        private string email;
        private string role;
        private string classId;
        private Profile profile;

        public User()
        {
            uid = null;
            email = null;
            role = null;
            classId = null;
            profile = new Profile();
        }

        public string Uid { get { return uid; } set { uid = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Role { get { return role; } set { role = value; } }
        public string ClassId { get { return classId; } set { classId = value; } }
        public Profile Profile { get { return profile; } set { profile = value; } }
    }
}