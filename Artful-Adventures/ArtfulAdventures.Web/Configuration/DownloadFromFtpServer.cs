namespace ArtfulAdventures.Web.Configuration
{
    using System.Diagnostics;
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
            var localFiles = Directory.GetFiles(@"wwwroot\images").ToList();
            localFiles = localFiles.Select(x => "/" + Path.GetFileName(x)).ToList();
            
            
            var client = FtpClientConfiguration.GetFtpClient();
           
            var listing = await client.GetNameListing();
           
            foreach (var item in listing)
            {
                ftpRemotePaths.Add(item);
            }
            var result = ftpRemotePaths.Except(localFiles).ToList();
            if(result.Count == 0)
            {
                return;
            }
            await client.Connect();
            await client.DownloadFiles(localPath, result.ToArray(), FtpLocalExists.Skip);
            await client.Disconnect();
        }
    }
}
