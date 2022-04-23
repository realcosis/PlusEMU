using System.Collections.Generic;
using Plus.Core.FigureData.Types;
using Plus.HabboHotel.Users.Clothing.Parts;
using Plus.Utilities;

namespace Plus.Core.FigureData;

public interface IFigureDataManager
{
    void Init();
    string ProcessFigure(string figure, string gender, ICollection<ClothingParts> clothingParts, bool hasHabboClub);
    Palette GetPalette(int colorId);
    bool TryGetPalette(int palletId, out Palette palette);
    int GetRandomColor(int palletId);
    private const string DefaultFigure =
        "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";

    public static string FilterFigure(string figure)
    {
        return StringCharFilter.IsValid(figure) ? figure : DefaultFigure;
    }
}