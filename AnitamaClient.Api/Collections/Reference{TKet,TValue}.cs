using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api.Collections
{
    internal class Reference<TKey, TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        private static ReferenceDictionary<TKey, TValue> dic = References.Get<TKey, TValue>();

        public TKey Key { get; set; }

        public TValue Value
        {
            get => dic[Key];
            set
            {
                if(value == null)
                    this.Key = default(TKey);
                else
                    this.Key = dic.Add(value);
            }
        }
    }
}
