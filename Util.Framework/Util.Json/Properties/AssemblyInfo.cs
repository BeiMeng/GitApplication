#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if PORTABLE40
[assembly: AssemblyTitle("Json.NET Portable .NET 4.0")]
#elif PORTABLE
[assembly: AssemblyTitle("Json.NET Portable")]
#elif NETFX_CORE
[assembly: AssemblyTitle("Json.NET WinRT")]
#elif NET20
[assembly: AssemblyTitle("Json.NET .NET 2.0")]
[assembly: AllowPartiallyTrustedCallers]
#elif NET35
[assembly: AssemblyTitle("Json.NET .NET 3.5")]
[assembly: AllowPartiallyTrustedCallers]
#elif NET40
[assembly: AssemblyTitle("Json.NET .NET 4.0")]
[assembly: AllowPartiallyTrustedCallers]
#else
[assembly: AssemblyTitle( "Util.Json" )]
[assembly: AllowPartiallyTrustedCallers]
#endif



[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany( "何镇汐" )]
[assembly: AssemblyProduct( "Util.Json" )]
[assembly: AssemblyCopyright( "Copyright 何镇汐 2015" )]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if !(PORTABLE40 || PORTABLE)
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM componenets.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("9ca358aa-317b-4925-8ada-4a29e943a363")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyFileVersion("6.0.4.17603")]
[assembly: CLSCompliant(true)]
