using System;
using System.Runtime.InteropServices;

namespace SignToolGUI.Class
{
    // Struct layout attribute to define the structure layout for P/Invoke, with sequential layout and Unicode character set
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WINTRUST_FILE_INFO
    {
        public uint cbStruct;         // Size of the structure in bytes
        public IntPtr pcwszFilePath;  // Pointer to the file path
        public IntPtr hFile;          // Handle to the file (optional)
        public IntPtr pgKnownSubject; // Pointer to a GUID that identifies the subject type (optional)
    }

    // Struct layout attribute to define the structure layout for P/Invoke, with sequential layout and Unicode character set
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WINTRUST_DATA
    {
        public uint cbStruct;              // Size of the structure in bytes
        public IntPtr pPolicyCallbackData; // Pointer to policy callback data (optional)
        public IntPtr pSIPClientData;      // Pointer to SIP client data (optional)
        public uint dwUIChoice;            // UI choice (e.g., no UI, modal, or modeless)
        public uint fdwRevocationChecks;   // Revocation checks (e.g., none, whole chain)
        public uint dwUnionChoice;         // Union choice (specifies type of structure)
        public IntPtr pFile;               // Pointer to WINTRUST_FILE_INFO structure
        public uint dwStateAction;         // State action (e.g., ignore, verify, close)
        public IntPtr hWVTStateData;       // Handle to WVT state data
        public IntPtr pwszURLReference;    // Pointer to URL reference (optional)
        public uint dwProvFlags;           // Provider flags (e.g., use IE settings)
        public uint dwUIContext;           // UI context (e.g., execute, install)
    }

    // Static class for native method definitions
    public static class NativeMethods
    {
        // Importing the WinVerifyTrust function from wintrust.dll for trust verification of files
        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        public static extern int WinVerifyTrust(
            IntPtr hwnd,                              // Handle to a window for displaying UI (optional)
            [MarshalAs(UnmanagedType.LPStruct)]
        Guid pgActionID,                              // GUID specifying the action to be performed
            IntPtr pWinTrustData                      // Pointer to WINTRUST_DATA structure
        );
    }
}