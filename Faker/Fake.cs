﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Faker.Selectors;

namespace Faker
{
    /// <summary>
    /// Used for dynamically generating fakes of simple POCO objects
    /// </summary>
    public class Fake<T> where T : new()
    {
        /// <summary>
        /// Engine used to power our fakes
        /// </summary>
        private readonly Matcher _matcher;

        public Fake()
        {
            _matcher = new Matcher();
        }

        /// <summary>
        /// Generates a single fake value for a given type
        /// </summary>
        /// <returns>A populated instance of a given class</returns>
        public T Generate()
        {
            //create a new instance of the type we want to Fake
            var instance = (T)Matcher.SafeObjectCreate(typeof(T));

            //Check to see if T is a value type and attempt to match that
            if (typeof (T).IsValueType)
            {
                _matcher.MatchStruct(ref instance);
            }
            else
            {
                //Match all of the properties of the object and come up with the most reasonable guess we can as to the type of data needed
                _matcher.Match(instance);
            }

            //Return the instance once matching is complete
            return instance; 
        }

        /// <summary>
        /// Generates a list of fake values for a given type
        /// </summary>
        /// <returns>A list of populated instances with length [count] of a given class</returns>
        public IList<T> Generate(int count)
        {
            //Create a list to hold all of the fakes we want to return back to the caller
            var items = new List<T>();

            //Build the list of objects
            for(var i = 0; i < count; i++)
            {
                items.Add(this.Generate());
            }

            //Return the list to the caller
            return items;
        }

        /// <summary>
        /// Adds a selector to the TypeTable; User-defined selectors always take precedence over the built-in ones.
        /// </summary>
        /// <typeparam name="TS">The type that matches the selector</typeparam>
        /// <param name="selector">A TypeSelectorBase instance for all instances of a TS type</param>
        public void AddSelector<TS>(TypeSelectorBase<TS> selector)
        {
            _matcher.TypeMap.AddSelector(selector, SelectorPosition.First);
        }
    }
}