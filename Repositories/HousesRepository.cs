




namespace gregslist_csharp.Repositories;

public class HousesRepository
{
  private readonly IDbConnection _db;

  public HousesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal House CreateHouse(House houseData)
  {
    string sql = @"
    INSERT INTO houses(sqft, bedrooms, bathrooms, imgUrl, description, price, creatorId)
    VALUES(@Sqft, @Bedrooms, @Bathrooms, @ImgUrl, @Description, @Price, @CreatorId);

    SELECT 
    houses.*,
    accounts.*
    FROM houses
    INNER JOIN accounts ON houses.creatorId = accounts.id
    WHERE houses.id = LAST_INSERT_ID();";

    House house = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }, houseData).FirstOrDefault();
    return house;
  }

  internal void DeleteHouse(int houseId)
  {
    string sql = "DELETE FROM houses WHERE id = @houseId LIMIT 1";

    int rowsAffected = _db.Execute(sql, new { houseId });

    if (rowsAffected == 0)
    {
      throw new Exception("No houses we deleted");
    }
    else if (rowsAffected > 1)
    {
      throw new Exception("Something went very wrong, more than 1 house was deleted!");
    }
  }

  internal House GetHouseById(int houseId)
  {
    string sql = @"
    SELECT houses.*, accounts.*
    FROM houses
    INNER JOIN accounts ON houses.creatorId = accounts.id
    WHERE houses.id = @houseId;";

    House house = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }, new { houseId }).FirstOrDefault();
    return house;
  }

  internal List<House> GetHouses()
  {
    string sql = @"
    SELECT houses.*, accounts.* 
    FROM houses
    INNER JOIN accounts ON houses.creatorId = accounts.id;";
    List<House> houses = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }).ToList();
    return houses;
  }

  internal House UpdateHouse(House house)
  {
    string sql = @"
    UPDATE houses
    SET
    sqft = @Sqft,
    bedrooms = @Bedrooms,
    bathrooms = @Bathrooms,
    imgUrl = @ImgUrl,
    description = @Description,
    price = @Price
    WHERE id = @Id
    LIMIT 1;

    SELECT houses.*, accounts.*
    FROM houses
    INNER JOIN accounts ON houses.creatorId = accounts.id;";

    House updatedHouse = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }, house).FirstOrDefault();
    return updatedHouse;
  }
}