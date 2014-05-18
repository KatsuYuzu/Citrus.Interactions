using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace Sample.ViewModels
{
    class PickPhotoPageViewModel : ViewModel
    {
        [Dependency]
        public INavigationService NavigationService { get; set; }

        private string pickedPhotoName;
        public string PickedPhotoName
        {
            get { return this.pickedPhotoName; }
            set { SetProperty(ref this.pickedPhotoName, value); }
        }

        private BitmapImage pickedPhotoImage;
        public BitmapImage PickedPhotoImage
        {
            get { return this.pickedPhotoImage; }
            set { SetProperty(ref this.pickedPhotoImage, value); }
        }

        private DelegateCommand<StorageFile> pickPhotoCommand;
        public DelegateCommand<StorageFile> PickPhotoCommand
        {
            get
            {
                return this.pickPhotoCommand
                    ?? (this.pickPhotoCommand = new DelegateCommand<StorageFile>(
                        photo =>
                        {
                            this.PickedPhotoName = photo.DisplayName;
                        },
                        photo => photo != null));
            }
        }

        private DelegateCommand<Exception> handleErrorCommand;
        public DelegateCommand<Exception> HandleErrorCommand
        {
            get
            {
                return this.handleErrorCommand
                    ?? (this.handleErrorCommand = new DelegateCommand<Exception>(
                        async ex =>
                        {
                            await new MessageDialog(ex.Message).ShowAsync();
                        }));
            }
        }

        private DelegateCommand<StorageFile> openPhotoCommand;
        public DelegateCommand<StorageFile> OpenPhotoCommand
        {
            get
            {
                return this.openPhotoCommand
                    ?? (this.openPhotoCommand = DelegateCommand<StorageFile>.FromAsyncHandler(
                        async photo =>
                        {
                            using (var stream = await photo.OpenAsync(FileAccessMode.Read))
                            {
                                var bitmapImage = new BitmapImage();
                                await bitmapImage.SetSourceAsync(stream);
                                this.PickedPhotoImage = bitmapImage;
                            }
                        },
                        photo => photo != null));
            }
        }
    }
}
