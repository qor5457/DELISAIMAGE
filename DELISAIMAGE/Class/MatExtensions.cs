using System.Collections.Generic;
using OpenCvSharp;

namespace DELISAIMAGE.Class
{
    public static partial class MatExtensions
    {
        public static Mat DebugDrawRects(this Mat imgBase, List<Rect> lstRect, Scalar drawColor)
        {
            var matCanvas = imgBase.Clone();

            // TODO : 리소스 정리
            if (lstRect.Count <= 0) 
                return matCanvas;
            
            var width = lstRect[0].Width;
            foreach (var rectTmp in lstRect)
            {
                matCanvas.Rectangle(rectTmp, drawColor);
            }
            return matCanvas;
        }
    }
}