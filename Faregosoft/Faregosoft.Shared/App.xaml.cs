using Faregosoft.Pages;
using Microsoft.Extensions.Logging;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Faregosoft
{
    public sealed partial class App : Application
    {
        public App()
        {
            ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
            }
#endif

#if NET5_0 && WINDOWS
            var window = new Window();
            window.Activate();
#else
            Window window = Window.Current;
#endif

            Frame rootFrame = window.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                window.Content = rootFrame;
            }

#if !(NET5_0 && WINDOWS)
            if (e.PrelaunchActivated == false)
#endif
            {
                if (rootFrame.Content == null)
                {
                    SaveParameters();
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                window.Activate();
            }
        }

        private void SaveParameters()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["ApiUrl"] = "https://faregosoftapiprep.azurewebsites.net/"; // "https://faregosoftapiprep.azurewebsites.net/";
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private static void ConfigureFilters(ILoggerFactory factory)
        {
            factory
                .WithFilter(new FilterLoggerSettings
                    {
                        { "Uno", LogLevel.Warning },
                        { "Windows", LogLevel.Warning },
                    }
                )
#if DEBUG
                .AddConsole(LogLevel.Debug);
#else
                .AddConsole(LogLevel.Information);
#endif
        }
    }
}
