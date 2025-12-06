namespace Advent_of_Code.Services;

public static class CSharpLang
{
    public const string ServiceKey = "cs";
}

public static class FSharpLang
{
    public const string ServiceKey = "fs";
}

public enum Lang
{
    CSharp,
    FSharp
}

public static class LangExtensions
{
    public static string ToDisplayString(this Lang lang) =>
        lang switch
        {
            Lang.CSharp => "C#",
            Lang.FSharp => "F#",
            
            _ => lang.ToString()
        };
}