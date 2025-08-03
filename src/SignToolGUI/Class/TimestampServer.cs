using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    public class TimestampServer
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public int Priority { get; set; }
        public int TimeoutSeconds { get; set; }

        public TimestampServer(string displayName, string url, bool isEnabled = true, int priority = 0, int timeoutSeconds = 30)
        {
            DisplayName = displayName;
            Url = url;
            IsEnabled = isEnabled;
            Priority = priority;
            TimeoutSeconds = timeoutSeconds;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class TimestampResult
    {
        public bool Success { get; set; }
        public string ServerUsed { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public int AttemptNumber { get; set; }
    }

    public class TimestampManager
    {
        private readonly List<TimestampServer> _timestampServers;
        private readonly List<TimestampServer> _defaultServers; // Store default servers
        private readonly List<TimestampServer> _trustedSigningServers; // Store trusted signing servers
        private readonly int _maxRetryAttempts;
        private readonly int _retryDelayMs;
        private static readonly HttpClient HttpClient = new HttpClient();

        public delegate void TimestampStatusHandler(string message);
        public event TimestampStatusHandler OnTimestampStatus;

        public TimestampManager(int maxRetryAttempts = 3, int retryDelayMs = 1000)
        {
            _timestampServers = new List<TimestampServer>();
            _defaultServers = new List<TimestampServer>();
            _trustedSigningServers = new List<TimestampServer>();
            _maxRetryAttempts = maxRetryAttempts;
            _retryDelayMs = retryDelayMs;

            // Initialize with default servers
            InitializeServerCollections();
            SetDefaultServers();
        }

        private void InitializeServerCollections()
        {
            // Initialize default servers collection
            _defaultServers.Clear();
            _defaultServers.Add(new TimestampServer("Sectigo (Comodo)", "http://timestamp.sectigo.com", true, 1, 30));
            _defaultServers.Add(new TimestampServer("DigiCert", "http://timestamp.digicert.com", true, 2, 30));
            _defaultServers.Add(new TimestampServer("GlobalSign (1)", "http://timestamp.globalsign.com/tsa/r6advanced1", true, 3, 30));
            _defaultServers.Add(new TimestampServer("GlobalSign (2)", "http://timestamp.globalsign.com/?signature=sha2", true, 4, 30));
            _defaultServers.Add(new TimestampServer("Certum", "http://time.certum.pl", true, 5, 30));

            // Initialize trusted signing servers collection
            _trustedSigningServers.Clear();
            _trustedSigningServers.Add(new TimestampServer("West Europe", "https://weu.codesigning.azure.net", true, 1, 30));
            _trustedSigningServers.Add(new TimestampServer("North Europe", "https://neu.codesigning.azure.net", true, 2, 30));
            _trustedSigningServers.Add(new TimestampServer("West US 2", "https://wus2.codesigning.azure.net", true, 3, 30));
            _trustedSigningServers.Add(new TimestampServer("West Central US", "https://wcus.codesigning.azure.net", true, 4, 30));
            _trustedSigningServers.Add(new TimestampServer("East US", "https://eus.codesigning.azure.net", true, 5, 30));
        }

        private void InitializeDefaultServers()
        {
            // This method is kept for backward compatibility but now uses the stored collection
            SetDefaultServers();
        }

        public void SetDefaultServers()
        {
            // Switch to default timestamp servers (for PFX and Windows Certificate Store)
            _timestampServers.Clear();
            foreach (var server in _defaultServers)
            {
                _timestampServers.Add(new TimestampServer(server.DisplayName, server.Url, server.IsEnabled, server.Priority, server.TimeoutSeconds));
            }
            //OnTimestampStatus?.Invoke("Switched to default timestamp servers for PFX/Certificate Store signing");
            //Message("Timestamp servers switched to default servers for PFX/Certificate Store signing", EventType.Information, 3005);
        }

        public void InitializeTrustedSigningServers()
        {
            // Switch to Trusted Signing endpoints (these are for signing endpoints, not timestamps)
            _timestampServers.Clear();
            _trustedSigningServers.Clear();
            _trustedSigningServers.Add(new TimestampServer("West Europe", "https://weu.codesigning.azure.net", true, 1, 30));
            _trustedSigningServers.Add(new TimestampServer("North Europe", "https://neu.codesigning.azure.net", true, 2, 30));
            _trustedSigningServers.Add(new TimestampServer("West US 2", "https://wus2.codesigning.azure.net", true, 3, 30));
            _trustedSigningServers.Add(new TimestampServer("West Central US", "https://wcus.codesigning.azure.net", true, 4, 30));
            _trustedSigningServers.Add(new TimestampServer("East US", "https://eus.codesigning.azure.net", true, 5, 30));

            foreach (var server in _trustedSigningServers)
            {
                _timestampServers.Add(new TimestampServer(server.DisplayName, server.Url, server.IsEnabled, server.Priority, server.TimeoutSeconds));
            }

            //OnTimestampStatus?.Invoke("Switched to Trusted Signing endpoint servers");
            //Message("Endpoint servers switched to Trusted Signing servers", EventType.Information, 3006);
        }

        public string GetNextTrustedSigningEndpoint(int attemptNumber)
        {
            var enabledServers = GetEnabledServers();

            if (enabledServers.Count == 0)
            {
                OnTimestampStatus?.Invoke("No enabled Trusted Signing endpoints available");
                return null;
            }

            // Use modulo to cycle through servers if we have more attempts than servers
            var serverIndex = (attemptNumber - 1) % enabledServers.Count;
            var selectedServer = enabledServers[serverIndex];

            OnTimestampStatus?.Invoke($"Attempt {attemptNumber}: Using Trusted Signing endpoint '{selectedServer.DisplayName}' ({selectedServer.Url})");

            // Log the endpoint server selection
            Message($"Trusted Signing attempt {attemptNumber}: Using endpoint '{selectedServer.DisplayName}' at {selectedServer.Url}", EventType.Information, 3007);

            return selectedServer.Url;
        }

        public void AddServer(string displayName, string url, bool isEnabled = true, int priority = 0, int timeoutSeconds = 30)
        {
            var server = new TimestampServer(displayName, url, isEnabled, priority, timeoutSeconds);
            _timestampServers.Add(server);

            // Sort by priority
            _timestampServers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        public void RemoveServer(string displayName)
        {
            _timestampServers.RemoveAll(s => s.DisplayName == displayName);
        }

        public List<TimestampServer> GetServers()
        {
            return _timestampServers.ToList();
        }

        public List<TimestampServer> GetEnabledServers()
        {
            return _timestampServers.Where(s => s.IsEnabled).OrderBy(s => s.Priority).ToList();
        }

        public void UpdateServerStatus(string displayName, bool isEnabled)
        {
            var server = _timestampServers.FirstOrDefault(s => s.DisplayName == displayName);
            if (server != null)
            {
                server.IsEnabled = isEnabled;
            }
        }

        public async Task<bool> TestServerAsync(TimestampServer server)
        {
            try
            {
                OnTimestampStatus?.Invoke($"Testing timestamp server: {server.DisplayName}...");

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(server.TimeoutSeconds)))
                {
                    var response = await HttpClient.GetAsync(server.Url, cts.Token);
                    var isReachable = response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed;

                    OnTimestampStatus?.Invoke($"Server {server.DisplayName}: {(isReachable ? "Available" : "Unavailable")}");
                    return isReachable;
                }
            }
            catch (Exception ex)
            {
                OnTimestampStatus?.Invoke($"Server {server.DisplayName}: Unavailable ({ex.Message})");
                return false;
            }
        }

        public async Task<List<TimestampServer>> GetAvailableServersAsync()
        {
            var enabledServers = GetEnabledServers();
            var availableServers = new List<TimestampServer>();

            foreach (var server in enabledServers)
            {
                if (await TestServerAsync(server))
                {
                    availableServers.Add(server);
                }
            }

            return availableServers;
        }

        public string GetNextTimestampUrl(int attemptNumber)
        {
            var enabledServers = GetEnabledServers();

            if (enabledServers.Count == 0)
            {
                OnTimestampStatus?.Invoke("No enabled timestamp servers available");
                return null;
            }

            // Use modulo to cycle through servers if we have more attempts than servers
            var serverIndex = (attemptNumber - 1) % enabledServers.Count;
            var selectedServer = enabledServers[serverIndex];

            OnTimestampStatus?.Invoke($"Attempt {attemptNumber}: Using timestamp server '{selectedServer.DisplayName}' ({selectedServer.Url})");

            // Log the timestamp server selection
            Message($"Timestamp attempt {attemptNumber}: Using server '{selectedServer.DisplayName}' at {selectedServer.Url}", EventType.Information, 3001);

            return selectedServer.Url;
        }

        public TimestampResult ProcessTimestampResult(bool success, string serverUrl, string errorMessage, int attemptNumber, TimeSpan responseTime)
        {
            var server = _timestampServers.FirstOrDefault(s => s.Url == serverUrl);
            var serverName = server?.DisplayName ?? serverUrl;

            var result = new TimestampResult
            {
                Success = success,
                ServerUsed = serverName,
                ErrorMessage = errorMessage,
                AttemptNumber = attemptNumber,
                ResponseTime = responseTime
            };

            if (success)
            {
                OnTimestampStatus?.Invoke($"Timestamp successful using '{serverName}' (Response time: {responseTime.TotalSeconds:F2}s)");
                Message($"Timestamp successful using server '{serverName}' in {responseTime.TotalSeconds:F2} seconds", EventType.Information, 3002);
            }
            else
            {
                OnTimestampStatus?.Invoke($"Timestamp failed using '{serverName}': {errorMessage}");
                Message($"Timestamp failed using server '{serverName}': {errorMessage}", EventType.Warning, 3003);
            }

            return result;
        }

        public int GetMaxRetryAttempts()
        {
            return Math.Max(_maxRetryAttempts, GetEnabledServers().Count);
        }

        public int GetRetryDelay()
        {
            return _retryDelayMs;
        }

        public void LoadConfiguration(Dictionary<string, object> config)
        {
            if (config == null) return;

            try
            {
                // Load server configurations
                if (config.ContainsKey("TimestampServers") && config["TimestampServers"] is List<Dictionary<string, object>> servers)
                {
                    _timestampServers.Clear();
                    foreach (var serverConfig in servers)
                    {
                        if (serverConfig.ContainsKey("DisplayName") && serverConfig.ContainsKey("Url"))
                        {
                            var displayName = serverConfig["DisplayName"].ToString();
                            var url = serverConfig["Url"].ToString();
                            var isEnabled = serverConfig.ContainsKey("IsEnabled") ? (bool)serverConfig["IsEnabled"] : true;
                            var priority = serverConfig.ContainsKey("Priority") ? Convert.ToInt32(serverConfig["Priority"]) : 0;
                            var timeout = serverConfig.ContainsKey("TimeoutSeconds") ? Convert.ToInt32(serverConfig["TimeoutSeconds"]) : 30;

                            AddServer(displayName, url, isEnabled, priority, timeout);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message($"Error loading timestamp configuration: {ex.Message}", EventType.Error, 3004);
                // Fall back to default servers if configuration loading fails
                SetDefaultServers();
            }
        }

        public Dictionary<string, object> SaveConfiguration()
        {
            var config = new Dictionary<string, object>();
            var servers = new List<Dictionary<string, object>>();

            foreach (var server in _timestampServers)
            {
                servers.Add(new Dictionary<string, object>
                {
                    ["DisplayName"] = server.DisplayName,
                    ["Url"] = server.Url,
                    ["IsEnabled"] = server.IsEnabled,
                    ["Priority"] = server.Priority,
                    ["TimeoutSeconds"] = server.TimeoutSeconds
                });
            }

            config["TimestampServers"] = servers;
            return config;
        }

        public void ReplaceAllServers(List<TimestampServer> newServers)
        {
            _timestampServers.Clear();
            foreach (var server in newServers)
            {
                _timestampServers.Add(new TimestampServer(server.DisplayName, server.Url, server.IsEnabled, server.Priority, server.TimeoutSeconds));
            }
            _timestampServers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }
    }
}