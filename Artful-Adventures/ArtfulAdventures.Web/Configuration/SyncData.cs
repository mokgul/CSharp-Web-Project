namespace ArtfulAdventures.Web.Configuration
{
    public static class SyncData 
    {
        public static async Task ExecuteAsync()
        {
            while(true)
            {
                await DownloadFromFtpServer.DownloadData();
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }

}
