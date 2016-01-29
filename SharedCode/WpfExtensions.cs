using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Okna.Plugins
{
    public static class WpfExtensions
    {
        public static T GetVisualChild<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static T GetVisualParent<T>(this DependencyObject depObj) where T : DependencyObject
        {
            while (true)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(depObj);
                if (parent == null) return null;
                T typedParent = parent as T;
                if (typedParent != null) return typedParent;
                depObj = parent;
            }
        }

        public static IEnumerable<DependencyObject> Descendants(this DependencyObject root, int depth)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;
                if (depth > 0)
                {
                    foreach (var descendent in Descendants(child, --depth))
                    {
                        yield return descendent;
                    }
                }
            }
        }

        public static IEnumerable<DependencyObject> Descendants(this DependencyObject root)
        {
            return Descendants(root, Int32.MaxValue);
        }

        public static IEnumerable<DependencyObject> Ancestors(this DependencyObject root)
        {
            DependencyObject current = VisualTreeHelper.GetParent(root);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }
    }
}
