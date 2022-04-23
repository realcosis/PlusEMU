using System.Collections.Generic;

namespace Plus.HabboHotel.Catalog.Pets;

public interface IPetRaceManager
{
    void Init();
    List<PetRace> GetRacesForRaceId(int raceId);
}