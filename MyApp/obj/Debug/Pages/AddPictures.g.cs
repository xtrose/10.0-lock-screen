﻿#pragma checksum "D:\Moses\Projekte\Windows\Windows Phone\10.0 Lock Screen\MyApp\MyApp\Pages\AddPictures.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "81CE271B3C37B9BEEAB8E50ED1FFE4EA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
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


namespace MyApp.Pages {
    
    
    public partial class AddPictures : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid grMain;
        
        internal System.Windows.Controls.TextBlock tbHeaderFormat;
        
        internal System.Windows.Controls.Grid grSelect;
        
        internal System.Windows.Controls.ListBox lbPictures;
        
        internal System.Windows.Controls.Grid grCut;
        
        internal System.Windows.Controls.Viewbox vbCut;
        
        internal System.Windows.Controls.Image imgCut;
        
        internal System.Windows.Shapes.Rectangle recCut;
        
        internal System.Windows.Media.CompositeTransform transform;
        
        internal System.Windows.Controls.TextBlock tbSizePercent;
        
        internal System.Windows.Controls.Slider slCut;
        
        internal System.Windows.Controls.TextBlock tbPictureQuality;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MyApp;component/Pages/AddPictures.xaml", System.UriKind.Relative));
            this.grMain = ((System.Windows.Controls.Grid)(this.FindName("grMain")));
            this.tbHeaderFormat = ((System.Windows.Controls.TextBlock)(this.FindName("tbHeaderFormat")));
            this.grSelect = ((System.Windows.Controls.Grid)(this.FindName("grSelect")));
            this.lbPictures = ((System.Windows.Controls.ListBox)(this.FindName("lbPictures")));
            this.grCut = ((System.Windows.Controls.Grid)(this.FindName("grCut")));
            this.vbCut = ((System.Windows.Controls.Viewbox)(this.FindName("vbCut")));
            this.imgCut = ((System.Windows.Controls.Image)(this.FindName("imgCut")));
            this.recCut = ((System.Windows.Shapes.Rectangle)(this.FindName("recCut")));
            this.transform = ((System.Windows.Media.CompositeTransform)(this.FindName("transform")));
            this.tbSizePercent = ((System.Windows.Controls.TextBlock)(this.FindName("tbSizePercent")));
            this.slCut = ((System.Windows.Controls.Slider)(this.FindName("slCut")));
            this.tbPictureQuality = ((System.Windows.Controls.TextBlock)(this.FindName("tbPictureQuality")));
        }
    }
}

