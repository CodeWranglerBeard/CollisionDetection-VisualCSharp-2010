using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
    /// <summary>
    /// Represents a line object or face of a polygon. 
    /// </summary>
    public class csLine
    {
        /// <summary>
        /// The point where this line starts. 
        /// </summary>
        public csVector P0;

        /// <summary>
        /// The point where this line ends. 
        /// </summary>
        public csVector P1;

		/// <summary>
		/// A broadPhaseBox surrounding this line object. 
		/// </summary>
        public csBroadphaseBox broadBox;

		/// <summary>
		/// The slope of this line object. 
		/// Used during mathematical calculations. 
		/// </summary>
        public double dSlope;

		/// <summary>
		/// The axis shift of this line object. 
		/// Used during mathematical calculations. 
		/// </summary>
        public double dAxisShift;

        /// <summary>
        /// A vector describing where the left hand-side of this line is. 
        /// </summary>
        public csVector normal;

        /// <summary>
        /// A vector starting at the origin point of this line and pointing to the target point of this line. 
		/// Useful to project another vector onto this line object. 
        /// </summary>
        public csVector lineVect;

		/// <summary>
		/// A bool describing whether this line object is parallel to the Y axis or not. 
		/// </summary>
        public bool bParallelToAxisY = false;

		/// <summary>
		/// A bool describing whether this line object is parallel to the X axis or not. 
		/// </summary>
        public bool bParallelToAxisX = false;

		/// <summary>
		/// Constructor. 
		/// Gets its own slope and axis shift. 
		/// </summary>
		/// <param name="P0In">The starting point. </param>
		/// <param name="P1In">The ending point. </param>
		/// <param name="bStaticLine">Determines whether to precalculate the normal vector and broadphase box of this line. </param>
        public csLine(csVector P0In, csVector P1In, bool bStaticLine = false)
        {
            this.P0 = P0In;
            this.P1 = P1In;

			// Get a vector describing this line. 
			this.lineVect = csVectorMaths.GetVector(this.P0, this.P1);

			//Get the slope of this line. 
            this.dSlope = csLine.GetSlope(this.P0, this.P1);
			// Check if the slope of the line is x-axis or y-axis parallel. 
			// If it parallel to an axis, set the according bool to true, so that later
			// the bools can be used to determine how to calculate collisions with this line,
			// since x-axis or y-axis parallel lines require a different procedure. 
            if (this.dSlope == 0)
            {
                this.bParallelToAxisX = true;
            }
            else if (this.dSlope == double.PositiveInfinity || this.dSlope == double.NegativeInfinity)
            {
                this.bParallelToAxisY = true;
            }

			// Get the y-axis shift of this line. 
            this.dAxisShift = csLine.GetAxisShift(this.P0, this.dSlope);

			// Check if this is a static line. 
			// If it is, precalculate the broadphase box and normal of this line, 
			// as they will only ever need to be calculated once. 
            if (bStaticLine)
            {
                this.broadBox = new csBroadphaseBox(this);

                this.normal = csVectorMaths.GetOrthoVector(this.lineVect);
            } // end if
        } // end constr

		/// <summary>
		/// Constructor. 
		/// Does not get its own slope.
		/// Gets its own axis shift. 
		/// </summary>
		/// <param name="P0In">The starting point. </param>
		/// <param name="P1In">The ending point. </param>
		/// <param name="dSlope">The slope of this line, calculated and passed to this object externally. 
		/// Useful when performing calculations with lines with the same slope, but different coordinates
		/// and not needing to recalculate the slope every time. </param>
		public csLine(csVector P0In, csVector P1In, double dSlope)
        {
            this.P0 = P0In;
            this.P1 = P1In;

			// Get a vector describing this line. 
			this.lineVect = csVectorMaths.GetVector(this.P0, this.P1);

			this.dSlope = dSlope;

			if (this.dSlope == 0)
			{
				this.bParallelToAxisX = true;
			}
			else if (this.dSlope == double.PositiveInfinity || this.dSlope == double.NegativeInfinity)
			{
				this.bParallelToAxisY = true;
			}

			// Get the y-axis shift of this line. 
			this.dAxisShift = csLine.GetAxisShift(this.P0, this.dSlope);

		}

        /// <summary>
        /// Gets the slope between two given points. 
        /// </summary>
        /// <param name="P0">The starting point. </param>
        /// <param name="P1">The ending point. </param>
        /// <returns>The slope between the two points. </returns>
        public static double GetSlope(csVector P0, csVector P1)
        {
            return ((P1.y - P0.y) / (P1.x - P0.x));
        } // end mtd

		/// <summary>
		/// Gets the slope of this line object.  
		/// </summary>
		/// <returns>The slope of this line object. </returns>
		public double GetSlope()
		{
			return ((this.P1.y - this.P0.y) / (this.P1.x - this.P0.x));
		} // end mtd

        /// <summary>
        /// Gets the y-axis shift of a given point, based on the given slope. 
        /// </summary>
        /// <param name="P"></param>
        /// <param name="dSlope"></param>
        /// <returns></returns>
        public static double GetAxisShift(csVector P, double dSlope)
        {
            return (P.y - (dSlope * P.x));
        } // end mtd

        /// <summary>
        /// Gets the y-axis coordinate on this line, based on the given x coordinate. 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetLineCoordY(double x)
        {
            return (this.dSlope * x + this.dAxisShift);
        } // end mtd

        /// <summary>
        /// Gets the y-axis coordinate on a given line, based on the given x coordinate. 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GetLineCoordY(csLine line, double x)
        {
            return (line.dSlope * x + line.dAxisShift);
        } // end metd

        /// <summary>
        /// Gets the point of intersection of this line object with a given line object. 
        /// Returns the point of intersection.  
        /// Returns null if the lines are parallel. 
        /// </summary>
        /// <param name="lineA"></param>
        /// <returns></returns>
        public static csVector GetIntersection(csLine lineA, csLine lineB)
        {
            // Intersection for two non-axis-aligned lines
	        // x = (b1 - b2) / (m2 - m1)

	        // Intersection for an x-axis-aligned line with non-axis-aligned line
	        // x = (b2 - b1) / m1

	        // Check for line parallelism
	        if (lineB.dSlope == lineA.dSlope)
		        return null;

	        double dResultX, dResultY;

            /**** Check x-axis parallelism ****/
            if (lineB.bParallelToAxisX)
            {
                dResultX = (lineB.dAxisShift - lineA.dAxisShift) / lineA.dSlope;
            }
            else if (lineA.bParallelToAxisX)
            {
                dResultX = (lineA.dAxisShift - lineB.dAxisShift) / lineB.dSlope;
            }
            else
            { // No parallelism, use general approach
                dResultX = (lineB.dAxisShift - lineA.dAxisShift) / (lineA.dSlope - lineB.dSlope);
            } // end else

            /**** Check y-axis parallelism ***/
            if (lineB.bParallelToAxisY)
            {
                dResultX = lineB.P0.x;
				dResultY = lineA.GetLineCoordY(dResultX);
            }
            else if (lineA.bParallelToAxisY)
            {
                dResultX = lineA.P0.x;
				dResultY = lineB.GetLineCoordY(dResultX);
            }
            else
            { // No parallelism, use general approach
                dResultY = lineB.GetLineCoordY(dResultX);
            } // end else

            /**** Check if both lines are axis parallel ****/
            if (lineA.bParallelToAxisX && lineB.bParallelToAxisY)
            {
                // lineA x parallel, lineB y parallel
                dResultX = lineB.P0.x;
                dResultY = lineA.P0.y;
            }
            else if (lineA.bParallelToAxisY && lineB.bParallelToAxisX)
            {
                // lineB x parallel, lineA y parallel
                dResultX = lineA.P0.x;
                dResultY = lineB.P0.y;
            }
	
	        // Return point of intersection
            return new csVector(dResultX, dResultY);
        } // end mtd

    } // end cs
} // end ns
