using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace AnitamaClient.Api
{
    public interface IPopulatable
    {
        bool NeedPopulate { get; }
        IAsyncAction PopulateAsync();
    }
}
