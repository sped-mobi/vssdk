using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using Task = System.Threading.Tasks.Task;

/// <summary>
/// A logger made specifically for Visual Studio extensions. Thank you Mads Christiansen.
/// </summary>
public static class VSLogger
{
    private static IVsOutputWindowPane pane;
    private static IServiceProvider _provider;
    private static Guid _guid;
    private static string _name;
    private static object _syncRoot = new object();

    /// <summary>
    /// Initializes the logger and Application Insights telemetry client.
    /// </summary>
    /// <param name="provider">The service provider or Package instance.</param>
    /// <param name="name">The name to use for the custom Output Window pane.</param>
    /// <param name="version">The version of the Visual Studio extension.</param>
    /// <param name="telemetryKey">The Applicatoin Insights instrumentation key (usually a GUID).</param>
    public static void Initialize(IServiceProvider provider, string name)
    {
        ThreadHelper.JoinableTaskFactory.Run(() => InitializeAsync(provider, name));
    }

    public static async Task InitializeAsync(IServiceProvider provider, string name)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

        _provider = provider;
        _name = name;
    }

    /// <summary>
    /// Logs a message to the Output Window.
    /// </summary>
    /// <param name="message">The message to output.</param>
    public static void Log(string message)
    {
        ThreadHelper.JoinableTaskFactory.Run(() => LogAsync(message));
    }

    public static async Task LogAsync(string message)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

        if (string.IsNullOrEmpty(message))
            return;

        try
        {
            if (EnsurePane())
            {
                pane.OutputStringThreadSafe(DateTime.Now + ": " + message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
        }
    }

    /// <summary>
    /// Logs an exception to the output window and tracks it in Application Insights.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    public static void Log(Exception ex)
    {
        ThreadHelper.JoinableTaskFactory.Run(() => LogAsync(ex));
    }

    public static async Task LogAsync(Exception ex)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

        if (ex != null)
        {
            await LogAsync(ex.ToString());
        }
    }

    /// <summary>
    /// Removes all text from the Output Window pane.
    /// </summary>
    public static void Clear()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (pane != null)
        {
            pane.Clear();
        }
    }

    /// <summary>
    /// Deletes the Output Window pane.
    /// </summary>
    public static void DeletePane()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (pane != null)
        {
            try
            {
                IVsOutputWindow output = (IVsOutputWindow)_provider.GetService(typeof(SVsOutputWindow));
                Assumes.Present(output);
                output.DeletePane(ref _guid);
                pane = null;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }

    private static bool EnsurePane()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (pane == null)
        {
            lock (_syncRoot)
            {
                if (pane == null)
                {
                    _guid = Guid.NewGuid();
                    IVsOutputWindow output = (IVsOutputWindow)_provider.GetService(typeof(SVsOutputWindow));
                    Assumes.Present(output);
                    output.CreatePane(ref _guid, _name, 1, 1);
                    output.GetPane(ref _guid, out pane);
                }
            }
        }

        return pane != null;
    }
}
