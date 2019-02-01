﻿namespace PrecisionGazeMouse {
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;
    using Tobii.Interaction;

    static class Program {
        /// <summary>
        /// Gets the singleton EyeX host instance.
        /// </summary>
        public static Host EyeXHost { get; } = new Host();

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e) {
            try {
                Exception ex = (Exception)e.ExceptionObject;
                MessageBox.Show("Unhandled domain exception:\n\n" + ex.Message);
            } catch (Exception exc) {
                try {
                    MessageBox.Show("Fatal exception happend inside UnhandledExceptionHandler: \n\n" + exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } finally {
                    Application.Exit();
                }
            }

            // It should terminate our main thread so Application.Exit() is unnecessary here
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t) {
            try {
                MessageBox.Show("Unhandled exception catched.\n Application is going to close now.");
            } catch {
                try {
                    MessageBox.Show("Fatal exception happend inside UIThreadException handler", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                } finally {
                    Application.Exit();
                }
            }

            // Here we can decide if we want to end our application or do something else
            Application.Exit();
        }


        private static bool IsRunAsAdministrator() {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void StartApp() {
            EyeXHost.InitializeWpfAgent();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrecisionGazeMouseForm());

            EyeXHost.Dispose();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // Add handler for UI thread exceptions
            Application.ThreadException += UIThreadException;

            // Force all WinForms errors to go through handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // This handler is for catching non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Try to switch to admin role
            if (!IsRunAsAdministrator()) {
                // It is not possible to launch a ClickOnce app as administrator directly, so instead we launch the
                // app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Start the new process
                try {
                    Process.Start(processInfo);
                } catch (Exception) {
                    // The user did not allow the application to run as administrator
                    MessageBox.Show("Warning: If you don't run with Administrator permissions, the gaze mouse will stop working when an Administrator window is selected.");
                    StartApp();
                }
            } else {
                StartApp();
            }
        }
    }
}