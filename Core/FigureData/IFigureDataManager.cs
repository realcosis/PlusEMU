using System.Collections.Generic;
using Plus.Core.FigureData.Types;
using Plus.HabboHotel.Users.Clothing.Parts;

namespace Plus.Core.FigureData;

public interface IFigureDataManager
{
    void Init();
    string ProcessFigure(string figure, string gender, ICollection<ClothingParts> clothingParts, bool hasHabboClub);
    Palette GetPalette(int colorId);
    bool TryGetPalette(int palletId, out Palette palette);
    int GetRandomColor(int palletId);
}