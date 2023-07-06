namespace ArtfulAdventures.Web.Configuration
{
    using FluentFTP.Client.BaseClient;
    using FluentFTP;
    using System.Net;

    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    public static class FtpClientConfiguration
    {
        public static AsyncFtpClient GetFtpClient()
        {
            var client = new AsyncFtpClient();
            client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            client.Host = ftpServerUrl;
            client.Port = ftpPort;
            client.Config.TransferChunkSize = 4000000;
            client.Config.LocalFileBufferSize = 4000000;
            client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

            return client;
        }
        private static void OnValidateCertificate(BaseFtpClient control, FtpSslValidationEventArgs e)
        {
            if (e.PolicyErrors != System.Net.Security.SslPolicyErrors.None)
            {
                // invalid cert, do you want to accept it?
                e.Accept = true;
            }
            else
            {
                e.Accept = true;
            }
        }
    }
}
