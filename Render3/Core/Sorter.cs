using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render3.Core
{
    public class Sorter<T, U> where T : IComparable<T>
    {
        public class SorterNode
        {
            public SorterNode left;
            public T key;
            public U value;
            public SorterNode right;
            public IEnumerable<KeyValuePair<T, U>> Walk()
            {
                if (left != null)
                    foreach (KeyValuePair<T, U> a in left.Walk())
                        yield return a;
                yield return new KeyValuePair<T, U>(key, value);
                if (right != null)
                    foreach (KeyValuePair<T, U> a in right.Walk())
                        yield return a;
            }
            public void Add(T key, U value)
            {
                int a = key.CompareTo(this.key);
                if (a>0)
                {
                    if (right != null)
                    {
                        right.Add(key,value);
                    } else
                    {
                        right = new SorterNode() { key = key, value = value };
                    }
                } else
                {
                    if (left != null)
                    {
                        left.Add(key, value);
                    }
                    else
                    {
                        left = new SorterNode() { key = key, value = value };
                    }
                }
            }
        }
        private SorterNode root=new SorterNode();
        public IEnumerable<KeyValuePair<T, U>> Walk()
        {
            if (root.left != null)
                foreach (KeyValuePair<T, U> a in root.left.Walk())
                    yield return a;
            if (root.right != null)
                foreach (KeyValuePair<T, U> a in root.right.Walk())
                    yield return a;
        }
            public void Add(T key, U value)
        {
            root.Add(key, value);
        }
        public void Clear()
        {
            root = new SorterNode();
        }
    }
}
