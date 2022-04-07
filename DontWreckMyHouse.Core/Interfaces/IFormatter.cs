using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IFormatter<T>
    {
        public T Deserialize(string data);
        public string Serialize(T data);
    }
}
