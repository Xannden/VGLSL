﻿using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Xannden.VSGLSL.Data;

namespace Xannden.VSGLSL.Packages
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[ProvideService(typeof(GLSLLanguageInfo))]
	[ProvideLanguageService(typeof(GLSLLanguageInfo), GLSLConstants.Name, 106, EnableLineNumbers = true)]
	[ProvideLanguageExtension(typeof(GLSLLanguageInfo), ".glsl")]
	[Guid(PackageGuidString)]
	public sealed class GLSLPackage : Package
	{
		/// <summary>
		/// GLSLPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "40d37d04-e60a-4b6e-8390-a2055347798d";

		private GLSLLanguageInfo languageInfo;

		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			this.languageInfo = new GLSLLanguageInfo();
			((IServiceContainer)this).AddService(typeof(GLSLLanguageInfo), this.languageInfo, true);
		}

		#endregion Package Members
	}
}