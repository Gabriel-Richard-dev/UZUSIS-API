using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using UZUSIS.Application.Dtos.Administrador;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;
using UZUSIS.Core.Settings;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class AdminAuthService : BaseService, IAdminAuthService
{
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _hasher;
    public AdminAuthService(INotificator notificator, IMapper mapper, 
        IAdministradorRepository administradorRepository,
        IJwtService jwtService, IOptions<JwtSettings> jwtSettings) : base(notificator, mapper)
    {
        _administradorRepository = administradorRepository;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> Login(LoginUsuarioDto adminDto)
    {
        var admin = await _administradorRepository.Obter(adminDto.Email);
        
        if (admin is null)
        {
            Notificator.Handle("Admin nao existe");
            return string.Empty;
        }
        if(_hasher.VerifyHashedPassword(admin, admin.Senha, adminDto.Senha) != PasswordVerificationResult.Failed)
            return await GenerateToken(admin);

        throw new InvalidCredentialException("entrei na exception");
    }

    private async Task<string> GenerateToken(Administrador admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = await _jwtService.GetCurrentSigningCredentials();

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Role, ETipoUsuario.Administrador.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours((int)_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    
}