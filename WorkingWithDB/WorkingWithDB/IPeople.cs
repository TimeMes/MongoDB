using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithDB
{
    public interface IPeople
    {
        string Name { get; set; }
        int Age {  get; set; }
        int Balance {  get; set; }

        string ToString();
    }
}
