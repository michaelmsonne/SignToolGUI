using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SignToolUI.Class
{
    public class OSVersionInfo
    {
        #region Bit

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SystemInfo lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SystemInfo lpSystemInfo);

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        [StructLayout(LayoutKind.Explicit)]
        public struct ProcessorInfoUnion
        {
            [FieldOffset(0)]
            internal uint dwOemId;
            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;
            [FieldOffset(2)]
            internal ushort wReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemInfo
        {
            internal ProcessorInfoUnion uProcessorInfo;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort dwProcessorLevel;
            public ushort dwProcessorRevision;
        }

        // 64 bit OS detection
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        public enum SoftwareArchitecture
        {
            Unknown = 0,
            Bit32 = 1,
            Bit64 = 2
        }

        public enum ProcessorArchitecture
        {
            Unknown = 0,
            Bit32 = 1,
            Bit64 = 2,
            Itanium64 = 3
        }

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            var handle = LoadLibrary("kernel32");
            if (handle == IntPtr.Zero) return null;
            var fnPtr = GetProcAddress(handle, "IsWow64Process");
            if (fnPtr != IntPtr.Zero)
                return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer(fnPtr,
                    typeof(IsWow64ProcessDelegate));
            return null;
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            var fnDelegate = GetIsWow64ProcessDelegate();
            if (fnDelegate == null)
                return false;
            var retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out var isWow64);
            if (retVal == false)
                return false;
            return isWow64;
        }

        // Bestemmer om den aktuelle applikation er 32 eller 64-bit.
        public static string ApplicationBitVer
        {
            get
            {
                string pbits;
                Environment.GetEnvironmentVariables();
                switch (IntPtr.Size * 8)
                {
                    case 64:
                        pbits = @" (64 bit)";
                        break;
                    case 32:
                        pbits = @" (32 bit)";
                        break;
                    default:
                        pbits = @"";
                        break;
                }
                return pbits;
            }
        }

        // Tjek om OS software er 32 eller 64 bit
        // ## BRUGES IKKE ##
        public static SoftwareArchitecture OsBits
        {
            get
            {
                SoftwareArchitecture osbits;
                switch (IntPtr.Size * 8)
                {
                    case 64:
                        osbits = SoftwareArchitecture.Bit64;
                        break;
                    case 32:
                        osbits = Is32BitProcessOn64BitProcessor()
                            ? SoftwareArchitecture.Bit64
                            : SoftwareArchitecture.Bit32;
                        break;
                    default:
                        osbits = SoftwareArchitecture.Unknown;
                        break;
                }
                return osbits;
            }
        }

        // Windows bit udgave
        public static string Osbit()
        {
            var os = Environment.Is64BitOperatingSystem;
            var bit = os ? 64 : 32;
            var windowsBitVer = Convert.ToString(bit);
            return windowsBitVer;
        }

        // Bestemmer om den aktuelle processor er 32 eller 64-bit.
        public static ProcessorArchitecture ProcessorBits
        {
            get
            {
                var pbits = ProcessorArchitecture.Unknown;
                try
                {
                    var lSystemInfo = new SystemInfo();
                    GetNativeSystemInfo(ref lSystemInfo);
                    switch (lSystemInfo.uProcessorInfo.wProcessorArchitecture)
                    {
                        case 9: // PROCESSOR_ARCHITECTURE_AMD64
                            pbits = ProcessorArchitecture.Bit64;
                            break;
                        case 6: // PROCESSOR_ARCHITECTURE_IA64
                            pbits = ProcessorArchitecture.Itanium64;
                            break;
                        case 0: // PROCESSOR_ARCHITECTURE_INTEL
                            pbits = ProcessorArchitecture.Bit32;
                            break;
                        default: // PROCESSOR_ARCHITECTURE_UNKNOWN
                            pbits = ProcessorArchitecture.Unknown;
                            break;
                    }
                }
                catch
                {
                    // ignored
                }
                return pbits;
            }
        }

        #endregion BITS

        #region Windows build nummer.

        // Henter det store versionsnummer på operativsystemet, der kører på denne computer.
        public static int MajorVersion
        {
            get
            {
                if (IsWindows10())
                    return 10;
                var exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                if (string.IsNullOrEmpty(exactVersion)) return Environment.OSVersion.Version.Major;
                var splitVersion = exactVersion.Split('.');
                return int.Parse(splitVersion[0]);
            }
        }

        // Henter det "mindre" versionsnummer på operativsystemet, der kører på denne computer.
        public static int MinorVersion
        {
            get
            {
                if (IsWindows10())
                    return 0;
                var exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                if (string.IsNullOrEmpty(exactVersion)) return Environment.OSVersion.Version.Minor;
                var splitVersion = exactVersion.Split('.');
                return int.Parse(splitVersion[1]);
            }
        }


        // Henter revisionsversionsnummeret til operativsystemet, der kører på denne computer.
        public static int RevisionVersion => IsWindows10() ? 0 : Environment.OSVersion.Version.Revision;

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref Osversioninfoex osVersionInfo);

        // Henter det versionsversionsnummer på operativsystemet, der kører på denne computer.
        public static int BuildVersion => int.Parse(RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuildNumber", "0"));

        // Henter den fulde versionstreng af operativsystemet, der kører på denne computer.
        public static string VersionString => Version.ToString();

        // Henter den fulde version af operativsystemet kørende på denne computer.
        public static Version Version => new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);

        #endregion

        #region Windows udgave navn

        // Ser om det er Windows 10 på denne PC, hvis ja brug den funktion så info bliver korrekt
        private static bool IsWindows10()
        {
            var productName = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "");
            if (productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Osversioninfoex
        {
            // Originalt public og ikke readonly
            public int dwOSVersionInfoSize;
            private readonly int dwMajorVersion;
            private readonly int dwMinorVersion;
            private readonly int dwBuildNumber;
            private readonly int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string szCSDVersion;
            public readonly short wServicePackMajor;
            public readonly short wServicePackMinor;
            public readonly short wSuiteMask;
            public readonly byte wProductType;
            private readonly byte wReserved;
        }

        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(int osMajorVersion, int osMinorVersion, int spMajorVersion, int spMinorVersion, out int edition);

        [DllImport("user32")]
        public static extern int GetSystemMetrics(int nIndex);

        // Henter den udgave af operativsystemet, der kører på denne computer.
        private static string _sEdition;
        public static string Edition
        {
            get
            {
                if (_sEdition != null)
                    return _sEdition;
                var edition = string.Empty;
                var osVersion = Environment.OSVersion;
                var osVersionInfo = new Osversioninfoex { dwOSVersionInfoSize = Marshal.SizeOf(typeof(Osversioninfoex)) };

                if (GetVersionEx(ref osVersionInfo))
                {
                    var majorVersion = osVersion.Version.Major;
                    var minorVersion = osVersion.Version.Minor;
                    var productType = osVersionInfo.wProductType;
                    var suiteMask = osVersionInfo.wSuiteMask;

                    switch (majorVersion)
                    {
                        case 4:
                            switch (productType)
                            {
                                case VerNtWorkstation:
                                    // Windows NT 4.0 Workstation
                                    edition = "Workstation";
                                    break;
                                case VerNtServer:
                                    // Windows NT 4.0 Server
                                    edition = (suiteMask & VerSuiteEnterprise) == 0 ? "Standard Server" : "Enterprise Server";
                                    break;
                            }
                            break;
                        case 5:
                            switch (productType)
                            {
                                case VerNtWorkstation:
                                    if ((suiteMask & VerSuitePersonal) == 0)
                                        edition = GetSystemMetrics(86) == 0 ? "Professional" : "Tablet Edition";
                                    else
                                        edition = "Home";
                                    break;
                                case VerNtServer:
                                    if (minorVersion == 0)
                                        // Windows 2000 Server
                                        if ((suiteMask & VerSuiteDatacenter) != 0) edition = "Datacenter Server";
                                        else if ((suiteMask & VerSuiteEnterprise) != 0) edition = "Advanced Server";
                                        else edition = "Server";
                                    else
                                        // Windows Server 2003
                                        if ((suiteMask & VerSuiteDatacenter) != 0) edition = "Datacenter";
                                    else if ((suiteMask & VerSuiteEnterprise) != 0) edition = "Enterprise";
                                    else if ((suiteMask & VerSuiteBlade) != 0) edition = "Web Edition";
                                    else edition = "Standard";
                                    break;
                            }
                            break;
                        case 6:
                            int ed;
                            if (GetProductInfo(majorVersion, minorVersion, osVersionInfo.wServicePackMajor,
                                osVersionInfo.wServicePackMinor, out ed))
                                switch (ed)
                                {
                                    case ProductBusiness:
                                        edition = "Business";
                                        break;
                                    case ProductBusinessN:
                                        edition = "Business N";
                                        break;
                                    case ProductClusterServer:
                                        edition = "HPC Edition";
                                        break;
                                    case ProductClusterServerV:
                                        edition = "HPC Edition without Hyper-V";
                                        break;
                                    case ProductDatacenterServer:
                                        edition = "Datacenter";
                                        break;
                                    case ProductDatacenterServerCore:
                                        edition = "Datacenter (core installation)";
                                        break;
                                    case ProductDatacenterServerV:
                                        edition = "Datacenter without Hyper-V";
                                        break;
                                    case ProductDatacenterServerCoreV:
                                        edition = "Datacenter without Hyper-V (core installation)";
                                        break;
                                    case ProductEmbedded:
                                        edition = "Embedded";
                                        break;
                                    case ProductEnterprise:
                                        edition = "Enterprise";
                                        break;
                                    case ProductEnterpriseN:
                                        edition = "Enterprise N";
                                        break;
                                    case ProductEnterpriseE:
                                        edition = "Enterprise E";
                                        break;
                                    case ProductEnterpriseServer:
                                        edition = "Enterprise";
                                        break;
                                    case ProductEnterpriseServerCore:
                                        edition = "Enterprise (core installation)";
                                        break;
                                    case ProductEnterpriseServerCoreV:
                                        edition = "Enterprise without Hyper-V (core installation)";
                                        break;
                                    case ProductEnterpriseServerIa64:
                                        edition = "Enterprise for Itanium-based Systems";
                                        break;
                                    case ProductEnterpriseServerV:
                                        edition = "Enterprise without Hyper-V";
                                        break;
                                    case ProductEssentialbusinessServerMgmt:
                                        edition = "Essential Business MGMT";
                                        break;
                                    case ProductEssentialbusinessServerAddl:
                                        edition = "Essential Business ADDL";
                                        break;
                                    case ProductEssentialbusinessServerMgmtsvc:
                                        edition = "Essential Business MGMTSVC";
                                        break;
                                    case ProductEssentialbusinessServerAddlsvc:
                                        edition = "Essential Business ADDLSVC";
                                        break;
                                    case ProductHomeBasic:
                                        edition = "Home Basic";
                                        break;
                                    case ProductHomeBasicN:
                                        edition = "Home Basic N";
                                        break;
                                    case ProductHomeBasicE:
                                        edition = "Home Basic E";
                                        break;
                                    case ProductHomePremium:
                                        edition = "Home Premium";
                                        break;
                                    case ProductHomePremiumN:
                                        edition = "Home Premium N";
                                        break;
                                    case ProductHomePremiumE:
                                        edition = "Home Premium E";
                                        break;
                                    case ProductHomePremiumServer:
                                        edition = "Home Premium Server";
                                        break;
                                    case ProductHyperv:
                                        edition = "Microsoft Hyper-V Server";
                                        break;
                                    case ProductMediumbusinessServerManagement:
                                        edition = "Windows Essential Business Management Server";
                                        break;
                                    case ProductMediumbusinessServerMessaging:
                                        edition = "Windows Essential Business Messaging Server";
                                        break;
                                    case ProductMediumbusinessServerSecurity:
                                        edition = "Windows Essential Business Security Server";
                                        break;
                                    case ProductProfessional:
                                        edition = "Professional";
                                        break;
                                    case ProductProfessionalN:
                                        edition = "Professional N";
                                        break;
                                    case ProductProfessionalE:
                                        edition = "Professional E";
                                        break;
                                    case ProductSbSolutionServer:
                                        edition = "SB Solution Server";
                                        break;
                                    case ProductSbSolutionServerEm:
                                        edition = "SB Solution Server EM";
                                        break;
                                    case ProductServerForSbSolutions:
                                        edition = "Server for SB Solutions";
                                        break;
                                    case ProductServerForSbSolutionsEm:
                                        edition = "Server for SB Solutions EM";
                                        break;
                                    case ProductServerForSmallbusiness:
                                        edition = "Windows Essential Solutions";
                                        break;
                                    case ProductServerForSmallbusinessV:
                                        edition = "Windows Essential Solutions without Hyper-V";
                                        break;
                                    case ProductServerFoundation:
                                        edition = "Foundation";
                                        break;
                                    case ProductSmallbusinessServer:
                                        edition = "Windows Small Business Server";
                                        break;
                                    case ProductSmallbusinessServerPremium:
                                        edition = "Windows Small Business Premium";
                                        break;
                                    case ProductSmallbusinessServerPremiumCore:
                                        edition = "Windows Small Business Premium (core installation)";
                                        break;
                                    case ProductSolutionEmbeddedserver:
                                        edition = "Solution Embedded";
                                        break;
                                    case ProductSolutionEmbeddedserverCore:
                                        edition = "Solution Embedded (core installation)";
                                        break;
                                    case ProductStandardServer:
                                        edition = "Standard";
                                        break;
                                    case ProductStandardServerCore:
                                        edition = "Standard (core installation)";
                                        break;
                                    case ProductStandardServerSolutions:
                                        edition = "Standard Solutions";
                                        break;
                                    case ProductStandardServerSolutionsCore:
                                        edition = "Standard Solutions (core installation)";
                                        break;
                                    case ProductStandardServerCoreV:
                                        edition = "Standard without Hyper-V (core installation)";
                                        break;
                                    case ProductStandardServerV:
                                        edition = "Standard without Hyper-V";
                                        break;
                                    case ProductStarter:
                                        edition = "Starter";
                                        break;
                                    case ProductStarterN:
                                        edition = "Starter N";
                                        break;
                                    case ProductStarterE:
                                        edition = "Starter E";
                                        break;
                                    case ProductStorageEnterpriseServer:
                                        edition = "Enterprise Storage";
                                        break;
                                    case ProductStorageEnterpriseServerCore:
                                        edition = "Enterprise Storage (core installation)";
                                        break;
                                    case ProductStorageExpressServer:
                                        edition = "Express Storage";
                                        break;
                                    case ProductStorageExpressServerCore:
                                        edition = "Express Storage (core installation)";
                                        break;
                                    case ProductStorageStandardServer:
                                        edition = "Standard Storage";
                                        break;
                                    case ProductStorageStandardServerCore:
                                        edition = "Standard Storage (core installation)";
                                        break;
                                    case ProductStorageWorkgroupServer:
                                        edition = "Workgroup Storage";
                                        break;
                                    case ProductStorageWorkgroupServerCore:
                                        edition = "Workgroup Storage (core installation)";
                                        break;
                                    case ProductUndefined:
                                        edition = "Unknown product";
                                        break;
                                    case ProductUltimate:
                                        edition = "Ultimate";
                                        break;
                                    case ProductUltimateN:
                                        edition = "Ultimate N";
                                        break;
                                    case ProductUltimateE:
                                        edition = "Ultimate E";
                                        break;
                                    case ProductWebServer:
                                        edition = "Web Server";
                                        break;
                                    case ProductWebServerCore:
                                        edition = "Web Server (core installation)";
                                        break;
                                }
                            break;
                    }
                }
                _sEdition = edition;
                return edition;
            }
        }

        // Henter navnet på operativsystemet, der kører på denne computer.
        private static string _sName;
        public static string Name
        {
            get
            {
                if (_sName != null)
                    return _sName;
                var name = "Unknown";
                var osVersion = Environment.OSVersion;
                var osVersionInfo = new Osversioninfoex { dwOSVersionInfoSize = Marshal.SizeOf(typeof(Osversioninfoex)) };

                if (GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;

                    if (majorVersion == 6 && minorVersion == 2)
                    {
                        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx
                        // For applikationer, der er blevet manifestet for Windows 8.1 og Windows 10. Applikationer, der ikke er manifesteret for 8.1 eller 10
                        // returnerer Windows 8/8.1 OS versionen værdi (6.2/6.3).
                        // Ved at læse registreringsdatabasen får vi den nøjagtige version - hvilket betyder at vi endda kan sammenligne med Win 8 og Win 8.1.

                        var exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
                        if (!string.IsNullOrEmpty(exactVersion))
                        {
                            var splitResult = exactVersion.Split('.');
                            majorVersion = Convert.ToInt32(splitResult[0]);
                            minorVersion = Convert.ToInt32(splitResult[1]);
                        }
                        if (IsWindows10())
                        {
                            majorVersion = 10;
                            minorVersion = 0;
                        }
                    }
                    switch (osVersion.Platform)
                    {
                        case PlatformID.Win32S:
                            name = "Windows 3.1";
                            break;
                        case PlatformID.WinCE:
                            name = "Windows CE";
                            break;
                        case PlatformID.Win32Windows:
                            {
                                if (majorVersion == 4)
                                {
                                    var csdVersion = osVersionInfo.szCSDVersion;
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            if (csdVersion == "B" || csdVersion == "C") name = "Windows 95 OSR2";
                                            else name = "Windows 95";
                                            break;
                                        case 10:
                                            name = csdVersion == "A" ? "Windows 98 Second Edition" : "Windows 98";
                                            break;
                                        case 90:
                                            name = "Windows Me";
                                            break;
                                    }
                                }
                                break;
                            }
                        case PlatformID.Win32NT:
                            {
                                var productType = osVersionInfo.wProductType;
                                switch (majorVersion)
                                {
                                    case 3:
                                        name = "Windows NT 3.51";
                                        break;
                                    case 4:
                                        switch (productType)
                                        {
                                            case 1:
                                                name = "Windows NT 4.0";
                                                break;
                                            case 3:
                                                name = "Windows NT 4.0 Server";
                                                break;
                                        }
                                        break;
                                    case 5:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                name = "Windows 2000";
                                                break;
                                            case 1:
                                                name = "Windows XP";
                                                break;
                                            case 2:
                                                name = "Windows Server 2003";
                                                break;
                                        }
                                        break;
                                    case 6:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                if (productType == 1) name = "Windows Vista";
                                                else if (productType == 3) name = "Windows Server 2008";
                                                break;
                                            case 1:
                                                if (productType == 1) name = "Windows 7";
                                                else if (productType == 3) name = "Windows Server 2008 R2";
                                                break;
                                            case 2:
                                                if (productType == 1) name = "Windows 8";
                                                else if (productType == 3) name = "Windows Server 2012";
                                                break;
                                            case 3:
                                                if (productType == 1) name = "Windows 8.1";
                                                else if (productType == 3) name = "Windows Server 2012 R2";
                                                break;
                                        }
                                        break;
                                    case 10:
                                        if (minorVersion == 0)
                                            if (productType == 1) name = "Windows 10";
                                            else if (productType == 3) name = "Windows Server 2016";
                                        break;
                                }
                                break;
                            }
                    }
                }
                _sName = name;
                return name;
            }
        }

        #endregion Windows udgave navn

        #region Registry Methods

        // Hent info fra reg i Windows
        private static string RegistryRead(string registryPath, string field, string defaultValue)
        {
            var rtn = "";
            var backSlash = "";
            var newRegistryPath = "";
            try
            {
                RegistryKey ourKey = null;
                var splitResult = registryPath.Split('\\');
                if (splitResult.Length > 0)
                {
                    splitResult[0] = splitResult[0].ToUpper(); // Opret den første input som store bogstaver...
                    if (splitResult[0] == "HKEY_CLASSES_ROOT") ourKey = Registry.ClassesRoot;
                    else if (splitResult[0] == "HKEY_CURRENT_USER") ourKey = Registry.CurrentUser;
                    else if (splitResult[0] == "HKEY_LOCAL_MACHINE") ourKey = Registry.LocalMachine;
                    else if (splitResult[0] == "HKEY_USERS") ourKey = Registry.Users;
                    else if (splitResult[0] == "HKEY_CURRENT_CONFIG") ourKey = Registry.CurrentConfig;

                    if (ourKey != null)
                    {
                        for (var i = 1; i < splitResult.Length; i++)
                        {
                            newRegistryPath += backSlash + splitResult[i]; backSlash = "\\";
                        }
                        if (newRegistryPath != "")
                        {
                            ourKey = ourKey.OpenSubKey(newRegistryPath);
                            if (ourKey != null)
                            {
                                rtn = (string)ourKey.GetValue(field, defaultValue);
                                ourKey.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return rtn;
        }

        #endregion Registry Methods

        #region Produkttyper

        private const int ProductUndefined = 0x00000000;
        private const int ProductUltimate = 0x00000001;
        private const int ProductHomeBasic = 0x00000002;
        private const int ProductHomePremium = 0x00000003;
        private const int ProductEnterprise = 0x00000004;
        private const int ProductHomeBasicN = 0x00000005;
        private const int ProductBusiness = 0x00000006;
        private const int ProductStandardServer = 0x00000007;
        private const int ProductDatacenterServer = 0x00000008;
        private const int ProductSmallbusinessServer = 0x00000009;
        private const int ProductEnterpriseServer = 0x0000000A;
        private const int ProductStarter = 0x0000000B;
        private const int ProductDatacenterServerCore = 0x0000000C;
        private const int ProductStandardServerCore = 0x0000000D;
        private const int ProductEnterpriseServerCore = 0x0000000E;
        private const int ProductEnterpriseServerIa64 = 0x0000000F;
        private const int ProductBusinessN = 0x00000010;
        private const int ProductWebServer = 0x00000011;
        private const int ProductClusterServer = 0x00000012;
        //private const int ProductHomeServer = 0x00000013;
        private const int ProductStorageExpressServer = 0x00000014;
        private const int ProductStorageStandardServer = 0x00000015;
        private const int ProductStorageWorkgroupServer = 0x00000016;
        private const int ProductStorageEnterpriseServer = 0x00000017;
        private const int ProductServerForSmallbusiness = 0x00000018;
        private const int ProductSmallbusinessServerPremium = 0x00000019;
        private const int ProductHomePremiumN = 0x0000001A;
        private const int ProductEnterpriseN = 0x0000001B;
        private const int ProductUltimateN = 0x0000001C;
        private const int ProductWebServerCore = 0x0000001D;
        private const int ProductMediumbusinessServerManagement = 0x0000001E;
        private const int ProductMediumbusinessServerSecurity = 0x0000001F;
        private const int ProductMediumbusinessServerMessaging = 0x00000020;
        private const int ProductServerFoundation = 0x00000021;
        private const int ProductHomePremiumServer = 0x00000022;
        private const int ProductServerForSmallbusinessV = 0x00000023;
        private const int ProductStandardServerV = 0x00000024;
        private const int ProductDatacenterServerV = 0x00000025;
        private const int ProductEnterpriseServerV = 0x00000026;
        private const int ProductDatacenterServerCoreV = 0x00000027;
        private const int ProductStandardServerCoreV = 0x00000028;
        private const int ProductEnterpriseServerCoreV = 0x00000029;
        private const int ProductHyperv = 0x0000002A;
        private const int ProductStorageExpressServerCore = 0x0000002B;
        private const int ProductStorageStandardServerCore = 0x0000002C;
        private const int ProductStorageWorkgroupServerCore = 0x0000002D;
        private const int ProductStorageEnterpriseServerCore = 0x0000002E;
        private const int ProductStarterN = 0x0000002F;
        private const int ProductProfessional = 0x00000030;
        private const int ProductProfessionalN = 0x00000031;
        private const int ProductSbSolutionServer = 0x00000032;
        private const int ProductServerForSbSolutions = 0x00000033;
        private const int ProductStandardServerSolutions = 0x00000034;
        private const int ProductStandardServerSolutionsCore = 0x00000035;
        private const int ProductSbSolutionServerEm = 0x00000036;
        private const int ProductServerForSbSolutionsEm = 0x00000037;
        private const int ProductSolutionEmbeddedserver = 0x00000038;
        private const int ProductSolutionEmbeddedserverCore = 0x00000039;
        //private const int ???? = 0x0000003A;
        private const int ProductEssentialbusinessServerMgmt = 0x0000003B;
        private const int ProductEssentialbusinessServerAddl = 0x0000003C;
        private const int ProductEssentialbusinessServerMgmtsvc = 0x0000003D;
        private const int ProductEssentialbusinessServerAddlsvc = 0x0000003E;
        private const int ProductSmallbusinessServerPremiumCore = 0x0000003F;
        private const int ProductClusterServerV = 0x00000040;
        private const int ProductEmbedded = 0x00000041;
        private const int ProductStarterE = 0x00000042;
        private const int ProductHomeBasicE = 0x00000043;
        private const int ProductHomePremiumE = 0x00000044;
        private const int ProductProfessionalE = 0x00000045;
        private const int ProductEnterpriseE = 0x00000046;
        private const int ProductUltimateE = 0x00000047;
        //private const int PRODUCT_UNLICENSED = 0xABCDABCD;

        private const int VerNtWorkstation = 1;
        //private const int VerNtDomainController = 2;
        private const int VerNtServer = 3;
        //private const int VerSuiteSmallbusiness = 1;
        private const int VerSuiteEnterprise = 2;
        //private const int VerSuiteTerminal = 16;
        private const int VerSuiteDatacenter = 128;
        //private const int VerSuiteSingleuserts = 256;
        private const int VerSuitePersonal = 512;
        private const int VerSuiteBlade = 1024;

        #endregion

        #region Service pack

        // Henter service pack oplysningerne i operativsystemet som kører på denne computer.
        public static string ServicePack
        {
            get
            {
                var servicePack = string.Empty;
                var osVersionInfo = new Osversioninfoex { dwOSVersionInfoSize = Marshal.SizeOf(typeof(Osversioninfoex)) };
                if (GetVersionEx(ref osVersionInfo)) servicePack = osVersionInfo.szCSDVersion;
                return servicePack;
            }
        }

        #endregion

        #region Username

        // Hent nuværende brugers brugernavn fra Windows
        public static string UserName
        {
            get
            {
                try
                {
                    return Environment.UserDomainName + '\\' + Environment.UserName;
                }
                catch
                {
                    return Environment.UserName;
                }
            }
        }

        #endregion
    }
}
