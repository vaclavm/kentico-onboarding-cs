using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using ToDoList.Contracts.Models;

namespace ToDoList.Api.Tests.Utilities
{
    internal static class EqualConstraintExtensions
    {
        public static EqualConstraint UsingToDoComparer(this EqualConstraint constraint)
            => constraint.Using(ToDoComparer.Instance.Value);

        private class ToDoComparer : IEqualityComparer<ToDo>
        {
            private ToDoComparer() { }

            public static Lazy<ToDoComparer> Instance 
                => new Lazy<ToDoComparer>(() => new ToDoComparer());

            public bool Equals(ToDo x, ToDo y)
                => x?.Id == y?.Id && x?.Text == y?.Text;

            public int GetHashCode(ToDo obj)
                => obj.Id.GetHashCode();
        }
    }
}
