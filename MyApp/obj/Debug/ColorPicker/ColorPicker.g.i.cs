﻿#pragma checksum "D:\Moses\Projekte\Windows Phone\Lock Screen 10\MyApp\MyApp\ColorPicker\ColorPicker.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E454000F88D5F74A62C09A7F73DCDC09"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MyApp.Controls {
    
    
    public partial class ColorPicker : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid layoutRoot;
        
        internal System.Windows.Controls.Image colorImage;
        
        internal System.Windows.Controls.Canvas canvasImage;
        
        internal System.Windows.Shapes.Ellipse ellipsePixel;
        
        internal System.Windows.Shapes.Rectangle rectangleSelectedColor;
        
        internal System.Windows.Controls.TextBlock tbSelectedColor;
        
        internal System.Windows.Controls.Image colorImage1;
        
        internal System.Windows.Controls.Image colorImage2;
        
        internal System.Windows.Controls.Image colorImage3;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MyApp;component/ColorPicker/ColorPicker.xaml", System.UriKind.Relative));
            this.layoutRoot = ((System.Windows.Controls.Grid)(this.FindName("layoutRoot")));
            this.colorImage = ((System.Windows.Controls.Image)(this.FindName("colorImage")));
            this.canvasImage = ((System.Windows.Controls.Canvas)(this.FindName("canvasImage")));
            this.ellipsePixel = ((System.Windows.Shapes.Ellipse)(this.FindName("ellipsePixel")));
            this.rectangleSelectedColor = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangleSelectedColor")));
            this.tbSelectedColor = ((System.Windows.Controls.TextBlock)(this.FindName("tbSelectedColor")));
            this.colorImage1 = ((System.Windows.Controls.Image)(this.FindName("colorImage1")));
            this.colorImage2 = ((System.Windows.Controls.Image)(this.FindName("colorImage2")));
            this.colorImage3 = ((System.Windows.Controls.Image)(this.FindName("colorImage3")));
        }
    }
}

