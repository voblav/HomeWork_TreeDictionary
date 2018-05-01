using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDictionary
{
    public class TreeDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable
    {
        #region inner class Itemm
        private class Itemm
        {
            public KeyValuePair<TKey, TValue> _pair;
            public Itemm _left = null;
            public Itemm _right = null;
            public Itemm(TKey key, TValue value)
            {
                _pair = new KeyValuePair<TKey, TValue>(key, value);
            }
            public Itemm(KeyValuePair<TKey, TValue> pair)
            {
                _pair = pair;
            }
        }
        #endregion
        #region Fields

        bool _allowDuplicateKeys;
        int _counter = 0;
        Itemm _root = null;

        #endregion
        #region Ctor

        public TreeDictionary(bool allowDuplicateKeys = false)
        {
            _allowDuplicateKeys = allowDuplicateKeys;
        }

        #endregion
        #region Properties & Indexer

        public TValue this[TKey key]
        {
            get
            {
                TValue value = default(TValue);
                bool flag = true;
                foreach (var _item in this)
                {
                    if (_item.Key.CompareTo(key) == 0)
                    {
                        value = _item.Value;
                        flag = false;
                        break;
                    }
                }
                if (flag) throw new ArgumentException("Key not existent");
                return value;
            }
            set
            {
                Stack<Itemm> stack = new Stack<Itemm>();
                Itemm temp = _root;
                while (temp != null || stack.Count != 0)
                {
                    if (stack.Count != 0)
                    {
                        temp = stack.Pop();
                        if (temp._right != null) temp = temp._right;
                        if (temp._pair.Key.CompareTo(key) == 0)
                        {
                            temp._pair = new KeyValuePair<TKey, TValue>(key, value);
                            break;
                        }
                        else temp = null;
                    }
                    while (temp != null)
                    {
                        stack.Push(temp);
                        temp = temp._left;
                    }
                }
            }
        }
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> myList = new List<TKey>();
                foreach (var item in this) myList.Add(item.Key);
                return myList;
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> myList = new List<TValue>();
                foreach (var item in this) myList.Add(item.Value);
                return myList;
            }
        }
        public int Count { get => _counter; private set => _counter = value; }
        public bool IsReadOnly { get => false; }

        #endregion
        #region Utility

        private Itemm FindWithParentByKey(TKey key, out Itemm parent)
        {
            Itemm current = _root;
            parent = null;
            while (current != null)
            {
                int result = current._pair.Key.CompareTo(key);
                if (result > 0)
                {
                    parent = current;
                    current = current._left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current._right;
                }
                else break;
            }
            return current;
        }

        private Itemm FindWithParentByItem(KeyValuePair<TKey, TValue> item, out Itemm parent)
        {
            Itemm current = _root;
            parent = null;
            while (current != null)
            {
                int result = current._pair.Key.CompareTo(item.Key);
                if (result > 0)
                {
                    parent = current;
                    current = current._left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current._right;
                }
                else
                {
                    if (current._pair.Value.Equals(item.Value)) break;
                    else
                    {
                        parent = current;
                        current = current._right;
                    }
                }
            }
            return current;
        }

        private void RemoveRight(Itemm current, Itemm parent)
        {
            if (parent == null) _root = current._left;
            else
            {
                if (parent._pair.Key.CompareTo(current._pair.Key) > 0)
                {
                    parent._left = current._left;
                }
                else parent._right = current._left;
            }
        }
        private void RemoveRightLeft(Itemm current, Itemm parent)
        {
            current._right._left = current._left;
            if (parent == null) _root = current._right;
            else
            {
                if (parent._pair.Key.CompareTo(parent._pair.Key) > 0)
                {
                    parent._left = current._right;
                }
                else
                {
                    parent._right = current._right;
                }
            }
        }
        private void RemoveLeftRightAre(Itemm current, Itemm parent)
        {
            Itemm mostLeft = current._right._left;
            Itemm mostLeftParent = current._right;
            while (mostLeft._left != null)
            {
                mostLeftParent = mostLeft;
                mostLeft = mostLeft._left;
            }
            mostLeftParent._left = mostLeft._right;
            mostLeft._left = current._left;
            mostLeft._right = current._right;

            if (parent == null) _root = mostLeft;
            else
            {
                if (parent._pair.Key.CompareTo(current._pair.Key) > 0)
                {
                    parent._left = mostLeft;
                }
                else parent._right = mostLeft;
            }
        }

        #endregion
        #region Methods

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_root == null)
            {
                _root = new Itemm(item);
                _counter++;
            }
            else
            {
                Add(item, _root);
            }
        }
        void Add(KeyValuePair<TKey, TValue> pair, Itemm parent)
        {
            if (_allowDuplicateKeys && pair.Key.CompareTo(parent._pair.Key) == 0)
            {
                parent._pair = pair;
            }
            else if (parent._pair.Key.CompareTo(pair.Key) > 0)
            {
                if (parent._left == null)
                {
                    parent._left = new Itemm(pair);
                    _counter++;
                }
                else Add(pair, parent._left);
            }
            else
            {
                if (parent._right == null)
                {
                    parent._right = new Itemm(pair);
                    _counter++;
                }
                else Add(pair, parent._right);
            }
        }
        public void Clear()
        {
            _root = null;
            _counter = 0;
        }
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            foreach (var pair in this)
            {
                if (item.Value.Equals(pair.Value))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsKey(TKey key)
        {
            foreach (var pair in this)
            {
                if (pair.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrIndex)
        {
            foreach (var item in this)
            {
                array[arrIndex++] = item;
            }
        }
        public bool Remove(TKey key)
        {
            Itemm current;
            Itemm parent;
            current = FindWithParentByKey(key, out parent);
            if (current == null) return false;
            Count--;
            if (current._right == null) RemoveRight(current, parent);
            else if (current._right._left == null) RemoveRightLeft(current, parent);
            else RemoveLeftRightAre(current, parent);
            return true;
        }
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Itemm current;
            Itemm parent;
            if (_allowDuplicateKeys == false) Remove(item.Key);
            current = FindWithParentByItem(item, out parent);
            if (current == null) return false;
            Count--;
            if (current._right == null) RemoveRight(current, parent);
            else if (current._right._left == null) RemoveRightLeft(current, parent);
            else RemoveLeftRightAre(current, parent);
            return true;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var item in this)
            {
                if (item.Key.CompareTo(key) == 0)
                {
                    value = item.Value;
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }
        #endregion
        #region IEnumerable
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Stack<Itemm> stack = new Stack<Itemm>();
            Itemm _temp = _root;

            while (_temp != null || stack.Count != 0)
            {
                if (stack.Count != 0)
                {
                    _temp = stack.Pop();

                    yield return _temp._pair;

                    if (_temp._right != null)
                    {
                        _temp = _temp._right;
                    }
                    else
                    {
                        _temp = null;
                    }
                }
                while (_temp != null)
                {
                    stack.Push(_temp);
                    _temp = _temp._left;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}

