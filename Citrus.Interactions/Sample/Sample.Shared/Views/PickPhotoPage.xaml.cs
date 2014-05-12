using PrismAdapter;
using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
#if WINDOWS_PHONE_APP
    public sealed partial class PickPhotoPage : Page, Citrus.Interactions.IFileOpenPickerContinuable
#else
    public sealed partial class PickPhotoPage : Page
#endif
    {
        public PrismNavigationHelper NavigationHelper { get; set; }

        public PickPhotoPage()
        {
            this.InitializeComponent();
            this.NavigationHelper = new PrismNavigationHelper(this);
            this.NavigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.NavigationHelper.EnablePlatformSupport();
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedTo(e);
        }

#if WINDOWS_PHONE_APP
        public void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            var photo = args.Files.SingleOrDefault();

            var vm = this.DataContext as Sample.ViewModels.PickPhotoPageViewModel;
            if (vm != null)
            {
                if (vm.PickPhotoCommand.CanExecute(photo))
                {
                    vm.PickPhotoCommand.Execute(photo);
                }
            }
        }
#endif
    }
}
