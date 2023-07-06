namespace ArtfulAdventures.Web.Configuration
{
    using System.Net;

    using FluentFTP;
    using FluentFTP.Client.BaseClient;

    using static ArtfulAdventures.Common.GeneralApplicationConstants;

    public static class DownloadFromFtpServer
    {

        public static async Task DownloadData()
        {
            var ftpRemotePaths = new List<string>();
            string localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var client = FtpClientConfiguration.GetFtpClient();
            await client.Connect();
            var listing = await client.GetNameListing();
            foreach (var item in listing)
            {
                ftpRemotePaths.Add(item);
            }
            await client.DownloadFiles(localPath, ftpRemotePaths.ToArray(), FtpLocalExists.Skip);
            await client.Disconnect();

        }
    }
}
