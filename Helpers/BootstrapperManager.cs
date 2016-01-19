//----------------------------------------------------------------------------------------------
// <copyright file="BootstrapperManager.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Wix.SonarQube;

public class BootstrapperManager : INotifyPropertyChanged
{
    private static BootstrapperManager instance;
    private AutoResetEvent autoEvent; 

    private BootstrapperManager()
    {
        this.LogVariables = new List<string>();
        this.LogVariables.Add("WixBundleLog");
        this.LogFilePath = Path.Combine(Path.GetTempPath(), "SonarQube_" + DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        this.SetupState = SetupState.Success;
    }

    public List<string> LogVariables { get; set; }

    public BootstrapperApplication Bootstrapper { get; set; }

    public LaunchAction LaunchAction { get; set; }

    public Result SetupExitState { get; set; }

    public SetupState SetupState { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public string LogFilePath { get; set; }

    protected void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public static BootstrapperManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BootstrapperManager();
            }

            return instance;
        }
    }

    public void Initialize(BootstrapperApplication bootstrapper, AutoResetEvent autoEvent)
    {
        this.Bootstrapper = bootstrapper;
        this.Bootstrapper.Error += this.OnError;
        this.Bootstrapper.ApplyComplete += this.OnApplyComplete;
        this.Bootstrapper.DetectPackageComplete += this.OnDetectPackageComplete;
        this.Bootstrapper.DetectComplete += this.OnDetectComplete;
        this.Bootstrapper.PlanComplete += this.OnPlanComplete;
        this.Bootstrapper.PlanPackageComplete += this.OnPlanPackageComplete;
        this.autoEvent = autoEvent;
        this.Bootstrapper.Engine.Detect();
    }

    private void OnError(object sender, Microsoft.Tools.WindowsInstallerXml.Bootstrapper.ErrorEventArgs e)
    {
        this.SetupExitState = Result.Cancel;
    }

    private void OnDetectComplete(object sender, DetectCompleteEventArgs e)
    {
        // Identify the logged in user here itself so that it can be consumed by custom actions later.
        this.Bootstrapper.Engine.StringVariables["CURRENTLOGGEDINUSER"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
    }

    public void ExecutePlan()
    {
        Bootstrapper.Engine.Plan(this.LaunchAction);
    }
   
    /// <summary>
    /// Method that gets invoked when the Bootstrapper ApplyComplete event is fired.
    /// This is called after a bundle installation has completed. Make sure we updated the view.
    /// </summary>
    void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
    {
        this.SetupExitState = e.Result;
    }

    /// <summary>
    /// Method that gets invoked when the Bootstrapper DetectPackageComplete event is fired.
    /// Checks the PackageId and sets the installation scenario. The PackageId is the ID
    /// specified in one of the package elements (msipackage, exepackage, msppackage,
    /// msupackage) in the WiX bundle.
    /// </summary>
    void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
    {
        if (e.PackageId == "SonarQubePackage")
        {
            if (e.State == PackageState.Absent)
            {
                this.LaunchAction = LaunchAction.Install;
            }

            else if (e.State == PackageState.Present)
            {
                this.LaunchAction = LaunchAction.Uninstall;
            }

            else if (e.State == PackageState.Obsolete)
            {
                this.SetupState = SetupState.FailedOnDowngrade;
            }

            autoEvent.Set();
        }
    }

    /// <summary>
    /// Method that gets invoked when the Bootstrapper PlanComplete event is fired.
    /// If the planning was successful, it instructs the Bootstrapper Engine to 
    /// install the packages.
    /// </summary>
    void OnPlanComplete(object sender, PlanCompleteEventArgs e)
    {
        if (e.Status >= 0)
        {
            Bootstrapper.Engine.Apply(System.IntPtr.Zero);
        }
    }

    void OnPlanPackageComplete(object sender, PlanPackageCompleteEventArgs e)
    {
        LogVariables.Add("WixBundleLog_" + e.PackageId);
        LogVariables.Add("WixBundleRollbackLog_" + e.PackageId);
    }
}
