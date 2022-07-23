namespace Notes.WEB.Shared.Common;

public static class ColorHelper
{
    public static int ToInt(string color)
    {
        switch (color.ToUpper())
        {
            case "#FF0000":
                return 1;
            case "#00FF00":
                return 2;
            case "#FF69B4":
                return 3;
            case "#FFFF00":
                return 4;
            case "#00FFFF":
                return 5;
            default:
                return 0;
        }
    }

    public static string ColorToString(int id)
    {
        switch (id)
        {
            case 1:
                return "#FF0000";
            case 2:
                return "#00FF00";
            case 3:
                return "#FF69B4";
            case 4:
                return "#FFFF00";
            case 5:
                return "#00FFFF";
            default:
                return "";
        }
    }
}