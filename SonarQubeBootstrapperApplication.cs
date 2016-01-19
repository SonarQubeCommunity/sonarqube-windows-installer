//----------------------------------------------------------------------------------------------
// <copyright file="SonarQubeBootstrapperApplication.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System.Threading;
using System.Windows.Forms;
using Wix.SonarQube;
using Wix.SonarQube.Dialogs;

[assembly: BootstrapperApplication(typeof(SonarQubeBootstrapperApplication))]
/// <summary>
/// Bootstrapper application for SonarQube installer. This is the entry point
/// of the UI. Depending on the packages state in the machine, UI dialogs are shown.
/// </summary>
public class SonarQubeBootstrapperApplication : BootstrapperApplication
{
    private AutoResetEvent autoEvent = new AutoResetEvent(false);

    /// <summary>
    /// Entry point that is called when the bootstrapper application is ready to run.
    /// </summary>
    protected override void Run()
    {
        BootstrapperManager.Instance.Initialize(this, autoEvent);
       
        DialogManager.Instance.Add<WelcomeDialog>()
                              .Add<LicenceDialog>()
                              .Add<SetupTypeDialog>()
                              .Add<EvaluationInstallDirDialog>()
                              .Add<SqlInstallationDialog>()
                              .Add<SqlServerNewUserCredentialsDialog>()
                              .Add<SqlServerInstanceValidateDialog>()
                              .Add<SonarQubeOptionsEditDialog>()
                              .Add<ConfigureNewDatabaseInstanceDialog>()
                              .Add<ConfigureOtherDatabaseConnection>()
                              .Add<CustomInstallationSummaryDialog>()
                              .Add<MaintenanceTypeDialog>()
                              .Add<ProgressDialog>()
                              .Add<ExitDialog>();

        // Wait until package detection is completed. After package detection,
        // we'll get to know if packages are already installed in the system or not.
        autoEvent.WaitOne();

        // If packages are not installed, start from the welcome dialog,
        // otherwise show maintenance dialog.

        if (BootstrapperManager.Instance.SetupState == SetupState.Success)
        {
            if (BootstrapperManager.Instance.LaunchAction == LaunchAction.Install)
            {
                DialogManager.Instance.First();
            }
            else
            {
                DialogManager.Instance.Go<MaintenanceTypeDialog>();
            }
        }
        else if (BootstrapperManager.Instance.SetupState == SetupState.FailedOnDowngrade)
        {
            MessageBox.Show(BootstrapperResources.DowngradeMessage, BootstrapperConstants.FormalProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        Engine.Quit(0);
    }
}
