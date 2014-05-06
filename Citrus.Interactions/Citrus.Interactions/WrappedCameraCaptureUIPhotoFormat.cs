using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace Citrus.Interactions
{
    /// <summary>
    /// キャプチャされた写真を格納するための形式を決定します。
    /// </summary>
    /// <remarks>Windows ストアアプリ用の Windows.Media.Capture.CameraCaptureUIPhotoFormat をラップしています。</remarks>
    public enum WrappedCameraCaptureUIPhotoFormat
    {
        /// <summary>
        /// JPEG 形式。
        /// </summary>
        Jpeg = 0,
        /// <summary>
        /// PNG 形式。
        /// </summary>
        Png = 1,
        /// <summary>
        /// JPEG-XR 形式。
        /// </summary>
        JpegXR = 2
    }
}
