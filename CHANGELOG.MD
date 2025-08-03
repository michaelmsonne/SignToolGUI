## Version 2.0.0.0 (00-00-2025)

### 🆕 New Features
- Added certificate monitoring functionality with new `CertificateMonitor` class and `CertificateStatus` Form
- Introduced comprehensive timestamp server management system
  - New `TimestampServerEditForm` for adding and editing individual timestamp servers
  - New `TimestampServerManagementForm` for centralized server configuration management
  - Added `TimestampServer` and `TimestampManager` classes for server handling and orchestration
  - Dynamic interface adaptation: "Timestamp Servers" for PFX/Certificate Store and "Endpoints" for Trusted Signing
- Built-in timestamp server availability testing and health monitoring
- Support for server prioritization, enabling/disabling, and timeout configuration
- Added certificate type persistence - application now remembers your preferred signing method (Windows Certificate Store, PFX Certificate, or Trusted Signing)

### 🎨 User Interface Enhancements
- Enhanced MainForm UI with new menu options for certificate monitoring and timestamp server management
- Introduced color-coded alerts for certificate expiry in both Windows Certificate Store and PFX scenarios
- Improved certificate information display with better visual feedback
- Added intuitive forms for managing timestamp server configurations
- Context-aware UI labels that change based on signing type (Trusted Signing vs. traditional methods)

### 🔒 Security Improvements
- **Major Security Enhancement**: Completely redesigned password encryption system
  - Replaced hardcoded encryption keys with machine-specific key derivation
  - Upgraded from basic encryption to AES-256 with PBKDF2 key derivation (100,000 iterations)
  - Implemented automatic migration from old encryption format to new secure method
  - Added machine-specific entropy sources (hardware identifiers, system properties)
  - Passwords encrypted on one machine cannot be decrypted on another (intentional security feature)
- Enhanced certificate validation and password security handling

### 🏗️ Architecture Improvements
- Refactored signing classes (`SignerPfx`, `SignerThumbprint`, `SignerTrustedSigning`) to inherit from new `SignerBase` abstract class
- Centralized common signing logic, reducing code redundancy and improving maintainability
- Added new `SecurePasswordManager` class for robust password encryption/decryption
- Enhanced certificate validation and monitoring capabilities
- Improved error handling and validation for certificate paths and passwords
- Better separation of concerns with dedicated security and configuration management classes

### ⚡ Performance & Reliability
- Implemented asynchronous operations for better application responsiveness
- Enhanced logging system for improved troubleshooting and debugging
- Added automatic failover to backup timestamp servers when primary servers are unavailable
- Improved stability when handling certificate operations and network-related timestamp failures
- Better configuration persistence and loading mechanisms

### 🐛 Bug Fixes
- Better error recovery for network-related timestamp failures
- Enhanced validation for certificate operations
- Improved stability in certificate monitoring scenarios
- Fixed configuration loading order to prevent UI overrides
- Better handling of corrupted or incompatible password data

### 🔧 Technical Details
- Enhanced compatibility with .NET Framework 4.8
- Improved machine-specific key generation using multiple entropy sources
- Added comprehensive error handling and logging for security operations
- Backward compatibility maintained through automatic password migration system

---

*This release represents a major milestone in security and usability, significantly enhancing the reliability, user experience, and enterprise-readiness of the SignTool GUI. The new security architecture ensures that sensitive certificate passwords are protected with industry-standard encryption while maintaining seamless user experience through automatic migration and intelligent configuration management.*

---

## Version 1.4.0.0 (17-03-2025):

- Updated Trusted Signing from v0.1.103.0 to the latest v0.1.108.0
- The tool now only displays Code Signing certificates with a private key for selection
- Added a direct link to the Azure Portal to help you find your Trusted Signing accounts
- New option to enable or disable timestamping when signing (supported for .pfx and Certificate Store certificates)
- Improved error handling and logging
- Added support for more versions of the Windows SDK
- New "Select All" option for bulk selecting/unselecting files to sign
- Minor UI improvements for a better user experience

## Version 1.3.0.0 (18-07-2024):
- Add support for Microsoft Trusted Signing
- Add check for if tool is code signed (via Windows API, valid or valid with my Code Signing Certificate via Thumbprint hosted on GitHub)
- Add multiple timestamp servers- Add save to logfile
- Bug fixes
    Like Certificate Store certs will reset on every sign

## Version 1.2.2.0 (04-07-2024):
- Add code to DPI aware and SignTool via API
- Add more status messages to statusstrip for file operations
- Performance tweaks
- Change arch for default signtool.exe
- GUI changes
- Bug fixes

## Version 1.2.1.0 (09-08-2023):
- Major release
- Added feature to find if signtool.exe is installed on the computer
- UI updates
- Add new feature for reset interface
- Add new feature for counting files
- Bug fixes like certificate information not showing up if saved cert at startup
- Minor changes
- Update shipped signtool.exe to last v.
- Updated to.net 4.8

## Version 1.2.0.0 (30-06-2022):
- Feature additions
- Addressed issues

## Version 1.0.4.0 (31-05-2021):
- Significant changes of logic and signing
- Overhauled GUI

## Version 1.0.3.0 (30-04-2021):
- Fixed some bugs when signing multiple files at once from a folder
- Fixed issue for ECC SHA512 bug
- Performance enhancements

## Version 1.0.2.0 (31-03-2021):
- More features added
- Several fixes in GUI text
- Performance tweaks
- UI enhancements

## Version 1.0.1.0 (21-01-2021):
- Initial updates
- Bug fixes

## Version 1.0.0.0 (11-01-2021):
- First release