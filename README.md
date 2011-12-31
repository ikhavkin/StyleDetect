StyleDetect
===========

[ReSharper](http://www.jetbrains.com/resharper/) plug-in to set the style settings based on already existing C# code.

*Not working yet* =)

I plan to develop it mostly for VS2010. *WILL* break the VS2008 compatibility completely because of .NET 4.0 requirements.

Development setup
-----------------

1. Install R# 6.1 and R# SDK
- Open solution and build it
- Set StyleDetect project settings on the Debug tab as follows:
 - Set 'Start external program:' to `C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe` (change path appropriately for your system!)
 - Set 'Command line arguments:' to `/ReSharper.Plugin Codevolve.StyleDetect.dll /ReSharper.Internal`
 - Set 'Working directory:' to `D:\work\StyleDetect\StyleDetect\bin\Debug` (again adjust the path to Debug directory of the StyleDetect project)
- Run

You should see a new instance of Visual Studio, its ReSharper menu (in the main menu of VS) should contain dummy *StyleDetect* menu item.

Install
-------

The recommended way for now is to put `/ReSharper.Plugin PATH_TO_YOUR_BINARY_HERE\Codevolve.StyleDetect.dll` as a command-line parameter for VS executable (`devenv.exe`)
