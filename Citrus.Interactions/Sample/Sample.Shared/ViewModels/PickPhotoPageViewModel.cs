using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI.Popups;

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

    }
}
