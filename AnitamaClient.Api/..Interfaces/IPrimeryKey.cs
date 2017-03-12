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

    public class PrimeryKeyEqulityComparer<TKey, TValue> : IEqualityComparer<TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        public static PrimeryKeyEqulityComparer<TKey, TValue> Default { get; } = new PrimeryKeyEqulityComparer<TKey, TValue>(EqualityComparer<TKey>.Default);

        private readonly IEqualityComparer<TKey> keyComparer;

        public PrimeryKeyEqulityComparer(IEqualityComparer<TKey> keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        public bool Equals(TValue x, TValue y)
        {
            if(x == null)
            {
                if(y == null)
                    return true;
                return false;
            }
            if(y == null)
                return false;
            return this.keyComparer.Equals(x.GetPrimeryKey(), y.GetPrimeryKey());
        }

        public int GetHashCode(TValue obj)
        {
            if(obj == null)
                return 0;
            return this.keyComparer.GetHashCode(obj.GetPrimeryKey());
        }
    }
}
