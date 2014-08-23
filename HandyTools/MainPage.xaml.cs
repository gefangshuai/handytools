using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI;
using Windows.UI.StartScreen;
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
using HandyTools.Haoma;

namespace HandyTools
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public const string appbarTileId = "handytoolsTile";

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            NavigationCacheMode = NavigationCacheMode.Required;
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            StatusBar.GetForCurrentView().ForegroundColor = Colors.Black;
            App.MainPage = this;
            SqliteHelper.InitDb();
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            Init();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async void PinAppBarButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (!SecondaryTile.Exists(MainPage.appbarTileId))
           
            {
                // Pin
                Uri square150x150Logo = new Uri("ms-appx:///Assets/Logo.scale-240.png");
                string tileActivationArguments = MainPage.appbarTileId + " was pinned at = " +
                                                 DateTime.Now.ToLocalTime().ToString();
                string displayName = "HandyTools";

                TileSize newTileDesiredSize = TileSize.Square150x150;

                SecondaryTile secondaryTile = new SecondaryTile(MainPage.appbarTileId,
                    displayName,
                    tileActivationArguments,
                    square150x150Logo,
                    newTileDesiredSize);

                secondaryTile.VisualElements.Square30x30Logo = new Uri("ms-appx:///images/square30x30Tile-sdk.png");
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;
                secondaryTile.RoamingEnabled = false;

                await secondaryTile.RequestCreateAsync();
                
            }
            Init();

        }

        private void ToggleAppBarButton(bool showPinButton)
        {
            if (showPinButton)
            {
                this.PinAppBarButton.Label = "固定到开始";
                this.PinAppBarButton.Icon = new SymbolIcon(Symbol.Pin);
            }
            else
            {
                this.PinAppBarButton.Label = "取消固定";
                this.PinAppBarButton.Icon = new SymbolIcon(Symbol.UnPin);
            }

            this.PinAppBarButton.UpdateLayout();
        }
        void Init()
        {
            ToggleAppBarButton(!SecondaryTile.Exists(MainPage.appbarTileId));
        }
    }
}
