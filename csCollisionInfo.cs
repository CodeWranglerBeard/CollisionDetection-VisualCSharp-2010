using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
    /// <summary>
    /// Contains information about a collision of an AABB with a Line. 
    /// </summary>
    public class csCollisionInfo
    {
        /// <summary>
        /// The point of origin for the collision recoreded by this object. 
        /// </summary>
        public csVector pointOrigin;

        /// <summary>
        /// The point where the point of origin would be at the end of the update.
        /// </summary>
        public csVector pointTarget;

        /// <summary>
        /// The point where collision a was found on the obstacle. 
        /// </summary>
		public csVector pointIntersection;

        /// <summary>
        /// A vector describing the needed translation to get to the corrected position. 
        /// </summary>
		public csVector vectOriginToCorrected;

		/// <summary>
		/// The point at the end of the projected vector. 
		/// This will be a point on a given line, based on the projected vector. 
		/// </summary>
		public csVector pointProjected;

		/// <summary>
		/// The point of origin (top left corner) of the object that collided with an obstacle,
		/// after the position was corrected. 
		/// These are absolute world coordinates. 
		/// </summary>
		public csVector pointCorrectedOrigin;
    } // end cs
} // end ns
