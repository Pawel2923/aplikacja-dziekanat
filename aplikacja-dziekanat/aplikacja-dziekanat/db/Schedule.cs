namespace db
{
    public class Schedule
    {
        private string classType;
        private double duration;
        private string name;
        private string room;
        private string teacher;
        private string timeStart;

        public string ClassType { get { return classType; } set { classType = value; } }
        public double Duration { get { return duration; } set { duration = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Room { get { return room; } set { room = value; } }
        public string Teacher { get { return teacher; } set { teacher = value; } }
        public string TimeStart { get { return timeStart; } set { timeStart = value; } }
    }
}