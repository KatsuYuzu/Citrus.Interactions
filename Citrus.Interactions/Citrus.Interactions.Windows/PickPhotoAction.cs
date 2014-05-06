using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace Citrus.Interactions
{
    /// <summary>
    /// ファイル ピッカーを表示するアクションです。
    /// </summary>
    public class PickPhotoAction : DependencyObject, IAction
    {
        /// <summary>
        /// アクションの結果を表します。
        /// </summary>
        private enum Result
        {
            Canceled = 0,
            Executed = 1
        }

        private static List<string> _supportedExtensions;

        /// <summary>
        /// システムにインストールされているビットマップ デコーダーによってサポートされるすべてのファイル拡張子のコレクション。
        /// </summary>
        private static IReadOnlyList<string> SupportedExtensions
        {
            get
            {
                // 遅延初期化とする
                return _supportedExtensions
                    ?? (_supportedExtensions = BitmapDecoder.GetDecoderInformationEnumerator()
                            .SelectMany(x => x.FileExtensions)
                            .ToList());
            }
        }

        /// <summary>
        /// アクションの実行後に呼び出す <see cref="ICommand"/> を取得または設定します。
        /// </summary>
        public ICommand CallbackCommand
        {
            get { return (ICommand)GetValue(CallbackCommandProperty); }
            set { SetValue(CallbackCommandProperty, value); }
        }
        /// <summary>
        /// CallbackCommand 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CallbackCommandProperty =
            DependencyProperty.Register("CallbackCommand",
                                        typeof(ICommand),
                                        typeof(PickPhotoAction),
                                        new PropertyMetadata(null));

        /// <summary>
        /// アクションのエラーをハンドルする <see cref="ICommand"/> を取得または設定します。
        /// ただし、<see cref="CallbackCommand"/> 内の例外は補足しません。
        /// </summary>
        public ICommand ErrorHandleCommand
        {
            get { return (ICommand)GetValue(ErrorHandleCommandProperty); }
            set { SetValue(ErrorHandleCommandProperty, value); }
        }
        /// <summary>
        /// ErrorHandleCommand 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ErrorHandleCommandProperty =
            DependencyProperty.Register("ErrorHandleCommand",
                                        typeof(ICommand),
                                        typeof(PickPhotoAction),
                                        new PropertyMetadata(null));

        /// <summary>
        /// ファイル オープン ピッカーが項目を表示するために使用する表示モードを取得または設定します。
        /// </summary>
        public PickerViewMode PickerViewMode
        {
            get { return (PickerViewMode)GetValue(PickerViewModeProperty); }
            set { SetValue(PickerViewModeProperty, value); }
        }
        /// <summary>
        /// PickerViewMode 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty PickerViewModeProperty =
            DependencyProperty.Register("PickerViewMode",
                                        typeof(PickerViewMode),
                                        typeof(PickPhotoAction),
                                        new PropertyMetadata(PickerViewMode.List));

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="sender">使用されません。</param>
        /// <param name="parameter">使用されません。</param>
        /// <returns>アクションの結果を返します。</returns>
        public object Execute(object sender, object parameter)
        {
            if (this.CallbackCommand == null)
            {
                return Result.Canceled;
            }

#if WINDOWS_APP
            // 警告の抑制
            var _ = ExecuteAsync(this.PickerViewMode, this.CallbackCommand, this.ErrorHandleCommand);
#else
            PickPhoto(this.PickerViewMode);
#endif

            return Result.Executed;
        }

#if WINDOWS_APP
        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="viewMode">ファイル オープン ピッカーが項目を表示するために使用する表示モード。</param>
        /// <param name="callbakCommand">アクションの実行後に呼び出す <see cref="ICommand"/>。</param>
        /// <param name="errorHandleCommand">例外発生時に呼び出す <see cref="ICommand"/>。</param>
        /// <returns></returns>
        private async static Task ExecuteAsync(PickerViewMode viewMode, ICommand callbakCommand, ICommand errorHandleCommand)
        {
            StorageFile photo;
            try
            {
                photo = await PickPhotoAsync(viewMode);
            }
            catch (Exception ex)
            {
                if (errorHandleCommand != null && errorHandleCommand.CanExecute(ex))
                {
                    errorHandleCommand.Execute(ex);
                    return;
                }
                else
                {
                    throw;
                };
            }

            if (!callbakCommand.CanExecute(photo))
            {
                return;
            }

            callbakCommand.Execute(photo);
        }
#endif

        /// <summary>
        /// 写真を選択します。
        /// </summary>
        /// <param name="viewMode">ファイル オープン ピッカーが項目を表示するために使用する表示モード。</param>
        /// <returns></returns>
#if WINDOWS_APP
        private async static Task<StorageFile> PickPhotoAsync(PickerViewMode viewMode)
#else
        private static void PickPhoto(PickerViewMode viewMode)
#endif
        {
            var picker = new FileOpenPicker();

            picker.ViewMode = viewMode;

            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            foreach (var extension in SupportedExtensions)
            {
                picker.FileTypeFilter.Add(extension);
            }
#if WINDOWS_APP
            return await picker.PickSingleFileAsync();
#else
            picker.PickSingleFileAndContinue();
#endif
        }
    }
}
