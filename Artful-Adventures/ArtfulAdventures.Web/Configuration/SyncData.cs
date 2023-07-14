namespace ArtfulAdventures.Web.Configuration
{
    public static class SyncData 
    {
        public static async Task ExecuteAsync()
        {
            while(true)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                await DownloadFromFtpServer.DownloadData();
            }
        }
    }

}
