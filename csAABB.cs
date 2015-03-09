using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
	/// <summary>
	/// Represents a rectangular, axis aligned bounding box object. 
	/// </summary>
    public class csAABB
    {
		/// <summary>
		/// The points that define the corners of this AABB. 
		/// </summary>
        public csVector[] points;

		/// <summary>
		/// The broadphase box is a simpler AABB, which encompasses this AABB
		/// and this AABB's target location. 
		/// The target location is where this AABB would end up if it's velocity
		/// was added to the current position of the AABB. 
		/// </summary>
        public csBroadphaseBox broadBox;

		/// <summary>
		/// The coordinates, dimensions and velocity of this AABB. 
		/// </summary>
        private double _x, _y, _w, _h, _vx, _vy;

		/// <summary>
		/// The X-coordinate of this AABB. 
		/// </summary>
        public double x
        {
            get { return this._x; }
            set { this._x = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// The Y-coordinate of this AABB. 
		/// </summary>
        public double y
        {
            get { return this._y; }
            set { this._y = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// The width of this AABB. 
		/// </summary>
        public double w
        {
            get { return this._w; }
            set { this._w = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// The height of this AABB. 
		/// </summary>
        public double h
        {
            get { return this._h; }
            set { this._h = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// The x velocity of this AABB. 
		/// </summary>
        public double vx
        {
            get { return this._vx; }
            set { this._vx = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// The y velocity of this AABB. 
		/// </summary>
        public double vy
        {
            get { return this._vy; }
            set { this._vy = value; this.UpdatePointCoords(); }
        }

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="xIn"></param>
		/// <param name="yIn"></param>
		/// <param name="wIn"></param>
		/// <param name="hIn"></param>
		/// <param name="vxIn"></param>
		/// <param name="vyIn"></param>
        public csAABB(double xIn, double yIn, double wIn, double hIn, double vxIn, double vyIn)
        {
            this._x = xIn;
            this._y = yIn;
            this._w = wIn;
            this._h = hIn;
            this._vx = vxIn;
            this._vy = vyIn;

            this.points = new csVector[4];
            this.points[0] = new csVector(this.x, this.y);
            this.points[1] = new csVector(this.x + this.w, this.y);
            this.points[2] = new csVector(this.x, this.y + this.h);
            this.points[3] = new csVector(this.x + this.w, this.y + this.h);

            this.broadBox = new csBroadphaseBox(this);
        } // end constructor

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="xIn"></param>
		/// <param name="yIn"></param>
		/// <param name="wIn"></param>
		/// <param name="hIn"></param>
        public csAABB(double xIn, double yIn, double wIn, double hIn)
        {
            this._x = xIn;
            this._y = yIn;
            this._w = wIn;
            this._h = hIn;

            this.points = new csVector[4];
            this.points[0] = new csVector(this.x, this.y);
            this.points[1] = new csVector(this.x + this.w, this.y);
            this.points[2] = new csVector(this.x, this.y + this.h);
            this.points[3] = new csVector(this.x + this.w, this.y + this.h);

            this.broadBox = new csBroadphaseBox(this);
        } // end constructor

		/// <summary>
		/// Updates the points that define this AABB's corners. 
		/// </summary>
        private void UpdatePointCoords()
        {
            this.points[0].x = this.x;
            this.points[0].y = this.y;

            this.points[1].x = this.x + this.w;
            this.points[1].y = this.y;

            this.points[2].x = this.x;
            this.points[2].y = this.y + this.h;

            this.points[3].x = this.x + this.w;
            this.points[3].y = this.y + this.h;

            this.broadBox.UpdateBroadphaseBox(this);
        } // end mtd

		/// <summary>
		/// Checks if two AABBs are overlapping. 
		/// Returns true if they are overlapping. 
		/// </summary>
		/// <param name="b1">The first AABB. </param>
		/// <param name="b2">The second AABB. </param>
		/// <returns>A bool, which indicates whether there was a collision or not. </returns>
        public bool CheckAABB(csAABB b1, csAABB b2)
        {
            return !(b1.x + b1.w < b2.x || b1.x > b2.x + b2.w || b1.y + b1.h < b2.y || b1.y > b2.y + b2.h);
        } // end mtd

		/// <summary>
		/// Gets the target points, based on the current velocity. 
		/// A target point is an origin point plus the current velocity, resulting
		/// in the position where the point will end up at the end of the next update. 
		/// </summary>
		/// <returns>Array of origin points with velocity added, resulting in target points. </returns>
		public csVector[] GetTargetPoints()
		{
			csVector[] vectsTarget = new csVector[this.points.Length];

			for (int iCurOrigin = 0; iCurOrigin < this.points.Length; iCurOrigin++)
			{
				vectsTarget[iCurOrigin] = new csVector(this.points[iCurOrigin].x + this.vx, this.points[iCurOrigin].y + this.vy);
			}

			return vectsTarget;
		} // end mtd

    } // end cs
} // end ns
