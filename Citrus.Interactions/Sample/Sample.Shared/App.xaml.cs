using Citrus.Interactions;
using Microsoft.Practices.Unity;
using PrismAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// 空のアプリケーション テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234227 を参照してください

namespace Sample
{
    /// <summary>
    /// 既定の Application クラスに対してアプリケーション独自の動作を実装します。
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
        private ContinuationManager continuationManager;
#endif
        private readonly UnityContainer container = new UnityContainer();

        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        /// 最初の行であり、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために呼び出されたときに
        /// 検索結果やその他の情報を表示するために使用されます。
        /// </summary>
        /// <param name="e">起動要求とプロセスの詳細を表示します。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            // Create bootstrapper.
            var boot = new PrismAdapterBootstrapper
            {
                // customize resolve process.
                Resolve = type => this.container.Resolve(type)
            };

            // setup and run application.
            await boot.Setup(e);

            // register instances and types.
            boot.RegistPrismInstanceTo(this.container)
                .Run(nav => nav.Navigate("Main", e.Arguments));
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// アプリを起動した後のコンテンツの移行を復元します。
        /// </summary>
        /// <param name="sender">ハンドラーがアタッチされたオブジェクト。</param>
        /// <param name="e">ナビゲーション イベントの詳細。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            if (rootFrame != null)
            {
                rootFrame.ContentTransitions =
                    this.transitions
                    ?? (this.transitions = new TransitionCollection { new NavigationThemeTransition() });
                rootFrame.Navigated -= this.RootFrame_FirstNavigated;
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            var e = args as IContinuationActivatedEventArgs;
            if (e != null)
            {
                (this.continuationManager ?? (this.continuationManager = new ContinuationManager()))
                    .Continue(e);
            }
        }
#endif
    }
}