## Trusted Signing command

```
.\signtool.exe sign /v /debug /fd SHA256 /tr "http://timestamp.acs.microsoft.com" /td SHA256 /dlib "C:\Program Files\PackageManagement\NuGet\Packages\Microsoft.Trusted.Signing.Client.1.0.60\bin\x64\Azure.CodeSigning.Dlib.dll" /dmdf "C:\Users\MichaelMortenSonne\metadata.json" "C:\Users\MichaelMortenSonne\program-SetupFiles\program.msi"
```

metadata.json:

```
{
"Endpoint": "https://weu.codesigning.azure.net/",
"CodeSigningAccountName": "sonnes",
"CertificateProfileName": "Michael"
}
```
