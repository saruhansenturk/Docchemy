﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docchemy.Test.Directory1
{
    /// <summary>
    /// You are reading now Test Class 1.
    /// </summary>
    internal class TestClass1
    {
        /// <summary>
        /// This method is for the test method 1.
        /// </summary>
        /// <param name="name">This parameters coming for set the users name.</param>
        /// <param name="age">This paramteter coming for set the users age.</param>
        void TestMethod1(string name, float age)
        {
            var user = new
            {
                name = name,
                age = age
            };// this variable is a annonymous type. It returns name and age.



        }
    }
}
