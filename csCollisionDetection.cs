using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
	/// <summary>
	/// A class containing methods to perform collision checks of objects with one another. 
	/// </summary>
    public static class csCollisionDetection
    {
		/// <summary>
		/// Performs collision checks with an AABB versus a given set of obstacle lines. 
		/// </summary>
		/// <param name="oAABB">The AABB to check for collisions. </param>
		/// <param name="linObstacles">An array of line objects to check for collisions with. </param>
		/// <param name="iCheckIterations">The amount of recursive collision checks to perform. 
		/// Prevents tunneling through other obstacles, by testing the corrected
		/// position for a new collision. 2 iterations should be minimum. </param>
		/// <param name="linExcludedObstacles">A list of line objects to be excluded from collision checks. </param>
		/// <returns>A List of csCollisionInfo objects. These can be used to resolve collision or obtain more information about the collision. </returns>
		public static List<csCollisionInfo> CheckCollision_AABB_Line(csAABB oAABB, csLine[] linObstaclesPreCheck, 
		int iCheckIterations = 2, List<csLine> linExcludedObstacles = null)
		{
			// 1. Find the obstacle line which the AABB collides with first during the update. 
			// The first collided with obstacle line is the one all further collision checks
			// will be performed with. 
				// For this, choose an origin point from the AABB, as well as the origin's corresponding target point.
				// The target point is the point where the origin point would end up at the end of the update,
				// if the x and y velocities were added to the position of the point. 
				// Form a line between origin point and target point. 
				// Get the intersections for each obstacle line with the just created line. 
				// Obstacle lines which are parallel to the just formed line will be ignored (this would be the case
				// after a previous collision has been resolved, for example). 
				// Get vectors between each intersection point and the origin point. 
				// Get the absolute displacement of each vector and compare it to the next, if the next one
				// has a smaller absolute displacement, memorise it, as well as the line its end point
				// sits on. That line will be the one the AABB collides with before the others. 

			// 2. Find out which target point penetrates the furthest. This is be the point
			// which will be projected onto the obstacle line. 
				// To find out which target point penetrates the furthest, points
				// of intersection must be found. A movement path will be formed
				// between every origin point and its corresponding target point.
				// The points of intersection of these movement paths with the 
				// obstacle line will be found. 
				// Then vectors will be formed between each of the points of 
				// intersection and the target points.
				// The vector with the biggest absolute displacement must then be found
				// and memorised. 
				// This vector will then be projected onto the obstacle line, with its
				// offset being the point of penetration. This will yield the point
				// on the obstacle line where the corrected position of the AABB would
				// be. 

			// 3. Get the information to pass on and perform recursive checks as necessary. 

			// Cancel this method call if the amount of checks to perform is 0 or
			// less than 0. 
			if (iCheckIterations <= 0)
				return null;

			/**** Check broadphase ****/
			// Check for overlap of the broadphase box of every obstacle line with the
			// broadphase box of the AABB. 
			// Those which overlap will be memorised via a list. 
			// After all obstacle lines whose broadphase boxes overlap with the AABB's
			// broadphase box have been found, cast them to an Array. 
			List<csLine> linObstaclesCheckedPassed = new List<csLine>();
			
			foreach(csLine linObstaclePreCheck in linObstaclesPreCheck)
			{
				if (csCollisionDetection.CheckOverlap_broadBox_broadBox(oAABB.broadBox, linObstaclePreCheck.broadBox))
					linObstaclesCheckedPassed.Add(linObstaclePreCheck);
			}

			csLine[] linObstacles = linObstaclesCheckedPassed.ToArray();

			/**** Find first collided with obstacle line ****/
			// The obstacle line that the AABB collides with first during the update. 
			csLine linObstacleResult = null;

			// The vector with the smallest absolute displacement. 
			// Used during the following for loop to determine the first collided with
			// obstacle line. 
			csVector vectIntersectionToOriginResult = null;

			// An array holding the target points of the AABB. 
			// Used to find the relational properties of the AABB's points
			// compared to the line. 
			csVector[] pointsAABBTarget = oAABB.GetTargetPoints();

			// An array holding the origin points of the AABB whose corresponding target points
			// are behind the obstacle line. Only these will be viable for detection of the
			// furthest penetrating target point later on. 
			csVector[] pointsAABBTargetOriginObstacle = null;

			for (int iCurLine = 0; iCurLine < linObstacles.Length; iCurLine++)
			{
				if (linExcludedObstacles != null)
				{
					bool bCurLineExcluded = false;

					// Check to see if the current obstacle line is exluded from the collision checks.
					foreach (csLine linExcludedObstacle in linExcludedObstacles)
					{
						if (linExcludedObstacle == linObstacles[iCurLine])
						{
							bCurLineExcluded = true;
							break;
						} // end if
					} // end foreach

					// If the current obstacle is excluded from collision checks, skip
					// this loop iteration. 
					if (bCurLineExcluded)
						continue;
				}

				// Get object describing the relational properties of the AABB's target points 
				// compared to the line. 
				// These will later be used to find the furthest penetrating point. 
				csPointsLineGeomRelation plgrTargetPointsCurLine = new csPointsLineGeomRelation(linObstacles[iCurLine], pointsAABBTarget);

				// Get object describing the relational properties of the AABB's origin points 
				// compared to the line. 
				// These will be used to find out whether the AABB is already behind the obstacle line
				// or not. If the AABB is already behind the obstacle line, the obstacle line
				// will be skipped.
				csPointsLineGeomRelation plgrOriginPointsCurLine = new csPointsLineGeomRelation(linObstacles[iCurLine], oAABB.points);

				// If the target points of the AABB are all in front of the the currently looked at obstacle line,
				// there can't be a collision with that obstacle line. 
				// If the origin points of the AABB are all behind the currently looked at obstacle line,
				// there can't be a collision with that obstacle line. 
				// This also has the effect that the AABB can not "break out" of the "world", but can "break in". 
				if (plgrTargetPointsCurLine.PointsRelationalState == csPointsLineGeomRelation.eRelationalState.OnlyPointsInfront ||
					plgrOriginPointsCurLine.PointsRelationalState == csPointsLineGeomRelation.eRelationalState.OnlyPointsBehind)
				{
					continue;
				} // end if
				else
				{
					// First assign the target points behind the line to an array, then
					// subtract the velocity from these points, to travel "backwards", 
					// to retrieve the corresponding origin points. 
					pointsAABBTargetOriginObstacle = new csVector[plgrTargetPointsCurLine.pointsBehindLine.Length];

					for (int iCurOriginPoint = 0; iCurOriginPoint < plgrTargetPointsCurLine.pointsBehindLine.Length; iCurOriginPoint++)
					{
						pointsAABBTargetOriginObstacle[iCurOriginPoint] = new csVector(
						plgrTargetPointsCurLine.pointsBehindLine[iCurOriginPoint].x - oAABB.vx,
						plgrTargetPointsCurLine.pointsBehindLine[iCurOriginPoint].y - oAABB.vy);
					} // end foreach
				} // end else

				// This line object represents the movement path of the AABB during this update. 
				// This line object is used to get points of intersection with the obstacle
				// lines. Vectors formed between intersection points and the point of origin
				// of the movement path are used to find the first collided with obstacle line. 
				csLine linAABBVelocityPath = new csLine(oAABB.points[0], new csVector(oAABB.points[0].x + oAABB.vx, oAABB.points[0].y + oAABB.vy));

				// Only proceed if the currently looked at obstacle line isn't parallel to the 
				// velocity path of the AABB. 
				if (Math.Abs(linAABBVelocityPath.dSlope) != Math.Abs(linObstacles[iCurLine].dSlope))
				{
					csVector pointIntersection = csLine.GetIntersection(linAABBVelocityPath, linObstacles[iCurLine]);

					csVector vectIntersectionToOrigin = csVectorMaths.GetVector(pointIntersection, oAABB.points[0]);

					if (vectIntersectionToOriginResult == null)
					{
						// Assign result vector if there isn't one yet. 
						vectIntersectionToOriginResult = vectIntersectionToOrigin;

						// Make sure to memorise currently looked at obstacle line. 
						linObstacleResult = linObstacles[iCurLine];
					} // end if
					else if (csVectorMaths.GetVectDisplacement(vectIntersectionToOrigin) < csVectorMaths.GetVectDisplacement(vectIntersectionToOriginResult))
					{
						// If the absolute displacement of the current vector from the intersection to origin point
						// is smaller than the absolute displacement of the current result vector, assign the result
						// vector to be the current vector. 
						vectIntersectionToOriginResult = vectIntersectionToOrigin;

						// Make sure to memorise currently looked at obstacle line. 
						linObstacleResult = linObstacles[iCurLine];
					} // end else if
				} // end if
			} // end for

			// Check for the case that there is no result obstacle line. 
			// Return null in such a case. 
			if (linObstacleResult == null)
				return null;

			// This vector is no longer necessary, it was only used to get the first
			// collided with obstacle line. 
			vectIntersectionToOriginResult = null;

			/**** Find target point penetrating the furthest ****/
			// This is the result point of intersection which serves as an offset for the
			// vector projection of the intersection to target point vector. 
			csVector pointIntersectionResult = null;

			// The result intersection to target point vector will be projected onto
			// the obstacle line, to find the corrected position of the AABB. 
			csVector vectIntersectionToTargetResult = null;

			// The result origin point. 
			// Will be used later to determine the movement the translation of the AABB
			// to the corrected position. 
			csVector pointOriginResult = null;

			// Represents the slope of the AABB movement path. 
			// Used in the following for loop to assign a slope
			// to the AABB movement path, which is then used
			// to calculate points of intersection. 
			double dSlopeAABBVelocityPath = double.NaN;

			// Find the target point (of the points behind the obstacle line) which penetrates
			// the furthest. 
			for (int iCurOriginPoint = 0; iCurOriginPoint < pointsAABBTargetOriginObstacle.Length; iCurOriginPoint++)
			{
				csVector pointCurOrigin = pointsAABBTargetOriginObstacle[iCurOriginPoint];

				csVector pointCurTarget = new csVector(pointsAABBTargetOriginObstacle[iCurOriginPoint].x + oAABB.vx, 
				pointsAABBTargetOriginObstacle[iCurOriginPoint].y + oAABB.vy);

				// This line object represents the movement path of the AABB during this update. 
				// This line object is used to get points of intersection with the obstacle lines.
				csLine linAABBVelocityPath;

				// To avoid recalculating the slope every iteration, it is saved in a double
				// declared outside the loop and then updated in the first loop iteration,
				// then the value only gets reused in the following loop iterations. 
				if (double.IsNaN(dSlopeAABBVelocityPath))
				{
					linAABBVelocityPath = new csLine(pointCurOrigin, new csVector(pointCurOrigin.x + oAABB.vx, pointCurOrigin.y + oAABB.vy));
					dSlopeAABBVelocityPath = linAABBVelocityPath.dSlope;
				}
				else
				{
					linAABBVelocityPath = new csLine(pointCurOrigin, new csVector(pointCurOrigin.x + oAABB.vx, pointCurOrigin.y + oAABB.vy), dSlopeAABBVelocityPath);
				}

				csVector pointIntersection = csLine.GetIntersection(linAABBVelocityPath, linObstacleResult);

				csVector vectIntersectionToTarget = csVectorMaths.GetVector(pointIntersection, pointCurTarget);

				if (pointIntersectionResult == null && vectIntersectionToTargetResult == null)
				{
					// Assign result vector if there isn't one yet. 
					vectIntersectionToTargetResult = vectIntersectionToTarget;

					// Make sure to memorise the point of intersection. 
					pointIntersectionResult = pointIntersection;

					// Make sure to memorise the origin point.
					pointOriginResult = pointCurOrigin;
				} // end if
				else if (csVectorMaths.GetVectDisplacement(vectIntersectionToTarget) > csVectorMaths.GetVectDisplacement(vectIntersectionToTargetResult))
				{
					// If the absolute displacement of the current vector from the intersection to target point
					// is bigger than the absolute displacement of the current result vector, assign the result
					// vector to be the current vector. 
					vectIntersectionToTargetResult = vectIntersectionToTarget;

					// Make sure to memorise the point of intersection. 
					pointIntersectionResult = pointIntersection;

					// Make sure to memorise the origin point.
					pointOriginResult = pointCurOrigin;
				}
			} // end for

			/**** Project vector onto obstacle line to get corrected position ****/
			csVector vectProjected = csVectorMaths.ProjectVector(vectIntersectionToTargetResult, linObstacleResult.lineVect);

			// The point at the end of the projected vector, on the obstacle line. 
			csVector pointProjected = new csVector(pointIntersectionResult.x + vectProjected.x, pointIntersectionResult.y + vectProjected.y);

			// Get a vector describing the translation required to get the AABB to the corrected position. 
			// The corrected position can be found by subtracting the projected point from the origin point. 
			csVector vectOriginToCorrected = new csVector(pointProjected.x - pointOriginResult.x, pointProjected.y - pointOriginResult.y);

			// Create a list of collInfo objects.
			// At first only the collInfo object created during this method call will be added.
			// If recursive checks don't return null, the collInfo objects found by them
			// will also be added to this list and the list returned. 
			List<csCollisionInfo> collectedCollInfos = new List<csCollisionInfo>();

			csCollisionInfo collInfoResult = new csCollisionInfo();
			collInfoResult.pointIntersection = pointIntersectionResult;
			collInfoResult.pointOrigin = pointOriginResult;
			collInfoResult.vectOriginToCorrected = vectOriginToCorrected;
			collInfoResult.pointProjected = pointProjected;
			collInfoResult.pointTarget = new csVector(pointOriginResult.x + oAABB.vx, pointOriginResult.y + oAABB.vy);

			// These are absolute coordinates where the AABB would end up for resolving the collision
			// recorde by this collInfo object. When there are multiple collInfo objects returned 
			// this is especially useful, as one doesn't have to trace the path to get to the corrected
			// position, but can instead just apply these coordinates to the AABB. 
			collInfoResult.pointCorrectedOrigin = new csVector(oAABB.x + vectOriginToCorrected.x, oAABB.y + vectOriginToCorrected.y);

			collectedCollInfos.Add(collInfoResult);

			// Only go into this block if this isn't the last recurvise check. 
			// Otherwise these calculations would be wasted computational time,
			// the method calls itself, subtracts one from the recursive checks int
			// and then that newly called method cancels immediately and returns null. 
			// Up until the point when that method call finds out it's not supposed to do anything
			// this method call has already calculated many unnecessary values to pass on.
			if (iCheckIterations - 1 > 0)
			{

				// If the list of excluded obstacles hasn't been declared yet, do so now. 
				if (linExcludedObstacles == null)
					linExcludedObstacles = new List<csLine>();

				// Add the currently looked at and checked with obstacle line to the list of excluded
				// obstacle lines. It is unlikely that a recursive check would register a collision 
				// with this obstacle line. 
				linExcludedObstacles.Add(linObstacleResult);

				csVector vectOriginToIntersection = csVectorMaths.GetVector(pointOriginResult, pointIntersectionResult);
				csVector vectIntersectionToProjected = csVectorMaths.GetVector(pointIntersectionResult, pointProjected);

				// An AABB describing the location at the point of intersection recorded by this 
				// collInfo object. 
				csAABB oAABBAtIntersection = new csAABB(
					oAABB.x + vectOriginToIntersection.x, oAABB.y + vectOriginToIntersection.y,
					oAABB.w, oAABB.h,
					vectIntersectionToProjected.x, vectIntersectionToProjected.y);

				// Perform recursive method call, excluding the currently looked at obstacle line and 
				// subtracting 1 from the current iteration count. 
				List<csCollisionInfo> collInfoResultRecursive = csCollisionDetection.CheckCollision_AABB_Line(oAABBAtIntersection, linObstaclesPreCheck, 
				iCheckIterations: iCheckIterations - 1, linExcludedObstacles: linExcludedObstacles);

				// It is possible that the recursive check yields no results, in which case
				// it will be disregarded, otherwise the collInfo objects returned by
				// the recursive check will be added to the list of collInfos contained in
				// this current method call. 
				if (collInfoResultRecursive != null)
				{
					foreach(csCollisionInfo curCollInfo in collInfoResultRecursive)
					{
						// Memorise all the collInfos collected by recursive calls. 
						collectedCollInfos.Add(curCollInfo);
					} // end foreach
				} // end if
			} // end if

			return collectedCollInfos;
		} // end mtd

        /// <summary>
        /// Checks for overlap of two broadphase boxes. 
        /// </summary>
        /// <param name="AABBbroadBox"></param>
        /// <param name="lineBroadBox"></param>
        /// <returns>true, if there is a collision. </returns>
        public static bool CheckOverlap_broadBox_broadBox(csBroadphaseBox broadBox1, csBroadphaseBox broadBox2)
        {
            if (broadBox1.x > (broadBox2.x + broadBox2.w) || (broadBox1.x + broadBox1.w) < broadBox2.x ||
            broadBox1.y > (broadBox2.y + broadBox2.h) || (broadBox1.y + broadBox1.h) < broadBox2.y)
            {
                return false;
            } // end if
            else
            {
                return true;
            } // end else
        } // end mtd

        /// <summary>
        /// Checks if the given point and AABB are overlapping. 
        /// </summary>
        /// <param name="AABB">An AABB to check with. </param>
        /// <param name="point">A point to check with. </param>
        /// <returns>true, if there is an overlap. </returns>
        public static bool CheckOverlap_AABB_point(csAABB AABB, csVector point)
        {
			// It is possible that floating point numbers cause problems for the
			// exclusive check. Therefore it is better to sacrifice some
			// "precision" and opt for rounded up/down numbers. 
            point.x = Math.Round(point.x);
            point.y = Math.Round(point.y);

			// This exclusive check doesn't check if the point is contained
			// by the AABB, but rather if the point couldn't possibly be
			// contained. 
			// The idea being that it is faster to determine if the point
			// couldn't be contained than the other way around. 
            if ( AABB.x > point.x || (AABB.x + AABB.w) < point.x ||
            AABB.y > point.y || (AABB.y + AABB.h) < point.y )
            {
                return false;
            } // end if
            else
            {
                return true;
            } // end else
        } // end mtd

        /// <summary>
        /// Checks if the given point and broadPhaseBox are overlapping. 
        /// </summary>
		/// <param name="broadBox">A broadPhaseBox to check with. </param>
		/// <param name="point">A point to check with. </param>
        /// <returns>true, if there is an overlap. </returns>
        public static bool CheckOverlap_broadphaseBox_point(csBroadphaseBox broadBox, csVector point)
        {
            point.x = Math.Round(point.x);
            point.y = Math.Round(point.y);

            if (broadBox.x > point.x || (broadBox.x + broadBox.w) < point.x ||
            broadBox.y > point.y || (broadBox.y + broadBox.h) < point.y)
            {
                return false;
            } // end if
            else
            {
                return true;
            } // end else
        } // end mtd

    } // end cs
} // end ns
