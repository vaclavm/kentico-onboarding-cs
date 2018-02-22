using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Contracts
{
    public interface IDependencyConstruct
    {
        void RegistryType<TFrom, TTo>() where TTo : TFrom;
    }
}
