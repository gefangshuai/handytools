using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Popups;
using HandyTools.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍
using HandyTools.Data;
using Calendar = System.Globalization.Calendar;
using DayOfWeek = Windows.Globalization.DayOfWeek;

namespace HandyTools.Tuili
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StarPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableCollection<StarDay> StarDays { get; set; }
        public ObservableCollection<StarDay> StarDaysTomorrow { get; set; }
        public ObservableCollection<StarWeek> StarDaysWeek { get; set; }
        public ObservableCollection<StarDay> StarDaysMonth { get; set; }
        public ObservableCollection<StarDay> StarDaysYear { get; set; } 

        public StarPage()
        {
            StarDays = new ObservableCollection<StarDay>();
            StarDaysTomorrow = new ObservableCollection<StarDay>();
            StarDaysWeek = new ObservableCollection<StarWeek>();
            StarDaysMonth = new ObservableCollection<StarDay>();
            StarDaysYear = new ObservableCollection<StarDay>();

            this.InitializeComponent();

            StarDayListView.DataContext = this;
            StarTomorrowListView.DataContext = this;
            StarWeekListView.DataContext = this;
            StarMonthListView.DataContext = this;
            StarYearListView.DataContext = this;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        # region InitData
        private async void InitData()
        {
            SettinGrid.Width = Window.Current.CoreWindow.Bounds.Width;
            StarComboBox.ItemsSource = AppData.GetStars();
            var star = SettingsHelper.GetStar();
            if (star == null)
            {
                MessageDialog dialog = new MessageDialog("第一次使用本程序需要设置您的基本信息，是否设置?", "提示");
                dialog.Commands.Add(new UICommand("设置", command =>
                {
                    SettingsPopup.IsOpen = true;
                }));
                dialog.Commands.Add(new UICommand("不设置"));
                await dialog.ShowAsync();
            }
            else
            {
                StarTextBlock.Text = star.ToString();
            }
        }

        /// <summary>
        /// 获取与此 <see cref="Page"/> 关联的 <see cref="NavigationHelper"/>。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取此 <see cref="Page"/> 的视图模型。
        /// 可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源; 通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。 首次访问页面时，该状态将为 null。</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="sender">事件的来源；通常为 <see cref="NavigationHelper"/></param>
        ///<param name="e">提供要使用可序列化状态填充的空字典
        ///的事件数据。</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        #endregion
        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// <para>
        /// 应将页面特有的逻辑放入用于
        /// <see cref="NavigationHelper.LoadState"/>
        /// 和 <see cref="NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </para>
        /// </summary>
        /// <param name="e">提供导航方法数据和
        /// 无法取消导航请求的事件处理程序。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SettingAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = true;
        }

        private void StarPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitData();
            //LoadData();
        }

        private async void LoadData()
        {
            ProgressPanel.Visibility = Visibility.Visible;
            var star = SettingsHelper.GetStar();
            switch (DataPivot.SelectedIndex)
            {
                case 0:
                    await LoadStarDay(star);
                    break;
                case 1:
                    await LoadStarTomorrow(star);
                    break;
                case 2:
                    await LoadStarWeek(star);
                    break;
                case 3:
                    await LoadStarMonth(star);
                    break;
                case 4:
                    await LoadStarYear(star);
                    break;
            }
            ProgressPanel.Visibility = Visibility.Collapsed;
        }

        private async Task LoadStarDay(Star star)
        {
            try
            {
                StarDays.Clear();
                List<StarDay> starDays = await SqliteHelper.GetStarDays(DateTime.Now.ToString("yyyy-MM-dd"), star.Id);
                if (starDays.Count == 0)
                {
                    await LoadStarDayFromJson(star);
                }
                else
                {
                    foreach (var starDay in starDays)
                    {
                        StarDays.Add(starDay);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task LoadStarTomorrow(Star star)
        {
            try
            {
                StarDaysTomorrow.Clear();
                List<StarDay> starDays = await SqliteHelper.GetStarDays(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), star.Id);
                if (starDays.Count == 0)
                {
                    await LoadStarTomorrowFromJson(star);
                }
                else
                {
                    foreach (var starDay in starDays)
                    {
                        StarDaysTomorrow.Add(starDay);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task LoadStarWeek(Star star)
        {
            try
            {
                StarDaysWeek.Clear();
                await LoadStarWeekFromJson(star);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task LoadStarMonth(Star star)
        {
            try
            {
                StarDaysMonth.Clear();
                await LoadStarMonthFromJson(star);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        private async Task LoadStarYear(Star star)
        {
            try
            {
                StarDaysYear.Clear();
                await LoadStarYearFromJson(star);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async Task LoadStarDayFromJson(Star star)
        {
            string json = await HttpClientHelper.GetWithUtf8(string.Format(API.StarDay, star.Id));
            if (json != null)
                LoadDayAndTomorrowFromJson(star, json, StarDays, true);
        }

        private async Task LoadStarTomorrowFromJson(Star star)
        {
            string json = await HttpClientHelper.GetWithUtf8(string.Format(API.StarTomorrow, star.Id));
            if (json != null)
                LoadDayAndTomorrowFromJson(star, json, StarDaysTomorrow, false);
        }

        private async Task LoadStarWeekFromJson(Star star)
        {
            string json = await HttpClientHelper.GetWithUtf8(string.Format(API.StarWeek, star.Id));
            if (json != null)
            {
                JsonArray array = JsonArray.Parse(json);
                foreach (var item in array)
                {
                    StarWeek starWeek = new StarWeek();
                    if (item.ValueType == JsonValueType.Object)
                    {
                        if (item.GetObject().ContainsKey("title"))
                            starWeek.Title = item.GetObject()["title"].GetString().Replace("\n", "");

                        if (item.GetObject().ContainsKey("title2")) 
                        {
                            JsonArray titleArray = item.GetObject()["title2"].GetArray();
                            starWeek.Title1 = titleArray.GetStringAt(0).Replace("\n", "");
                            starWeek.Title2 = titleArray.GetStringAt(1).Replace("\n", "");
                        }
                        if (item.GetObject().ContainsKey("rank"))
                        {
                            if (item.GetObject()["rank"].ValueType == JsonValueType.Array)
                            {
                                JsonArray rankArray = item.GetObject()["rank"].GetArray();
                                starWeek.Rank1 = (int) rankArray.GetNumberAt(0);
                                starWeek.Rank2 = (int) rankArray.GetNumberAt(1);
                            }
                            else
                            {
                                starWeek.Rank = (int)item.GetObject()["rank"].GetNumber();
                            }
                        }
                        if (item.GetObject().ContainsKey("value"))
                            starWeek.Value = item.GetObject()["value"].GetString().Replace("\n", "");

                        if (item.GetObject().ContainsKey("value2"))
                        {
                            JsonArray valueArray = item.GetObject()["value2"].GetArray();
                            starWeek.Value1 = valueArray.GetStringAt(0).Replace("\n", "");
                            starWeek.Value2 = valueArray.GetStringAt(1).Replace("\n", "");
                        }
                    }

                    if (!string.IsNullOrEmpty(starWeek.Title))
                    {
                        StarDaysWeek.Add(starWeek);
                    }
                }
            }
                
        }

        private async Task LoadStarMonthFromJson(Star star)
        {
            string json = await HttpClientHelper.GetWithUtf8(string.Format(API.StarMonth, star.Id));
            if (json != null)
            {
                JsonArray array = JsonArray.Parse(json);
                foreach (var item in array)
                {
                    StarDay starDay = new StarDay();
                    if (item.ValueType == JsonValueType.Object)
                    {
                        if (item.GetObject().ContainsKey("title"))
                            starDay.Title = item.GetObject()["title"].GetString();
                        if (item.GetObject().ContainsKey("rank"))
                            starDay.Rank = (int)item.GetObject()["rank"].GetNumber();
                        if (item.GetObject().ContainsKey("value"))
                            starDay.Value = item.GetObject()["value"].GetString();
                    }
                    StarDaysMonth.Add(starDay);
                }
            }

        }
        private async Task LoadStarYearFromJson(Star star)
        {
            string json = await HttpClientHelper.GetWithUtf8(string.Format(API.StarYear, star.Id));
            if (json != null)
            {
                JsonArray array = JsonArray.Parse(json);
                foreach (var item in array)
                {
                    StarDay starDay = new StarDay();
                    if (item.ValueType == JsonValueType.Object)
                    {
                        if (item.GetObject().ContainsKey("title"))
                            starDay.Title = item.GetObject()["title"].GetString();
                        if (item.GetObject().ContainsKey("rank"))
                            starDay.Rank = (int)item.GetObject()["rank"].GetNumber();
                        if (item.GetObject().ContainsKey("value"))
                            starDay.Value = item.GetObject()["value"].GetString();
                    }
                    StarDaysYear.Add(starDay);
                }
            }

        }

        private  void LoadDayAndTomorrowFromJson(Star star, string json, ObservableCollection<StarDay> starDays, bool today)
        {
            JsonArray array = JsonArray.Parse(json);
            foreach (var item in array)
            {
                StarDay starDay = new StarDay();
                if (item.ValueType == JsonValueType.Object)
                {
                    if (item.GetObject().ContainsKey("title"))
                        starDay.Title = item.GetObject()["title"].GetString();
                    if (item.GetObject().ContainsKey("rank"))
                        starDay.Rank = (int)item.GetObject()["rank"].GetNumber();
                    if (item.GetObject().ContainsKey("value"))
                        starDay.Value = item.GetObject()["value"].GetString();
                }

                if (!string.IsNullOrEmpty(starDay.Title))
                {
                    starDay.StarId = star.Id;
                    starDay.Day = today ? DateTime.Now.ToString("yyyy-MM-dd") : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    starDays.Add(starDay);
                    SqliteHelper.SaveStarDay(starDay);
                }
            }
        }

      

        private async void OkAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var star = StarComboBox.SelectedItem as Star;
            if (star != null)
            {
                SettingsHelper.AddStar(star);
                StarTextBlock.Text = star.ToString();
                SettingsPopup.IsOpen = false;
                LoadData();
            }
        }

        private void CancelAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = false;
        }

        private void StarTextBlock_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SettingsPopup.IsOpen = true;
        }

        private async void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var star = SettingsHelper.GetStar();
            if (star != null)
            {
                LoadData();
            }
        }

        private void ClearAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDialog tipDialog = new MessageDialog("确定要删除缓存的星座数据吗?","提示");
            tipDialog.Commands.Add(new UICommand("删除", async command =>
            {
                await SqliteHelper.ClearStarDay();
                MessageDialog dialog = new MessageDialog("缓存数据已删除！", "提示");
                dialog.ShowAsync();
            }));
            tipDialog.Commands.Add(new UICommand("取消"));
            tipDialog.ShowAsync();
        }
    }
}
