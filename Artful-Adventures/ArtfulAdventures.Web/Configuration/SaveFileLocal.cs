namespace ArtfulAdventures.Web.Configuration
{
    using System.Drawing;


    public static class SaveFileLocal
    {
        public static void SaveAsync(string filePath, Bitmap newImage)
        {
            using (var stream = new MemoryStream())
            {
                newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                var imageBytes = stream.ToArray();

                //Save the file to the local folder
                using (var str = new FileStream(
        filePath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                {
                    str.Write(imageBytes, 0, imageBytes.Length);
                }
            }
        }

        public static Bitmap ResizeImage(IFormFile file)
        {
            Image image = Image.FromStream(file.OpenReadStream(), true, true);
            var newWidth = image.Width;
            var newHeight = image.Height;
            if (image.Width > 200 && image.Height > 200)
            {
                newWidth = (int)(image.Width * 0.5);
                newHeight = (int)(image.Height * 0.5);
            }

            var newImage = new Bitmap(image, new Size(newWidth, newHeight));
            return newImage;
        }
    }
}
