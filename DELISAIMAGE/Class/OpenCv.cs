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
namespace DELISAIMAGE.Class
{
    public class OpenCv
    {
        private Excel Excel;

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
                var k = modelImages.ToList().Where(x => x.Imagepath == modelImage.Imagepath).ToList();
                var df = ListToDataTable.ToDataTable(k);

                foreach (var boxLocation in boxLocations)
                {
                    var Loadmat = Cv2.ImRead(modelImage.Imagepath, ImreadModes.Grayscale);
                    Point[][] contours;
                    var crap = new Mat();
                    var blur = new Mat();
                    var thresh = new Mat();
                    crap = new Mat(Loadmat, new Rect(boxLocation.X, boxLocation.Y, boxLocation.Width, boxLocation.Height));
                    var kernel = Mat.Eye(1, 1, MatType.CV_8SC1);
                    Cv2.MorphologyEx(crap, blur, MorphTypes.Open, kernel);
                    Cv2.Threshold(blur, thresh, 0, 256, ThresholdTypes.Otsu);
                    Cv2.FindContours(thresh, out contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                    for (int i = 0; i < contours.Length; i++)
                    {
                        Cv2.DrawContours(crap, contours, i, Scalar.Blue, 2, LineTypes.AntiAlias);
                    }
                    
                    boxLocation.Count = contours.Length;
                    crap.SaveImage( $"{Folder.Filepath}{boxLocation.Name}.png");
                }
                var b = ListToDataTable.ToDataTable(boxLocations);

                Excel.Excel_Save(ListToDataTable.MergeTablesByIndex(df, b));
            }
        }
        
        

        // public static async Task Select222(List<ModelImage> modelImages)
        // {
        //     // foreach (var modelImage in modelImages)
        //     // {
        //     //
        //     //     foreach (var boxLocation in modelImage.BoxData)
        //     //     {
        //     //         var Loadmat = Cv2.ImRead(modelImage.Imagepath, ImreadModes.Grayscale);
        //     //         var crap = new Mat(Loadmat, new Rect(boxLocation.X,boxLocation.Y,boxLocation.Width,boxLocation.Height));
        //     //         var simple = new Mat(); var blob = SimpleBlobDetector.Create(); var key1 = blob.Detect(crap).ToList();
        //     //         var fast = new Mat(); var fastb = FastFeatureDetector.Create(); var key2 = fastb.Detect(crap).ToList();
        //     //         SimpleBlobDetector.Params pParams = new SimpleBlobDetector.Params();
        //     //         var filterMat = new Mat();
        //     //         pParams.MinArea = 0; pParams.BlobColor = 2;
        //     //         pParams.MinThreshold = 10;
        //     //         pParams.FilterByColor = false; var filter = SimpleBlobDetector.Create(pParams); var key3 = filter.Detect(crap).ToList();
        //     //         
        //     //         Cv2.DrawKeypoints(crap,key1, simple, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //     //         Cv2.DrawKeypoints(crap,key2, fast, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //     //         Cv2.DrawKeypoints(crap,key3, filterMat, Scalar.Red,DrawMatchesFlags.DrawRichKeypoints);
        //     //         
        //     //         Cv2.ImShow("simple",simple);
        //     //         Cv2.ImShow("fast",fast);
        //     //         Cv2.ImShow("filterMat",filterMat);
        //     //         
        //     //         
        //     //         Point[][] contours;
        //     //         var gray = new Mat();
        //     //         var blur = new Mat();
        //     //         var thresh = new Mat();
        //     //         var crap1 = new Mat(Loadmat, new Rect(boxLocation.X,boxLocation.Y,boxLocation.Width,boxLocation.Height));
        //     //         var kernel = Mat.Eye(1, 1,MatType.CV_8SC1);
        //     //         Cv2.CvtColor(crap1, gray, ColorConversionCodes.BGR2GRAY);
        //     //         Cv2.MorphologyEx(gray,blur,MorphTypes.Open,kernel);
        //     //         Cv2.Threshold(blur, thresh, 0, 256,ThresholdTypes.Otsu);
        //     //         Cv2.FindContours(thresh, out contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
        //     //         for (int i = 0; i < contours.Length; i++)
        //     //         {
        //     //             Cv2.DrawContours(crap1, contours, i, Scalar.Blue, 2, LineTypes.AntiAlias);
        //     //         }
        //     //         Cv2.ImShow("eye",crap1);
        //     //     }
        //     // }
        // }
    }
}