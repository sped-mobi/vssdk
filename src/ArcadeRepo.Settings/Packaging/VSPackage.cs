using System;
using System.Runtime.InteropServices;
using System.Threading;
using $(RootNamespace).Options;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Packaging
{
    [Guid("75E87631-D74D-4DC7-8EC2-8CEDC72F6489")]
    [ProvideOptionPage(typeof(RepositoryGeneratorOptionPage), CategoryName, PageName, CategoryResourceID, PageNameResourceID, true)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true, RegisterUsing = RegistrationMethod.CodeBase)]
    public class $(SettingsPackageName) : AsyncPackage
    {
        public const string CategoryName = "$(OfficialName)";
        public const string PageName = "$(OfficialName) Settings";
        public const int CategoryResourceID = 0;
        public const int PageNameResourceID = 0;

    }
}
