﻿#pragma checksum "D:\Moses\Projekte\Windows\Windows Phone\10.0 Lock Screen\MyApp\MyApp\Pages\ChangeColor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CAB9105688D2BA766AC758A52D2295EB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Coding4Fun.Toolkit.Controls;
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
    
    
    public partial class ChangeColor : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid grMain;
        
        internal Coding4Fun.Toolkit.Controls.ColorPicker colorPicker;
        
        internal System.Windows.Controls.TextBlock tbTransparency;
        
        internal System.Windows.Controls.Slider slTransparency;
        
        internal System.Windows.Shapes.Rectangle recUsedColor1;
        
        internal System.Windows.Shapes.Rectangle recUsedColor2;
        
        internal System.Windows.Shapes.Rectangle recUsedColor3;
        
        internal System.Windows.Shapes.Rectangle recUsedColor4;
        
        internal System.Windows.Shapes.Rectangle recUsedColor5;
        
        internal System.Windows.Controls.Button btnAccentColor;
        
        internal System.Windows.Controls.Button btnBackgroundColor;
        
        internal System.Windows.Controls.Button btnForegroundColor;
        
        internal System.Windows.Controls.Button btnYes;
        
        internal System.Windows.Controls.Button btnNo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MyApp;component/Pages/ChangeColor.xaml", System.UriKind.Relative));
            this.grMain = ((System.Windows.Controls.Grid)(this.FindName("grMain")));
            this.colorPicker = ((Coding4Fun.Toolkit.Controls.ColorPicker)(this.FindName("colorPicker")));
            this.tbTransparency = ((System.Windows.Controls.TextBlock)(this.FindName("tbTransparency")));
            this.slTransparency = ((System.Windows.Controls.Slider)(this.FindName("slTransparency")));
            this.recUsedColor1 = ((System.Windows.Shapes.Rectangle)(this.FindName("recUsedColor1")));
            this.recUsedColor2 = ((System.Windows.Shapes.Rectangle)(this.FindName("recUsedColor2")));
            this.recUsedColor3 = ((System.Windows.Shapes.Rectangle)(this.FindName("recUsedColor3")));
            this.recUsedColor4 = ((System.Windows.Shapes.Rectangle)(this.FindName("recUsedColor4")));
            this.recUsedColor5 = ((System.Windows.Shapes.Rectangle)(this.FindName("recUsedColor5")));
            this.btnAccentColor = ((System.Windows.Controls.Button)(this.FindName("btnAccentColor")));
            this.btnBackgroundColor = ((System.Windows.Controls.Button)(this.FindName("btnBackgroundColor")));
            this.btnForegroundColor = ((System.Windows.Controls.Button)(this.FindName("btnForegroundColor")));
            this.btnYes = ((System.Windows.Controls.Button)(this.FindName("btnYes")));
            this.btnNo = ((System.Windows.Controls.Button)(this.FindName("btnNo")));
        }
    }
}

