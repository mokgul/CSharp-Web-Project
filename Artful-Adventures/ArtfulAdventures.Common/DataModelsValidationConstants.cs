namespace ArtfulAdventures.Common;

public static class DataModelsValidationConstants
{
    public static class ApplicationUserConstants
    {
        public const int UrlMinLength = 3;
        public const int UrlMaxLength = 2048;
        public const int NameMinLength = 3;
        public const int NameMaxLength = 70;
        public const int BioMinLength = 10;
        public const int BioMaxLength = 200;
        public const int CityNameMinLength = 3;
        public const int CityNameMaxLength = 60;
        public const int AboutMinLength = 10;
        public const int AboutMaxLength = 2000;
        
    }
    public static class BlogConstants
    {
        public const int TitleMinLength = 20;
        public const int TitleMaxLength = 100;
        public const string DateFormat = "yyyy-MM-dd H:mm";
        public const int UrlMinLength = 3;
        public const int UrlMaxLength = 2048;
        public const int ContentMinLength = 100;
        public const int ContentMaxLength = 10000;
    }

    public static class CommentConstants
    {
        public const int AuthorMinLength = 5;
        public const int AuthorMaxLength = 50;
        public const int ContentMinLength = 10;
        public const int ContentMaxLength = 1000;
        public const string DateFormat = "yyyy-MM-dd H:mm";
    }

    public static class ChallengeConstants
    {
        public const int TitleMinLength = 20;
        public const int TitleMaxLength = 100;
        public const int CreatorMinLength = 5;
        public const int CreatorMaxLength = 50;
        public const string DateFormat = "yyyy-MM-dd H:mm";
        public const int UrlMinLength = 3;
        public const int UrlMaxLength = 2048;
        public const int RequirementsMinLength = 20;
        public const int RequirementsMaxLength = 1000;
    }

    public static class PictureConstants
    {
        public const int UrlMinLength = 3;
        public const int UrlMaxLength = 2048;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;
        public const string DateFormat = "yyyy-MM-dd H:mm";
    }
}

