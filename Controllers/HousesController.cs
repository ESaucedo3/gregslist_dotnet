namespace gregslist_csharp.Controllers;

[ApiController, Route("api/houses")]
public class HousesController : ControllerBase
{
  private readonly HousesService _housesService;
  private readonly Auth0Provider _auth0Provider;

  public HousesController(HousesService housesService, Auth0Provider auth0Provider)
  {
    _housesService = housesService;
    _auth0Provider = auth0Provider;
  }

  [HttpGet]
  public ActionResult<List<House>> GetHouses()
  {
    try
    {
      List<House> houses = _housesService.GetHouses();
      return Ok(houses);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpGet("{houseId}")]
  public ActionResult<House> GetHouseById(int houseId)
  {
    try
    {
      House house = _housesService.GetHouseById(houseId);
      return Ok(house);
    }
    catch (Exception e)
    {

      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpPost]
  public async Task<ActionResult<House>> CreateHouse([FromBody] House houseData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      houseData.CreatorId = userInfo.Id;
      House createdHouse = _housesService.CreateHouse(houseData);
      return Ok(createdHouse);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpPut("{houseId}")]
  public async Task<ActionResult<House>> UpdateHouse(int houseId, [FromBody] House houseUpdateData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      House updatedHouse = _housesService.UpdateHouse(houseId, houseUpdateData, userInfo.Id);
      return Ok(updatedHouse);
    }
    catch (Exception e)
    {

      return BadRequest(e.Message);
    }
  }

  [Authorize, HttpDelete("{houseId}")]
  public async Task<ActionResult<string>> DeleteHouse(int houseId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string deletedMsg = _housesService.DeleteHouse(houseId, userInfo.Id);
      return Ok(deletedMsg);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}