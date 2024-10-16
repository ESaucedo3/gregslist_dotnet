

namespace gregslist_csharp.Services;

public class HousesService
{
  private readonly HousesRepository _repository;
  public HousesService(HousesRepository repository)
  {
    _repository = repository;
  }

  internal House CreateHouse(House houseData)
  {
    House createdHouse = _repository.CreateHouse(houseData);
    return createdHouse;
  }

  internal string DeleteHouse(int houseId, string userId)
  {
    House house = GetHouseById(houseId);
    if (house.CreatorId != userId)
    {
      throw new Exception($"You cannot delete something that doesn't belong to you!");
    }

    _repository.DeleteHouse(houseId);
    return $"House deleted";
  }

  internal House GetHouseById(int houseId)
  {
    House house = _repository.GetHouseById(houseId);

    if (house == null)
    {
      throw new Exception($"No ID with: {houseId} found");
    }

    return house;
  }

  internal List<House> GetHouses()
  {
    List<House> houses = _repository.GetHouses();
    return houses;
  }

  internal House UpdateHouse(int houseId, House houseUpdateData, string userId)
  {
    House house = GetHouseById(houseId);
    if (house.CreatorId != userId)
    {
      throw new Exception("You cannot update something that isn't yours!");
    }

    house.Sqft = houseUpdateData.Sqft ?? house.Sqft;
    house.Bedrooms = houseUpdateData.Bedrooms ?? house.Bedrooms;
    house.Bathrooms = houseUpdateData.Bathrooms ?? house.Bathrooms;
    house.ImgUrl = houseUpdateData.ImgUrl ?? house.ImgUrl;
    house.Description = houseUpdateData.Description ?? house.Description;
    house.Price = houseUpdateData.Price ?? house.Price;

    House updatedHouse = _repository.UpdateHouse(house);
    return updatedHouse;
  }
}