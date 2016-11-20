using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowOffset.Models
{
    internal class DimensionLayer : ICollection<Dimension>
    {
        private readonly List<Dimension> _dims = new List<Dimension>();

        public int Count
        {
            get { return _dims.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(Dimension item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _dims.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Dimension item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Dimension[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Dimension item)
        {
            throw new NotImplementedException();
        }

        public Dimension this[int index]
        {
            get { return _dims[index]; }
            set { _dims[index] = value; }
        }

        public IEnumerator<Dimension> GetEnumerator()
        {
            return _dims.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
