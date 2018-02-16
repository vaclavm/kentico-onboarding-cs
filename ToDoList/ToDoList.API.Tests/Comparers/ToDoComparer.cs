using System.Collections;

using ToDoList.Contracts.Models;

namespace ToDoList.API.Tests.Comparers
{
    public class ToDoComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var expected = (ToDo) x;
            var actual = (ToDo) y;
            
            if (expected == null || actual == null) 
            {
                return 1;
            }

            if (expected.Id.Equals(actual.Id) && expected.Text.Equals(actual.Text))
            {
                return 0;
            }

            return 1;
        }
    }
}
