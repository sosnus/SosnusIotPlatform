﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InputKeyboardMouse
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int ScrollCnt = 0;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += AcceleratorKeyActivated;
            GridMain.PointerMoved += new PointerEventHandler(Pointer_Moved);
            GridMain.PointerPressed += new PointerEventHandler(Pointer_Clicked);
            GridMain.PointerWheelChanged += new PointerEventHandler(Pointer_Wheel_Changed);
        }

        void Pointer_Clicked(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint currentPoint = e.GetCurrentPoint(GridMain);
            Windows.UI.Input.PointerPointProperties pointerProperties = currentPoint.Properties;

            string temp = "none";

            if (pointerProperties.IsLeftButtonPressed) temp = "Left";
            if (pointerProperties.IsMiddleButtonPressed) temp = "Scroll";
            if (pointerProperties.IsRightButtonPressed) temp = "Right";

            spKeyboard.Children.Insert(0, (new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.Blue),
                Text = Convert.ToString($"{temp} Mouse Button Clicked at " + DateTime.Now.ToString("h:mm:ss fffffff"))
            }));
        }


        void Pointer_Wheel_Changed(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint currentPoint = e.GetCurrentPoint(GridMain);
            Windows.UI.Input.PointerPointProperties pointerProperties = currentPoint.Properties;

            spScroll.Children.Insert(1, (new TextBlock()
            {
                //spKey.Items.Add(new TextBlock() {
                //spKey.Children.Add(new TextBlock() {
                //FontFamily = new FontFamily("Lucida Console"),
                Foreground = new SolidColorBrush(Colors.Yellow),
                Text = Convert.ToString($"Scroll={ScrollCnt}+{pointerProperties.MouseWheelDelta/120}={ScrollCnt+ pointerProperties.MouseWheelDelta / 120} at " + DateTime.Now.ToString("h:mm:ss fffffff"))
            }));
            ScrollCnt += pointerProperties.MouseWheelDelta / 120;
            scrollTbl.Text = $" ScrollCnt = {ScrollCnt}";
        }

        void Pointer_Moved(object sender, PointerRoutedEventArgs e)
        {
            int roundCnt = 0;
            Windows.UI.Input.PointerPoint currentPoint = e.GetCurrentPoint(GridMain);
            spMouse.Children.Insert(0, (new TextBlock() {
                Foreground = new SolidColorBrush(Colors.Cyan),
                Text = Convert.ToString($"Cursor on X={Math.Round(currentPoint.Position.X, roundCnt)} Y={Math.Round(currentPoint.Position.Y, roundCnt)} at " + DateTime.Now.ToString("h:mm:ss fffffff"))
            }));
        }

        Brush SpecialKeyDetector(VirtualKey myKey)
        {
            SolidColorBrush brush;
            if (myKey.ToString().Length == 1)   brush = new SolidColorBrush(Colors.LimeGreen);
            else                                brush = new SolidColorBrush(Colors.Red);
            return brush;
        }


        private void AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.EventType.ToString().Contains("Down"))
            {
                spKeyboard.Children.Insert(0, (new TextBlock() {
                //spKey.Items.Add(new TextBlock() {
                    //spKey.Children.Add(new TextBlock() {
                    //FontFamily = new FontFamily("Lucida Console"),
                    Foreground = SpecialKeyDetector(args.VirtualKey),
                    Text = Convert.ToString($"Pressed {args.VirtualKey} at "+ DateTime.Now.ToString("h:mm:ss fffffff") )
                }));
            }
        }

        private void btnScrollCntReset_Click(object sender, RoutedEventArgs e)
        {
            ScrollCnt = 0;
            scrollTbl.Text = $" ScrollCnt = {ScrollCnt}";

        }
    }
}
