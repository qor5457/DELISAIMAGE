using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DELISAIMAGE.Converter;
using DELISAIMAGE.Model;
using OpenCvSharp;
using OpenCvSharp.XImgProc;
using OpenCvSharp.XPhoto;
using Point = OpenCvSharp.Point;
using Rect = OpenCvSharp.Rect;
using Size = OpenCvSharp.Size;

namespace DELISAIMAGE.Class
{
    public class OpenCv
    {
        private Excel Excel;
        private Mat asd = new();
        public OpenCv()
        {
            Excel = new Excel();
        }

        public async Task Select(List<ModelImage> modelImages, List<BoxLocation> boxLocations)
        {
            DataTable dataTable = new DataTable();
            
            foreach (var modelImage in modelImages)
            {
                Folder.Create(modelImage.Imagepath + "Selcet");
                var First = ListToDataTable.ToDataTable(modelImages.ToList().Where(x => x.Imagepath == modelImage.Imagepath).ToList());
                foreach (var boxLocation in boxLocations)
                {
                    var _package = new AnalyzeSpotSettings();
                    _package.AnalyzeSize = new Size(boxLocation.Width, boxLocation.Height);
                    _package.AnalyzeOffset = new Point(boxLocation.X, boxLocation.Y);
                    
                    var Loadmat = Cv2.ImRead(modelImage.Imagepath);
                    var analyzeMat = Loadmat.Crop(_package.AnalyzeSize, _package.AnalyzeOffset, _package.AnalyzeOffsetType);
                    var result = ContourMatch.Run(analyzeMat).ToList();
                    boxLocation.Count = result.Count;
                    var debugMat = analyzeMat.DebugDrawRects(result.Select(x => x.BoundingBox).ToList(), Scalar.Red);
                    debugMat.SaveImage($"{Folder.Filepath}{boxLocation.Name}.tiff");
                }
                var second = ListToDataTable.ToDataTable(boxLocations);
            
                Excel.Excel_Save(ListToDataTable.MergeTablesByIndex(First, second));
            }
        }
        // public static async Task Select222(List<ModelImage> modelImages, List<BoxLocation> boxLocations)
        // {
        //     foreach (var modelImage in modelImages)
        //     {
        //     
        //         foreach (var boxLocation in boxLocations)
        //         {
        //             var Loadmat = Cv2.ImRead(modelImage.Imagepath, ImreadModes.Grayscale);
        //             
        //             var crap = new Mat(Loadmat, new Rect(boxLocation.X,boxLocation.Y,boxLocation.Width,boxLocation.Height));
        //             // var simple = new Mat(); var blob = SimpleBlobDetector.Create(); var key1 = blob.Detect(crap).ToList();
        //             var fast = new Mat(); var fastb = FastFeatureDetector.Create(); var key2 = fastb.Detect(crap).ToList();
        //             SimpleBlobDetector.Params pParams = new SimpleBlobDetector.Params();
        //             var filterMat = new Mat();
        //             pParams.FilterByArea = true;
        //             pParams.MinArea = 0.1f;
        //             pParams.FilterByColor = true;
        //             pParams.BlobColor = 0;
        //             var filter = SimpleBlobDetector.Create(pParams); var key3 = filter.Detect(crap).ToList();
        //             
        //             // Cv2.DrawKeypoints(crap,key1, simple, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //             Cv2.DrawKeypoints(crap,key2, fast, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //             Cv2.DrawKeypoints(crap,key3, filterMat, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //             //
        //             // Cv2.ImShow("crap",crap);
        //             Cv2.ImShow("fast",fast);
        //             Cv2.ImShow("filterMat",filterMat);
        //             
        //             
        //             // Point[][] contours;
        //             // var gray = new Mat();
        //             // var blur = new Mat();
        //             // var thresh = new Mat();
        //             // var crap1 = new Mat(Loadmat, new Rect(boxLocation.X,boxLocation.Y,boxLocation.Width,boxLocation.Height));
        //             // var kernel = Mat.Eye(1, 1,MatType.CV_8SC1);
        //             // Cv2.CvtColor(crap1, gray, ColorConversionCodes.BGR2GRAY);
        //             // Cv2.MorphologyEx(gray,blur,MorphTypes.Open,kernel);
        //             // Cv2.Threshold(blur, thresh, 0, 256,ThresholdTypes.Otsu);
        //             // Cv2.FindContours(thresh, out contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        //             // for (int i = 0; i < contours.Length; i++)
        //             // {
        //             //     Cv2.DrawContours(crap1, contours, i, Scalar.Blue, 2, LineTypes.AntiAlias);
        //             // }
        //             // Cv2.ImShow("eye",crap1);
        //         }
        //     }
        // }
    }
}