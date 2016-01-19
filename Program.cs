//----------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Deployment.WindowsInstaller;
using SonarQubeBootstrapper.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Windows.Forms;
using Wix.SonarQube;
using WixSharp;
using WixSharp.Bootstrapper;

public class Program
{
    // A timeout of 1/2 min for the processes to exit which we are spawning
    // to complete different tasks.
    private static int ProcessExitWaitTime = 500 * 60;

    public static void Main(string[] args)
    {
        string sonarQubeMsi = BuildSonarQubeMsi();

        string sqlExpressMsiX64 = BuildSqlExpressMsi(ArchitectureType.X64, new Guid("c2dd93c3-bf14-41ef-a87d-2b75a195b447"));
        CreateBundle(ArchitectureType.X64, sonarQubeMsi, sqlExpressMsiX64, new Guid("6f330b47-2577-43ad-9095-1861bb25844a"));

        string sqlExpressMsiX86 = BuildSqlExpressMsi(ArchitectureType.X86, new Guid("A6FF85E3-476D-47F3-A2D1-B5C87E38213E"));
        CreateBundle(ArchitectureType.X86, sonarQubeMsi, sqlExpressMsiX86, new Guid("966D9B0F-7801-47D4-BA0A-FF18B1398B62"));
    }

    /// <summary>
    /// Builds setup.exe for SonarQube installation
    /// </summary>
    static void CreateBundle(string archType, string sonarQubeMsi, string sqlExpressMsi, Guid upgradeCode)
    {
        string bundleName = string.Format("{0} (x{1})", BootstrapperConstants.FormalProductName, archType);
        string outFileName = string.Format(BootstrapperConstants.SonarQubeProductName + "-x{0}.exe", archType);
        var bootstrapper = new Bundle(bundleName,
                                      new PackageGroupRef("NetFx40Web"),
                                      new MsiPackage(sqlExpressMsi)
                                      {
                                          Id = "SqlExpressPackage",
                                          InstallCondition = "INSTALLSQL=\"TRUE\""
                                      },
                                      new MsiPackage(sonarQubeMsi)
                                      {
                                          Id = "SonarQubePackage",
                                          MsiProperties = "INSTALLDIR=[INSTALLDIR];SETUPTYPE=[SETUPTYPE];PORT=[PORT];INSTANCE=[INSTANCE];"
                                                          + "SQLAUTHTYPE=[SQLAUTHTYPE];INSTALLSERVICE=[INSTALLSERVICE];DATABASEUSERNAME=[DATABASEUSERNAME];"
                                                          + "DATABASEPASSWORD=[DATABASEPASSWORD];DATABASENAME=[DATABASENAME];"
                                                          + "CURRENTLOGGEDINUSER=[CURRENTLOGGEDINUSER];DBENGINE=[DBENGINE];DBPORT=[DBPORT]"
                                      });

        bootstrapper.Version = new Version(BootstrapperConstants.ProductVersion);
        bootstrapper.UpgradeCode = upgradeCode;
        bootstrapper.Application = new ManagedBootstrapperApplication("%this%");
        bootstrapper.PreserveTempFiles = true;
        bootstrapper.StringVariablesDefinition = "INSTALLDIR=" + BootstrapperConstants.DefaultInstallDir
                                                 + ";INSTALLSQL=FALSE;SETUPTYPE=EVALUATION;PORT=9000;"
                                                 + "SQLAUTHTYPE=WINDOWS;INSTALLSERVICE=YES;INSTANCE=LOCALHOST;"
                                                 + "DATABASENAME=sonarqubedb;DATABASEUSERNAME=sonarqube;DATABASEPASSWORD=sonarqube;"
                                                 + "CURRENTLOGGEDINUSER=default;DBENGINE=H2;DBPORT=1433";
        bootstrapper.IconFile = @"Resources\setup.ico";
        bootstrapper.Build(outFileName);        
    }

    /// <summary>
    /// Builds msi for SonarQube installation
    /// </summary>
    static string BuildSonarQubeMsi()
    {
        var project = new Project(
                             // Project name
                             "SonarQube",

                             // Default installation location.
                             new Dir(BootstrapperConstants.DefaultInstallDir,
                                     // File to be copied at installation location.
                                     new Files(@"SonarQube\*.*"),
                                     new WixSharp.File(new Id("sqljdbc64"), @"sqlauth\x64\sqljdbc_auth.dll")
                                     {
                                          Condition = "Msix64"
                                     },
                                     new WixSharp.File(new Id("sqljdbc32"), @"sqlauth\x86\sqljdbc_auth.dll")
                                     {
                                         Condition = "NOT Msix64"
                                     }),

                             new Dir(@"%Desktop%",
                                     new ExeFileShortcut("SonarQube", string.Format(@"[INSTALLDIR]\{0}\bin\windows-x86-64\StartSonar.bat",
                                                                                    BootstrapperConstants.SonarQubeProductName), "")
                                     {
                                         Condition = new Condition("INSTALLSERVICE=\"NO\" AND Msix64"),
                                         WorkingDirectory = "[INSTALLDIR]"
                                     },
                                     new ExeFileShortcut("SonarQube", string.Format(@"[INSTALLDIR]\{0}\bin\windows-x86-32\StartSonar.bat",
                                                                                    BootstrapperConstants.SonarQubeProductName), "")
                                     {
                                         Condition = new Condition("INSTALLSERVICE=\"NO\" AND NOT Msix64"),
                                         WorkingDirectory = "[INSTALLDIR]"
                                     }),

                             // Files not to be installed but required to be carried with MSI. These files are unpacked
                             // as and when required in user's temp folder and removed after usage.
                             new Binary(new Id("LicenseFile"), @"Resources\License.rtf"),
                             new Binary(new Id("SonarQubeLogo"), @"Resources\sonarqube.png"),
                             new Binary(new Id("NtRights"), @"tools\ntrights.exe"),

                             // Properties to be carried to Execute sequence from UI sequence with their values persisted.
                             new Property("SETUPTYPE", SetupType.Evaluation),
                             new Property("PORT", BootstrapperConstants.DefaultSonarQubePort),
                             new Property("INSTANCE", BootstrapperConstants.DefaultSqlInstance),
                             new Property("SQLAUTHTYPE", AuthenticationType.Sql),
                             new Property("INSTALLSERVICE", "YES"),
                             new Property("DATABASEUSERNAME", "sonarqube"),
                             new Property("DATABASEPASSWORD", "sonarqube"),
                             new Property("DATABASENAME", "sonarqubedb"),
                             new Property("CURRENTLOGGEDINUSER", "default"),
                             new Property("DBENGINE", DbEngine.H2),
                             new Property("DBPORT", "1433")
                       )
        {
            InstallScope = InstallScope.perMachine
        };

        project.GUID = new Guid("c2dd93c3-bf14-41ef-a87d-2b75a195b446");

        // Update InstallCustomActionsCount and UninstallCustomActionsCount in constants to
        // reflect the right progress during installation.
        // Progress is updated every time a message is received from a custom action. 

        ElevatedManagedAction rollbackUnzipSonarQube = new ElevatedManagedAction(new Id("Action1.1"), "RollbackUnzipSonarQube", Return.ignore, When.After, Step.InstallFiles, Condition.NOT_Installed)
                                                        {
                                                            UsesProperties = "INSTALLDIR",
                                                            Execute = Execute.rollback
                                                        };

        project.Actions = new WixSharp.Action[]
        {
                new ManagedAction(new Id("Action1.0"), "ChangeUserPrivilegesToLogOnAsService", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                {
                    Impersonate = true,
                    UsesProperties = "SETUPTYPE,CURRENTLOGGEDINUSER"
                },
                new SetPropertyAction(rollbackUnzipSonarQube.Id, "INSTALLDIR=[INSTALLDIR]", Return.check, When.After, Step.InstallInitialize, Condition.NOT_Installed),
                rollbackUnzipSonarQube,
                new ElevatedManagedAction(new Id("Action1.2"), "UnzipSonarQube", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed),
                new ElevatedManagedAction(new Id("Action1.3"), "SetSqlAuthInEnvironmentPath", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                {
                    Impersonate = true,
                    UsesProperties = "SETUPTYPE"
                },
                new ElevatedManagedAction(new Id("Action1.4"), "SetupSonarQubeConfiguration", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                {
                    UsesProperties = "SETUPTYPE,PORT,INSTANCE,SQLAUTHTYPE,INSTALLSERVICE,DATABASEUSERNAME,DATABASEPASSWORD,DATABASENAME,DBENGINE,DBPORT"
                }
        };

        ElevatedManagedAction rollbackInstallSonarQubeService = new ElevatedManagedAction(new Id("Action1.5"), "RollbackInstallSonarQubeService", Return.ignore, When.After, Step.InstallFiles, Condition.NOT_Installed)
        {
            UsesProperties = "INSTALLDIR",
            Execute = Execute.rollback
        };

        WixSharp.Action[] actions = new WixSharp.Action[]
            {
                new SetPropertyAction(rollbackInstallSonarQubeService.Id, "INSTALLDIR=[INSTALLDIR]", Return.check, When.After, Step.InstallInitialize, Condition.NOT_Installed),
                rollbackInstallSonarQubeService,
                new ElevatedManagedAction(new Id("Action1.6"), "InstallSonarQubeService", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                {
                    UsesProperties = "SETUPTYPE,SQLAUTHTYPE,INSTALLSERVICE,CURRENTLOGGEDINUSER,DBENGINE,DBPORT,PORT",
                    Condition = new Condition("(NOT Installed AND INSTALLSERVICE=\"YES\")")
                },
                new ElevatedManagedAction(new Id("Action1.7"), "StartSonarBatch", Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                {
                    UsesProperties = "INSTALLSERVICE",
                    Condition = new Condition("(NOT Installed AND INSTALLSERVICE=\"NO\")"),
                    Impersonate = true
                },
                new ElevatedManagedAction(new Id("Action1.8"), "UninstallSonarQubeService", Return.check, When.After, Step.RemoveFiles, Condition.Installed)
                {
                    UsesProperties = "INSTALLSERVICE",
                    Condition = new Condition("(Installed AND INSTALLSERVICE=\"YES\")")
                },
                new ElevatedManagedAction(new Id("Action1.9"), "CleanSonarQubeInstallation", Return.check, When.After, Step.RemoveFiles, Condition.Installed)
            };

        project.Actions = project.Actions.Concat<WixSharp.Action>(actions).ToArray<WixSharp.Action>();
        project.Version = new Version(BootstrapperConstants.ProductVersion);
        project.MajorUpgrade = new MajorUpgrade()
        {
             DowngradeErrorMessage = "A later version of SonarQube is already installed on this machine",
             Schedule = UpgradeSchedule.afterInstallInitialize
        };
        
        string msi = project.BuildMsi();

        int exitCode = WixSharp.CommonTasks.Tasks.DigitalySign(msi,
                                                               "SonarQubeInstaller.pfx",
                                                               "http://timestamp.verisign.com/scripts/timstamp.dll",
                                                               "password");

        if (exitCode != 0)
        {
            Console.WriteLine("Could not sign the MSI file.");
        }
        else
        {
            Console.WriteLine("The MSI file was signed successfully.");
        }

        return msi;
    }

    /// <summary>
    /// Builds msi for SQL installation. This msi just carries the sql express setup and
    /// expands it on installation.
    /// </summary>
    static string BuildSqlExpressMsi(string archType, Guid projectGuid)
    {
        string projectName = string.Format("SonarQube_SQLx{0}", archType);
        string sqlInstaller = string.Format(@"sqlexpress\SQLEXPR_x{0}_ENU.exe", archType);
        string binaryId = string.Format("SqlExpress{0}",archType);
        var project = new Project(
                             // Project name
                             projectName,

                             // Default installation location.
                             new Dir(BootstrapperConstants.DefaultInstallDir),

                             // Files not to be installed but required to be carried with MSI. These files are unpacked
                             // as and when required in user's temp folder and removed after usage.
                             new Binary(new Id(binaryId), sqlInstaller),

                             new Binary(new Id("ConfigurationFile"), @"sqlexpress\ConfigurationFile.ini")
                       )
        {
            InstallScope = InstallScope.perMachine
        };

        project.GUID = projectGuid;
        project.Version = new Version(BootstrapperConstants.ProductVersion);
        project.MajorUpgrade = new MajorUpgrade()
        {
            DowngradeErrorMessage = "A later version of SonarQube is already installed on this machine",
            Schedule = UpgradeSchedule.afterInstallInitialize
        };

        project.Actions = new WixSharp.Action[]
        {
                new ManagedAction("ExtractSqlExpress", Return.check, When.After, Step.InstallFinalize, Condition.NOT_Installed)
        };

        string msi = project.BuildMsi();
        
        int exitCode = WixSharp.CommonTasks.Tasks.DigitalySign(msi,
                                                               "SonarQubeInstaller.pfx",
                                                               "http://timestamp.verisign.com/scripts/timstamp.dll",
                                                               "password");

        if (exitCode != 0)
        {
            Console.WriteLine("Could not sign the MSI file.");
        }
        else
        {
            Console.WriteLine("The MSI file was signed successfully.");
        }

        return msi;
    }

    public class CustomActions
    {
#region Custom actions for installation
        /// <summary>
        /// Appends the path of Sql Auth dll to the environment variable PATH for
        /// both machine and the current logged on user.
        /// </summary>
        [CustomAction]
        public static ActionResult ChangeUserPrivilegesToLogOnAsService(Session session)
        {
            return session.HandleErrors(() =>
            {
                string ntrightsPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"ntrights.exe");
                string setupType = session.Property("SETUPTYPE");
                string currentLoggedInUser = session.Property("CURRENTLOGGEDINUSER");

                if (SetupType.Express.Equals(setupType, StringComparison.InvariantCultureIgnoreCase))
                {
                    SendProgressMessageToBA(session, "Granting user log on as service permission", 1);

                    session.SaveBinary("NtRights", ntrightsPath);

                    try
                    {
                        Process ntRightsProcess = new Process();

                        ntRightsProcess.StartInfo.FileName = ntrightsPath;
                        ntRightsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        ntRightsProcess.StartInfo.CreateNoWindow = true;
                        ntRightsProcess.StartInfo.UseShellExecute = false;
                        ntRightsProcess.StartInfo.Arguments = "+r SeServiceLogonRight -u \"" + currentLoggedInUser + "\"";

                        ntRightsProcess.Start();

                        ntRightsProcess.WaitForExit(ProcessExitWaitTime);

                        // This needs to be done as this process can wait for standard input in case of failure.
                        // We nicely kill this process ourselves in this case to not stop the setup from progressing.
                        if (!ntRightsProcess.HasExited)
                        {
                            ntRightsProcess.Kill();
                        }

                        if (ntRightsProcess.ExitCode == 0)
                        {
                            // [TODO] Add strings for localization
                            SendMessageBoxMessageToBA(session, string.Format("The account {0} has been granted the Log On As A Service right.",
                                                                    currentLoggedInUser));
                        }
                        else
                        {
                            SendMessageBoxMessageToBA(session, string.Format("The account {0} couldn't be grated the Log On As A Service right.",
                                                                    currentLoggedInUser));
                        }
                    }
                    catch (Exception e)
                    {
                        session.Log("[ChangeUserPrivilegesToLogOnAsService] {0}", e.Message);
                    }
                    finally
                    {
                        // We can safely try to delete this file here as Delete() doesn't throw exception for
                        // file not found.
                        System.IO.File.Delete(ntrightsPath);
                    }
                }
            });
        }

        /// <summary>
        /// Extracts the bundled SQL express setup to temp location
        /// </summary>
        [CustomAction]
        public static ActionResult ExtractSqlExpress(Session session)
        {
            return session.HandleErrors(() =>
            {
                SendProgressMessageToBA(session, "Extracting Sql Express setup", 2);

                string setupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"SqlExpress.exe");
                string configPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"ConfigurationFile.ini");
                string sqlComponentName = "SqlExpress32";

                if (Environment.Is64BitOperatingSystem)
                {
                    sqlComponentName = "SqlExpress64";
                }

                session.SaveBinary(sqlComponentName, setupPath);
                session.SaveBinary("ConfigurationFile", configPath);
            });
        }

        /// <summary>
        /// Unpacks the installed SonarQube zip file
        /// </summary>
        [CustomAction]
        public static ActionResult UnzipSonarQube(Session session)
        {            
            try
            {
                SendProgressMessageToBA(session, "Unzipping SonarQube 5.2", 3);
                
                string archivePath = Path.Combine(session.Property("INSTALLDIR"), BootstrapperConstants.SonarQubeProductName + ".zip");
                string extractPath = session.Property("INSTALLDIR");

                ZipFile.ExtractToDirectory(archivePath, extractPath);
                System.IO.File.Delete(archivePath);                
                return ActionResult.Success;
            }
            catch (Exception e)
            {
                session.Log("[UnzipSonarQube] {0}", e.Message);
                return ActionResult.Failure;
            }
        }

        /// <summary>
        /// Deletes SonarQube Installation Folder
        /// </summary>
        [CustomAction]
        public static ActionResult RollbackUnzipSonarQube(Session session)
        {
            try
            {
                string extractPath = session.CustomActionData["INSTALLDIR"];
                Directory.Delete(extractPath, true);
                return ActionResult.Success;
            }
            catch (Exception e)
            {
                session.Log("[RollbackUnzipSonarQube] {0}", e.Message);
                return ActionResult.Success;
            }
        }

        /// <summary>
        /// Appends the path of Sql Auth dll to the environment variable PATH for
        /// both machine and the current logged on user.
        /// </summary>
        [CustomAction]
        public static ActionResult SetSqlAuthInEnvironmentPath(Session session)
        {
            return session.HandleErrors(() =>
            {
                SendProgressMessageToBA(session, "Configuring SQL authentication dll", 4);

                string setupType = session.Property("SETUPTYPE");

                if (SetupType.Express.Equals(setupType, StringComparison.InvariantCultureIgnoreCase))
                {
                    SetPathToSqlAuthDllForTarget(session, EnvironmentVariableTarget.Machine);
                    SetPathToSqlAuthDllForTarget(session, EnvironmentVariableTarget.User);
                }
            });
        }

        /// <summary>
        /// Updates sonarqube and wrapper configuration files.
        /// Optionally, installs sonarqube service.
        /// </summary>
        [CustomAction]
        public static ActionResult SetupSonarQubeConfiguration(Session session)
        {
            return session.HandleErrors(() =>
            {
                SendProgressMessageToBA(session, "Configuring SonarQube", 5);

                // The default temp directory can be only accessed by SYSTEM account. Hence we change
                // the temp directory to the one present in SonarQube installation folder.
                try
                {
                    System.IO.File.AppendAllText(Path.Combine(session.Property("INSTALLDIR"), BootstrapperConstants.SonarQubeProductName, @"conf\wrapper.conf"),

                                                 Environment.NewLine
                                                 + "wrapper.java.additional.2=-Djava.io.tmpdir=../../temp/"
                                                 + Environment.NewLine);
                }
                catch (Exception e)
                {
                    session.Log("[SetupSonarQubeConfiguration] {0}", e.Message);
                }

                try
                {
                    string setupType = session.Property("SETUPTYPE");

                    if (setupType.Equals(SetupType.Express, StringComparison.InvariantCultureIgnoreCase)
                        || setupType.Equals(SetupType.Custom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Update SonarQube configuration file
                        session.Log("[SetupSonarQubeConfiguration] Updating sonar.properties for {0} setup.", setupType);
                        SonarConfigurationFileEditor.UpdateSonarPropertiesFileForSql(session);
                    }
                }
                catch (Exception e)
                {
                    session.Log("[SetupSonarQubeConfiguration] {0}", e.Message);
                }
            });
        }

        [CustomAction]
        public static ActionResult InstallSonarQubeService(Session session)
        {            
            try
            {
                // Install SonarQube NT service and start it on success. 
                if (InstallSonarQubeNTService(session))
                {
                    SendProgressMessageToBA(session, "Installing SonarQube service", 6);

                    if (ChangeUserAccountOfServiceForWindowsAuthentication(session))
                    {
                        if (StartSonarQubeNTService(session))
                        {
                            return ActionResult.Success;
                        }
                        else
                        {
                            //[TODO] Localization
                            MessageBox.Show("Failed to start SonarQube service! Installation will be rolled back");
                            return ActionResult.Failure;
                        }
                    }
                    else
                    {
                        //[TODO] Localization
                        MessageBox.Show("Windows User Authentication failed! Installation will be rolled back");
                        return ActionResult.Failure;
                    }
                }
                else
                {
                    //[TODO] Localization
                    MessageBox.Show("Failed to install SonarQube service! Installation will be rolled back");
                    return ActionResult.Failure;
                }
            }
            catch(Exception e)
            {
                session.Log("[InstallSonaQubeService] {0}", e.Message);
                return ActionResult.Failure;
            }
        }

        [CustomAction]
        public static ActionResult StartSonarBatch(Session session)
        {
            try
            {
                SendProgressMessageToBA(session, "Starting SonarQube process", 6);

                Process sonarServerBatchProcess = new Process();
                JavaVersionInformation information = JavaVersionHelper.GetJavaVersionInformation();

                sonarServerBatchProcess.StartInfo.FileName = session.Property("INSTALLDIR") +
                                                                string.Format(@"\{0}\bin\windows-x86-{1}\StartSonar.bat",
                                                                            BootstrapperConstants.SonarQubeProductName,
                                                                            (int)information.Architecture);

                sonarServerBatchProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                sonarServerBatchProcess.StartInfo.UseShellExecute = false;
                sonarServerBatchProcess.Start();                
                return ActionResult.Success;
            }
            catch (Exception e)
            {
                session.Log("[StartSonarBatch] {0}", e.Message);
                return ActionResult.Failure;
            }            
        }       

        /// <summary>
        /// Uninstalls SonarQube service
        /// </summary>
        [CustomAction]
        public static ActionResult RollbackInstallSonarQubeService(Session session)
        {
            try
            {                
                StopSonarQubeNTService(session);
                UninstallSonarQubeNTService(session);
                return ActionResult.Success;
            }
            catch (Exception e)
            {
                session.Log("[RollbackInstallSonarQubeService] {0}", e.Message);
                return ActionResult.Success;
            }
        }
#endregion

#region Custom actions for uninstallation
        /// <summary>
        /// Stops the sonarqube service and uninstalls it.
        /// </summary>
        [CustomAction]
        public static ActionResult UninstallSonarQubeService(Session session)
        {
            return session.HandleErrors(() =>
            {
                try
                {
                    SendProgressMessageToBA(session, "Uninstalling SonarQube", 1);

                    StopSonarQubeNTService(session);

                    UninstallSonarQubeNTService(session);
                }
                catch (Exception e)
                {
                    session.Log("[UninstallService] {0}", e.Message);
                }
            });
        }

        /// <summary>
        /// Removes the installed folder from installation location.
        /// </summary>
        [CustomAction]
        public static ActionResult CleanSonarQubeInstallation(Session session)
        {
            return session.HandleErrors(() =>
            {
                SendProgressMessageToBA(session, "Cleaning SonarQube", 2);

                string extractPath = session.Property("INSTALLDIR");

                try
                {
                    Directory.Delete(extractPath, true);
                }
                catch (Exception e)
                {
                    session.Log("[CleanSonarQubeInstallation] {0}", e.Message);
                }
            });
        }
#endregion
    }

    /// <summary>
    /// Installs SonarQube NT service.
    /// </summary>
    public static bool InstallSonarQubeNTService(Session session)
    {
        bool result = true;
        Process installServiceProcess = new Process();
        JavaVersionInformation information = JavaVersionHelper.GetJavaVersionInformation();

        installServiceProcess.StartInfo.FileName = session.Property("INSTALLDIR") +
                                                   string.Format(@"\{0}\bin\windows-x86-{1}\InstallNTService.bat",
                                                                 BootstrapperConstants.SonarQubeProductName,
                                                                 (int)information.Architecture);

        installServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        installServiceProcess.StartInfo.CreateNoWindow = true;
        installServiceProcess.StartInfo.UseShellExecute = false;
        installServiceProcess.Start();

        // We have to set the wait time for this process as in case of failures, batch file
        // prompts user about the failure and waits for the user response (key hit) to exit.
        installServiceProcess.WaitForExit(ProcessExitWaitTime);

        // This needs to be done as this batch file can wait for standard input in case of failure.
        // We nicely kill this process ourselves in this case to not stop the setup from progressing.
        if (!installServiceProcess.HasExited)
        {
            installServiceProcess.Kill();
        }

        if (installServiceProcess.ExitCode != 0)
        {
            session.Log("[InstallSonarQubeNTService] Process exited with {0}", installServiceProcess.ExitCode);
            result = false;
        }

        return result;
    }

    /// <summary>
    /// Changes installed SonarQube service's log on user to current logged on user
    /// if windows authentication is chosen.
    /// </summary>
    public static bool ChangeUserAccountOfServiceForWindowsAuthentication(Session session)
    {
        string authType = session.Property("SQLAUTHTYPE");
        string setupType = session.Property("SETUPTYPE");
        string currentLoggedInUser = session.Property("CURRENTLOGGEDINUSER");

        bool success = true;

        // If authentication type is Windows, we need to change the service log on to current user.
        // For SQL auth, it works with System log on which is the default one.
        if (AuthenticationType.Windows.Equals(authType, StringComparison.InvariantCultureIgnoreCase) &&
            SetupType.Express.Equals(setupType, StringComparison.InvariantCultureIgnoreCase))
        {
            try
            {
                string queryString = "SELECT * FROM WIN32_SERVICE WHERE DISPLAYNAME='SonarQube'";

                ObjectQuery oQuery = new ObjectQuery(queryString);
                ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(oQuery);
                ManagementObjectCollection objectCollection = objectSearcher.Get();

                foreach (ManagementObject mObject in objectCollection)
                {
                    string serviceName = mObject.GetPropertyValue("Name") as string;
                    string fullServiceName = "Win32_Service.Name='" + serviceName + "'";
                    ManagementObject mo = new ManagementObject(fullServiceName);

                    string username = currentLoggedInUser;
                    string password = string.Empty;

                    // Ask current logged on user's password
                    if (!WindowsAuthenticationHelper.PromptForPassword(session, username, out password))
                    {
                        // [TODO] Localization
                        SendMessageBoxMessageToBA(session, "Authentication failed. Service may not run as expected.");
                        success = false;                        
                    }
                    else
                    {
                        mo.InvokeMethod("Change", new object[] { null, null, null, null, null, null, username, password, null, null, null });
                    }
                }
            }
            catch (Exception e)
            {
                session.Log("[ChangeUserAccountForService] {0}", e.Message);
            }
        }
        return success;
    }
    
    /// <summary>
    /// Starts the installed SonarQube service.
    /// </summary>
    public static bool StartSonarQubeNTService(Session session)
    {
        bool result = true;

        Process runServiceProcess = new Process();

        JavaVersionInformation information = JavaVersionHelper.GetJavaVersionInformation();

        runServiceProcess.StartInfo.FileName = session.Property("INSTALLDIR") +
                                               string.Format(@"\{0}\bin\windows-x86-{1}\StartNTService.bat",
                                                             BootstrapperConstants.SonarQubeProductName,
                                                             (int)information.Architecture);

        runServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        runServiceProcess.StartInfo.CreateNoWindow = true;
        runServiceProcess.StartInfo.UseShellExecute = false;

        runServiceProcess.Start();

        // We have to set the wait time for this process as in case of failures, batch file
        // prompts user about the failure and waits for the user response (key hit) to exit.
        runServiceProcess.WaitForExit(ProcessExitWaitTime);

        // This needs to be done as this batch file can wait for standard input in case of failure.
        // We nicely kill this process ourselves in this case to not stop the setup from progressing.
        if (!runServiceProcess.HasExited)
        {
            runServiceProcess.Kill();
        }

        if (runServiceProcess.ExitCode != 0)
        {
            session.Log("[StartSonarQubeNTService] Process exited with {0}", runServiceProcess.ExitCode);
            result = false;
        }

        return result;
    }

    /// <summary>
    /// Stops the running instance of SonarQube service.
    /// </summary>
    public static bool StopSonarQubeNTService(Session session)
    {
        bool result = true;
        Process stopServiceProcess = new Process();
        JavaVersionInformation information = JavaVersionHelper.GetJavaVersionInformation();
        
        stopServiceProcess.StartInfo.FileName = session.Property("INSTALLDIR") +
                                                string.Format(@"\{0}\bin\windows-x86-{1}\StopNTService.bat",
                                                              BootstrapperConstants.SonarQubeProductName,
                                                              (int)information.Architecture);

        stopServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        stopServiceProcess.StartInfo.CreateNoWindow = true;
        stopServiceProcess.StartInfo.UseShellExecute = false;
        stopServiceProcess.Start();

        // We have to set the wait time for this process as in case of failures, batch file
        // prompts user about the failure and waits for the user response (key hit) to exit.
        stopServiceProcess.WaitForExit(ProcessExitWaitTime);

        // This needs to be done as this batch file can wait for standard input in case of failure.
        // We nicely kill this process ourselves in this case to not stop the setup from progressing.
        if (!stopServiceProcess.HasExited)
        {
            stopServiceProcess.Kill();
        }

        if (stopServiceProcess.ExitCode != 0)
        {
            session.Log("[StopSonarQubeNTService] Process exited with {0}", stopServiceProcess.ExitCode);
            result = false;
        }

        return result;
    }

    /// <summary>
    /// Uninstalls the installed SonarQube service.
    /// </summary>
    public static bool UninstallSonarQubeNTService(Session session)
    {
        bool result = true;
        
        Process uninstallServiceProcess = new Process();

        JavaVersionInformation information = JavaVersionHelper.GetJavaVersionInformation();

        uninstallServiceProcess.StartInfo.FileName = session.Property("INSTALLDIR") +
                                                     string.Format(@"\{0}\bin\windows-x86-{1}\UninstallNTService.bat",
                                                                   BootstrapperConstants.SonarQubeProductName,
                                                                   (int)information.Architecture);

        uninstallServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        uninstallServiceProcess.StartInfo.CreateNoWindow = true;
        uninstallServiceProcess.StartInfo.UseShellExecute = false;
        uninstallServiceProcess.Start();

        // We have to set the wait time for this process as in case of failures, batch file
        // prompts user about the failure and waits for the user response (key hit) to exit.
        uninstallServiceProcess.WaitForExit(ProcessExitWaitTime);

        // This needs to be done as this batch file can wait for standard input in case of failure.
        // We nicely kill this process ourselves in this case to not stop the setup from progressing.
        if (!uninstallServiceProcess.HasExited)
        {
            uninstallServiceProcess.Kill();
        }

        if (uninstallServiceProcess.ExitCode != 0)
        {
            session.Log("[UninstallSonarQubeNTService] Process exited with {0}", uninstallServiceProcess.ExitCode);
            result = false;
        }

        return result;
    }

    public static bool SetPathToSqlAuthDllForTarget(Session session, EnvironmentVariableTarget target)
    {
        // Set the path for sql auth dll so that SonarQube service can find it for sql connection

        string pathVal = Environment.GetEnvironmentVariable("Path", target);
        string pathToSqlAuthDll = string.Empty;

        // If Path variable's value is null, we need to check if we're remotely connected to the machine
        // If yes, we return without doing anything otherwise log an error.
        if (pathVal == null)
        {
            string sessionName = Environment.GetEnvironmentVariable("SessionName", EnvironmentVariableTarget.Machine);

            if (sessionName != null &&
                sessionName.StartsWith("RDP"))
            {
                session.Log("[SetPathToSqlAuthDllForTarget] Remotely connected to machine {0}", sessionName);

                return true;
            }
            else
            {
                session.Log("[SetPathToSqlAuthDllForTarget] Couldn't get path environment variable's value");

                return false;
            }
        }

        pathToSqlAuthDll = session.Property("INSTALLDIR");
        
        if (!pathVal.Contains(pathToSqlAuthDll))
        {
            Environment.SetEnvironmentVariable("Path",
                                               Environment.GetEnvironmentVariable("Path", target)
                                               + ";" + pathToSqlAuthDll,
                                               target);
        }

        return true;
    }

    public static void SendProgressMessageToBA(Session session, string message, int progress)
    {
        Record messageRecord = new Record(4);
        messageRecord.SetInteger(1, (int)MessageId.Progress);
        messageRecord.SetString(2, message);
        messageRecord.SetInteger(3, progress);

        session.Message(InstallMessage.Warning, messageRecord);
    }

    public static void SendMessageBoxMessageToBA(Session session, string message)
    {
        Record messageRecord = new Record(3);
        messageRecord.SetInteger(1, (int)MessageId.MessageBox);
        messageRecord.SetString(2, message);
        
        session.Message(InstallMessage.Warning, messageRecord);
    }
}
