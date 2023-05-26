using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Data.Models
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Category
    {
        [EnumMember(Value = "Film & Animation")]
        FilmAndAnimation = 1,
        [EnumMember(Value = "Autos & Vehicles")]
        AutosAndVehicles = 2,
        Music = 3,
        [EnumMember(Value = "Pets & Animals")]
        PetsAndAnimals = 4,
        Sports = 5,
        [EnumMember(Value = "Travel & Events")]
        TravelAndEvents = 6,
        Gaming = 7,
        [EnumMember(Value = "People & Blogs")]
        PeopleAndBlogs = 8,
        Comedy = 9,
        Entertainment = 10,
        [EnumMember(Value = "News & Politics")]
        NewsAndPolitics = 11,
        [EnumMember(Value = "Howto & Style")]
        HowtoAndStyle = 12,
        Eductaion = 13,
        [EnumMember(Value = "Science & Technology")]
        ScienceAndTechnology = 14,
        [EnumMember(Value = "Nonprofits & Activism")]
        NonprofitsAndActivism = 15,
    }
}