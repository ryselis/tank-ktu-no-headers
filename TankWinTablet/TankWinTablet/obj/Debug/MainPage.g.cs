﻿

#pragma checksum "C:\Users\petku_000\Documents\Darbas\Tankas\tank-no-headers\tank-ktu-no-headers\TankWinTablet\TankWinTablet\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0A8A293D089364B72A7EAE458D4B421D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TankWinTablet
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 14 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.startButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 16 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.turretLeftButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 17 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.forwardButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 18 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.turretRightButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 19 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.LeftButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 20 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.rightButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 21 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.gunLiftButton_Copy_Click;
                 #line default
                 #line hidden
                #line 21 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.gunLiftButton_Copy_Tapped;
                 #line default
                 #line hidden
                #line 21 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerExited += this.gunLiftButton_Copy_PointerExited;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 22 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.backwardButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 23 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.fireButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 24 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.stopButton_ClickNew;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 25 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).DragEnter += this.tankBodyImage_DragEnter;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


