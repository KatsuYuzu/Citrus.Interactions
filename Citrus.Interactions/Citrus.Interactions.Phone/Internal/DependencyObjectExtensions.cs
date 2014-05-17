using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Citrus.Interactions.Internal
{
    static class DependencyObjectExtensions
    {
        public static IEnumerable<DependencyObject> Children(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            var childrenCount = VisualTreeHelper.GetChildrenCount(obj);

            for (int i = 0; i < childrenCount; i++)
            {
                yield return VisualTreeHelper.GetChild(obj, i);
            }
        }

        public static IEnumerable<DependencyObject> Descendants(this DependencyObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            foreach (var child in obj.Children())
            {
                yield return child;

                foreach (var grandChild in child.Descendants())
                {
                    yield return grandChild;
                }
            }
        }

        public static T FindAction<T>(this DependencyObject obj)
            where T: IAction
        {
            return obj
                .Descendants()
                .SelectMany(Interaction.GetBehaviors)
                .SelectMany(x =>
                {
                    var eventTrigger = x as EventTriggerBehavior;
                    if (eventTrigger != null)
                    {
                        return eventTrigger.Actions;
                    }

                    var dataTrigger = x as DataTriggerBehavior;
                    if (dataTrigger != null)
                    {
                        return dataTrigger.Actions;
                    }

                    return null;
                })
                .OfType<T>()
                .SingleOrDefault();
        }
    }
}
