#define DEBUG_TEST1

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestApp1
{
    public partial class frmMain : Form
    {
		/**** Declarations ****/
        private Graphics graphics;
        private Pen penBlack, penLightGray, penRed, penBlue, penGreen, penPurple, penOrange;

		private csAABB oAABBTesting, oAABBTarget;

		private csLine[] oWorldBoundaryLines;

		/// <summary>
		/// Constructor. 
		/// Sets up a graphics object and a few pens to choose from. 
		/// Creates an instance of an AABB for testing purposes. 
		/// </summary>
        public frmMain()
        {
            InitializeComponent();

            this.graphics = this.CreateGraphics();
            this.penBlack = new Pen(Color.Black);
			this.penLightGray = new Pen(Color.LightGray);

            this.penRed = new Pen(Color.Red);
            this.penBlue = new Pen(Color.Blue);
            this.penGreen = new Pen(Color.Green);
			this.penPurple = new Pen(Color.Purple);
			this.penOrange = new Pen(Color.Orange);

            this.oAABBTesting = new csAABB(400, 400, 20, 30, 50, 50);
			this.oAABBTarget = new csAABB(oAABBTesting.x + oAABBTesting.vx, oAABBTesting.y + oAABBTesting.vy, oAABBTesting.w, oAABBTesting.h);

			/**** Set up lines that make up the boundaries of the "world" ****/

			double dOffset = 400;

			this.oWorldBoundaryLines = new csLine[] {
				new csLine(new csVector(0 + dOffset, 0 + dOffset), new csVector(100 + dOffset, 0 + dOffset), true),
				new csLine(new csVector(100 + dOffset, 0 + dOffset), new csVector(200 + dOffset, -100 + dOffset), true),
				new csLine(new csVector(200 + dOffset, -100 + dOffset), new csVector(200 + dOffset, -200 + dOffset), true),
				new csLine(new csVector(200 + dOffset, -200 + dOffset), new csVector(100 + dOffset, -300 + dOffset), true),
				new csLine(new csVector(100 + dOffset, -300 + dOffset), new csVector(0 + dOffset, -300 + dOffset), true),
				new csLine(new csVector(0 + dOffset, -300 + dOffset), new csVector(-100 + dOffset, -200 + dOffset), true),
				new csLine(new csVector(-100 + dOffset, -200 + dOffset), new csVector(-100 + dOffset, -100 + dOffset), true),
				new csLine(new csVector(-100 + dOffset, -100 + dOffset), new csVector(0 + dOffset, 0 + dOffset), true)
            };
        } // end constr

		/// <summary>
		/// Updates the testing AABB and performs a collision check between the testing AABB and 
		/// the line objects that make up the "world" boundaries. 
		/// </summary>
		/// <param name="xIn"></param>
		/// <param name="yIn"></param>
		/// <param name="wIn"></param>
		/// <param name="hIn"></param>
		/// <param name="vxIn"></param>
		/// <param name="vyIn"></param>
        public void DoTest(double xIn, double yIn, double wIn, double hIn, double vxIn, double vyIn)
        {
			// Clears the form of any previously drawn lines. 
            this.graphics.Clear(Color.White);

			// Set the testing AABB's properties. 
			// Repositions the testing AABB. 
			this.oAABBTesting.x = xIn;
			this.oAABBTesting.y = yIn;
			this.oAABBTesting.w = wIn;
			this.oAABBTesting.h = hIn;
			this.oAABBTesting.vx = vxIn;
			this.oAABBTesting.vy = vyIn;
            
			// Draw indicators for points of AABB. 
			for (int iCurPoint = 0; iCurPoint < this.oAABBTesting.points.Length; iCurPoint++)
			{
				this.DrawText(this.oAABBTesting.points[iCurPoint], iCurPoint.ToString(), Color.Black);
			}

			// Update the AABB representing the target location of the testing AABB. 
			this.oAABBTarget.x = this.oAABBTesting.x + this.oAABBTesting.vx;
			this.oAABBTarget.y = this.oAABBTesting.y + this.oAABBTesting.vy;
			this.oAABBTarget.w = this.oAABBTesting.w;
			this.oAABBTarget.h = this.oAABBTesting.h;

            // Draw geometric shapes
            // testing AABB
			this.DrawBox(oAABBTesting.broadBox, this.penLightGray);
			this.DrawAABB(oAABBTesting, this.penBlack);
            this.DrawAABB(oAABBTarget, this.penBlue);

            // world boundary lines
			for (int i = 0; i < this.oWorldBoundaryLines.Length; i++)
            {
				this.DrawBox(this.oWorldBoundaryLines[i].broadBox, this.penLightGray);
				this.DrawLine(this.oWorldBoundaryLines[i].P0, this.oWorldBoundaryLines[i].P1, this.penBlack);
            } // end for

            /**** Collision Detection ****/
			List<csCollisionInfo> collInfoResult = csCollisionDetection.CheckCollision_AABB_Line(oAABBTesting, this.oWorldBoundaryLines, iCheckIterations: 3);

			if (collInfoResult != null)
			{
				int iPointSize = 8;

				foreach(csCollisionInfo curCollInfo in collInfoResult)
				{
					// Create vector from point of intersection to projected point. 
					csVector vectIntersectionToProjected = csVectorMaths.GetVector(curCollInfo.pointIntersection, curCollInfo.pointProjected);

					// Create new AABB at the corrected position.
					csAABB oAABBCorrected = new csAABB(curCollInfo.pointCorrectedOrigin.x, curCollInfo.pointCorrectedOrigin.y,
					this.oAABBTesting.w, this.oAABBTesting.h, vectIntersectionToProjected.x, vectIntersectionToProjected.y);

					// Draw AABB at corrected position. 
					this.DrawAABB(oAABBCorrected, this.penOrange);

					// Draw path of movement. 
					this.DrawLine(curCollInfo.pointOrigin, curCollInfo.pointTarget, this.penBlue);

					// Draw line from target point to projected point. 
					this.DrawLine(curCollInfo.pointTarget, curCollInfo.pointProjected, this.penPurple);

					// Draw the origin point. 
					this.DrawPoint(curCollInfo.pointOrigin, iPointSize, this.penGreen);

					// Draw the point of intersection. 
					this.DrawPoint(curCollInfo.pointIntersection, iPointSize, this.penRed);

					// Draw the projected point. 
					this.DrawPoint(curCollInfo.pointProjected, iPointSize, this.penPurple);
				} // end foreach
			} // end if
        } // end mtd

		/// <summary>
		/// Draws a line on the form from P0 to P1, using the given pen. 
		/// </summary>
		/// <param name="P0">The starting point. </param>
		/// <param name="P1">The ending point. </param>
		/// <param name="pen">A pen object. </param>
        public void DrawLine(csVector P0, csVector P1, Pen pen)
        {
            int x0, x1, y0, y1;

            int.TryParse(Math.Truncate(P0.x).ToString(), out x0);
            int.TryParse(Math.Truncate(P0.y).ToString(), out y0);
            int.TryParse(Math.Truncate(P1.x).ToString(), out x1);
            int.TryParse(Math.Truncate(P1.y).ToString(), out y1);

            this.graphics.DrawLine(pen, x0, y0, x1, y1);
        }

		/// <summary>
		/// Draws the four lines that make up an AABB on the form. 
		/// </summary>
		/// <param name="oAABB">An AABB object. </param>
		/// <param name="pen">A pen object.</param>
        public void DrawAABB(csAABB oAABB, Pen pen)
        {
            csVector TL = new csVector(oAABB.x, oAABB.y);
            csVector TR = new csVector(oAABB.x + oAABB.w, oAABB.y);
            csVector BL = new csVector(oAABB.x, oAABB.y + oAABB.h);
            csVector BR = new csVector(oAABB.x + oAABB.w, oAABB.y + oAABB.h);

            this.DrawLine(TL, TR, pen);
            this.DrawLine(TR, BR, pen);
            this.DrawLine(BR, BL, pen);
            this.DrawLine(BL, TL, pen);
        }

		/// <summary>
		/// Draws the four lines that make up an AABB on the form. 
		/// </summary>
		/// <param name="box">An AABB object. </param>
		/// <param name="pen">A pen object.</param>
        public void DrawBox(csBroadphaseBox box, Pen pen)
        {
            csVector TL = new csVector(box.x, box.y);
            csVector TR = new csVector(box.x + box.w, box.y);
            csVector BL = new csVector(box.x, box.y + box.h);
            csVector BR = new csVector(box.x + box.w, box.y + box.h);

            this.DrawLine(TL, TR, pen);
            this.DrawLine(TR, BR, pen);
            this.DrawLine(BR, BL, pen);
            this.DrawLine(BL, TL, pen);
        }

		/// <summary>
		/// Draws a crosshair at a given point on the form. 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="pixelSize"></param>
		/// <param name="pen"></param>
        public void DrawPoint(csVector point, int pixelSize, Pen pen)
        {
            this.DrawLine(point, new csVector(point.x + pixelSize, point.y), pen);
            this.DrawLine(point, new csVector(point.x - pixelSize, point.y), pen);
            this.DrawLine(point, new csVector(point.x, point.y + pixelSize), pen);
            this.DrawLine(point, new csVector(point.x, point.y - pixelSize), pen);
        }

		/// <summary>
		/// Draws a given string at the given point on the form. 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="sText"></param>
		/// <param name="pen"></param>
		public void DrawText(csVector point, string sText, Color color, int iFontSize = 8)
		{
			this.graphics.DrawString(sText, new Font("Arial", iFontSize), new SolidBrush(color), new PointF((float)point.x, (float)point.y));
		}

		/// <summary>
		/// Converts a decimal into a double and returns the double. 
		/// </summary>
		/// <param name="dDecimalIn"></param>
		/// <returns></returns>
		public double GetDecimalInDouble(decimal dDecimalIn)
		{
			double dResult;
			double.TryParse(dDecimalIn.ToString(), out dResult);
			return dResult;
		}

		/**** Events ****/

        private void nudBox_ValueChanged(object sender, EventArgs e)
        {
            this.DoTest(
                this.oAABBTesting.x,
                this.oAABBTesting.y,
                this.GetDecimalInDouble(nudBox_W.Value),
                this.GetDecimalInDouble(nudBox_H.Value),
                this.oAABBTesting.vx,
                this.oAABBTesting.vy
            );
        }

		/// <summary>
		/// The event whenever a mouse button is pressed. 
		/// Used to set the target location of the testing AABB and to initiate the collision checks. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            csVector mousePos = new csVector(e.X, e.Y);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.DoTest(
                    mousePos.x,
                    mousePos.y,
                    this.GetDecimalInDouble(nudBox_W.Value),
                    this.GetDecimalInDouble(nudBox_H.Value),
                    this.oAABBTesting.vx,
                    this.oAABBTesting.vy
                );
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.DoTest(
                    this.oAABBTesting.x,
                    this.oAABBTesting.y,
                    this.GetDecimalInDouble(nudBox_W.Value),
                    this.GetDecimalInDouble(nudBox_H.Value),
                    mousePos.x - this.oAABBTesting.x,
                    mousePos.y - this.oAABBTesting.y
                );
            }
        }
    } // end cs
} // end ns
