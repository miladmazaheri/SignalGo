﻿using MvvmGo.ViewModels;
using SignalGo.Publisher.Engines.Models;
using SignalGo.Publisher.Models;
using SignalGo.Publisher.Views.Extra;

namespace SignalGo.Publisher.Helpers
{
    /// <summary>
    /// Do Authorization before Executing Command on protected Server's, using Interactive Dialog And automatic Method's
    /// </summary>
    public class CommandAuthenticator : BaseViewModel
    {
        private static int retryAttemp { get; set; } = 0;
        /// <summary>
        /// Interactive Authorization on the specified server
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        public static bool Authorize(ref ServerInfo serverInfo)
        {
        GetThePass:
            if (retryAttemp > 2)
            {
                retryAttemp = 0;
                return false;
            }
            InputDialogWindow inputDialog = new InputDialogWindow($"Please enter your secret for Server", "Access Control", serverInfo.ServerName);
            if (inputDialog.ShowDialog() == true)
            {
                if (serverInfo.ProtectionPassword != PasswordEncoder.ComputeHash(inputDialog.Answer))
                {
                    if (System.Windows.Forms.MessageBox.Show("password does't match!", "Access Denied", System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Retry)
                    {
                        retryAttemp++;
                        goto GetThePass;
                    }
                    else
                    {
                        serverInfo.IsChecked = false;
                        serverInfo.ServerLastUpdate = "Access Denied!";
                    }
                }
            }
            else return false;
            // add to authenticated server's list
            ServerInfo.Servers.Add(serverInfo.Clone());

            return true;
        }

    }
}
