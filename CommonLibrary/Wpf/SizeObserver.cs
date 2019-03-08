using System;
using System.Windows;
using JetBrains.Annotations;

namespace CommonLibrary.Wpf
{
    public static class SizeObserver
    {
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(SizeObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
            "ObservedWidth",
            typeof(double),
            typeof(SizeObserver));

        public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
            "ObservedHeight",
            typeof(double),
            typeof(SizeObserver));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static bool GetObserve([NotNull] FrameworkElement frameworkElement)
        {
            if(frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetObserve([NotNull] FrameworkElement frameworkElement, bool observe)
        {
            if (frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            frameworkElement.SetValue(ObserveProperty, observe);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static double GetObservedWidth([NotNull] FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            return (double)frameworkElement.GetValue(ObservedWidthProperty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetObservedWidth([NotNull] FrameworkElement frameworkElement, double observedWidth)
        {
            if (frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            frameworkElement.SetValue(ObservedWidthProperty, observedWidth);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static double GetObservedHeight([NotNull] FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            return (double)frameworkElement.GetValue(ObservedHeightProperty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetObservedHeight([NotNull] FrameworkElement frameworkElement, double observedHeight)
        {
            if (frameworkElement == null) throw new ArgumentNullException(nameof(frameworkElement));

            frameworkElement.SetValue(ObservedHeightProperty, observedHeight);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
            }
        }

        private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }

        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {
            // WPF 4.0 onwards
            frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
            frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);

            // WPF 3.5 and prior
            ////SetObservedWidth(frameworkElement, frameworkElement.ActualWidth);
            ////SetObservedHeight(frameworkElement, frameworkElement.ActualHeight);
        }
    }
}
