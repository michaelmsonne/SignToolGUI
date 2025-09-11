using System.Net.Http;
using System.Threading.Tasks;

namespace SignToolGUI.Class
{
    internal class Globals
    {
        internal class ToolName
        {
            internal static string SignTool = "SignTool";
            internal static string SignToolGui = "SignTool GUI";
        }

        internal class MsgBox
        {
            internal static string Warning = "Warning";
            internal static string Error = "Error";
        }

        internal class DigitalCertificates
        {
            internal static string DefaultCertificateStoreCurrentUser = "Current User";
            internal static string CertificateInfoIsNotAvailable = "Certificate information is not available.";
            internal static string CertificateInfoCouldNotBeRetrieved = "Certificate information could not be retrieved";
        }

        internal class ToolStates
        {
            internal static string CodeSignedBuild = @"Signed build";
            internal static string CodeSignedBuildMichael = @"Signed build (by Michael Morten Sonne)";
            internal static string NotCodeSignedBuild = @"Unsigned build";
            internal static string MichaelCodeSignThumbprintOffline = "D6A630B8F65C473C19F8B694491130073FCCDB32";
        }

        internal class ToolStings
        {
            internal static string URLAzurePortalTrustedSigning = @"https://portal.azure.com/#browse/Microsoft.CodeSigning%2Fcodesigningaccounts";
        }


        internal static async Task<string> FetchCurrentCertificateThumbprintAsync()
        {
            const string url = "https://raw.githubusercontent.com/michaelmsonne/michaelmsonne/main/Trusted_Publisher_Certificate/CurrentCertificateThumbprint.txt";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string fetchedThumbprint = await client.GetStringAsync(url);
                    return fetchedThumbprint.Trim();
                }
            }
            catch
            {
                // Return the hardcoded thumbprint if unable to fetch online
                return ToolStates.MichaelCodeSignThumbprintOffline;
            }
        }
    }
}