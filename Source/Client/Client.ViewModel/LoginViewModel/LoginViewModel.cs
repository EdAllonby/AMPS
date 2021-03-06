﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Client.Model.LoginModel;
using Client.Service;
using Client.ViewModel.Commands;
using Shared;
using Shared.Configuration;
using Shared.Domain;
using Shared.Message.LoginMessage;
using Utility;

namespace Client.ViewModel.LoginViewModel
{
    /// <summary>
    /// The View Model for the Login view.
    /// </summary>
    public class LoginViewModel : ViewModel
    {
        private readonly IClientService clientService;
        private readonly ClientLogOnParser logOnParser = new();
        private bool canOpenWindow;
        private LoginModel loginModel = new();

        /// <summary>
        /// Create a new view model for the login view.
        /// </summary>
        /// <param name="serviceRegistry"></param>
        public LoginViewModel(IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            if (!IsInDesignMode)
            {
                var configManager = serviceRegistry.GetService<AppConfigManager>();

                LoginModel.IPAddress = configManager.FindStoredValue("DefaultIPAddress");
                LoginModel.Port = configManager.FindStoredValue("DefaultPort");

                clientService = serviceRegistry.GetService<IClientService>();

                clientService.BootstrapCompleted += OnClientBootstrapCompleted;
                var commandLineArgs = new List<string>(Environment.GetCommandLineArgs());

                commandLineArgs.RemoveAt(0);

                if (commandLineArgs.Count != 0)
                {
                    Log.Info("Command line arguments found, attempting to parse");

                    bool result = logOnParser.TryParseCommandLineArguments(Environment.GetCommandLineArgs(), out var loginDetails);
                    if (result)
                    {
                        AttemptLogin(loginDetails);
                    }
                }
            }
        }

        /// <summary>
        /// The underlying model.
        /// </summary>
        public LoginModel LoginModel
        {
            get => loginModel;
            set
            {
                if (Equals(value, loginModel)) return;
                loginModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Attempt to log in.
        /// </summary>
        public ICommand Login => new RelayCommand(LoginToServer, CanLogin);

        /// <summary>
        /// When the login view is requested to close.
        /// </summary>
        public static ICommand Closing
        {
            get { return new RelayCommand(() => Application.Current.Shutdown()); }
        }

        /// <summary>
        /// Fires when the login attempt was unsuccessful.
        /// </summary>
        public event EventHandler<LoginErrorEventArgs> LoginErrored;

        /// <summary>
        /// Fires when open the band chooser view is requested.
        /// </summary>
        public event EventHandler OpenBandChooserViewRequested;

        /// <summary>
        /// Fires when open the band maker view is requested.
        /// </summary>
        public event EventHandler OpenBandMakerViewRequested;

        private void AttemptLogin(LoginDetails loginDetails)
        {
            LoginResult result = clientService.LogOn(loginDetails);
            switch (result)
            {
                case LoginResult.Success:
                    Log.Debug("Waiting for client bootstrap to complete");
                    canOpenWindow = true;
                    break;

                case LoginResult.AlreadyConnected:
                    EventUtility.SafeFireEvent(LoginErrored, this, new LoginErrorEventArgs(result, $"User already connected with username: {LoginModel.Username}"));
                    break;

                case LoginResult.ServerNotFound:
                    EventUtility.SafeFireEvent(LoginErrored, this, new LoginErrorEventArgs(result, "Could not find server, check connection settings."));
                    break;

                case LoginResult.IncorrectPassword:
                    EventUtility.SafeFireEvent(LoginErrored, this, new LoginErrorEventArgs(result, "The password is incorrect. Please try again."));
                    break;

                case LoginResult.UnknownError:
                    EventUtility.SafeFireEvent(LoginErrored, this, new LoginErrorEventArgs(result, "There is a server-side error. Please report this."));
                    break;
            }
        }

        private void OnClientBootstrapCompleted(object sender, EventArgs e)
        {
            if (canOpenWindow)
            {
                OpenNextView();
            }
        }

        private void OpenNextView()
        {
            // If the user does not have a band, show them the band maker view, otherwise go to the main view.
            User user = ServiceRegistry.GetService<IClientService>().ClientUser;

            if (user.Bands.Any())
            {
                Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenBandChooserViewRequested, this));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => EventUtility.SafeFireEvent(OpenBandMakerViewRequested, this));
            }
        }

        private void LoginToServer()
        {
            bool result = logOnParser.TryParseLogonDetails(LoginModel.Username, LoginModel.Password, LoginModel.IPAddress, LoginModel.Port, out var loginDetails);
            if (result)
            {
                AttemptLogin(loginDetails);
            }
            else
            {
                MessageBox.Show("One or more entries were invalid, double check the formatting");
            }
        }

        private bool CanLogin()
        {
            return string.IsNullOrWhiteSpace(LoginModel.Error);
        }
    }
}