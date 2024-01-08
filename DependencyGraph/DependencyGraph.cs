// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        // A dictionary of the dependees. It uses a string key and a hashset of values, the hashset is a useful
        // data structure because it is like a dictionary with no values only keys, this allows for constant operations.
        // dependent : dependees
        private Dictionary<string, HashSet<string>> dependees;
        // dependee : dependents
        private Dictionary<string, HashSet<string>> dependents;
        // The size of the dependency graph.
        private int size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        /// <returns> The size of the DependencyGraph </returns>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        /// <param name="s"> the dependent </param>
        /// <returns> The size of dependees or 0 </returns>
        public int this[string s]
        {
            get {
                // If the dependent s truly is in the dependees dictionary, return the size of the dependees hashset associated with it.
                if (dependees.ContainsKey(s))
                {
                    return dependees[s].Count;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        /// <param name="s"> The dependee </param>
        /// <returns> True if the dependee has dependents, false if not. </returns>
        public bool HasDependents(string s)
        {
            // If the dependee s is in the dependents dictionary, that means that it has a list of dependents.
            if (dependents.ContainsKey(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        /// <param name="s"> The dependent </param>
        /// <returns> True if the dependent has dependees, false if not. </returns>
        public bool HasDependees(string s)
        {
            // If the dependent s is in the dependees dictionary, it has a list of dependees.
            if (dependees.ContainsKey(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        /// <param name="s"> The dependee </param>
        /// <returns> A hashset of dependents. </returns>
        public IEnumerable<string> GetDependents(string s)
        {
            // If the dependee is in the dictionary:
            if (dependents.ContainsKey(s))
            {
                // Return a copy of its dependents as a hashset.
                return new HashSet<string>(dependents[s]);
            }
            // Otherwise, return an empty set.
            else
            {
                return new HashSet<string>();
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        /// <param name="s"> The dependent </param>
        /// <returns> A hashset of dependees. </returns>
        public IEnumerable<string> GetDependees(string s)
        {
            // If the dependent is in the dictionary:
            if (dependees.ContainsKey(s))
            {
                // Return a copy of its dependees as a hashset.
                return new HashSet<string>(dependees[s]);
            }
            // Otherwise, return an empty set.
            else
            {
                return new HashSet<string>();
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            // If the dependee is already in its dictionary, add t to its hashset of dependents.
            if (dependents.ContainsKey(s))
            {
                // If t is not already a dependent of s, add it and increment size.
                if (!(dependents[s].Contains(t)))
                {
                    dependents[s].Add(t);
                    // Increment size.
                    size++;
                }
            }
            // Otherwise add the dependee as the value and a new hashset containing t as the key in the dependents dictionary
            else
            {
                dependents.Add(s, new HashSet<string> { t });
                // Increment size.
                size++;
            }

            // Do the inverse for the other mirror dictionary. No size adjustments needed.
            if (dependees.ContainsKey(t))
            {
                dependees[t].Add(s);
            }
            else
            {
                dependees.Add(t, new HashSet<string> { s });
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"> The dependee </param>
        /// <param name="t"> The dependent </param>
        public void RemoveDependency(string s, string t)
        {
            // If the s is in the dependents dict, and t is in the dependees dict, decrement size.
            if (dependents.ContainsKey(s) && dependees.ContainsKey(t))
            {
                size--;
            }

            // If the dependee s is in the dependents dictionary: remove the dependent t from s' hashset.
            if (dependents.ContainsKey(s))
            {
                dependents[s].Remove(t);
                // If the hashset associated with the dependee is empty, remove the dependee from the dictionary.
                if (dependents[s].Count == 0)
                {
                    dependents.Remove(s);
                }
            }

            // If the dependent t is in the dependees dictionary: remove the dependee s from t's hashset.
            if (dependees.ContainsKey(t))
            {
                dependees[t].Remove(s);
                // If the hashset associated with the dependent is empty, remove the dependent from the dictionary.
                if (dependees[t].Count == 0)
                {
                    dependees.Remove(t);
                }
            }

            // Otherwise do nothing.
            else
            {
                return;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        /// <param name="s"> The dependee </param>
        /// <param name="newDependents"> The list of new dependents </param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // Get all the old dependent from the dependee s.
            IEnumerable<string> oldDependents = GetDependents(s);

            // for each of the old dependents: remove it.
            foreach(string dependent in oldDependents)
            {
                RemoveDependency(s, dependent);
            }
            // for each of the new dependents: add it.
            foreach(string dependent in newDependents)
            {
                AddDependency(s, dependent);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        /// <param name="s"> The dependent </param>
        /// <param name="newDependees"> The list of new dependees </param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // Get all the old dependees from the dependent s.
            IEnumerable<string> oldDependees = GetDependees(s);

            // for each of the old dependees: remove it.
            foreach(string dependee in oldDependees)
            {
                RemoveDependency(dependee, s);
            }
            // for each of the new dependees: add it.
            foreach(string dependee in newDependees)
            {
                AddDependency(dependee, s);
            }
        }

    }

}
