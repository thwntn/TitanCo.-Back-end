namespace ReferenceController;

[ApiController]
[Route(nameof(Group))]
public class Group(IGroup groupService, IJwt jwtService) : Controller
{
    private readonly IGroup _groupService = groupService;
    private readonly IJwt _jwtService = jwtService;

    [HttpPost]
    public IActionResult Create(GroupDatatransformer.Create create)
    {
        var group = _groupService.Create(_jwtService.ReadToken(Request).userId, create.groupName);
        return Ok(group);
    }

    [HttpGet]
    public IActionResult List()
    {
        var groups = _groupService.List(_jwtService.ReadToken(Request).userId);
        return Ok(groups);
    }

    [HttpGet("{groupId}")]
    public IActionResult Info([FromRoute] string groupId)
    {
        var info = _groupService.Info(_jwtService.ReadToken(Request).userId, groupId);
        return Ok(info);
    }

    [HttpDelete("{groupId}")]
    public IActionResult Remove(string groupId)
    {
        _groupService.Remove(_jwtService.ReadToken(Request).userId, groupId);
        return Ok(null);
    }

    [HttpPost($"{nameof(AddMember)}")]
    public IActionResult AddMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var message = _groupService.InviteMember(_jwtService.ReadToken(Request).userId, modifyMember);
        return Ok(message);
    }

    [HttpPatch($"{nameof(RemoveMember)}")]
    public IActionResult RemoveMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var remove = _groupService.RemoveMember(_jwtService.ReadToken(Request).userId, modifyMember);
        return Ok(remove);
    }

    [HttpGet(nameof(ListStogare) + "/{groupId}")]
    public IActionResult ListStogare([FromRoute] string groupId)
    {
        var stogares = _groupService.ListStogare(_jwtService.ReadToken(Request).userId, groupId);
        return Ok(stogares);
    }

    [HttpPatch(nameof(Rename))]
    public IActionResult Rename([FromBody] GroupDatatransformer.Rename rename)
    {
        var stogare = _groupService.Rename(_jwtService.ReadToken(Request).userId, rename);
        return Ok(stogare);
    }

    [HttpGet(nameof(ListDestination) + "/{groupId}/{stogareId}")]
    public IActionResult ListDestination([FromRoute] string groupId, string stogareId)
    {
        var destinations = _groupService.ListDestination(_jwtService.ReadToken(Request).userId, groupId, stogareId);
        return Ok(destinations);
    }

    [HttpGet(nameof(ListRequest))]
    public IActionResult ListRequest([FromRoute] string groupId, string stogareId)
    {
        var groups = _groupService.ListRequest(_jwtService.ReadToken(Request).userId);
        return Ok(groups);
    }

    [Authorize]
    [HttpPatch(nameof(AcceptInvite) + "/{groupId}")]
    public IActionResult AcceptInvite([FromRoute] string groupId)
    {
        var accept = _groupService.AcceptInvite(_jwtService.ReadToken(Request).userId, groupId);
        return Ok(accept);
    }

    [Authorize]
    [HttpPost(nameof(ChangeImage) + "/{groupId}")]
    public async Task<IActionResult> ChangeImage([FromRoute] string groupId, [FromForm] IFormCollection form)
    {
        var group = await _groupService.ChangeImage(_jwtService.ReadToken(Request).userId, groupId, form.Files[0]);
        return Ok(group);
    }
}
