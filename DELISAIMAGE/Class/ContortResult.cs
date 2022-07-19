using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OpenCvSharp;

namespace DELISAIMAGE.Class
{
    public class ContourResult
    {
        public Rect BoundingBox { get; set; }
        

        /// <summary>
        /// 컨투어 데이터를 저장 합니다.
        /// </summary>
        public static bool Save(string filename, IEnumerable<ContourResult> data)
        {
            // if (data == null)
            //     return false;
            //
            // var serialize = JsonConvert.SerializeObject(data);
            // Debug.WriteLine(serialize);
            //
            // var info = new FileInfo(filename);
            // if (info.Directory is {Exists: false})
            // {
            //     info.Directory.Create();
            // }
            //
            // var outputFile = new StreamWriter(filename);
            // outputFile.WriteLine(serialize);
            // outputFile.Close();

            return true;
        }

        // /// <summary>
        // /// 컨투어 데이터를 읽어 옵니다.
        // /// </summary>
        // /// <param name="filename">저장 경로</param>
        // /// <returns>성공시 True, 그밖에는 False 반환.</returns>
        // public static IEnumerable<ContourResult> Load(string filename)
        // {
        //     if (!File.Exists(filename))
        //         return null;
        //
        //     var jsonString = File.ReadAllText(filename);
        //     Debug.WriteLine($"{jsonString}");
        //
        //     // return JsonConvert.DeserializeObject<IEnumerable<ContourResult>>(jsonString);
        // }
        //
        // #endregion        
    }
    
    /// <summary>
    /// 컨투어 매치를 처리하는 클래스 입니다.
    /// </summary>
    public class ContourMatch
    {
        /// <summary>
        /// 컨투어 매치를 시작 합니다.
        /// </summary>
        /// <param name="imagePath">이미지 경로</param>
        /// <returns>컨투어 매치 결과 반환</returns>
        public static IEnumerable<ContourResult> Run(string imagePath)
        {
            var mat = Cv2.ImRead(imagePath, ImreadModes.Unchanged);
            return Run(mat);
        }
        
        /// <summary>
        /// 컨투어 매치를 시작 합니다.
        /// </summary>
        /// <param name="image">이미지 인스턴스 (MAT)</param>
        /// <returns>컨투어 매치 결과 반환</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<ContourResult> Run(Mat image)
        {
            var matContrast = ImagePreprocess2(image);
            var result = MatchingSet(matContrast);

            return result.Select(x => new ContourResult() { BoundingBox = x });
        }

        private static Mat ImagePreprocess2(Mat mat)
        {
            var labImage = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
            var labImageTemp = new Mat();
            Cv2.MedianBlur(labImage, labImageTemp, 99);
            labImageTemp = ~labImageTemp;
            var labImageColor = labImageTemp.CvtColor(ColorConversionCodes.GRAY2BGR);

            var dst = new Mat();
            Cv2.AddWeighted(mat, 0.5, labImageColor, 0.5, 0, dst);

            var contour_gray = dst.CvtColor(ColorConversionCodes.BGR2GRAY);
            var blur = contour_gray.BilateralFilter(9, 5, 5);
            var th = blur.Threshold(100, 255, ThresholdTypes.Binary);
            // var otsu = blur.Threshold(0, 255, ThresholdTypes.Binary & ThresholdTypes.Otsu);
            return th;
        }
        
        /// <summary>
        /// 이미지 전처리 프로세스로 이미지를 선명하게 한 후 이전화 처리 합니다.
        /// </summary>
        private static Mat ImagePreprocess(Mat mat)
        {
            // var image = mat.ToGray();
            var image = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
            var sharpImage = UnsharpMask(image, new Size(5, 5));
            var blurImage = new Mat();
            Cv2.GaussianBlur(sharpImage, blurImage, new Size(5, 5), 0);
            var th = blurImage.Threshold(160, 255, ThresholdTypes.Binary);
            var otsu = th.Threshold(0, 255, ThresholdTypes.Binary & ThresholdTypes.Otsu);

            // var path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            //     "SmallMachines", "DELISA", "Test", "sharpImage.tiff");
            // sharpImage.SaveImage(path1);
            //
            // var path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            //     "SmallMachines", "DELISA", "Test", "blurImage.tiff");
            // blurImage.SaveImage(path2);
            
            
            return th;
            // return otsu;
        }        
        
        /// <summary>
        /// 이미지를 선명하게 처리 합니다.
        /// </summary>
        /// <returns>선명한 이미지 반환</returns>
        private static Mat UnsharpMask(Mat image, Size ksize, double sigma = 1.0, float amount = 1.0f, float threshold = 0.0f)
        {
            var blurredImage = new Mat();
            var lapImage = new Mat();
            Cv2.MedianBlur(image, blurredImage, 5);
            Cv2.Laplacian(blurredImage, lapImage, blurredImage.Type());
            blurredImage -= (0.9 * lapImage);

            return blurredImage;
        }            
        
        /// <summary>
        /// 컨투어 알고리즘으로 바운딩 박스를 찾습니다.
        /// </summary>
        /// <param name="baseImg"></param>
        /// <returns></returns>
        private static IEnumerable<Rect> MatchingSet(Mat baseImg)
        {
            Debug.WriteLine("Find Contours");

            var detectList = new List<Rect>();
            var contourRectList = new List<Rect>();

            using var hierarchy1 = new Mat();
            Cv2.FindContours(baseImg, out var contours, hierarchy1, RetrievalModes.List,
                ContourApproximationModes.ApproxSimple);

            const int minPixel = 0;
            const int maxPixel = 100;
            
            foreach (var contour in contours)
            {
                var rect = Cv2.BoundingRect(contour);
                if (rect.Width < minPixel || rect.Height < minPixel || rect.Width > maxPixel ||
                    rect.Height > maxPixel)
                {
                }
                else
                {
                    contourRectList.Add(Cv2.BoundingRect(contour));
                }
            }

            // outline cut
            for (var i = 0; i < contourRectList.Count; ++i)
            {
                var rect = contourRectList[i];
                if ((rect.X + rect.Width / 2) > baseImg.Cols || (rect.Y + rect.Height / 2) > baseImg.Rows)
                {
                    contourRectList.RemoveAt(i--);
                }
            }

            // remove inner contours
            contourRectList.Sort((Rect o1, Rect o2) =>
            {
                if (o1.Width > o2.Width)
                {
                    return 1;
                }

                if (o1.Width < o2.Width)
                {
                    return -1;
                }

                return 0;
            });

            for (var i = 0; i < contourRectList.Count; ++i)
            {
                var rect = new Rect(contourRectList[i].X, contourRectList[i].Y, contourRectList[i].Width,
                    contourRectList[i].Height);
                detectList.Add(rect);

                // ProgressEvent?.Invoke(loadRating + ((i / contourRectList.Count) * matchRating));
            }

            Debug.WriteLine($"lstCandidateMat Size: {detectList.Count}");

            return detectList;
        }        
        
        
        public static Mat DrawRectsOnMatch(Mat imgBase, List<Rect> lstRect, Scalar drawColor)
        {
            var matCanvas = imgBase.Clone();

            // TODO : 리소스 정리
            if (lstRect.Count <= 0) 
                return matCanvas;
            
            var width = lstRect[0].Width;
            foreach (var rectTmp in lstRect)
            {
                //rectTmp.Width = rectTmp.Height = width;
                ////rectTmp.X += lstRect[i].X;
                ////rectTmp.Y += lstRect[i].Y;
                //rectTmp.X += (Configurations.SizeSampleImg - width) / 2;
                //rectTmp.Y += (Configurations.SizeSampleImg - width) / 2;
                matCanvas.Rectangle(rectTmp, drawColor);
            }
            return matCanvas;
        }        
    }
    
    public enum OffsetType
    {
        LeftTop,
        Top,
        RightTop,
        Left,
        Center,
        Right,
        LeftBottom,
        Bottom,
        RightBottom
    }
    
    public static partial class MatExtensions
    {
        public static Mat Crop(this Mat mat, Size size, Point offset, OffsetType type = OffsetType.Center)
        {
            double left = 0, top = 0, right = mat.Width, bottom = mat.Height;

            var centerX = mat.Width * 0.5 + offset.X;
            var centerY = mat.Height * 0.5 + offset.Y;
            switch (type)
            {
                case OffsetType.LeftTop:
                    left = Math.Max(offset.X, 0);
                    top = Math.Max(offset.Y, 0);
                    break;
                case OffsetType.Top:
                    left = Math.Max(centerX - size.Width * 0.5, 0);
                    top = Math.Max(offset.Y, 0);
                    break;
                case OffsetType.RightTop:
                    left = Math.Max(mat.Width - size.Width + offset.X, 0);
                    top = Math.Max(offset.Y, 0);
                    break;
                case OffsetType.Left:
                    left = Math.Max(offset.X, 0);
                    top = Math.Max(centerY - size.Height * 0.5, 0);
                    break;
                case OffsetType.Center:
                    left = Math.Max(centerX - size.Width * 0.5, 0);
                    top = Math.Max(centerY - size.Height * 0.5, 0);
                    break;
                case OffsetType.Right:
                    left = Math.Max(mat.Width - size.Width + offset.X, 0);
                    top = Math.Max(centerY - size.Height * 0.5, 0);
                    break;
                case OffsetType.LeftBottom:
                    left = Math.Max(offset.X, 0);
                    top = Math.Max(mat.Height - size.Height + offset.Y, 0);
                    break;
                case OffsetType.Bottom:
                    left = Math.Max(centerX - size.Width * 0.5, 0);
                    top = Math.Max(mat.Height - size.Height + offset.Y, 0);
                    break;
                case OffsetType.RightBottom:
                    left = Math.Max(mat.Width - size.Width + offset.X, 0);
                    top = Math.Max(mat.Height - size.Height + offset.Y, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            right = Math.Min(left + size.Width, mat.Width);
            bottom = Math.Min(top + size.Height, mat.Height);
            
            var roi = Rect.FromLTRB((int)left, (int)top, (int)right - 1, (int)bottom - 1);
            return new Mat(mat, roi);
        }
        
        public static Mat Crop(this Mat mat, double scale, Point offset, OffsetType type = OffsetType.Center)
        {
            var width = (int) (mat.Cols * scale);
            var height = (int) (mat.Rows * scale);
            return Crop(mat, new Size(width, height), offset, type);
        }
        
        /// <summary>
        /// 회색 이미지를 복제 합니다.
        /// </summary>
        /// <param name="matInput">복제 대상 이미지</param>
        /// <returns>복제된 이미지 반환</returns>
        public static Mat ToGray(this Mat matInput)
        {
            return matInput.Channels() switch
            {
                3 => matInput.CvtColor(ColorConversionCodes.BGR2GRAY),
                4 => matInput.CvtColor(ColorConversionCodes.BGRA2GRAY),
                _ => matInput.Clone()
            };
        }
        
        /// <summary>
        /// 회색 이미지를 복제 합니다.
        /// </summary>
        /// <param name="matInput">복제 대상 이미지</param>
        /// <returns>복제된 이미지 반환</returns>
        public static Mat CloneAsGray(this Mat matInput)
        {
            var matBase = new Mat();
            switch (matInput.Channels())
            {
                case 1:
                    matBase = matInput.Clone();
                    break;
                case 3:
                    matBase = matInput.CvtColor(ColorConversionCodes.BGR2GRAY);
                    break;
                case 4:
                    matBase = matInput.CvtColor(ColorConversionCodes.BGRA2GRAY);
                    break;
            }

            return matBase;
        }

        /// <summary>
        /// 여러개의 이미지를 하나의 이미지로 합칩니다.
        /// </summary>
        /// <param name="images">이미지 리스트</param>
        public static Mat ImageMerge(this IEnumerable<Mat> images)
        {
            var enumerable = images.ToList();
            if (!enumerable.Any())
                return null;

            Mat source = null;
            var index = 0;
            foreach (var mat in enumerable)
            {
                if (index == 0)
                {
                    source = mat;
                }
                else
                {
                    Cv2.HConcat(source!, mat, source);
                }

                index++;
            }

            return source;
        }

        /// <summary>
        /// 여러 이미지로 부터 평균값 이미지를 가져옵니다.
        /// </summary>
        /// <param name="images">이미지 목록</param>
        /// <returns>평균값 이미지</returns>
        public static Mat GetGrayMean(this IEnumerable<Mat> images)
        {
            var enumerable = images.ToList();
            if (!enumerable.Any())
                return null;

            // Create a 0 initialized image to use as accumulator
            var m = new Mat(enumerable[0].Rows, enumerable[0].Cols, MatType.CV_64FC1);
            m.SetTo(new Scalar(0, 0, 0, 0));
            
            // Use a temp image to hold the conversion of each input image to CV_64FC3
            // This will be allocated just the first time, since all your images have
            // the same size.

            var temp = new Mat();
            foreach (var mat in enumerable)
            {
                // Convert the input images to CV_65FC1
                mat.ConvertTo(temp, MatType.CV_64FC1);
                
                // ... so you can accumulate
                m += temp;
            }
            
            // Convert back to CV_8UC1 type, applying the division to get the actual mean
            m.ConvertTo(m, MatType.CV_64FC1, 1.0 / enumerable.Count);
            // m.ConvertTo(m, MatType.CV_8UC1, 1.0 / enumerable.Count);
            
            return m;
        }
    }
}