using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using HandyTools.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace HandyTools.Haoma
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChangYongPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<Changyong> changyongs; 
        public ChangYongPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            NavigationCacheMode = NavigationCacheMode.Required;
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            try
            {
                var localData = await FileHelper.GetChangyongDataFromLocalFile();
                if (localData == null)
                {
                    var confirmDialog = new MessageDialog("系统监测到您本地没有数据文件，是否要联网下载？", "提示");
                    confirmDialog.Commands.Add(new UICommand("下载"));
                    confirmDialog.Commands.Add(new UICommand("下次再说"));
                    var cmd = await confirmDialog.ShowAsync();
                    if (cmd.Label.Equals("下载"))
                    {
                        string data = await DownloadData();
                        await FileHelper.WriteChangyongDataToLocalFile(data);
                        localData = data;
                    }
                    else
                    {
                        // 取消下载
                        return;
                    }
                }
                var changyongCategories = HtmlHelper.ParseChangyong(localData);
                LoadData(changyongCategories);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

        }

        private async Task<string> DownloadData()
        {
            try
            {
                string data = await HttpClientHelper.GetWithUtf8(API.ChangYongData);
                if (string.IsNullOrWhiteSpace(data))
                {
                    MessageDialog dialog = new MessageDialog("数据错误，请稍后重试！", "错误");
                    dialog.ShowAsync();
                    return null;
                }
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void LoadData(IEnumerable<ChangyongCategory> changyongCategories)
        {
            changyongs = new List<Changyong>();
            foreach (var changyongCategory in changyongCategories)
            {
                changyongs.AddRange(changyongCategory.Changyongs);
            }
            cvsActivities.Source = from c in changyongs group c by c.Category into g select new { GroupName = g.Key, Items = g, Count = g.Count(), IWidth = Window.Current.Bounds.Width };
            var listViewBase = DataSemanticZoom.ZoomedOutView as ListViewBase;
            if (listViewBase != null)
                listViewBase.ItemsSource = cvsActivities.View.CollectionGroups;
        }

        private async void LoadData()
        {
            string data = await FileHelper.GetChangyongDataFromLocalFile();
            LoadData(HtmlHelper.ParseChangyong(data));
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Changyong;
            PhoneCallManager.ShowPhoneCallUI(item.Value.Replace("-", ""), item.Name);
        }

        private void SearchAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchChangyongPage), changyongs);
        }

        private async void BaoAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var mailUri = new Uri("mailto:gefangshuai@live.com?subject=关于《全国公共服务号码》的反馈");
            await Launcher.LaunchUriAsync(mailUri);
        }

        private async void UpdateAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            string webData = await DownloadData();
            string localData = await FileHelper.GetChangyongDataFromLocalFile();

            if (!string.IsNullOrWhiteSpace(webData) && !string.IsNullOrWhiteSpace(localData))
            {

                var webCategories = HtmlHelper.ParseChangyong(webData);
                var localCategories = HtmlHelper.ParseChangyong(localData);
                var wc = webCategories as IList<ChangyongCategory> ?? webCategories.ToList();
                var lc = localCategories as IList<ChangyongCategory> ?? localCategories.ToList();
                if (!wc.FirstOrDefault().UpdateTime.Equals(lc.FirstOrDefault().UpdateTime)) // 有更新
                {
                    var dialog = new MessageDialog("发现新数据，是否更新?", "提示");
                    dialog.Commands.Add(new UICommand("更新", async (action) =>
                    {
                        await UpdateData();
                    }));
                    dialog.Commands.Add(new UICommand("不更新", (action) =>
                    {

                    }));
                    dialog.ShowAsync();
                }
                else
                {
                    new MessageDialog("无更新", "提示").ShowAsync();
                }
            }
        }

        private async Task UpdateData()
        {
            var data = await DownloadData();
            await FileHelper.WriteChangyongDataToLocalFile(data);
            new MessageDialog("更新成功!", "提示").ShowAsync();
            LoadData();
        }

        private async void ForceAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            await UpdateData();
        }
    }
}
