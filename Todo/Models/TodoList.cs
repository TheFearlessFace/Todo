using SQLite;
using System;

namespace Todo
{
    public class TodoList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool Done { get; set; }
    }
}

