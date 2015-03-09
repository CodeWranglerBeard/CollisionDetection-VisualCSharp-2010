using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp1
{
    /// <summary>
    /// Represents an axis-aligned bounding box without velocity. 
    /// </summary>
    public class csBroadphaseBox
    {
        public double x, y, w, h;

        public csBroadphaseBox()
        { } // end constructor

        public csBroadphaseBox(csAABB box)
        {
            this.UpdateBroadphaseBox(box);
        } // end constructor

        public csBroadphaseBox(csLine line)
        {
            this.UpdateBroadphaseBox(line);
        } // end constructor

        /// <summary>
        /// Updates the dimensions of this broadphase box. 
        /// </summary>
        public void UpdateBroadphaseBox(double xIn, double yIn, double wIn, double hIn)
        {
            this.x = xIn;
            this.y = yIn;
            this.w = wIn;
            this.h = hIn;
        } // end mtd

        /// <summary>
        /// Updates the dimensions of this broadphase box. 
        /// Gets an AABB's dimensions to update this object's dimensions with. 
        /// </summary>
        public void UpdateBroadphaseBox(csAABB box)
        {
            // x
            if (box.vx >= 0)
            { this.x = box.x; }
            else
            { this.x = box.x + box.vx; }

            // y
            if (box.vy >= 0)
            { this.y = box.y; }
            else
            { this.y = box.y + box.vy; }

            // w
            if (box.vx >= 0)
            { this.w = box.w + box.vx; }
            else
            { this.w = box.w - box.vx; }

            // h
            if (box.vy >= 0)
            { this.h = box.h + box.vy; }
            else
            { this.h = box.h - box.vy; }
        } // end mtd

        /// <summary>
        /// Updates the dimensions of this broadphase box. 
        /// Gets a line's dimensions to update this object's dimensions with. 
        /// </summary>
        public void UpdateBroadphaseBox(csLine line)
        {
            // x
            if (line.P1.x < line.P0.x)
            {
                this.x = line.P1.x;
                this.w = line.P0.x - line.P1.x;
            }
            else
            {
                this.x = line.P0.x;
                this.w = line.P1.x - line.P0.x;
            }

            // y
            if (line.P1.y < line.P0.y)
            {
                this.y = line.P1.y;
                this.h = line.P0.y - line.P1.y;
            }
            else
            {
                this.y = line.P0.y;
                this.h = line.P1.y - line.P0.y;
            }
        } // end mtd
    } // end cs
} // end ns