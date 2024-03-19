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
        var group = _groupService.Create(create.GroupName);
        return Ok(group);
    }

    [HttpGet]
    public IActionResult List()
    {
        var groups = _groupService.List();
        return Ok(groups);
    }

    [HttpGet("{groupId}")]
    public IActionResult Info([FromRoute] Guid groupId)
    {
        var info = _groupService.Info(groupId);
        return Ok(info);
    }

    [HttpDelete("{groupId}")]
    public IActionResult Remove(Guid groupId)
    {
        _groupService.Remove(groupId);
        return Ok(null);
    }

    [HttpPost($"{nameof(AddMember)}")]
    public IActionResult AddMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var message = _groupService.InviteMember(modifyMember);
        return Ok(message);
    }

    [HttpPatch($"{nameof(RemoveMember)}")]
    public IActionResult RemoveMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var remove = _groupService.RemoveMember(modifyMember);
        return Ok(remove);
    }

    [HttpGet(nameof(ListStogare) + "/{groupId}")]
    public IActionResult ListStogare([FromRoute] Guid groupId)
    {
        var stogares = _groupService.ListStogare(groupId);
        return Ok(stogares);
    }

    [HttpPatch(nameof(Rename))]
    public IActionResult Rename([FromBody] GroupDatatransformer.Rename rename)
    {
        var stogare = _groupService.Rename(rename);
        return Ok(stogare);
    }

    [HttpGet(nameof(ListDestination) + "/{groupId}/{stogareId}")]
    public IActionResult ListDestination([FromRoute] Guid groupId, Guid stogareId)
    {
        var destinations = _groupService.ListDestination(groupId, stogareId);
        return Ok(destinations);
    }

    [HttpGet(nameof(ListRequest))]
    public IActionResult ListRequest([FromRoute] Guid groupId, Guid stogareId)
    {
        var groups = _groupService.ListRequest();
        return Ok(groups);
    }

    [Authorize]
    [HttpPatch(nameof(AcceptInvite) + "/{groupId}")]
    public IActionResult AcceptInvite([FromRoute] Guid groupId)
    {
        var accept = _groupService.AcceptInvite(groupId);
        return Ok(accept);
    }

    [Authorize]
    [HttpPost(nameof(ChangeImage) + "/{groupId}")]
    public async Task<IActionResult> ChangeImage([FromRoute] Guid groupId, [FromForm] IFormCollection form)
    {
        var group = await _groupService.ChangeImage(groupId, form.Files[0]);
        return Ok(group);
    }
}
