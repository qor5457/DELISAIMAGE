using System.Collections.Generic;
using System.IO;
using DELISAIMAGE.Class;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCvSharp;

namespace DELISAIMAGE.Class
{
    public class AnalyzeSpotSettings
    {
        public Size ImageSize { get; set; }
        public Size AnalyzeSize { get; set; }
        public Point AnalyzeOffset { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OffsetType AnalyzeOffsetType { get; set; }

        public Dictionary<string, int> SpotPositions { get; set; }

        #region Save & Load Method

        /// <summary>
        /// 데이터를 저장 합니다.
        /// </summary>
        public static bool Save(string filename, AnalyzeSpotSettings settings)
        {
            if (settings == null)
                return false;

            var serialize = JsonConvert.SerializeObject(settings);

            var info = new FileInfo(filename);
            if (info.Directory is { Exists: false })
            {
                info.Directory.Create();
            }

            var outputFile = new StreamWriter(filename);
            outputFile.WriteLine(serialize);
            outputFile.Close();

            return true;
        }

        /// <summary>
        /// 데이터를 읽어 옵니다.
        /// </summary>
        public static AnalyzeSpotSettings Load(string filename)
        {
            if (!File.Exists(filename))
                return null;

            var jsonString = File.ReadAllText(filename);

            return JsonConvert.DeserializeObject<AnalyzeSpotSettings>(jsonString);
        }

        #endregion
    }
}