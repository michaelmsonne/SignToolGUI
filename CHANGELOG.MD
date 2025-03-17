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