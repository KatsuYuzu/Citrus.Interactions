forfiles /m Citrus.Interactions.*.nupkg /c "cmd /c del @file"
.nuget\NuGet.exe pack Citrus.Interactions.nuspec
forfiles /m Citrus.Interactions.*.nupkg /c "cmd /c explorer /select,@file"
