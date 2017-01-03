using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCore
{
    public class ListCircular<T>
    {
        private int _max, _idx;
        private List<T> _lst;

        public int Max
        {
            get { return this._max; }
            set {
                int cmax = ((this._max > value) ? (this._max - value) : 0);
                this._max = value;
                if (cmax > 0)
                {
                    for (int i = 0; i <= cmax; i++)
                    {
                        this._lst.RemoveAt(i);
                    }
                }
            }
        }
        public int Count
        {
            get { return this._lst.Count; }
        }

        public ListCircular(int max)
        {
            this._idx = 0;
            this._max = max;
            this._lst = new List<T>();
        }
        public void Add(T ele)
        {
            if (this._lst.Count > this._max)
            {
                this._lst.RemoveAt(0);
            }
            this._lst.Add(ele);
            this._idx = (this._lst.Count - 1);
        }
        public void Clear()
        {
            this._lst.Clear();
            this._idx = 0;
        }
        public T Next()
        {
            if ((this._lst.Count - 1) >= this._idx)
            {
                this._idx = 0;
            }
            else
            {
                this._idx++;
            }
            return this._Return(this._idx);
        }
        public T Prev()
        {
            if (this._idx <= 0 )
            {
                this._idx = (this._lst.Count - 1);
            }
            else
            {
                this._idx--;
            }
            return this._Return(this._idx);
        }
        public T Last()
        {
            return this._Return((this._lst.Count - 1));
        }
        public bool CheckRange(string src, int range)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return false;
            }
            int cnt = (this._lst.Count - 1);
            range = ((range > cnt) ? cnt : range);
            for (int i = 0; i < range; i++)
            {
                if (src.Equals(this._lst[(cnt - i)]))
                {
                    return true;
                }
            }
            return false;
        }
        internal T _Return(int idx)
        {
            if (this._lst.Count <= 0)
            {
                return default(T);
            }
            return this._lst[idx];
        }
    }
}
