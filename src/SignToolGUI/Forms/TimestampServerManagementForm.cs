using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SignToolGUI.Class;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Forms
{
    public partial class TimestampServerManagementForm : Form
    {
        private TimestampManager _timestampManager;
        private List<TimestampServerManager> _currentServers;
        private bool _isModified = false;
        private string _configIniPath;
        private bool _isTrustedSigning;

        public TimestampServerManagementForm(TimestampManager timestampManager, string configIniPath, bool isTrustedSigning = false)
        {
            _timestampManager = timestampManager;
            _configIniPath = configIniPath; // Store the config path
            _isTrustedSigning = isTrustedSigning;
            _currentServers = new List<TimestampServerManager>();
            InitializeComponent();
            UpdateGroupBoxTitle();
            LoadServers();
        }

        private void UpdateGroupBoxTitle()
        {
            if (_isTrustedSigning)
            {
                groupBoxServers.Text = "Endpoints";
                this.Text = "Trusted Signing Endpoint Management";
            }
            else
            {
                groupBoxServers.Text = "Timestamp Servers";
                this.Text = "Timestamp Server Management";
            }
        }

        private void LoadServers()
        {
            _currentServers = _timestampManager.GetServers().ToList();
            RefreshServerList();
        }

        private void RefreshServerList()
        {
            listViewServers.Items.Clear();

            foreach (var server in _currentServers.OrderBy(s => s.Priority))
            {
                var item = new ListViewItem(server.Priority.ToString());
                item.SubItems.Add(server.DisplayName);
                item.SubItems.Add(server.Url);
                item.SubItems.Add(server.IsEnabled ? "Yes" : "No");
                item.SubItems.Add($"{server.TimeoutSeconds}s");
                item.Tag = server;

                if (!server.IsEnabled)
                {
                    item.ForeColor = Color.Gray;
                }

                listViewServers.Items.Add(item);
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            var hasSelection = listViewServers.SelectedItems.Count > 0;
            var selectedIndex = hasSelection ? listViewServers.SelectedIndices[0] : -1;

            buttonEdit.Enabled = hasSelection;
            buttonRemove.Enabled = hasSelection;

            // Disable testing if in Trusted Signing mode
            if (_isTrustedSigning)
            {
                buttonTestAll.Enabled = false;
                buttonTest.Enabled = false;
            }
            else
            {
                buttonTestAll.Enabled = hasSelection;
                buttonTest.Enabled = hasSelection;
            }

            //buttonTest.Enabled = hasSelection;
            buttonMoveUp.Enabled = hasSelection && selectedIndex > 0;
            buttonMoveDown.Enabled = hasSelection && selectedIndex < listViewServers.Items.Count - 1;
            buttonApply.Enabled = _isModified; // This should enable/disable based on modifications
        }

        private void ListViewServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new TimestampServerEditForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var newServer = dialog.GetTimestampServer();
                    newServer.Priority = _currentServers.Count > 0 ? _currentServers.Max(s => s.Priority) + 1 : 1;
                    _currentServers.Add(newServer);
                    RefreshServerList();
                    _isModified = true;
                    UpdateButtonStates(); // Make sure Apply button gets enabled
                }
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 0) return;

            var selectedServer = (TimestampServerManager)listViewServers.SelectedItems[0].Tag;
            using (var dialog = new TimestampServerEditForm(selectedServer))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var editedServer = dialog.GetTimestampServer();
                    var index = _currentServers.FindIndex(s => s.DisplayName == selectedServer.DisplayName && s.Url == selectedServer.Url);
                    if (index >= 0)
                    {
                        editedServer.Priority = selectedServer.Priority; // Preserve priority
                        _currentServers[index] = editedServer;
                        RefreshServerList();
                        _isModified = true; // This should mark as modified
                        UpdateButtonStates(); // Make sure Apply button gets enabled
                    }
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 0) return;

            var selectedServer = (TimestampServerManager)listViewServers.SelectedItems[0].Tag;
            var result = MessageBox.Show(
                $"Are you sure you want to remove '{selectedServer.DisplayName}'?",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _currentServers.RemoveAll(s => s.DisplayName == selectedServer.DisplayName && s.Url == selectedServer.Url);
                RefreshServerList();
                _isModified = true; // This should mark as modified
                UpdateButtonStates(); // Make sure Apply button gets enabled
            }
        }

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 0) return;

            var selectedIndex = listViewServers.SelectedIndices[0];
            if (selectedIndex > 0)
            {
                var orderedServers = _currentServers.OrderBy(s => s.Priority).ToList();
                var temp = orderedServers[selectedIndex].Priority;
                orderedServers[selectedIndex].Priority = orderedServers[selectedIndex - 1].Priority;
                orderedServers[selectedIndex - 1].Priority = temp;

                RefreshServerList();
                listViewServers.Items[selectedIndex - 1].Selected = true;
                _isModified = true;
            }
        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 0) return;

            var selectedIndex = listViewServers.SelectedIndices[0];
            if (selectedIndex < listViewServers.Items.Count - 1)
            {
                var orderedServers = _currentServers.OrderBy(s => s.Priority).ToList();
                var temp = orderedServers[selectedIndex].Priority;
                orderedServers[selectedIndex].Priority = orderedServers[selectedIndex + 1].Priority;
                orderedServers[selectedIndex + 1].Priority = temp;

                RefreshServerList();
                listViewServers.Items[selectedIndex + 1].Selected = true;
                _isModified = true;
            }
        }

        private async void ButtonTest_Click(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 0) return;

            var selectedServer = (TimestampServerManager)listViewServers.SelectedItems[0].Tag;
            buttonTest.Enabled = false;
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Marquee;

            try
            {
                var isAvailable = await _timestampManager.TestServerAsync(selectedServer);
                var message = $"Server '{selectedServer.DisplayName}' is {(isAvailable ? "available" : "unavailable")}.";
                var icon = isAvailable ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
                MessageBox.Show(message, "Server Test Result", MessageBoxButtons.OK, icon);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error testing server: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
                buttonTest.Enabled = true;
            }
        }

        private async void ButtonTestAll_Click(object sender, EventArgs e)
        {
            buttonTestAll.Enabled = false;
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;
            progressBar.Maximum = _currentServers.Count;

            var results = new List<string>();

            foreach (var server in _currentServers)
            {
                try
                {
                    var isAvailable = await _timestampManager.TestServerAsync(server);
                    results.Add($"{server.DisplayName}: {(isAvailable ? "Available" : "Unavailable")}");
                }
                catch (Exception ex)
                {
                    results.Add($"{server.DisplayName}: Error - {ex.Message}");
                }
                progressBar.Value++;
            }

            progressBar.Visible = false;
            buttonTestAll.Enabled = true;

            var resultMessage = string.Join("\n", results);
            MessageBox.Show(resultMessage, "Test All Servers Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"This will reset all {(_isTrustedSigning ? "endpoints" : "timestamp servers")} to default settings. Continue?",
                "Reset to Defaults",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Create a temporary manager to get default servers
                var tempManager = new TimestampManager();
                if (_isTrustedSigning)
                {
                    tempManager.InitializeTrustedSigningServers();
                }
                else
                {
                    tempManager.SetDefaultServers();
                }
                _currentServers = tempManager.GetServers().ToList();
                RefreshServerList();
                _isModified = true;
            }
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (_isModified)
            {
                ApplyChanges();
            }
        }

        private void ApplyChanges()
        {
            try
            {
                // Use the new ReplaceAllServers method to update the timestamp manager
                _timestampManager.ReplaceAllServers(_currentServers);

                // Save the configuration to the INI file to persist the changes
                SaveTimestampConfigurationToFile();

                _isModified = false;
                UpdateButtonStates(); // This will disable the Apply button
                Message($"{(_isTrustedSigning ? "Endpoint" : "Timestamp server")} configuration applied and saved", EventType.Information, 3014);

                MessageBox.Show("Changes have been applied and saved successfully.", "Configuration Applied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Message($"Error applying {(_isTrustedSigning ? "endpoint" : "timestamp server")} changes: {ex.Message}", EventType.Error, 3017);
            }
        }

        private void SaveTimestampConfigurationToFile()
        {
            try
            {
                var iniFile = new IniFile(_configIniPath); // Use the stored path
                var timestampConfig = _timestampManager.SaveConfiguration();

                // Convert the configuration to a JSON string for storage
                var jsonString = System.Text.Json.JsonSerializer.Serialize(timestampConfig);
                iniFile.WriteValue("Timestamp", "ServerConfiguration", jsonString);

                Message($"{(_isTrustedSigning ? "Endpoint" : "Timestamp server")} configuration saved to file", EventType.Information, 3018);
            }
            catch (Exception ex)
            {
                Message($"Error saving {(_isTrustedSigning ? "endpoint" : "timestamp")} configuration to file: {ex.Message}", EventType.Error, 3019);
                throw; // Re-throw to let the calling method handle it
            }
        }
    }
}
