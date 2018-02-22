using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

using ToDoList.API.Models;

namespace ToDoList.API.Tests.Utilities
{
    internal static class EqualConstraintExtensions
    {
        public static EqualConstraint UsingToDoComparer(this EqualConstraint constraint)
            => constraint.Using(ToDoComparer.Instance.Value);

        private class ToDoComparer : IEqualityComparer<ToDoModel>
        {
            public static Lazy<ToDoComparer> Instance 
                => new Lazy<ToDoComparer>(() => new ToDoComparer());

            public bool Equals(ToDoModel x, ToDoModel y)
                => x?.Id == y?.Id && x?.Text == y?.Text;

            public int GetHashCode(ToDoModel obj)
                => obj.Id.GetHashCode();
        }
    }
}
