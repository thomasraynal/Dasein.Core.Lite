SET msBuildLocation="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"

dotnet publish --configuration Debug ./Demo/Dasein.Core.Lite.Demo.Host

call %msBuildLocation% ./Demo/Dasein.Core.Lite.Demo.Desktop/Dasein.Core.Lite.Demo.Desktop.csproj

start /d "." dotnet run --no-build --project ./Demo/Dasein.Core.Lite.Demo.Host

timeout 7

start  ./Demo/Dasein.Core.Lite.Demo.Desktop/bin/Debug/Dasein.Service.Demo.Desktop.exe

pause