using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace cglr2
{
    public partial class cglr2 : Form
    {
        // ================= Attributes ================
        // Tooltiping
        private ToolTip toolTip1;
        private readonly string[] Tooltips;

        // Transformations object
        PolygonalGrObjCollection GraphicsObject;

        private float RotationValue, OldRotValue;
        PointF ScalingVals, OldScalingVals;
        PointF TranslationVals, OldTransVals;
        PointF RealRelatPoint;  //The point, which some transformations are relative to.
        Point ScreenRelativityPoint;    //Screen coordinates of the RealRelatPoint.

        // Drawing on screen
        int ScreenScaling;      //Variable, that determines relation between the real and screen sizes.
        SizeF RealPicBoxSize;
        float[] RealBorders;    //Real borders of the screen. Indexes: 0 - left, 1 - right, 2 - bottom, 3 - top
        PointF RealCentreCoord; //Real coordinates of the screen centre.
        private bool ScreenSizeChanged;     
        private bool RelatPChangedByMouse;  //Flag, that tells whether the relativity point was changed by the user or not.

        // ======================= Methods =======================
        public cglr2()
        {
            InitializeComponent();

            GraphicsObject = new Sword();

            RotationValue = OldRotValue = 0;
            ScalingVals = new PointF(1, 1);
            OldScalingVals = new PointF(1, 1);

            RealPicBoxSize = new SizeF();
            RealCentreCoord = new PointF(RealPicBoxSize.Width / 2, RealPicBoxSize.Height / 2);
            RealBorders = new float[4];

            SetRelativityPoint(new PointF(0, 0));

            RelatPChangedByMouse = false;
            ScreenSizeChanged = false;
            //Setting up the tooltips
            toolTip1 = new ToolTip();
            Tooltips = new string[4]
            {
                // 0 - tooltip for translation.
                "Reposition the object.\n +value moves it to the right horizontaly and to the top verticaly,\n" +
                " -value moves it to the left horizontaly and to the bottom verticaly.",    
                // 1 - tooltip for rotation.
                "Rotate the object.\n +value rotates counter-clockwise,\n -value rotates clockwise.",  
                // 3 - tooltip for scaling.
                "Scale the size of the object relative to the point.\n" +
                " Value of 1 returns the object to initial size.\n Values > 1 will make the object bigger.\n" +
                " Values < 1 will make it smaller.\n" +
                "All values must be > 0.", 
                //4 - tooltip for the relativity point
                "This is the point, relative to which the transformations will be performed.\n(All except translation)" 
            };

            //Set up the delays for the tooltip
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 400;
            toolTip1.ReshowDelay = 500;
            //Set up the tooltip
            toolTip1.SetToolTip(TransHelp, Tooltips[0]);
            toolTip1.SetToolTip(RotationHelp, Tooltips[1]);
            toolTip1.SetToolTip(ScalingHelp, Tooltips[2]);
            toolTip1.SetToolTip(RelatPointHelp, Tooltips[3]);
        }

        //======================== Drawing ====================
        private void MainPB_Paint(object sender, PaintEventArgs e)
        {
            // If needed - transform the object.
            if (RotationValue != OldRotValue)
            {
                GraphicsObject.Rotate(RotationValue - OldRotValue, RealRelatPoint);
                OldRotValue = RotationValue;
            }
            if (TranslationVals != OldTransVals)
            {
                GraphicsObject.Translate(TranslationVals.X - OldTransVals.X, TranslationVals.Y - OldTransVals.Y);
                OldTransVals = TranslationVals;
            }
            if (ScalingVals != OldScalingVals)
            {
                GraphicsObject.Scale(ScalingVals.X / OldScalingVals.X, ScalingVals.Y / OldScalingVals.Y, RealRelatPoint);
                OldScalingVals = ScalingVals;
            }
            CalculateObjectPos();

            // Get values, needed for drawing
            Point[][] NormalisedObject = GraphicsObject.GetNormalisedElementsOutline(RealBorders[0], RealBorders[1], RealBorders[2], RealBorders[3], MainPB.Size).ToArray();
            PolygonalGraphicsObj[] InternalObjects = GraphicsObject.GetElements().ToArray();

            // Draw
            for (int i = 0; i < InternalObjects.Length; i++) 
            {
                e.Graphics.FillPolygon(new SolidBrush(InternalObjects[i].FillColor), NormalisedObject[i]);
                e.Graphics.DrawPolygon(new Pen(InternalObjects[i].OutlineColor, 2), NormalisedObject[i]);
            }

            DrawAxes(e);
            
            int RelatPointDiameter = MainPB.Width / 90;
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), ScreenRelativityPoint.X - RelatPointDiameter / 2,
                ScreenRelativityPoint.Y - RelatPointDiameter / 2, RelatPointDiameter, RelatPointDiameter);
        }

        // Draws coordinate axes and markers using PaintEventArgs e
        private void DrawAxes(PaintEventArgs e)
        {
            int yAxisXCoordinate = Normalisation.NormaliseX(0, RealBorders[0], RealBorders[1], MainPB.Width),     //Coordinates of axes on the graph
                xAxisYCoordinate = Normalisation.NormaliseY(0, RealBorders[2], RealBorders[3], MainPB.Height);    
            bool xAxisVisible = true, yAxisVisible = true;

            if (yAxisXCoordinate < 0)
            {
                yAxisXCoordinate = 0;
                yAxisVisible = false;
            }
            else if (yAxisXCoordinate > MainPB.Width)
            {
                yAxisXCoordinate = MainPB.Width;
                yAxisVisible = false;
            }

            if (xAxisYCoordinate < 0)
            {
                xAxisYCoordinate = 0;
                xAxisVisible = false;
            }
            else if (xAxisYCoordinate > MainPB.Height)
            {
                xAxisYCoordinate = MainPB.Height;
                xAxisVisible = false;
            }

            // Get markers values
            float[] XMarkers = Coordinates.SpreadXMarkers(RealBorders[0], RealBorders[1], (RealBorders[1] - RealBorders[0]) / 10);
            float[] YMarkers = Coordinates.SpreadYMarkers(RealBorders[2], RealBorders[3], (RealBorders[3] - RealBorders[2]) / 10);
            // Get markers screen coordinates
            int[] nXMarkers = Normalisation.NormaliseXArray(XMarkers, RealBorders[0], RealBorders[1], MainPB.Width);
            int[] nYMarkers = Normalisation.NormaliseYArray(YMarkers, RealBorders[2], RealBorders[3], MainPB.Height);

            int MarkersRadius = 3;
            // Draw
            Font mFont = new Font(Font.FontFamily, 8, FontStyle.Bold);
            Pen mPen = new Pen(Color.LightGray, 2);
            SolidBrush mBrush = new SolidBrush(Color.Black);
            for (int i = 0; i < nXMarkers.Length; i++)
            {
                e.Graphics.DrawLine(mPen, nXMarkers[i], 0, nXMarkers[i], MainPB.Height);
                e.Graphics.FillEllipse(mBrush, nXMarkers[i] - MarkersRadius, xAxisYCoordinate - MarkersRadius, MarkersRadius * 2, MarkersRadius * 2);
                e.Graphics.DrawString(XMarkers[i].ToString(), mFont, Brushes.Black, nXMarkers[i],
                    xAxisYCoordinate + mFont.Height > MainPB.Height ? xAxisYCoordinate - 1.5f * mFont.Height : xAxisYCoordinate + 2);
            }
            for (int i = 0; i < nYMarkers.Length; i++)
            {
                e.Graphics.DrawLine(mPen, 0, nYMarkers[i], MainPB.Width, nYMarkers[i]);
                e.Graphics.FillEllipse(mBrush, yAxisXCoordinate - MarkersRadius, nYMarkers[i] - MarkersRadius, MarkersRadius * 2, MarkersRadius * 2);
                e.Graphics.DrawString(YMarkers[i].ToString(), mFont, Brushes.Black,
                    yAxisXCoordinate + YMarkers[i].ToString().Length * mFont.Size > MainPB.Width ? yAxisXCoordinate - YMarkers[i].ToString().Length * mFont.Size : yAxisXCoordinate,
                    nYMarkers[i]);
            }

            if (yAxisVisible)
                e.Graphics.DrawLine(new Pen(Color.Black, 2), yAxisXCoordinate, 0, yAxisXCoordinate, MainPB.Height);
            if (xAxisVisible)
                e.Graphics.DrawLine(new Pen(Color.Black, 2), 0, xAxisYCoordinate, MainPB.Width, xAxisYCoordinate);
        }

        // ============================================ Calculations ============================================
        
        // Calculates screen dimentions based on Scaling and Screen centre parameters.
        private void CalculateScreenDimentions(int Scaling, PointF ScrnCentre)
        {
            bool ScalingNeeded = (Scaling != ScreenScaling && Scaling >= ZoomScrlBar.Minimum && Scaling <= ZoomScrlBar.Maximum);
            if (ScalingNeeded || ScreenSizeChanged)
            {
                RealPicBoxSize.Width = MainPB.Width / Scaling;
                RealPicBoxSize.Height = MainPB.Height / Scaling;
                ScreenSizeChanged = true;
            }

            if (ScrnCentre != RealCentreCoord || ScreenSizeChanged)
            {
                RealCentreCoord = ScrnCentre;
                RealBorders[0] = RealCentreCoord.X - RealPicBoxSize.Width / 2;
                RealBorders[1] = RealCentreCoord.X + RealPicBoxSize.Width / 2;
                RealBorders[2] = RealCentreCoord.Y - RealPicBoxSize.Height / 2;
                RealBorders[3] = RealCentreCoord.Y + RealPicBoxSize.Height / 2;
                ScreenSizeChanged = false;
            }

            if (ScalingNeeded)
                ZoomScrlBar.Value = ScreenScaling = Scaling;
        }

        // Calculates and adjusts the object possition on the screen by moving the screen centre or changing screen scaling if needed
        private void CalculateObjectPos()
        {
            //Get graphics object dimentions
            float minX = GraphicsObject.GetLeftBorder();
            float maxX = GraphicsObject.GetRightBorder();
            float minY = GraphicsObject.GetBottomBorder();
            float maxY = GraphicsObject.GetTopBorder();
            //Make first calculation
            CalculateScreenDimentions(ZoomScrlBar.Value, RealCentreCoord);
            //Then check if this calculation is suitable

            // Horizontal check
            if (RealPicBoxSize.Width < maxX - minX) //If object`s width is bigger than the screen width
            {
                //Resize the screen
                int NewScaling = AutoRescalingBox.Checked ? MainPB.Width / (int)Math.Floor(maxX - minX) : ScreenScaling;
                CalculateScreenDimentions(NewScaling, new PointF((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY));
            }
            else
            {
                if (minX < RealBorders[0])  //If the object`s left border sticks out of the screen
                {
                    //Move screen to the left
                    float NewCentreX = RealCentreCoord.X - (RealBorders[0] - minX);
                    CalculateScreenDimentions(ScreenScaling, new PointF(NewCentreX, RealCentreCoord.Y));
                }
                else if (maxX > RealBorders[1]) //If it happens with the right side
                {
                    //Move screen to the right
                    float NewCentreX = RealCentreCoord.X + (maxX - RealBorders[1]);
                    CalculateScreenDimentions(ScreenScaling, new PointF(NewCentreX, RealCentreCoord.Y));
                }
            }

            // Vertical check
            if (RealPicBoxSize.Height < maxY - minY)    //If object`s height is bigger than the screen height
            {
                //Resize the screen
                int NewScaling = AutoRescalingBox.Checked ? MainPB.Height / (int)Math.Floor(maxY - minY) : ScreenScaling;
                CalculateScreenDimentions(NewScaling, new PointF((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY));
            }
            else
            {
                if (minY < RealBorders[2])
                {
                    //Move screen to the bottom
                    float NewCentreY = RealCentreCoord.Y - (RealBorders[2] - minY);
                    CalculateScreenDimentions(ScreenScaling, new PointF(RealCentreCoord.X, NewCentreY));
                }
                else if (maxY > RealBorders[3])
                {
                    //Move screen to the top
                    float NewCentreY = RealCentreCoord.Y + (maxY - RealBorders[3]);
                    CalculateScreenDimentions(ScreenScaling, new PointF(RealCentreCoord.X, NewCentreY));
                }
            }
            // If the object is too small
            if ((RealPicBoxSize.Height - (maxY - minY) > RealPicBoxSize.Height * (2f / 3f)) && (RealPicBoxSize.Width - (maxX - minX) > RealPicBoxSize.Width * (2f / 3f))) 
            {
                //Resize the screen        
                int NewScaling = AutoRescalingBox.Checked ? MainPB.Height / (int)Math.Ceiling((maxY - minY) * 2) : ScreenScaling;
                CalculateScreenDimentions(NewScaling, new PointF((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY));
            }

            SetRelativityPoint(RealRelatPoint);
            
        }

        // Sets the relativity point, which is essential for most transformations
        private void SetRelativityPoint(PointF NewPoint)
        {
            RealRelatPoint = NewPoint;
            ScreenRelativityPoint.X = Normalisation.NormaliseX(RealRelatPoint.X, RealBorders[0], RealBorders[1], MainPB.Width);
            ScreenRelativityPoint.Y = Normalisation.NormaliseY(RealRelatPoint.Y, RealBorders[2], RealBorders[3], MainPB.Height);
        }

        private void SetRelativityPoint(Point NewPoint)
        {
            ScreenRelativityPoint = NewPoint;
            RealRelatPoint.X = (float)Normalisation.DenormaliseX(ScreenRelativityPoint.X, MainPB.Width, RealBorders[0], RealBorders[1]);
            RealRelatPoint.Y = (float)Normalisation.DenormaliseY(ScreenRelativityPoint.Y, MainPB.Height, RealBorders[2], RealBorders[3]);
        }

        // Form evemts ============================================
        //Translation tooltip displaying.
        private void TransHelp_Click(object sender, EventArgs e)
        {
            toolTip1.Show(Tooltips[0], TransHelp);
        }
        //Rotation tooltip displaying.
        private void RotationHelp_Click(object sender, EventArgs e)
        {
            toolTip1.Show(Tooltips[1], RotationHelp);
        }
        //Scaling tooltip displaying.
        private void ScalingHelp_Click(object sender, EventArgs e)
        {
            toolTip1.Show(Tooltips[2], ScalingHelp);
        }
        //Relativity point tooltip displaying.
        private void RelatPointHelp_Click(object sender, EventArgs e)
        {
            toolTip1.Show(Tooltips[3], RelatPointHelp);
        }

        //The relativity point change
        private void RelatXTB_ValueChanged(object sender, EventArgs e)
        {
            if (!RelatPChangedByMouse)
            {
                SetRelativityPoint(new PointF((float)RelatXTB.Value, (float)RelatYTB.Value));
                MainPB.Refresh();
            }
        }
        private void RelatYTB_ValueChanged(object sender, EventArgs e)
        {
            if (!RelatPChangedByMouse)
            {
                SetRelativityPoint(new PointF((float)RelatXTB.Value, (float)RelatYTB.Value));
                MainPB.Refresh();
            }
        }
        //The relativity point was set by mouse click
        private void MainPB_MouseClick(object sender, MouseEventArgs e)
        {
            RelatPChangedByMouse = true;
            SetRelativityPoint(e.Location);
            RelatXTB.Value = (decimal)RealRelatPoint.X;
            RelatYTB.Value = (decimal)RealRelatPoint.Y;
            RelatPChangedByMouse = false;
            MainPB.Refresh();
        }

        //Translation values changed
        private void HorTransBox_ValueChanged(object sender, EventArgs e)
        {
            if ((float)HorTransBox.Value != TranslationVals.X)
            {
                TranslationVals.X = (float)HorTransBox.Value;
                MainPB.Refresh();
            }
        }
        private void VertTransBox_ValueChanged(object sender, EventArgs e)
        {
            if ((float)VertTransBox.Value != TranslationVals.Y)
            {
                TranslationVals.Y = (float)VertTransBox.Value;
                MainPB.Refresh();
            }
        }

        //Rotaion values changed
        private void RotDegBox_ValueChanged(object sender, EventArgs e)
        {
            if (RotDegBox.Value < 0)
                RotDegBox.Value += 360;
            else if (RotDegBox.Value >= 360)
                RotDegBox.Value -= 360;
            RotationValue = (float)RotDegBox.Value;
            MainPB.Refresh();
        }

        //Object scaling values changed
        private void HorScalBox_ValueChanged(object sender, EventArgs e)
        {
            ScalingVals.X = (float)HorScalBox.Value;
            MainPB.Refresh();
        }
        private void VertScalBox_ValueChanged(object sender, EventArgs e)
        {
            ScalingVals.Y = (float)VertScalBox.Value;
            MainPB.Refresh();
        }

        //Object reflection
        private void ReflectXButton_Click(object sender, EventArgs e)
        {
            GraphicsObject.Reflect(false, true, RealRelatPoint);
            MainPB.Refresh();
        }
        private void ReflectYButon_Click(object sender, EventArgs e)
        {
            GraphicsObject.Reflect(true, false, RealRelatPoint);
            MainPB.Refresh();
        }

        //Screen scaling
        private void AutoRescalingBox_CheckStateChanged(object sender, EventArgs e)
        {
            ZoomScrlBar.Enabled = !AutoRescalingBox.Checked;
            MainPB.Refresh();
        }
        private void ZoomScrlBar_ValueChanged(object sender, EventArgs e)
        {
            if (ScreenScaling != ZoomScrlBar.Value)
            {
                MainPB.Refresh();
            }
        }
        private void CentreScreenButton_Click(object sender, EventArgs e)
        {
            float lb = GraphicsObject.GetLeftBorder(), rb = GraphicsObject.GetRightBorder(),
                bb = GraphicsObject.GetBottomBorder(), tb = GraphicsObject.GetTopBorder();
            CalculateScreenDimentions(ScreenScaling, new PointF((rb - lb) / 2 + lb, (tb - bb) / 2 + bb));
            MainPB.Refresh();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            ScreenSizeChanged = true;
            MainPB.Refresh();
        }

        //Enabling and disabling object parts
        private void EnableBladeCB_CheckedChanged(object sender, EventArgs e)
        {
            GraphicsObject.Element(0).Enabled = EnableBladeCB.Checked;
        }
        private void EnableHiltCB_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableHiltCB.Checked)
            {
                EnableGuardCB.Enabled = true;
                EnableGripCB.Enabled = true;
                EnablePommelCB.Enabled = true;
            }
            else 
            {
                EnableGuardCB.Enabled = false;
                EnableGripCB.Enabled = false;
                EnablePommelCB.Enabled = false;
            }
            GraphicsObject.Element(1).Enabled = EnableHiltCB.Checked;
        }
        private void EnableGuardCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!EnableGuardCB.Checked && !EnableGripCB.Checked && !EnablePommelCB.Checked)
                EnableHiltCB.Checked = false;
            else
                EnableHiltCB.Checked = true;
            GraphicsObject.Element(1).Element(0).Enabled = EnableGuardCB.Checked;
        }
        private void EnableGripCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!EnableGuardCB.Checked && !EnableGripCB.Checked && !EnablePommelCB.Checked)
                EnableHiltCB.Checked = false;
            else
                EnableHiltCB.Checked = true;
            GraphicsObject.Element(1).Element(1).Enabled = EnableGripCB.Checked;
        }
        private void EnablePommelCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!EnableGuardCB.Checked && !EnableGripCB.Checked && !EnablePommelCB.Checked)
                EnableHiltCB.Checked = false;
            else
                EnableHiltCB.Checked = true;
            GraphicsObject.Element(1).Element(2).Enabled = EnablePommelCB.Checked;
        }
    }
}