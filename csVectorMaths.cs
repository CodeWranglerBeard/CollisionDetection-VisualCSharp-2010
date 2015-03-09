using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
    /// <summary>
    /// Static class.
    /// Contains various methods which aid in performing vector math. 
    /// </summary>
    public static class csVectorMaths
    {
        /// <summary>
        /// Gets a vector based on two points. 
        /// The direction of the vector flows from P0 to P1. 
        /// </summary>
        /// <param name="P0">The starting point. </param>
        /// <param name="P1">The ending point. The vector will point will "flow" to this point. </param>
        /// <returns>A vector between the given points. </returns>
        public static csVector GetVector(csVector P0, csVector P1)
        {
            return new csVector(P1.x - P0.x, P1.y - P0.y);
        } // end mtd

        /// <summary>
        /// Gets the scalar product (dot product) of two vectors. 
        /// The returned sign of the number is of interest. If the sign is positive, the vectors are facing the same direction. 
        /// If the sign is negative, the vectors are facing in opposite directions
        /// If the the number is 0, the vectors are perpendicular to one another.
        /// </summary>
        /// <param name="vectA">A vector. </param>
        /// <param name="vectB">A vector. </param>
        /// <returns>The scalar or dot product of the vectors. </returns>
        public static double GetScalarProduct(csVector vectA, csVector vectB)
        {
            // dp = (a.x*b.x + a.y*b.y)
            // vectorA * vectorB = a * b cos(alpha)
            // vectorA * vectorB = (a.x * b.x) + (a.y * b.y)

            return ((vectA.x * vectB.x) + (vectA.y * vectB.y));
        } // end mtd

        /// <summary>
        /// Gets the absolute length (magnitude) of a given vector. 
        /// </summary>
        /// <param name="vect">The vector to get the length of. </param>
        /// <returns>The length of the given vector. </returns>
        public static double GetVectorLength(csVector vect)
        {
            // Math.sqrt(vector.x*vector.x + vector.y*vector.y)

            return ( Math.Sqrt( (vect.x * vect.x) + (vect.y * vect.y) ) );
        } // end mtd

        /// <summary>
        /// Gets the unit vector of a given vector. 
        /// </summary>
        /// <param name="vect">The vector to get a unit length vector of. </param>
        /// <returns>A unit length vector. </returns>
        public static csVector NormalizeVector(csVector vect)
        {
            double length = csVectorMaths.GetVectorLength(vect);
            return new csVector( vect.x / length, vect.y / length );
        } // end mtd

        /// <summary>
        /// Projects vector A onto vector B. 
        /// Returns the projected vector. 
		/// The projected vector only describes relational translation. This vector 
		/// must therefore be "added" onto the position the starting point of vector A
		/// to get the absolute coordinates of the projected point at the end of the 
		/// projected vector. 
        /// </summary>
        /// <param name="vectA">The vector to be projected. </param>
        /// <param name="vectB">The vector the other vector will be projected on. </param>
        /// <param name="bIsUnitVector">A bool which determines whether vector B is a unit length vector or not. </param>
        /// <returns>The projected vector. </returns>
        public static csVector ProjectVector(csVector vectA, csVector vectB, bool bIsUnitVector = false)
        {
            // dp = dotproduct of a and b
            // dp = (a.x*b.x + a.y*b.y)
            // proj.x = ( dp / (b.x*b.x + b.y*b.y) * b.x );
            // proj.y = ( dp / (b.x*b.x + b.y*b.y) * b.y );
            // If b is a unit vector: 
            // proj.x = dp*b.x;
            // proj.y = dp*b.y;

            csVector projectedVect = new csVector(0, 0);
            double dp = csVectorMaths.GetScalarProduct(vectA, vectB);

            if (bIsUnitVector)
            {
                projectedVect.x = (dp * vectB.x);
                projectedVect.y = (dp * vectB.y);
                return projectedVect;
            } // end if
            else
            {
                projectedVect.x = (dp / (vectB.x * vectB.x + vectB.y * vectB.y)) * vectB.x;
                projectedVect.y = (dp / (vectB.x * vectB.x + vectB.y * vectB.y)) * vectB.y;

                return projectedVect;
            } // end else
        } // end mtd

        /// <summary>
        /// Gets a vector perpendicular to the given vector. 
        /// The bool determines whether the vector will be on the right or left hand side of the given vector. 
        /// </summary>
        /// <param name="vect">A vector to get an ortho vector of. </param>
        /// <param name="bRightSide">A bool which determines whether the new ortho vector will be on the "left" or "right" 
		/// side of the given vector. </param>
        /// <returns>A new ortho vector, which is perpendicular to the given vector. </returns>
        public static csVector GetOrthoVector(csVector vect, bool bRightSide = false)
        {
            // (x,y) be the given vector, then (y, -x) is perpendicular to that, because:
            // (x,y)*(y,-x) = x*y - x*y = 0

            if (bRightSide)
	        { // "normal" on right side
		        return new csVector(-vect.y, vect.x);
	        } // end if
	        else
	        { // "normal" on left side
                return new csVector(vect.y, -vect.x);
	        } // end else
        } // end mtd

        /// <summary>
        /// Gets the cos based radians angle between two given vectors. 
        /// </summary>
        /// <param name="vectA">A vector. </param>
        /// <param name="vectB">A vector. </param>
        /// <returns>The cos angle between the given vectors in radians. </returns>
        public static double GetCosAngle(csVector vectA, csVector vectB)
        {
            // cos alpha = (vectorA*vectorB) / (|vectorA|*|vectorB|)
            // cos alpha = ((a.x*b.x) + (a.y*b.y)) / (Math.sqrt(a.x²+a.y²) * Math.sqrt(b.x²+b.y²))

            return ( csVectorMaths.GetScalarProduct(vectA, vectB) / csVectorMaths.GetVectorLength(vectA) * csVectorMaths.GetVectorLength(vectB) );
        } // end mtd

        /// <summary>
        /// Gets the absolute, total displacement of a vector. 
        /// The sign is ignored, only the raw numbers are getting summed up. 
        /// </summary>
        /// <param name="vect">A vector to get the absolute displacement of. </param>
        /// <returns>The absolute displacement of the given vector, ignoring the sign. </returns>
        public static double GetVectDisplacement(csVector vect)
        {
            return Math.Abs(vect.x) + Math.Abs(vect.y);
        } // end mtd

		/// <summary>
		/// Returns a new vector, with the x and y components summed up. 
		/// So vectA.x + vectB.x and vectA.y + vectb.y
		/// </summary>
		/// <param name="vectA">A vector. </param>
		/// <param name="vectB">A vector. </param>
		/// <returns>The sum of the given vectors. </returns>
		public static csVector GetVectorSum(csVector vectA, csVector vectB)
		{
			return new csVector(vectA.x + vectB.x, vectA.y + vectB.y);
		}

		/// <summary>
		/// Returns a new vector, with the x components of the vectors subtracted from one another,
		/// as well as the y components of the vectors subtracted from one another. 
		/// So vectA.x - vectB.x and vectA.y - vectb.y
		/// </summary>
		/// <param name="vectA">A vector. </param>
		/// <param name="vectB">A vector. </param>
		/// <returns>The difference of the given vectors. </returns>
		public static csVector GetVectorDifference(csVector vectA, csVector vectB)
		{
			return new csVector(vectA.x - vectB.x, vectA.y - vectB.y);
		}

    } // end cs
} // end ns
