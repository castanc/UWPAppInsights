using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;

namespace AppInsights2
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        public static string appInsightsKey = "a994763b-c74f-4a3a-a673-bd9bae9e43d8";
        public static Microsoft.ApplicationInsights.TelemetryClient telemetry;
        public static bool TelemetryEnabled = false;

        /// <summary>
        /// Initialize telemetry for ApplicationInsights in Azure
        /// </summary>
        public static void InitializeTelemetry()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                WindowsCollectors.Metadata | WindowsCollectors.Session | WindowsCollectors.UnhandledException);

            telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.InstrumentationKey = appInsightsKey;
            TelemetryEnabled = telemetry.IsEnabled();
        }

        public static void LogTelemetry(string text)
        {
            if (TelemetryEnabled)
            {
                //telemetry.TrackTrace($"UWP::{text}", 1,{ });

                //    var obj = {
                //    message: "Channel::" + message,
                //    severityLevel: 1,
                //    properties: logDetails
                //};

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Parameter1", "value1");
                dict.Add("parameter2", "value2");
                telemetry.TrackTrace("", 1, dict);


            }
        }


        public static void LogException(Exception ex)
        {
            if (TelemetryEnabled)
                telemetry.TrackException(ex);
        }


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            InitializeTelemetry();
            LogTelemetry("Trace meessge");
            

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
