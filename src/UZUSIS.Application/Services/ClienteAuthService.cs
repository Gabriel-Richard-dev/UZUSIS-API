using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Token;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;
using UZUSIS.Core.Settings;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ClienteAuthService : BaseService, IClienteAuthService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordHasher<Cliente> _hasher;

    public ClienteAuthService(INotificator notificator, IMapper mapper,
        IClienteRepository clienteRepository, IPasswordHasher<Cliente> hasher,
        IJwtService jwtService, IOptions<JwtSettings> jwtSettings) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
        _hasher = hasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(LoginUsuarioDto loginUsuarioDto)
    {
        var cliente = await _clienteRepository.Obter(loginUsuarioDto.Email);

        if (cliente is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        bool senhaValida = (_hasher.VerifyHashedPassword(cliente, 
                                cliente.Senha, 
                                loginUsuarioDto.Senha) !=
                            PasswordVerificationResult.Failed);

        if (senhaValida)
            return new TokenDto
            {
                Token = await GenerateToken(cliente)
            };

        Notificator.Handle("Não foi possivel realizar o login");
        return null;
    }
    
    
    
    
    
    
    private async Task<string> GenerateToken(Cliente cliente)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = await _jwtService.GetCurrentSigningCredentials();

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Role, ETipoUsuario.Cliente.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours((int)_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    
}