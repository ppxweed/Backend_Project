using System;
using System.Globalization;

namespace Optimisation
{

    public class Verification : Exception

    //All the same as AUtoProfile
    { public Verification() : base() {}
        public Verification(string obj, params object[] index) 
            : base(String.Format(CultureInfo.CurrentCulture, obj, index))
        {
        }
       

        public Verification(string obj) : base(obj) { }

        
    }
}