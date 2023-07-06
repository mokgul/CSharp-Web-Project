namespace ArtfulAdventures.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ArtfulAdventures.Data;
    using ArtfulAdventures.Web.ViewModels.HashTag;
    using ArtfulAdventures.Web.ViewModels.Picture;
    using Microsoft.EntityFrameworkCore;
    using ArtfulAdventures.Web.ViewModels;
    using System.Drawing;
    using FluentFTP;
    using System.Net;

    using static ArtfulAdventures.Common.GeneralApplicationConstants;
    using FluentFTP.Client.BaseClient;
    using FluentFTP.Rules;

    public class ExploreController : Controller
    {
        private readonly ArtfulAdventuresDbContext _data;

        public ExploreController(ArtfulAdventuresDbContext data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
            {
                Id = h.Id,
                Name = h.Type
            }).ToListAsync();

            var urls = await _data.Pictures.Select(p => new PictureVisualizeViewModel()
            {
                PictureUrl = p.Url,
            }).ToArrayAsync();
            var ftpRemotePaths = new List<string>();
            foreach (var url in urls)
            {
                var path = Path.GetFileName(url.PictureUrl);
                if (!ftpRemotePaths.Contains(path))
                    ftpRemotePaths.Add(path);
            }
            string localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var client = new AsyncFtpClient();
            client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            client.Host = ftpServerUrl;
            client.Port = ftpPort;
            client.Config.TransferChunkSize = 4000000;
            client.Config.LocalFileBufferSize = 4000000;
            client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
            await client.Connect();
            await client.DownloadFiles(localPath, ftpRemotePaths.ToArray(), FtpLocalExists.Skip);
            await client.Disconnect();


            //var client = new AsyncFtpClient();
            //client.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            //client.Host = ftpServerUrl;
            //client.Port = ftpPort;
            //client.Config.TransferChunkSize = 4000000;
            //client.Config.LocalFileBufferSize = 4000000;
            //client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            //client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
            //var files = new List<MemoryStream>();
            //var fileNames = new List<string> { };
            //foreach (var fileName in ftpRemotePaths) fileNames.Add(fileName);
            //var images = new List<string>();
            //await client.Connect();

            //foreach (var fileName in fileNames)
            //{
            //    using (var stream = new MemoryStream())
            //    {
            //        await client.DownloadStream(stream, fileName);
            //        var bytes = new byte[stream.Length];
            //        stream.Position = 0;
            //        stream.Read(bytes, 0, (int)stream.Length);
            //        var base64 = Convert.ToBase64String(bytes);
            //        images.Add(String.Format("data:image/gif;base64,{0}", base64));
            //        files.Add(stream);
            //    }
            //}
            //await client.Disconnect();
            ExploreViewModel model = new ExploreViewModel()
            {
                HashTags = hashtags,
                PicturesIds = ftpRemotePaths.ToArray().Select(p => new PictureVisualizeViewModel()
                {
                    PictureUrl = p
                }).ToArray(),
                //Files = images
            };

            return View(model);

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

