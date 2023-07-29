namespace ArtfulAdventures.Web.Configuration;

public static class DeleteFromFtpServer
{
    public static async Task DeleteFile(string pathToDelete)
    {
        var client = FtpClientConfiguration.GetFtpClient();
        var listing = await client.GetNameListing();
        var ftpPath = "/" + Path.GetFileName(pathToDelete);
        if(!listing.Contains(ftpPath))
        {
            return;
        }
        await client.Connect();
        await client.DeleteFile("/" + Path.GetFileName(ftpPath));
        await client.Disconnect();
    }
}

