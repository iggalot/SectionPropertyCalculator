using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SectionPropertyCalculator.Controls.Adorners
{
    /// <summary>
    /// A class for creating adorner icons for resizing a framework element.
    /// </summary>
    public class ResizeAdorner : Adorner
    {
        private double _min_width = 1;
        private double _min_height = 1;

        public VisualCollection AdornerVisuals;
        Thumb thumb1, thumb2;
        Rectangle Rec;
        TextBlock txtBlock_VertDim;
        TextBlock txtBlock_HorizDim;
        TextBlock txtBlock_PlateNum;
        Ellipse Ell;
        TextBlock txtBlock_Centroid;


        /// <summary>
        /// Details for the adorner
        /// </summary>
        public double SCREEN_SCALE_FACTOR;
        public double ControlModelWidth_Original;   // original width of the control measured in pixels
        public double ControlModelHeight_Original;  // original height of the control measured in pixels

        public double ControlModelWidth_Current;   // current width of the control measured in pixels
        public double ControlModelHeight_Current;  // current height of the control measured in pixels

        public Point ControlModelCentroid; // Centroid of the control on screen measured in pixels
        public double ControlModelLeftOffset = 0; // Amount that the offset needs to be moved left from its original point (reference 0,0) on the control
        public double ControlModelRightOffset = 0; // Amount that the offset needs to be moved up from its original point (reference 0,0) on the control

        public int ControlModelId;


        // Events associated with the adorner to signal the main applicaton that something has changed
        public static readonly RoutedEvent OnAdornerModifiedEvent = EventManager.RegisterRoutedEvent("UpdateModelRequired", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateCanvasControl));


        // expose our event
        public event RoutedEventHandler OnAdornerModified
        {
            add { AddHandler(OnAdornerModifiedEvent, value); }
            remove
            {
                RemoveHandler(OnAdornerModifiedEvent, value);
            }
        }


        public ResizeAdorner(UIElement adornedElement, int id, Point centroid_point, Point upper_left, double scale=1.0) : base(adornedElement)
        {
            Rectangle rect = adornedElement as Rectangle;
            ControlModelWidth_Original = rect.Width;
            ControlModelHeight_Original = rect.Height;

            ControlModelWidth_Current = rect.Width;
            ControlModelHeight_Current = rect.Height;

            //ControlModelUpperLeftPt = upper_left;
            ControlModelCentroid = centroid_point;
            ControlModelId = id;

            SCREEN_SCALE_FACTOR = scale;

            AdornerVisuals = new VisualCollection(this);

            // Create the adorner elements
            //thumb1 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            thumb2 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };


            Rec = new Rectangle() { Stroke = Brushes.Coral, StrokeThickness = 2, StrokeDashArray = { 3, 2 }, Fill=Brushes.Transparent, IsHitTestVisible=false };
            txtBlock_VertDim = new TextBlock() { Text = "VertiDim", FontSize= 20, IsHitTestVisible = false };
            txtBlock_HorizDim = new TextBlock() { Text = "HorizDim", FontSize = 20, IsHitTestVisible = false };
            txtBlock_PlateNum = new TextBlock() { Text = "PlateNumber", FontSize = 40, IsHitTestVisible = false };
            Ell = new Ellipse() { Stroke = Brushes.Black, Fill = Brushes.Black, IsHitTestVisible = false };
            txtBlock_Centroid = new TextBlock() { Text = "CentroidCoords", FontSize = 14, IsHitTestVisible = false };

            // Add events
            //thumb1.DragDelta += Thumb2_DragDelta;
            thumb2.DragDelta += Thumb2_DragDelta;
            //platecanvas_thumb1.DragDelta += PlateCanvas_Thumb1_DragDelta;
            //platecanvas_thumb2.DragDelta += PlateCanvas_Thumb2_DragDelta;

            // Add elements to our list of visuals
            AdornerVisuals.Add(Rec);
            //AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);
            AdornerVisuals.Add(txtBlock_VertDim);
            AdornerVisuals.Add(txtBlock_HorizDim);
            AdornerVisuals.Add(txtBlock_PlateNum);
            AdornerVisuals.Add(txtBlock_Centroid);
            AdornerVisuals.Add(Ell);

        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;
            Point upper_left = ele.PointToScreen(new Point(0, 0));

            double delta_x = ele.Width + e.HorizontalChange < 0 ? 0 : e.HorizontalChange;
            double delta_y = ele.Height + e.VerticalChange < 0 ? 0 : e.VerticalChange;

            ele.Height = (ele.Height + 2 * delta_y) < _min_height ? _min_height : (ele.Height + 2 * delta_y);
            ele.Width = (ele.Width + 2 * delta_x) < _min_width ? _min_width : (ele.Width + 2 * delta_x);
            //ele.Height = ele.Height - e.VerticalChange < 0 ? 0 : ele.Height - e.VerticalChange;
            //ele.Width = ele.Width - e.HorizontalChange < 0 ? 0 : ele.Width - e.HorizontalChange;

            ControlModelWidth_Current = ele.Width;
            ControlModelHeight_Current = ele.Height;
            ControlModelLeftOffset += e.HorizontalChange;
            ControlModelRightOffset += e.VerticalChange;


            //ControlModelUpperLeftPt = new Point(upper_left.X - delta_x, upper_left.Y - delta_y);
            //           ControlCentroid = new Point(ControlUpperLeftPt.X + 0.5 * ele.Width, ControlUpperLeftPt.Y + 0.5 * ele.Height);

            RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));
        }

        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
 //           var ele = (FrameworkElement)AdornedElement;
 //           Point upper_left = ele.PointToScreen(new Point(0, 0));

 //           double delta_x = ele.Width - e.HorizontalChange < 0 ? 0 : e.HorizontalChange;
 //           double delta_y = ele.Height - e.VerticalChange < 0 ? 0 : e.VerticalChange;

 //           ele.Height = (ele.Height - 2 * delta_y) < 0 ? 0 : (ele.Height - 2 * delta_y);
 //           ele.Width = (ele.Width - 2 * delta_x) < 0 ? 0 : (ele.Width - 2 * delta_x);
 //           //ele.Height = ele.Height - e.VerticalChange < 0 ? 0 : ele.Height - e.VerticalChange;
 //           //ele.Width = ele.Width - e.HorizontalChange < 0 ? 0 : ele.Width - e.HorizontalChange;

 //           ControlModelWidth_Current = ele.Width;
 //           ControlModelHeight_Current = ele.Height;
 //           ControlModelUpperLeftPt = new Point(upper_left.X - delta_x, upper_left.Y - delta_y);
 ////           ControlCentroid = new Point(ControlUpperLeftPt.X + 0.5 * ele.Width, ControlUpperLeftPt.Y + 0.5 * ele.Height);

 //           //Point from_position = ele.PointFromScreen(position);
 //           //Point trans_point = ele.Tra

 //           RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));
        }

        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        /// <summary>
        /// This draws the elements of the adorner.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // the resize rectangle
            //Rec.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));

            // Check that the visuals are loaded
            if(PresentationSource.FromVisual(AdornedElement) is null)
                return finalSize;


            // Get screen position of the adorned element
            double x = ControlModelLeftOffset;
            double y = ControlModelRightOffset;

            double new_width = ControlModelWidth_Current + 5 + 2*x;
            double new_height = ControlModelHeight_Current + 5 + 2*y;

            // make sure our width and height are not negative
            if (new_width < 0 || new_height < 0)
                return finalSize;
            else
            {
                ControlModelWidth_Current = new_width - 5;
                ControlModelHeight_Current = new_height - 5;
            }

            double new_left_offset = -x - 2.5;
            double new_top_offset = -y - 2.5;


            Rec.Arrange(new Rect(-x - 2.5, -y - 2.5, new_width, new_height));

            Console.WriteLine("W: " + ControlModelWidth_Current + "   H: " + ControlModelHeight_Current +  "    x: " + x + "   y: " + y);



            // the resize grips
            //thumb1.Arrange(new Rect(-5, -5, 10, 10));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, 10, 10));

            // plate number label
            txtBlock_PlateNum.Text = ControlModelId.ToString();
            txtBlock_PlateNum.Arrange(
                new Rect(0,
                    0,
                    50,
                    50)
                );

            //horizontal dimension
            txtBlock_HorizDim.Text = ((ControlModelWidth_Original) / SCREEN_SCALE_FACTOR).ToString();
            //            txtBlock_Horiz.Text = ((AdornedElement.DesiredSize.Width + 5 + 2.5) / SCREEN_SCALE_FACTOR).ToString();
            txtBlock_HorizDim.Arrange(new Rect(0.45 * (AdornedElement.DesiredSize.Width + 5 + 2.5), 0, 50, 50));

            // vertical dimension
            txtBlock_VertDim.Text = ((ControlModelHeight_Original)/SCREEN_SCALE_FACTOR).ToString();
            //txtBlock_Vert.Text = ((AdornedElement.DesiredSize.Height + 5 + 2.5)/SCREEN_SCALE_FACTOR).ToString();
            txtBlock_VertDim.Arrange(new Rect(0, 0.45 * (AdornedElement.DesiredSize.Height + 5 + 2.5), 50, 50));

            // Centroid dot
            Ell.Arrange(new Rect(0.5 * (AdornedElement.DesiredSize.Width + 5),
                0.5 * (AdornedElement.DesiredSize.Height + 5), 10, 10));

            // Centroid coords.
            txtBlock_Centroid.Text = "(" + Math.Round(ControlModelCentroid.X, 2).ToString() + "," + Math.Round(ControlModelCentroid.Y, 2).ToString() + ")";
            txtBlock_Centroid.Arrange(new Rect(0.5 * (AdornedElement.DesiredSize.Width + 5),
                0.5 * (AdornedElement.DesiredSize.Height + 20), 100, 25));

            return base.ArrangeOverride(finalSize);
        }
    }
}
