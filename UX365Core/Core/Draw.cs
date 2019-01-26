using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace UX365Core
{
    public static class Draw
    {
        public static List<System.Windows.Window> AllWindows = new List<System.Windows.Window>();
        public static void UpdateSkin(Window wnd)
        {
            if (Core.IsPaused && wnd.Skin != null)
                return;

            System.Windows.Window WindowControl = GetWindowControl(wnd);

            // Delete skin
            if (wnd.Skin == null)
            {
                if(WindowControl != null)
                {
                    WindowControl.Tag = null;
                    WindowControl.Close();
                    AllWindows.Remove(WindowControl);
                }
                return;
            }

            // Set new skin or update
            if (WindowControl == null)
            {
                WindowControl = wnd.Skin.GetUserControl();
                WindowControl.Tag = wnd;

                WindowControl.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                //WindowControl.Content = wnd.Skin.GetUserControl();
                WindowControl.WindowStyle = System.Windows.WindowStyle.None;
                WindowControl.AllowsTransparency = true;
                WindowControl.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));

                var TitleTextBlock = GetControlObjectTitle(WindowControl);
                if (TitleTextBlock != null)
                {
                    TitleTextBlock.TargetUpdated += TitleTextBlock_TargetUpdated;
                    TitleTextBlock.Text = "";
                }

                var DragRegion = GetControlObjectDrag(WindowControl);
                if(DragRegion != null)
                {
                    DragRegion.MouseLeftButtonDown += DragRegion_MouseLeftButtonDown;
                }

                AllWindows.Add(WindowControl);
            }

            if (wnd.Visible)
            {
                WindowControl.Show();
                double paddingLeft = 0, paddingTop = 0, paddingBottom = 0, paddingRight = 0;

                var TitleTextBlock = GetControlObjectTitle(WindowControl);
                if (TitleTextBlock != null) 
                    TitleTextBlock.Text = wnd.Title;

                var GridWindowTopBorder = GetControlObjectGrid(WindowControl, "TopBorder");
                if (GridWindowTopBorder != null)
                {
                    paddingTop += GridWindowTopBorder.Height; // 25 in default theme
                }

                var GridWindowTop = GetControlObjectGrid(WindowControl, "WindowTop");
                if (GridWindowTop != null)
                {
                    if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        paddingTop += GridWindowTop.Height - 1;
                        GridWindowTop.Visibility = System.Windows.Visibility.Visible;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, GridWindowTop.Height - 1, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);
                    }
                    else if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        GridWindowTop.Visibility = System.Windows.Visibility.Hidden;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, 0, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);
                    }
                }
                else
                    GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, 0, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);

                var GridWindowBottom = GetControlObjectGrid(WindowControl, "WindowBottom");
                if (GridWindowBottom != null)
                {
                    if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        paddingBottom += GridWindowBottom.Height - 1;
                        GridWindowBottom.Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        GridWindowBottom.Visibility = System.Windows.Visibility.Hidden;
                    }
                }


                var GridWindowLeft = GetControlObjectGrid(WindowControl, "WindowLeft");
                if (GridWindowLeft != null)
                {
                    if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        paddingLeft += GridWindowLeft.Width - 1; 
                        GridWindowLeft.Visibility = System.Windows.Visibility.Visible;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowLeft.Width - 1, GridWindowTopBorder.Margin.Top, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);
                    }
                    else if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        GridWindowLeft.Visibility = System.Windows.Visibility.Hidden;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(0, GridWindowTopBorder.Margin.Top, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);
                    }
                }
                else
                    GridWindowTopBorder.Margin = new System.Windows.Thickness(0, GridWindowTopBorder.Margin.Top, GridWindowTopBorder.Margin.Right, GridWindowTopBorder.Margin.Bottom);

                var GridWindowRight = GetControlObjectGrid(WindowControl, "WindowRight");
                if (GridWindowRight != null)
                {
                    if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        paddingRight += GridWindowRight.Width - 1;
                        GridWindowRight.Visibility = System.Windows.Visibility.Visible;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, GridWindowTopBorder.Margin.Top, GridWindowRight.Width - 1, GridWindowTopBorder.Margin.Bottom);
                    }
                    else if (wnd.WindowState == System.Windows.WindowState.Normal)
                    {
                        GridWindowRight.Visibility = System.Windows.Visibility.Hidden;
                        GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, GridWindowTopBorder.Margin.Top, 0, GridWindowTopBorder.Margin.Bottom);
                    }
                }
                else
                    GridWindowTopBorder.Margin = new System.Windows.Thickness(GridWindowTopBorder.Margin.Left, GridWindowTopBorder.Margin.Top, 0, GridWindowTopBorder.Margin.Bottom);



                WindowControl.Left = wnd.Location.X - paddingLeft;
                WindowControl.Top = wnd.Location.Y - paddingTop;

                WindowControl.Width = wnd.Size.Width + (paddingLeft + paddingRight);
                WindowControl.Height = wnd.Size.Height + paddingBottom + paddingBottom + GridWindowTopBorder.Height;

                WindowControl.Topmost = wnd.Topmost;
                WindowControl.WindowState = wnd.WindowState;

                WindowControl.ShowInTaskbar = false;
            }
            else
            {
                WindowControl.Hide();
            }
        }

        private static void DragRegion_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Log.WriteLine("Window drag");
        }

        private static void TitleTextBlock_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
        }

        public static void KillSkin(Window wnd)
        {
            wnd.SetSkin(null);
        }

        public static IEnumerable<T> FindVisualChildren<T>(System.Windows.DependencyObject depObj) where T : System.Windows.DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    System.Windows.DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static System.Windows.Controls.Grid GetControlObjectGrid(System.Windows.Window c, string tag)
        {
            var all = FindVisualChildren<System.Windows.Controls.Grid>(c).Where(x => x.Tag != null && x.Tag.ToString() == tag);
            return all.FirstOrDefault();
        }


        public static System.Windows.Controls.TextBlock GetControlObjectTitle(System.Windows.Window c)
        {
            var all = FindVisualChildren<System.Windows.Controls.TextBlock>(c).Where(x => x.Tag != null && x.Tag.ToString() == "WindowTitle");
            return all.FirstOrDefault();
        }

        public static System.Windows.Controls.Grid GetControlObjectDrag(System.Windows.Window c)
        {
            var all = FindVisualChildren<System.Windows.Controls.Grid>(c).Where(x => x.Tag != null && x.Tag.ToString() == "WindowDrag");
            return all.FirstOrDefault();
        }

        public static List<object> GetControlObject(System.Windows.Controls.UserControl c, Type type)
        {
            List<object> l = new List<object>();
            foreach(var i in ((System.Windows.Controls.Grid)c.Content).Children)
            {
                if (i.GetType() == type)
                    l.Add(i);
            }
            return l;
        }

        public static System.Windows.Window GetWindowControl(Window wnd)
        {
            for (int i = 0; i < AllWindows.Count; i++)
            {
                if (AllWindows[i].Tag == wnd)
                    return AllWindows[i];
            }

            return null;
        }

        public static void UpdateSkin(IntPtr wndHandle)
        {
            UpdateSkin(new Window(wndHandle, null));
        }

        public static void KillSkin(IntPtr wndHandle)
        {
            KillSkin(new Window(wndHandle, null));
        }

    }
}
