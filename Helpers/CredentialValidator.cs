//----------------------------------------------------------------------------------------------
// <copyright file="CredentialValidator.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace SonarQubeBootstrapper.Helpers
{
    public class CredentialValidator
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        // Test harness.
        // If you incorporate this code into a DLL, be sure to demand FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static bool ValidateCredential(Session session, string userName, string password)
        {
            SafeTokenHandle safeTokenHandle;
            bool result = false;

            try
            {
                string domainName = string.Empty;
                string userNameWithoutDomain = userName;

                if (userName.Contains(@"\"))
                {
                    domainName = userName.Substring(0, userName.IndexOf('\\'));
                    userNameWithoutDomain = userName.Substring(userName.IndexOf('\\') + 1);
                }
                
                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;

                // Call LogonUser to obtain a handle to an access token.
                result = LogonUser(userNameWithoutDomain, domainName, password,
                                   LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                                   out safeTokenHandle);

                if (false == result)
                {
                    int ret = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ret);
                }
            }
            catch (Exception ex)
            {
                session.Log("[ValidateCredential] {0} ", ex.Message);
            }

            return result;
        }
    }
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}
