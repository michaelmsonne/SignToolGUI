using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    public abstract class SignerBase
    {
        public string SignToolExe { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool Timestamp { get; set; }
        public TimestampManager TimestampManager { get; set; }

        public delegate void StatusReport(string message);
        public event StatusReport OnSignToolOutput;

        protected SignerBase(string executable, TimestampManager timestampManager = null)
        {
            SignToolExe = executable;
            TimestampManager = timestampManager ?? new TimestampManager();

            // Subscribe to timestamp manager status updates
            TimestampManager.OnTimestampStatus += OnTimestampStatusReceived;
        }

        // Protected method that derived classes can use to raise the event
        protected virtual void RaiseOnSignToolOutput(string message)
        {
            OnSignToolOutput?.Invoke(message);
        }

        // Handle timestamp status messages
        private void OnTimestampStatusReceived(string message)
        {
            RaiseOnSignToolOutput(message);
        }

        protected bool VerifyFileExists()
        {
            return File.Exists(SignToolExe);
        }

        protected abstract string BuildSigningArguments(string targetAssembly, string timestampUrl = null);

        public async Task SignAsync(string targetAssembly)
        {
            await SignWithFallbackAsync(targetAssembly);
        }

        // Keep the synchronous version for backward compatibility
        public void Sign(string targetAssembly)
        {
            Task.Run(() => SignWithFallbackAsync(targetAssembly));
        }

        private async Task SignWithFallbackAsync(string targetAssembly)
        {
            if (!VerifyFileExists())
            {
                RaiseOnSignToolOutput(@"SignTool.exe can't be found!");
                return;
            }

            var startTime = DateTime.Now;
            var maxAttempts = Timestamp ? TimestampManager.GetMaxRetryAttempts() : 1;
            var success = false;
            var lastError = string.Empty;

            for (int attempt = 1; attempt <= maxAttempts && !success; attempt++)
            {
                try
                {
                    string timestampUrl = null;

                    if (Timestamp)
                    {
                        timestampUrl = TimestampManager.GetNextTimestampUrl(attempt);
                        if (string.IsNullOrEmpty(timestampUrl))
                        {
                            RaiseOnSignToolOutput("No timestamp servers available!");
                            break;
                        }
                    }

                    var arguments = BuildSigningArguments(targetAssembly, timestampUrl);
                    var attemptStartTime = DateTime.Now;

                    RaiseOnSignToolOutput($"Signing attempt {attempt}/{maxAttempts}...");
                    Message($"Starting signing attempt {attempt}/{maxAttempts} for file: {Path.GetFileName(targetAssembly)}", EventType.Information, 3010);

                    using (var process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo(SignToolExe)
                        {
                            Arguments = arguments,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        };

                        var output = string.Empty;
                        var error = string.Empty;

                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                output += e.Data + Environment.NewLine;
                                RaiseOnSignToolOutput(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                error += e.Data + Environment.NewLine;
                                RaiseOnSignToolOutput(e.Data);
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // Wait for the process to complete asynchronously
                        await Task.Run(() => process.WaitForExit());

                        var responseTime = DateTime.Now - attemptStartTime;
                        var exitCode = process.ExitCode;

                        if (exitCode == 0)
                        {
                            success = true;
                            RaiseOnSignToolOutput($"Signing completed successfully on attempt {attempt}");

                            if (Timestamp && !string.IsNullOrEmpty(timestampUrl))
                            {
                                TimestampManager.ProcessTimestampResult(true, timestampUrl, null, attempt, responseTime);
                            }

                            Message($"File signed successfully on attempt {attempt}: {Path.GetFileName(targetAssembly)}", EventType.Information, 3011);
                        }
                        else
                        {
                            lastError = !string.IsNullOrEmpty(error) ? error.Trim() : $"SignTool exited with code {exitCode}";

                            if (Timestamp && !string.IsNullOrEmpty(timestampUrl))
                            {
                                TimestampManager.ProcessTimestampResult(false, timestampUrl, lastError, attempt, responseTime);
                            }

                            if (attempt < maxAttempts)
                            {
                                RaiseOnSignToolOutput($"Attempt {attempt} failed, retrying with next timestamp server...");

                                // Wait before retrying
                                var retryDelay = TimestampManager.GetRetryDelay();
                                if (retryDelay > 0)
                                {
                                    await Task.Delay(retryDelay);
                                }
                            }
                            else
                            {
                                RaiseOnSignToolOutput($"All {maxAttempts} signing attempts failed");
                                Message($"All signing attempts failed for file: {Path.GetFileName(targetAssembly)}. Last error: {lastError}", EventType.Error, 3012);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                    RaiseOnSignToolOutput($"Exception during attempt {attempt}: {ex.Message}");

                    if (attempt >= maxAttempts)
                    {
                        Message($"Exception during signing: {ex.Message}", EventType.Error, 3013);
                        break;
                    }
                }
            }

            var totalTime = DateTime.Now - startTime;

            if (success)
            {
                RaiseOnSignToolOutput($"File signing completed successfully in {totalTime.TotalSeconds:F2} seconds");
            }
            else
            {
                RaiseOnSignToolOutput($"File signing failed after {maxAttempts} attempts. Last error: {lastError}");
            }
        }

        protected string GlobalOptionSwitches()
        {
            switch (Verbose)
            {
                case true when Debug:
                    return "/v /debug";
                case true:
                    return "/v";
                default:
                    return Debug ? "/debug" : string.Empty;
            }
        }
    }
}