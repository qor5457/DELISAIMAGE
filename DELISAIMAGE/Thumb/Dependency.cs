using System.Windows;
using System.Windows.Media;

namespace DELISAIMAGE
{

    public static class Dependency
    {
        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject tmp = VisualTreeHelper.GetParent(obj);
            while (tmp != null && !(tmp is T))
            {
                tmp = VisualTreeHelper.GetParent(tmp);
            }
            return tmp as T;
        }
    }
}