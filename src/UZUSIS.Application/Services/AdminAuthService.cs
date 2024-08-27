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
using UZUSIS.Application.Dtos.Token;
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
        IJwtService jwtService, IOptions<JwtSettings> jwtSettings, IPasswordHasher<Administrador> hasher) : base(notificator, mapper)
    {
        _administradorRepository = administradorRepository;
        _jwtService = jwtService;
        _hasher = hasher;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(LoginUsuarioDto adminDto)
    {
        var admin = await _administradorRepository.Obter(adminDto.Email)!;
        
        if (admin is null)
        {
            Notificator.Handle("Admin nao existe");
            return null;
        }

        var validation = _hasher.VerifyHashedPassword(admin, admin.Senha, adminDto.Senha)
                          != PasswordVerificationResult.Failed;
        
        if(validation)
        {
            return new TokenDto()
            {
                Token = await GenerateToken(admin)
            };
        }
        
        return null;
    }

    public async Task<string> GenerateToken(Administrador administrador)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, administrador.Nome));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, administrador.Email));
        
        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Emissor,
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key,
        });

        return tokenHandler.WriteToken(token);
    }
    
}