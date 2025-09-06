namespace Auth.Service.DTOs.Permissions;

public class PermissionDto
{
    public string Name { get; set; }
    public List<Dictionary<string, bool>>? PermissinForCRUDDtos { get; set; }
}
