namespace ArtfulAdventures.Web.Configuration;

using FluentFTP;

public static class UploadToFtpServer
{
    public async static Task UploadFile(string fileName, string filePath)
    {
        AsyncFtpClient client = FtpClientConfiguration.GetFtpClient();

        await client.Connect();
        await client.UploadFile(filePath, fileName);
        await client.Disconnect();
    }
}

