﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIndFull
{
    public class OpcionesCell : ViewCell
    {
        public OpcionesCell()
        {
            var image = new Image
            {
                HorizontalOptions = LayoutOptions.Start
            };
            image.SetBinding(Image.SourceProperty, new Binding("Imagen"));
            image.SetBinding(Image.BackgroundColorProperty, new Binding("BackGroundColor", BindingMode.OneWay, new ColorConverter()));
            image.WidthRequest = image.HeightRequest = 24;

            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Black,
                BackgroundColor = Color.Gray,
                FontSize = 20
            };
            nameLabel.SetBinding(Label.TextProperty, "Name");
            nameLabel.SetBinding(Label.BackgroundColorProperty, new Binding("BackGroundColor", BindingMode.OneWay, new ColorConverter()));

            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { image, nameLabel }
            };
            viewLayout.SetBinding(StackLayout.BackgroundColorProperty, new Binding("BackGroundColor", BindingMode.OneWay, new ColorConverter()));

            View = viewLayout;
        }
    }
}
