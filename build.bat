@echo off

:checkMsBuild
if exist "%msbuild%" goto checkNuget
set msbuild=
for %%a in (MSBuild.exe) do (set msbuild=%%~$PATH:a)
if exist %msbuild% goto checkNuget

rem Visual Studio Build Tools 2017
set msbuild=C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget
rem VS2017
set msbuild=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget
set msbuild=C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget
set msbuild=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget

rem VS2015, 2013, cf.: http://blogs.msdn.com/b/visualstudio/archive/2013/07/24/msbuild-is-now-part-of-visual-studio.aspx
set msbuild=c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget
rem VS2013
set msbuild=c:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe
if exist "%msbuild%" goto checkNuget
rem .NET Framework
for /D %%a in (%SYSTEMROOT%\Microsoft.NET\Framework\v4.0*) do set "msbuild=%%a\MSBuild.exe"
if exist "%msbuild%" goto checkNuget

echo error: can't find MSBuild.exe. Is .NET Framework installed?
exit /B 2

:checkNuget
for %%a in (NuGet.exe) do (set nugetPath=%%~$PATH:a)
if not defined nugetPath (
	echo error: can't find NuGet.exe in your path.
	exit /B 3
)

:setDefaults
if not defined Configuration set Configuration=Release
if not defined EnableNuGetPackageRestore set EnableNuGetPackageRestore=true

set solutionDir=%~dp0%
if not "%cd%\"=="%solutionDir%" (
  echo change to directory "%solutionDir%"
  cd /d "%solutionDir%"
)

:packageRestore
NuGet.exe restore -source "%PackageSources%"

:build
"%msbuild%" %* /p:CustomBeforeMicrosoftCommonTargets="%solutionDir%empty.targets"
