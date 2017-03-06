using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitamaClient.Api.Collections
{
    [System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
    public class ReferenceList<TKey, TValue> : IList<TValue>, IReadOnlyList<TValue>
        where TValue : class, IPrimeryKey<TKey>
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Collapsed)]
        private List<TKey> keys = new List<TKey>();
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Collapsed)]
        private ReferenceDictionary<TKey, TValue> reference = References.Get<TKey, TValue>();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        public TValue this[int index]
        {
            get => this.reference[this.keys[index]];
            set
            {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));
                this.keys[index] = value.GetPrimeryKey();
                this.reference.Add(value);
            }
        }

        public int Count => this.keys.Count;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool IsReadOnly => false;

        public void Add(TValue item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            this.keys.Add(item.GetPrimeryKey());
            this.reference.Add(item);
        }

        public void Clear()
        {
            this.keys.Clear();
        }

        public bool Contains(TValue item)
        {
            if(item == null)
                return false;
            return this.keys.Contains(item.GetPrimeryKey());
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for(var i = 0; i < array.Length - arrayIndex && i < this.keys.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach(var item in this.keys)
            {
                yield return this.reference[item];
            }
        }

        public int IndexOf(TValue item)
        {
            if(item == null)
                return -1;
            return this.keys.IndexOf(item.GetPrimeryKey());
        }

        public void Insert(int index, TValue item)
        {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            this.keys.Insert(index, item.GetPrimeryKey());
            this.reference.Add(item);
        }

        public bool Remove(TValue item)
        {
            if(item == null)
                return false;
            return this.keys.Remove(item.GetPrimeryKey());
        }

        public void RemoveAt(int index)
        {
            this.keys.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
