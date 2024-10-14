using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetStockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("MultipleOrigins")]
public class AuthenticateController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    // ฟังก์ชันสร้าง Constructor สำหรับ initial ค่าของ ApplicationDbContext
    public AuthenticateController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    // test connect database
    [HttpGet("testconnectdb")]
    public void TestConnection()
    {
        if (_context.Database.CanConnect())
        {
            Response.WriteAsync("Connected");
        }
        else
        {
            Response.WriteAsync("Not Connected");
        }
    }

    // Register for User
    [HttpPost]
    [Route("register-user")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterModel model)
    {
        // เช็คว่า username ซ้ำหรือไม่
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User already exists!"
                }
            );
        }

        // check email
        userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "Email already exists!"
                }
            );
        }

        // create User
        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                }
            );
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
        } 

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Manager));
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
            await _userManager.AddToRoleAsync(user, UserRolesModel.User);
        } else {
            await _userManager.AddToRoleAsync(user, UserRolesModel.User);
        }

        return Ok(new ResponseModel
        {
            Status = "Success",
            Message = "User registered successfully"
        });
    }
    
    // Register for Manager
    // Post api/authenticate/register-manager
    [HttpPost]
    [Route("register-manager")]
    public async Task<ActionResult> RegisterManager([FromBody] RegisterModel model)
    {
        // เช็คว่า username ซ้ำหรือไม่
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User already exists!"
                }
            );
        }

        userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "Email already exists!"
                }
            );
        }

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        // ถ้าสร้างไม่สำเร็จ
        if (!result.Succeeded)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                }
            );
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
        } 

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
        } 

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Manager));
        }else {
            await _userManager.AddToRoleAsync(user, UserRolesModel.Manager);
        }

        return Ok(new ResponseModel
        {
            Status = "Success",
            Message = "User registered successfully"
        });
    }

    // Register for Admin
    // Post api/authenticate/register-admin
    [HttpPost]
    [Route("register-admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User already exists!"
                }
            );
        }

        userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "Email already exists!"
                }
            );
        }

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                }
            );
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
        } 

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Manager));
        }

        if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
        } else {
            await _userManager.AddToRoleAsync(user, UserRolesModel.Admin);
        }

        return Ok(new ResponseModel
        {
            Status = "Success",
            Message = "User registered successfully"
        });
    }
    
    // Login for User
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username!);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userData = new
                {
                    userName = user.UserName,
                    email = user.Email,
                    roles = userRoles
                }
            });
        }

        return Unauthorized();
    }

    // ฟังก์ชันสร้าง Token
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1), // 24 hours
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;

    }

    // Logout
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name;
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return Ok(new ResponseModel { Status = "Success", Message = "User logged out!" });
            }
        }
        return Ok();
    }
    
    // Refresh Token
    [HttpPost]
    [Route("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenModel model)
    {
        var authHeader = Request.Headers["Authorization"];
        if (authHeader.ToString().StartsWith("Bearer"))
        {
            var token = authHeader.ToString().Substring(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user = new
                {
                    Name = jwtToken.Claims.First(x => x.Type == "unique_name").Value,
                    Role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value
                };

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var newToken = GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(newToken),
                    expiration = newToken.ValidTo
                });
            }
            catch
            {
                return Unauthorized();
            }
        }

        return Unauthorized();
    }
    

    public class RefreshTokenModel
    {
        public required string Token { get; set; }
    }

    // Validate Token Endpoint
    [HttpPost]
    [Route("validate-token")]
    public IActionResult ValidateToken([FromBody] TokenValidationModel model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!);

        try
        {
            tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, 
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var roles = jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            return Ok(new
            {
                Status = "Success",
                UserName = userName,
                Roles = roles
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new
            {
                Status = "Error",
                Message = "Token is not valid",
                Details = ex.Message
            });
        }
    }

    public class TokenValidationModel
    {
        public required string Token { get; set; }
    }

}