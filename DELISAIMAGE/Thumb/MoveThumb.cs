using DELISAIMAGE.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DELISAIMAGE
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb moveThumb = (MoveThumb)sender;
            var ContentControl = moveThumb.FindAncestor<ContentControl>();
            if (ContentControl != null)
            {
                double left = Canvas.GetLeft(ContentControl);
                double top = Canvas.GetTop(ContentControl);
                Canvas.SetLeft(ContentControl, left + e.HorizontalChange);
                Canvas.SetTop(ContentControl, top + e.VerticalChange);
            }
        }
    }
}