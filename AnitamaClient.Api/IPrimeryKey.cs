using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api
{
    public interface IPrimeryKey<T>
    {
        T GetPrimeryKey();
    }
}
