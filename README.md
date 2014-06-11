FLCLib
======

A C# library for playing back Autodesk FLC and FLI video files.


Layout
------

FLCLib/
 - Contains the source code for all supported variants/platforms, as well as a generic C# only implementation (which is the base for all other variants).

FLCPlayer.Metro/
 - Contains the source code for a Windows Modern UI test player (uses the projects in FLCLib/)

FLCTestPlayer/
 - Contains the source code for generic test player (uses the projects in FLCLib/)



FLCPlayer.Metro.sln
 - Solution file for the Windows Modern UI test player.

FLCTestPlayer.sln
 - Solution file for the generic test player.


Usage
-----
If you have one of the supported platforms, check one of the test player projects. The code isn't very clean, but should be largely self-explanatory.

Otherwise, create a new project for your platform and add a reference to FLCLib/FLCLibCS.csproj. Open the flc file by instantiating a new object of class FLCFile, set up the callbacks (especially OnFrameUpdated) and call Play(). Whenever OnFrameUpdated gets invoked, use FLCFile.GetFramebufferCopy () to obtain an array of RGBA pixel data. FLCFile has public Width and Height properties to help you interpret that array. Then use your platform specific rendering methods to display that pixel data.


License
-------

Copyright (c) 2014, Henning Thöle

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

