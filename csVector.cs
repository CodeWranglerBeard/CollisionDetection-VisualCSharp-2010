using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
    /// <summary>
    /// Represents a point or vector in 2D. 
    /// </summary>
    public class csVector
    {
        /// <summary>
        /// X coordinate component of this point/vector. 
        /// </summary>
        public double x;

        /// <summary>
        /// Y coordinate component of this point/vector. 
        /// </summary>
        public double y;

		/// <summary>
		/// Constructor. 
		/// Creates a vector or point from two doubles. 
		/// </summary>
		/// <param name="xIn"></param>
		/// <param name="yIn"></param>
        public csVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        } // end constructor

    } // end cs
} // end ns
