using System.ComponentModel;

namespace HookaTimes.BLL.Enums
{
    public enum MaritalStatusEnum
    {
        Single = 1,
        Married,
        Divorced,

    }


    public enum BodyTypeEnum
    {
        Skinny = 1,
        fat,
    }

    public enum GenderEnum
    {
        Male = 1,
        Female,
        [Description("Rather Not to Say")] RatherNottoSay,

    }

    public enum EyeEnum
    {
        Brown = 1,
        Black,
        Blue,
    }


    public enum HairEnum
    {
        Curly = 1,
        Straight,
    }
}
