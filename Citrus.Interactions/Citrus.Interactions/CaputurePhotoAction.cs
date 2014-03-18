using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Citrus.Interactions
{
    /// <summary>
    /// 写真をキャプチャするアクションです。
    /// </summary>
    public class CaputurePhotoAction : DependencyObject, IAction
    {
        /// <summary>
        /// アクションの結果を表します。
        /// </summary>
        private enum Result
        {
            Canceled = 0,
            Executed = 1
        }

        /// <summary>
        /// アクションの実行後に呼び出す <see cref="ICommand"/> を取得または設定します。
        /// </summary>
        public ICommand CallbackCommand
        {
            get { return (ICommand)GetValue(CallbackCommandProperty); }
            set { SetValue(CallbackCommandProperty, value); }
        }
        public static readonly DependencyProperty CallbackCommandProperty =
            DependencyProperty.Register("CallbackCommand",
                                        typeof(ICommand),
                                        typeof(CaputurePhotoAction),
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
        public static readonly DependencyProperty ErrorHandleCommandProperty =
            DependencyProperty.Register("ErrorHandleCommand",
                                        typeof(ICommand),
                                        typeof(CaputurePhotoAction),
                                        new PropertyMetadata(null));

        /// <summary>
        /// キャプチャされた写真を格納する形式を取得または設定します。
        /// </summary>
        public CameraCaptureUIPhotoFormat PhotoFormat
        {
            get { return (CameraCaptureUIPhotoFormat)GetValue(PhotoFormatProperty); }
            set { SetValue(PhotoFormatProperty, value); }
        }
        public static readonly DependencyProperty PhotoFormatProperty =
            DependencyProperty.Register("PhotoFormat",
                                        typeof(CameraCaptureUIPhotoFormat),
                                        typeof(CaputurePhotoAction),
                                        new PropertyMetadata(CameraCaptureUIPhotoFormat.Jpeg));

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

            // 警告の抑制
            var _ = ExecuteAsync(this.PhotoFormat, this.CallbackCommand, this.ErrorHandleCommand);

            return Result.Executed;
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="format">キャプチャされた写真を格納する形式を決定します。</param>
        /// <param name="callbakCommand">アクションの実行後に呼び出す <see cref="ICommand"/>。</param>
        /// <param name="errorHandleCommand">例外発生時に呼び出す <see cref="ICommand"/>。</param>
        /// <returns></returns>
        private async static Task ExecuteAsync(CameraCaptureUIPhotoFormat format, ICommand callbakCommand, ICommand errorHandleCommand)
        {
            StorageFile photo;
            try
            {
                photo = await CaputurePhotoAsync(format);
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

        /// <summary>
        /// 写真をキャプチャします。
        /// </summary>
        /// <param name="format">キャプチャされた写真を格納する形式を決定します。</param>
        /// <returns></returns>
        private async static Task<StorageFile> CaputurePhotoAsync(CameraCaptureUIPhotoFormat format)
        {
            var camera = new CameraCaptureUI();

            camera.PhotoSettings.Format = format;

            return await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
        }
    }
}
