namespace ReferenceController;

[ApiController]
[Route(nameof(Role))]
public class Role(IRole roleService) : Controller
{
    private readonly IRole _roleService = roleService;

    [Authorize]
    [HttpGet(nameof(RoleAccount))]
    public IActionResult RoleAccount()
    {
        var list = _roleService.RoleAccount();
        return Ok(list);
    }

    [Authorize]
    [HttpPatch(nameof(AssignRole))]
    public IActionResult AssignRole([FromBody] RoleDatabaseTransformer.AssignRole assignRole)
    {
        var roleAccount = _roleService.AssignRole(assignRole);
        return Ok(roleAccount);
    }

    [Authorize]
    [HttpPatch(nameof(UnsignRole))]
    public IActionResult UnsignRole([FromBody] RoleDatabaseTransformer.UnsignRole unsignRole)
    {
        var message = _roleService.UnsignRole(unsignRole);
        return Ok(message);
    }

    [HttpPatch(nameof(MakeAdmin))]
    public IActionResult MakeAdmin([FromBody] RoleDatabaseTransformer.MakeAdmin makeAdmin)
    {
        var roles = _roleService.MakeAdminAccount(makeAdmin.AccountId);
        return Ok(roles);
    }

    [Authorize]
    [HttpGet]
    public IActionResult List()
    {
        var list = _roleService.List();
        return Ok(list);
    }
}
