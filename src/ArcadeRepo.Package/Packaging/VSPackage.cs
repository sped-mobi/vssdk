// -----------------------------------------------------------------------
// <copyright file="$(PackageName).cs" company="$(RepoOrg)">
//     Copyright © 2019 $(RepoOrg). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using $(RootNamespace).Commands;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace $(RootNamespace).Packaging
{
    [Guid(GuidSymbols.PackageString)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true, RegisterUsing = RegistrationMethod.CodeBase)]
public class $(PackageName) : AsyncPackage, IVsInstalledProduct
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
{
    await base.InitializeAsync(cancellationToken, progress);

    await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

    VSLogger.Initialize(this, "$(OfficialName) Output");

    var service = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
    var handler = new CommandHandler();
    CommandRegistrar.RegisterCommands(service, handler);
}

public int IdBmpSplash(out uint pIdBmp)
{
    pIdBmp = 0U;
    return VSConstants.S_OK;
}

public int OfficialName(out string pbstrName)
{
    pbstrName = "$(OfficialName)";
    return VSConstants.S_OK;
}

public int ProductID(out string pbstrPID)
{
    pbstrPID = "$(ProductID)";
    return VSConstants.S_OK;
}

public int ProductDetails(out string pbstrProductDetails)
{
    pbstrProductDetails = "$(ProductDetails)";
    return VSConstants.S_OK;
}

public int IdIcoLogoForAboutbox(out uint pIdIco)
{
    pIdIco = 0U;
    return VSConstants.S_OK;
}
}
}
