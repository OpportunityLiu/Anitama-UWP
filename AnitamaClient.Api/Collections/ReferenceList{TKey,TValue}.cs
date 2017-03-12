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
        private static ReferenceDictionary<TKey, TValue> reference = References.Get<TKey, TValue>();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Collapsed)]
        protected List<TKey> Keys { get; } = new List<TKey>();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        public TValue this[int index]
        {
            get => reference[this.Keys[index]];
            set
            {
                if(value == null)
                    return;
                SetItem(index, value);
            }
        }

        public int Count => this.Keys.Count;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool IsReadOnly => false;

        protected virtual void InsertItem(int index, TValue item)
        {
            this.Keys.Insert(index, reference.Add(item));
        }

        protected virtual void SetItem(int index, TValue item)
        {
            this.Keys[index] = reference.Add(item);
        }

        protected virtual void RemoveItem(int index)
        {
            this.Keys.RemoveAt(index);
        }

        protected virtual void ClearItems()
        {
            this.Keys.Clear();
        }

        public void Add(TValue item)
        {
            if(item == null)
                return;
            InsertItem(this.Count, item);
        }

        public void Clear()
        {
            ClearItems();
        }

        public bool Contains(TValue item)
        {
            if(item == null)
                return false;
            return this.Keys.Contains(item.GetPrimeryKey());
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for(var i = 0; i < array.Length - arrayIndex && i < this.Keys.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach(var item in this.Keys)
            {
                yield return reference[item];
            }
        }

        public int IndexOf(TValue item)
        {
            if(item == null)
                return -1;
            return this.Keys.IndexOf(item.GetPrimeryKey());
        }

        public void Insert(int index, TValue item)
        {
            if(item == null)
                return;
            InsertItem(index, item);
        }

        public bool Remove(TValue item)
        {
            if(item == null)
                return false;
            var pk = item.GetPrimeryKey();
            var index = this.Keys.FindIndex(k => Equals(k, pk));
            if(index < 0)
                return false;
            RemoveItem(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            RemoveItem(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
