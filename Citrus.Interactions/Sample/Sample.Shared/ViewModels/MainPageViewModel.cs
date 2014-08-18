using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.ViewModels
{
    class MainPageViewModel : ViewModel
    {
        [Dependency]
        public INavigationService NavigationService { get; set; }

        private DelegateCommand navigateToPickPhotoPageCommand;
        public DelegateCommand NavigateToPickPhotoPageCommand
        {
            get
            {
                return this.navigateToPickPhotoPageCommand
                    ?? (this.navigateToPickPhotoPageCommand = new DelegateCommand(
                        () =>
                        {
                            this.NavigationService.Navigate("PickPhoto", null);
                        }));
            }
        }

        private DelegateCommand navigateToMultiLayoutPageCommand;
        public DelegateCommand NavigateToMultiLayoutPageCommand
        {
            get
            {
                return this.navigateToMultiLayoutPageCommand
                    ?? (this.navigateToMultiLayoutPageCommand = new DelegateCommand(
                        () =>
                        {
                            this.NavigationService.Navigate("MultiLayout", null);
                        }));
            }
        }
    }
}
