using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeDictionary;

namespace UnitTestTreeDict1
{
    [TestClass]
    public class TreeDictionaryTests
    {
        TreeDictionary<int, int> treeDict;

        [TestInitialize]
        public void Init()
        {
            treeDict = new TreeDictionary<int, int>();
        }

        [TestMethod]
        public void Ctor_Test()
        {
            Assert.AreNotEqual(null, treeDict);
        }
        [TestMethod]
        public void Indexer_Test()
        {
            treeDict.Add(0, 00);
            treeDict.Add(1, 11);
            treeDict.Add(2, 33);
            treeDict.Add(3, 88);
            treeDict[2] = 77;
            Assert.AreEqual(4, treeDict.Count);
            Assert.AreEqual(11, treeDict[1]);
            Assert.AreEqual(33, treeDict[2]);
        }
        [TestMethod]
        public void AddItems_Test()
        {
            treeDict.Add(0, 44);
            treeDict.Add(1, 55);
            treeDict.Add(2, 66);
            treeDict.Add(3, 99);
            Assert.AreEqual(4, treeDict.Count);
            Assert.AreEqual(true, treeDict.ContainsKey(0));
            Assert.AreEqual(true, treeDict.ContainsKey(1));
            Assert.AreEqual(true, treeDict.ContainsKey(2));
            Assert.AreEqual(true, treeDict.ContainsKey(3));
        }
        [TestMethod]
        public void AddPair_Test()
        {
            KeyValuePair<int, int> pairOne = new KeyValuePair<int, int>(0, 00);
            KeyValuePair<int, int> pairTwo = new KeyValuePair<int, int>(1, 11);
            KeyValuePair<int, int> pairThree = new KeyValuePair<int, int>(2, 22);
            treeDict.Add(pairOne);
            treeDict.Add(pairTwo);
            treeDict.Add(pairThree);
            Assert.AreEqual(true, treeDict.Contains(pairOne));
            Assert.AreEqual(true, treeDict.Contains(pairTwo));
            Assert.AreEqual(true, treeDict.Contains(pairThree));
        }
        [TestMethod]
        public void GetKeys_Test()
        {
            treeDict.Add(0, 44);
            treeDict.Add(1, 55);
            treeDict.Add(2, 66);

            ICollection<int> collectKeys = treeDict.Keys;
            Assert.AreEqual(treeDict.Count, collectKeys.Count);
            Assert.AreEqual(true, collectKeys.Contains(0));
            Assert.AreEqual(true, collectKeys.Contains(1));
            Assert.AreEqual(true, collectKeys.Contains(2));
        }
        [TestMethod]
        public void GetValues_Test()
        {
            treeDict.Add(0, 77);
            treeDict.Add(1, 66);
            treeDict.Add(2, 55);

            ICollection<int> collectValues = treeDict.Values;
            Assert.AreEqual(treeDict.Count, collectValues.Count);
            Assert.AreEqual(false, collectValues.Contains(177));
            Assert.AreEqual(false, collectValues.Contains(33));
            Assert.AreEqual(true, collectValues.Contains(66));
            Assert.AreEqual(true, collectValues.Contains(55));
        }
        [TestMethod]
        public void Enumerable_Test()
        {
            treeDict.Add(0, 88);
            treeDict.Add(1, 77);
            treeDict.Add(2, 44);

            Dictionary<int, int> tempD = new Dictionary<int, int>();
            foreach(var item in treeDict)
            {
                tempD.Add(item.Key, item.Value);
            }
            Assert.AreEqual(88, tempD[0]);
            Assert.AreEqual(77, tempD[1]);
            Assert.AreEqual(44, tempD[2]);
        }
        [TestMethod]
        public void Contains_Test()
        {
            treeDict.Add(0, 88);
            treeDict.Add(1, 77);
            treeDict.Add(2, 44);

            Assert.AreEqual(true, treeDict.Contains(new KeyValuePair<int, int>(1, 77)));
            Assert.AreEqual(false, treeDict.Contains(new KeyValuePair<int, int>(1, 33)));
        }
        [TestMethod]
        public void KeyContains_Test()
        {
            treeDict.Add(0, 88);
            treeDict.Add(1, 77);
            treeDict.Add(2, 44);

            Assert.AreEqual(true, treeDict.ContainsKey(2));
            Assert.AreEqual(false, treeDict.ContainsKey(4));
        }
        [TestMethod]
        public void CopyTo_Test()
        {
            treeDict.Add(0, 33);
            treeDict.Add(1, 44);
            treeDict.Add(2, 55);

            KeyValuePair<int, int>[] _pair = new KeyValuePair<int, int>[treeDict.Count];
            treeDict.CopyTo(_pair, 0);

            Assert.AreEqual(new KeyValuePair<int, int>(0, 33), _pair[0]);
            Assert.AreEqual(new KeyValuePair<int, int>(2, 55), _pair[2]);
        }
        [TestMethod]
        public void KeyRemove_Test()
        {
            treeDict = new TreeDictionary<int, int>(true);
            treeDict.Add(0, 44);
            treeDict.Add(1, 55);
            treeDict.Add(1, 56);
            treeDict.Add(2, 66);
            treeDict.Add(3, 99);

            treeDict.Remove(1);

            Assert.AreEqual(3, treeDict.Count);
            Assert.AreEqual(true, treeDict.ContainsKey(2));
            Assert.AreEqual(false, treeDict.Contains(new KeyValuePair<int, int>(3, 98)));
            Assert.AreEqual(true, treeDict.Contains(new KeyValuePair<int, int>(3, 99)));
            Assert.AreEqual(66, treeDict[2]);
        }
        [TestMethod]
        public void ItemRemove_Test()
        {
            treeDict = new TreeDictionary<int, int>(true);
            treeDict.Add(0, 44);
            treeDict.Add(1, 55);
            treeDict.Add(2, 66);
            treeDict.Add(3, 99);
            treeDict.Add(3, 101);
            treeDict.Add(4, 88);
            treeDict.Add(5, 77);
            treeDict.Add(8, 45);
            treeDict.Add(9, 34);

            treeDict.Remove(new KeyValuePair<int, int>(8, 45));
            treeDict.Remove(new KeyValuePair<int, int>(3, 99));
            treeDict.Remove(new KeyValuePair<int, int>(9, 34));

            Assert.AreEqual(6, treeDict.Count);
            Assert.AreEqual(false, treeDict.ContainsKey(6), "Contains");
            Assert.AreEqual(true, treeDict.Contains(new KeyValuePair<int, int>(0, 44)));
            Assert.AreEqual(true, treeDict.Contains(new KeyValuePair<int, int>(1, 55)));
            Assert.AreEqual(false, treeDict.Contains(new KeyValuePair<int, int>(8, 45)), "8, 45");
            Assert.AreEqual(false, treeDict.Contains(new KeyValuePair<int, int>(3, 99)), "3, 99");
            Assert.AreEqual(false, treeDict.Contains(new KeyValuePair<int, int>(9, 34)), "9, 34");
        }
        [TestMethod]
        public void TryGetValue_Test()
        {
            treeDict.Add(0, 44);
            treeDict.Add(1, 55);
            treeDict.Add(2, 66);
            treeDict.Add(3, 99);

            int expect;

            Assert.AreNotEqual(true, treeDict.TryGetValue(33, out expect));
            Assert.AreEqual(true, treeDict.TryGetValue(0, out expect));
            Assert.AreEqual(44, expect);

        }
    }
}
