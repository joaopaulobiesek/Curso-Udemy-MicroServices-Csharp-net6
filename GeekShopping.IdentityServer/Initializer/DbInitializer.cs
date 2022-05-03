using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<ApplicationUser> _role;

        public DbInitializer(RoleManager<ApplicationUser> role, UserManager<ApplicationUser> user, MySQLContext context)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initializer()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "joaopaulobiesek",
                Email = "joaopaulobiesek@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (77) 99914-6515",
                FirstName = "João Paulo",
                LastName = "Biesek de Oliveira"
            };

            _user.CreateAsync(admin, "*Joao123").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();
            var adminClaims = _user.AddClaimAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}")
            }).Result;
        }
    }
}
