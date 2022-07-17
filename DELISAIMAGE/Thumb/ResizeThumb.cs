using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DELISAIMAGE
{
 public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ResizeThumb moveThumb = (ResizeThumb)sender;
            var contentControl = moveThumb.FindAncestor<ContentControl>();
            
            if (contentControl != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, contentControl.ActualHeight - contentControl.MinHeight);
                        contentControl.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, contentControl.ActualHeight - contentControl.MinHeight);
                        Canvas.SetTop(contentControl, Canvas.GetTop(contentControl) + deltaVertical);
                        contentControl.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, contentControl.ActualWidth - contentControl.MinWidth);
                        Canvas.SetLeft(contentControl, Canvas.GetLeft(contentControl) + deltaHorizontal);
                        contentControl.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, contentControl.ActualWidth - contentControl.MinWidth);
                        contentControl.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}