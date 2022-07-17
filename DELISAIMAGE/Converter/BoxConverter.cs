// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.Windows.Data;
// //별로 인듯
// namespace DELISAIMAGE.Converter
// {
//     public class BoxConverter : IValueConverter
//     {
//         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//         {
//             object result = null;
//
//             if (value != null)
//             {
//                 var path = value.ToString();
//
//                 if (string.IsNullOrWhiteSpace(path) == false)
//                     result = Path.GetFileNameWithoutExtension(path);
//             }
//
//             return result;
//
//         }
//         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//         {
//             return value;
//         }
//     }
// }
