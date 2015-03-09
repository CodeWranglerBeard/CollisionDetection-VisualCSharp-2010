using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
	/// <summary>
	/// Holds information about the position of points in relation to a line. 
	/// </summary>
	public class csPointsLineGeomRelation
	{
		/// <summary>
		/// An enum of possible relational states of the points
		/// compared to the line.
		/// </summary>
		public enum eRelationalState
		{
			OnlyPointsInfront,
			OnlyPointsBehind,
			PointsInfrontAndBehind
		}

		private eRelationalState ePointsRelationalState;
		/// <summary>
		/// Indicates whether the given points are only in front,
		/// only behind or in front of and behind the given line. 
		/// </summary>
		public eRelationalState PointsRelationalState
		{
			get { return this.ePointsRelationalState; }
			private set { this.ePointsRelationalState = value; }
		}

		/// <summary>
		/// An array of points that are in front of the given line. 
		/// </summary>
		public csVector[] pointsInfrontOfLine;

		/// <summary>
		/// An array of points that are behind the given line. 
		/// </summary>
		public csVector[] pointsBehindLine;

		/// <summary>
		/// The line to find the relations of the points for. 
		/// </summary>
		public csLine linCompare;

		/// <summary>
		/// Constructor. 
		/// Creates arrays of points, distinguishing between "in front of" and "behind"
		/// the line. 
		/// </summary>
		/// <param name="linCompare">The line to find the relations of the points for. </param>
		/// <param name="points">An array of points to check for their positional relation to the given line. </param>
		public csPointsLineGeomRelation(csLine linCompare, csVector[] points)
		{
			this.linCompare = linCompare;

			List<csVector> pointsInfrontOfLine = new List<csVector>();

			List<csVector> pointsBehindLine = new List<csVector>();

			bool bHasPointsInFront = false;
			bool bHasPointsBehind = false;

			// Every point in the received array of points will be checked and assigned
			// to the appropriate list. 
			foreach (csVector curPoint in points)
			{
				// Form a vector between the origin of the line and the current point. 
				// Then form the dotproduct between this newly created vector and the
				// normal vector of the line. 
				// If the dotproduct is positive, the point is in front of the line, else
				// the point is behind the line. 
				csVector vectLineOriginToCurPoint = csVectorMaths.GetVector(this.linCompare.P0, curPoint);

				double dp = csVectorMaths.GetScalarProduct(this.linCompare.normal, vectLineOriginToCurPoint);

				if (dp >= 0)
				{
					pointsInfrontOfLine.Add(curPoint);
					bHasPointsInFront = true;
				} // end if
				else
				{
					pointsBehindLine.Add(curPoint);
					bHasPointsBehind = true;
				} // end else
			} // end foreach

			this.pointsInfrontOfLine = pointsInfrontOfLine.ToArray();

			this.pointsBehindLine = pointsBehindLine.ToArray();

			if (bHasPointsInFront && bHasPointsBehind)
			{
				this.PointsRelationalState = eRelationalState.PointsInfrontAndBehind;
			}
			else if(bHasPointsInFront)
			{
				this.PointsRelationalState = eRelationalState.OnlyPointsInfront;
			}
			else
			{
				this.PointsRelationalState = eRelationalState.OnlyPointsBehind;
			}
		} // end constr
	} // end cs


}
