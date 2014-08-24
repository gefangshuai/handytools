using System.Diagnostics;
using Windows.ApplicationModel.Store;
using Windows.Storage;
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
using HandyTools.Tuili;
using SharpYaml.Serialization;

namespace HandyTools.Shenfen
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ShenfenPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ShenfenPage()
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
        /// <see cref="Frame.Navigate(Category, Object)"/> 的导航参数，又提供
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (CurrentApp.LicenseInformation.IsTrial)
            {
                MessageDialog dialog = new MessageDialog("试用版暂不开放此功能，请购买完整版，谢谢支持！", "提示");
                await dialog.ShowAsync();
                navigationHelper.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void SearchAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var text = DataTextBox.Text;
            if (text.Length != 15 && text.Length != 18)
            {
                new MessageDialog("请输入正确的身份证号!", "错误").ShowAsync();
                return; 
            }

            Uri uri = new Uri("ms-appx:///Data/idcard");
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            string content = await FileIO.ReadTextAsync(file);
            var input = new StringReader(content);
            // Load the stream
            var yaml = new YamlStream();
            yaml.Load(input);

            Shenfen shenfen = new Shenfen();
            // 获得地区
            var mapping = (YamlMappingNode)(yaml.Documents[0].RootNode);
            foreach (var entry in mapping.Children)
            {
                if (text.Substring(0, 6).Equals(entry.Key.ToString()))
                {
                    shenfen.Region = entry.Value.ToString();
                }
            }

            // 解析15位身份证号
            if (text.Length == 15)
            {
                var year = text.Substring(6, 2);
                var mounth = text.Substring(8, 2);
                var day = text.Substring(10, 2);
                shenfen.BirthDay = string.Format("19{0}年{1}月{2}日", year, mounth, day);
                var s = int.Parse(text.Substring(14, 1));
                if (s%2 == 0)
                {
                    shenfen.Gender = "女";
                }
                else
                {
                    shenfen.Gender = "男";
                }
            }
            // 解析18位身份证号
            else
            {
                var year = text.Substring(6, 4);
                var mounth = text.Substring(10, 2);
                var day = text.Substring(12, 2);
                shenfen.BirthDay = string.Format("{0}年{1}月{2}日", year, mounth, day);
                var s = int.Parse(text.Substring(16, 1));
                if (s % 2 == 0)
                {
                    shenfen.Gender = "女";
                }
                else
                {
                    shenfen.Gender = "男";
                }
            }

            DataListView.DataContext = shenfen;
            DataListView.Visibility = Visibility.Visible;
        }

        private void ShenfenPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataTextBox.Focus(FocusState.Programmatic);
        }
    }
}
