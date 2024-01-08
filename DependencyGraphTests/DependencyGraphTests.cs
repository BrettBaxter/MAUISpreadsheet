using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        #region - PREMADE TESTS -
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }



        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        #endregion

        #region - MY TESTS -

        #region - TEST SIZE -
        /// <summary>
        /// Calls the constructor and checks the size, it should be zero.
        /// </summary>
        [TestMethod()]
        public void TestConstructor()
        {
            DependencyGraph dg = new DependencyGraph();
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Adds three pairs to the dg, checks the size, it should be 3.
        /// </summary>
        [TestMethod()]
        public void TestSizeWithAdd()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            Assert.AreEqual(3, dg.Size);
        }

        /// <summary>
        /// Adds 3 pairs and then removes 2, checks the size, it should be 1.
        /// </summary>
        [TestMethod()]
        public void TestSizeWithAddAndSubtract()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.RemoveDependency("A", "C");
            dg.RemoveDependency("C", "C");
            Assert.AreEqual(1, dg.Size);
        }

        /// <summary>
        /// Calls remove twice on an empty dg, should remain size 0.
        /// </summary>
        [TestMethod()]
        public void TestSizeEmptyWithRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.RemoveDependency("A", "C");
            dg.RemoveDependency("D", "C");
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Adds three pairs to the dg, calls the indexer, C has 3 dependees, so it should be three.
        /// </summary>
        [TestMethod()]
        public void TestSimpleSizeIndexer()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            int output = dg["C"];
            Assert.AreEqual(3, output);
        }

        /// <summary>
        /// Calls add 3 times on the dg, calls the indexer for something not in the dg, should return 0.
        /// </summary>
        [TestMethod()]
        public void TestSimpleSizeIndexer2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            int output = dg["0"];
            Assert.AreEqual(0, output);
        }

        /// <summary>
        /// Add 3 pairs to the dg, call the indexer, the B has 1 dependee, should be 1.
        /// </summary>
        [TestMethod()]
        public void TestSimpleSizeIndexer3()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "B");
            int output = dg["B"];
            Assert.AreEqual(1, output);
        }

        /// <summary>
        /// Calls add 3 times, remove once. C had 3 dependees but then 1 was removed, making it 2.
        /// </summary>
        [TestMethod()]
        public void TestSimpleSizeIndexerRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.RemoveDependency("C", "C");
            int output = dg["C"];
            Assert.AreEqual(2, output);
        }

        #endregion

        #region - TEST HAS DEPENDENTS / DEPENDEES -

        /// <summary>
        /// Call add 4 times, F is a dependent but not a dependee, so it has no dependents, should be false.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependentsFalse()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            Assert.IsFalse(dg.HasDependents("F"));
        }

        /// <summary>
        /// Call add 3 times, remove 1 time, B was a dependee, but then was removed and is no longer in the dg, should return false.
        /// </summary>
        [TestMethod()]
        public void TestHasDependentsFalseAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.RemoveDependency("B", "C");
            Assert.IsFalse(dg.HasDependents("B"));
        }

        /// <summary>
        /// Add 4 times, C is both a dependent and a dependee, should return true.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependentsTrue()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            Assert.IsTrue(dg.HasDependents("C"));
        }

        /// <summary>
        /// Add 4 times, remove 1 time, and then add again. B is a dependee so should return true.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependentsTrueAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            dg.RemoveDependency("B", "C");
            dg.AddDependency("B", "G");
            Assert.IsTrue(dg.HasDependents("B"));
        }

        /// <summary>
        /// Add 4 times, E is neither a dependent or dependee, so should return false.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependeesFalse()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            Assert.IsFalse(dg.HasDependees("E"));
        }

        /// <summary>
        /// Add 4 times, remove 1 time, F was removed so it should return false.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependeesFalseAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            dg.RemoveDependency("D", "F");
            Assert.IsFalse(dg.HasDependees("F"));
        }

        /// <summary>
        /// Add 4 times, C is indeed a dependent so should return true.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependeesTrue()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            Assert.IsTrue(dg.HasDependents("C"));
        }

        /// <summary>
        /// Add 4 times, remove 1 time, C is still a dependent so should return true.
        /// </summary>
        [TestMethod()]
        public void TestSimpleHasDependeesTrueAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            dg.RemoveDependency("C", "C");
            Assert.IsTrue(dg.HasDependees("C"));
        }

        #endregion

        #region - TEST GET DEPENDENTS / DEPENDEES -

        /// <summary>
        /// Add 4 times, call get dependents on A, A has one dependent C, size should be 1, then add once more, A now has C and G, should return 2.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependents()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependents = dg.GetDependents("A");
            Assert.AreEqual(1, dependents.Count());
            dg.AddDependency("A", "G");
            dependents = dg.GetDependents("A");
            Assert.AreEqual(2, dependents.Count());
        }

        /// <summary>
        /// Add 4 times, call get dependents on G, G is not in the dg so should return 0 for size.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependentsEmpty()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependents = dg.GetDependents("G");
            Assert.AreEqual(0, dependents.Count());
        }

        /// <summary>
        /// Add 4 times, call get dependents on A, A has C so should be 1, remove A and C. A is no longer in the dg, return 0.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependentsAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependents = dg.GetDependents("A");
            Assert.AreEqual(1, dependents.Count());
            dg.RemoveDependency("A", "C");
            dependents = dg.GetDependents("A");
            Assert.AreEqual(0, dependents.Count());
        }

        /// <summary>
        /// Add 4 times, call get dependees on C, C has 3 dependees A B C, should return 3. Add G C now C has 4, should return 4.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependees()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependees = dg.GetDependees("C");
            Assert.AreEqual(3, dependees.Count());
            dg.AddDependency("G", "C");
            dependees = dg.GetDependees("C");
            Assert.AreEqual(4, dependees.Count());
        }

        /// <summary>
        /// Add 4 times, call get dependees on A, A has not dependees, so return 0.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependeesEmpty()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependees = dg.GetDependees("A");
            Assert.AreEqual(0, dependees.Count());
        }

        /// <summary>
        /// Add 4 times, call get dependees on C, C has A B C so return 3. Add G C, now C has 4. Now remove 3, C only has 1 dependee left B, return 1.
        /// </summary>
        [TestMethod()]
        public void TestSimpleGetDependeesAfterRemove()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            IEnumerable<String> dependees = dg.GetDependees("C");
            Assert.AreEqual(3, dependees.Count());
            dg.AddDependency("G", "C");
            dependees = dg.GetDependees("C");
            Assert.AreEqual(4, dependees.Count());
            dg.RemoveDependency("G", "C");
            dg.RemoveDependency("C", "C");
            dg.RemoveDependency("A", "C");
            dependees = dg.GetDependees("C");
            Assert.AreEqual(1, dependees.Count());
        }

        #endregion

        #region - TEST ADD / REMOVE DEPENDENCY -

        /// <summary>
        /// Add 100 dependencies, ensure the size is 100.
        /// </summary>
        [TestMethod()]
        public void TestSimpleAddLoop()
        {
            DependencyGraph dg = new DependencyGraph();
            for(int i = 0; i < 100; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(100, dg.Size);
        }

        /// <summary>
        /// Add 100 dependencies, ensure it is 100, then do it again, should be the same size, 100.
        /// </summary>
        [TestMethod()]
        public void TestSimpleAddLoopDuplicates()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(100, dg.Size);
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(100, dg.Size);
        }

        /// <summary>
        /// Add 100 times, then remove 100 times, ensure the size is 0.
        /// </summary>
        [TestMethod()]
        public void TestSimpleAddThenRemoveLoop()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(100, dg.Size);
            for (int i = 0; i < 100; i++)
            {
                dg.RemoveDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Remove 100 times from an empty dg, ensure the size is 0.
        /// </summary>
        [TestMethod()]
        public void TestSimpleRemoveLoop()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 100; i++)
            {
                dg.RemoveDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(0, dg.Size);
        }

        #endregion

        #region - TEST REPLACE DEPENDENTS / DEPENDEES -

        /// <summary>
        /// Add 4 times, create a new hashset of new dependents. Replace the dependents of the dependee C with A G Z. C is still a dependent, but only to 2 dependees.
        /// Return 2. A is now a dependent of C, so it should be 1.
        /// </summary>
        [TestMethod()]
        public void TestSimpleReplaceDependents()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependents = new HashSet<String>();
            newDependents.Add("A");
            newDependents.Add("G");
            newDependents.Add("Z");
            dg.ReplaceDependents("C", newDependents);
            Assert.AreEqual(2, dg["C"]);
            Assert.AreEqual(1, dg["A"]);
        }

        /// <summary>
        /// Add 4 times, create a set of dependents A G Z, Replace M, which is not in the dg. This adds
        /// M as a dependee, and A G Z as its dependents. GetDependents(M).Count() should return 3. We can add J
        /// to M and have A G Z J, but then replace dependents for M again to have A G Z, making it 3 again.
        /// </summary>
        [TestMethod()]
        public void TestReplaceDependentsOfDependeeNotInDG()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependents = new HashSet<String>();
            newDependents.Add("A");
            newDependents.Add("G");
            newDependents.Add("Z");
            dg.ReplaceDependents("M", newDependents);
            Assert.AreEqual(3, dg.GetDependents("M").Count());
            dg.AddDependency("M", "J");
            dg.ReplaceDependents("M", newDependents);
            Assert.AreEqual(3, dg.GetDependents("M").Count());
        }

        /// <summary>
        /// Add 4 times, create a set of new dependees A G Z. Replace dependees of C, the dependees of C were A B C. C should have 3 dependees, and A should have
        /// 0 dependees.
        /// </summary>
        [TestMethod()]
        public void TestSimpleReplaceDependees()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependees = new HashSet<String>();
            newDependees.Add("A");
            newDependees.Add("G");
            newDependees.Add("Z");
            dg.ReplaceDependees("C", newDependees);
            Assert.AreEqual(3, dg["C"]);
            Assert.AreEqual(0, dg["A"]);
        }

        /// <summary>
        /// Add 4 times, create a set of new dependees A G Z, Replace the dependees of M which is not in the dg. This adds
        /// M to the dg with A G Z. It should be 3.
        /// </summary>
        [TestMethod()]
        public void TestReplaceDependeeesOfDependentNotInDG()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependees = new HashSet<String>();
            newDependees.Add("A");
            newDependees.Add("G");
            newDependees.Add("Z");
            dg.ReplaceDependees("M", newDependees);
            Assert.AreEqual(3, dg["M"]);
            dg.AddDependency("M", "J");
            dg.ReplaceDependees("M", newDependees);
            Assert.AreEqual(3, dg["M"]);
        }

        /// <summary>
        /// Add 4 times, create a set of new dependees A G Z H. Replace dependees of C, the dependees of C were A B C. C should have 4 dependees now, and A should have
        /// 0 dependees.
        /// </summary>
        [TestMethod()]
        public void TestSimpleReplaceDependeesBigger()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependees = new HashSet<String>();
            newDependees.Add("A");
            newDependees.Add("G");
            newDependees.Add("Z");
            newDependees.Add("H");
            dg.ReplaceDependees("C", newDependees);
            Assert.AreEqual(4, dg["C"]);
            Assert.AreEqual(0, dg["A"]);
        }

        /// <summary>
        /// Add 4 times, create a set of new dependees A G. Replace dependees of C, the dependees of C were A B C. C should have 2 dependees now, and A should have
        /// 0 dependees.
        /// </summary>
        [TestMethod()]
        public void TestSimpleReplaceDependeesSmaller()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("A", "C");
            dg.AddDependency("B", "C");
            dg.AddDependency("C", "C");
            dg.AddDependency("D", "F");
            HashSet<String> newDependees = new HashSet<String>();
            newDependees.Add("A");
            newDependees.Add("G");
            dg.ReplaceDependees("C", newDependees);
            Assert.AreEqual(2, dg["C"]);
            Assert.AreEqual(0, dg["A"]);
        }

        #endregion

        #endregion
    }
}