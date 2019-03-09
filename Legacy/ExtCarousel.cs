using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace ExtensionsCarousel
{
    public static class ExtCarousel
    {
        public static void PageRight(this CarouselPage carouselPage)
        {
            var pageCount = carouselPage.Children.Count;
            if (pageCount < 2)
                return;

            var index = carouselPage.Children.IndexOf(carouselPage.CurrentPage);
            index++;
            if (index >= pageCount)
                index = 0;

            carouselPage.CurrentPage = carouselPage.Children[index];
        }
    }
}