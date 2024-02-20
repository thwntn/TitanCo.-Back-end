namespace ReferenceController;

[ApiController]
[Route(nameof(Group))]
public class Group(IGroup groupService, ISecurity securityService) : Controller
{
    private readonly IGroup _groupService = groupService;
    private readonly ISecurity _securityService = securityService;

    [HttpPost]
    public IActionResult Create(GroupDatatransformer.Create create)
    {
        var group = _groupService.Create(_securityService.ReadToken(Request).userId, create.groupName);
        return Ok(group);
    }

    [HttpGet]
    public IActionResult List()
    {
        var groups = _groupService.List(_securityService.ReadToken(Request).userId);
        return Ok(groups);
    }

    [HttpDelete("{groupId}")]
    public IActionResult Remove(int groupId)
    {
        _groupService.Remove(_securityService.ReadToken(Request).userId, groupId);
        return Ok(null);
    }

    [HttpPatch($"{nameof(AddMember)}")]
    public IActionResult AddMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var message = _groupService.InviteMember(_securityService.ReadToken(Request).userId, modifyMember);
        return Ok(message);
    }

    [HttpPatch($"{nameof(RemoveMember)}")]
    public IActionResult RemoveMember(GroupDatatransformer.ModifyMember modifyMember)
    {
        var remove = _groupService.RemoveMember(_securityService.ReadToken(Request).userId, modifyMember);
        return Ok(remove);
    }

    [HttpGet(nameof(ListStogare) + "/{groupId}/{stogareId}")]
    public IActionResult ListStogare([FromRoute] int groupId, [FromRoute] int stogareId)
    {
        var stogares = _groupService.ListStogare(_securityService.ReadToken(Request).userId, groupId, stogareId);
        return Ok(stogares);
    }

    [HttpPatch(nameof(Rename))]
    public IActionResult Rename([FromBody] GroupDatatransformer.Rename rename)
    {
        var stogare = _groupService.Rename(_securityService.ReadToken(Request).userId, rename);
        return Ok(stogare);
    }

    [HttpGet(nameof(ListDestination) + "/{groupId}/{stogareId}")]
    public IActionResult ListDestination([FromRoute] int groupId, int stogareId)
    {
        var destinations = _groupService.ListDestination(
            _securityService.ReadToken(Request).userId,
            groupId,
            stogareId
        );
        return Ok(destinations);
    }

    [Authorize]
    [HttpPatch(nameof(AcceptInvite) + "/{groupId}")]
    public IActionResult AcceptInvite([FromRoute] int groupId)
    {
        var accept = _groupService.AcceptInvite(_securityService.ReadToken(Request).userId, groupId);
        return Ok(accept);
    }
}