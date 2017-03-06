using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api.Collections
{
    [System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
    public class ReferenceDictionary<TKey, TValue>
        : ICollection<TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Dictionary<TKey, TValue> dic;

        public ReferenceDictionary(IEqualityComparer<TKey> comparer)
        {
            this.dic = new Dictionary<TKey, TValue>(comparer);
        }

        public ReferenceDictionary()
        {
            this.dic = new Dictionary<TKey, TValue>();
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public TValue this[TKey key]
        {
            get
            {
                if(key == null)
                    return null;
                if(this.dic.TryGetValue(key, out var value))
                    return value;
                return null;
            }
            set => this.Add(value);
        }

        public TKey Add(TValue item)
        {
            if(item == null)
                return default(TKey);
            var k = item.GetPrimeryKey();
            this.dic[k] = item;
            return k;
        }

        public bool Contains(TValue item)
        {
            if(item == null)
                return false;
            return this.dic.ContainsKey(item.GetPrimeryKey());
        }

        public bool Remove(TValue item)
        {
            if(item == null)
                return false;
            return this.dic.Remove(item.GetPrimeryKey());
        }

        public bool ContainsKey(TKey key)
        {
            if(key == null)
                return false;
            return this.dic.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if(key == null)
                return false;
            return this.dic.Remove(key);
        }

        public void Clear()
        {
            this.dic.Clear();
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => this.dic.Count;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.dic.Keys;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.dic.Values;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.dic.Keys;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.dic.Values;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        bool ICollection<TValue>.IsReadOnly => false;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => this[key];

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            this.Add(value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.dic.Contains(item);
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return this.ContainsKey(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.dic).CopyTo(array, arrayIndex);
        }

        void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
        {
            this.dic.Values.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return this.dic.TryGetValue(key, out value);
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return this.dic.TryGetValue(key, out value);
        }
        
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this.dic.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public IEnumerator<TValue> GetEnumerator()
        {
            return this.dic.Values.GetEnumerator();
        }

        void ICollection<TValue>.Add(TValue item)
        {
            this.Add(item);
        }
    }
}
