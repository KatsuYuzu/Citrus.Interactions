Citrus.Interactions
===================
Citrus.Interactions - Action / Behavior library for universal Windows apps

Install
-------
NuGet, [Citrus.Interactions](https://www.nuget.org/packages/Citrus.Interactions/)  
```
PM> Install-Package Citrus.Interactions
```

Usage
------
xaml
```xml
<Button Content="PickPhotoAction">
    <Interactivity:Interaction.Behaviors>
        <Core:EventTriggerBehavior EventName="Click">
            <Interactions:PickPhotoAction CallbackCommand="{Binding PickPhotoCommand}" />
        </Core:EventTriggerBehavior>
    </Interactivity:Interaction.Behaviors>
</Button>
```

ViewModel, Sample used `Prism.StoreApps`
```csharp
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
```

Phone `App.xaml.cs`
```csharp
protected override void OnActivated(IActivatedEventArgs args)
{
    var e = args as IContinuationActivatedEventArgs;
    if (e != null)
    {
        (this.continuationManager ?? (this.continuationManager = new ContinuationManager()))
            .Continue(e);
    }
}
```

History
-------
ver 1.1.1-alpha - 2014-05-07
* Manually install of Behaviors SDK are now not required.

ver 1.1.0-alpha - 2014-05-06
* Changed target to UniversalApp

ver 1.0.0.0 - 2014-03-19
* Released

License
-------
under [MIT License](http://opensource.org/licenses/MIT)
