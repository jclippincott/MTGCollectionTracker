using System.ComponentModel;

namespace TCGPlayerApiWrapper.Enums; 

public enum Language {
    [Description("English")] EN = 1,
    [Description("Chinese (S)")] CS = 2,
    [Description("Chinese (T)")] CT = 3,
    [Description("French")] FR = 4,
    [Description("German")] DE = 5,
    [Description("Italian")] IT = 6,
    [Description("Japanese")] JP = 7,
    [Description("Korean")] KR = 8,
    [Description("Portuguese")] PT = 9,
    [Description("Russian")] RU = 10,
    [Description("Spanish")] SP = 11
}