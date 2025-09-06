using Auth.DataAccess.AppDbContexts;
using Auth.Service.DTOs.Permissions;
using Auth.Service.DTOs.Roles;
using Auth.Service.DTOs.Users;
using Auth.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Auth.Service.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(
    AppDbContext db,
    IUserService userService,
    IPermissionService permissionService,
    IRoleService roleService,
    ILogger logger)
    {
        //Adding permissions
        if (!await db.Permissions.AnyAsync())
        {
            var permissions = new List<PermissionForCreateDto>
        {
            new() { Id = Guid.Parse("1905e421-80d5-49f1-9b4b-904e2369006f"), Name = "Permission_Get", Description = "Permission_Get"},
            new() { Id = Guid.Parse("782a10cb-a697-4389-bd43-dc43d21d59bf"), Name = "Permission_Delete", Description = "Permission_Delete"},
            new() { Id = Guid.Parse("e7e8d3d0-1388-4742-8897-d5af6f1afc53"), Name = "Permission_Update", Description = "Permission_Update"},
            new() { Id = Guid.Parse("4f42f1a6-0abb-4678-8744-cb24d209cb2c"), Name = "Permission_Create", Description = "Permission_Create"},
            new() { Id = Guid.Parse("a644ad33-fdc0-4137-aa81-2389d1caaac1"), Name = "Role_Get", Description = "Role_Get"},
            new() { Id = Guid.Parse("229c93fa-9757-44c0-8ffc-bd197b4e0212"), Name = "Role_Delete", Description = "Role_Delete"},
            new() { Id = Guid.Parse("f565ff60-a69c-4bc6-b6c3-9e7dd4ca6b99"), Name = "Role_Update", Description = "Role_Update"},
            new() { Id = Guid.Parse("780ae01b-a51f-437e-8ffd-190b08c141da"), Name = "Role_Create", Description = "Role_Create"},
            new() { Id = Guid.Parse("0196ec44-a809-780c-9292-f45abf7ae43d"), Name = "User_Get", Description = "User_Get" },
            new() { Id = Guid.Parse("0196ec45-6a24-775c-91cb-002e412b3dfb"), Name = "User_Create", Description = "User_Create" },
            new() { Id = Guid.Parse("0196ec45-94a7-75ac-8403-2e004b739e43"), Name = "User_Delete", Description = "User_Delete" },
            new() { Id = Guid.Parse("0196ec45-c6bf-7e94-b5bf-5abff77cd823"), Name = "User_Update", Description = "User_Update" }
        };
            foreach (var permission in permissions)
            {
                await permissionService.CreateAsync(permission);
            }

            await db.SaveChangesAsync(); // Saving changes to the database
            logger.LogInformation("✅ Permissionlar yaratildi: {Count}", permissions.Count);
        }

        // ✅ 2. SuperAdmin rolini yaratish va permissionlarni biriktirish
        var existingRole = await db.Roles
            .Include(r => r.RolePermission)
            .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

        Guid roleId;

        if (existingRole is null)
        {
            var allPermissionIds = await db.Permissions.Select(p => p.Id).ToListAsync();

            var role = await roleService.CreateAsync(new RoleForCreationDto
            {
                Name = "SuperAdmin",
                Description = "All-access Super Admin",
                Permissions = allPermissionIds
            });

            await db.SaveChangesAsync(); // ✅ Saqlash
            logger.LogInformation("✅ SuperAdmin roli yaratildi va permissionlar biriktirildi.");

            roleId = role.Id;
        }

        else
        {
            logger.LogInformation("ℹ️ SuperAdmin roli mavjud. ID: {RoleId}", existingRole.Id);
            roleId = existingRole.Id;
        }

        // ✅ 3. Admin foydalanuvchini yaratish
        if (!await db.Users.AnyAsync())
        {
            var userDto = new UserForCreationDto
            {
                FullName = "Admin User",
                Email = "bekhzodkeldiyorov@gmail.com",
                Password = "tg092Oq5xCNPlciMeH",
                RoleId = roleId
            };

            await userService.CreateAsync(userDto);
            await db.SaveChangesAsync(); // ✅ Saqlash
            logger.LogInformation("✅ Admin foydalanuvchi yaratildi va SuperAdmin roliga biriktirildi.");
        }
        else
        {
            logger.LogInformation("ℹ️ Admin foydalanuvchi mavjud.");
        }
    }
}
