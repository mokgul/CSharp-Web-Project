namespace ArtfulAdventures.Data.Seeding;

using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Data.Models.Enums;

public class HashTagsSeed
{
    public readonly ICollection<HashTag> HashTags = new HashSet<HashTag>();
    public HashTagsSeed()
    {
        HashTags = GenerateHashTags();
    }

    private ICollection<HashTag> GenerateHashTags()
    {
        int id = 0;
        foreach (var type in Enum.GetValues(typeof(HashTagType)))
        {
            id++;
            HashTags.Add(new HashTag((HashTagType)type) {Id = id}); 
        }
        return HashTags;
    }
}

