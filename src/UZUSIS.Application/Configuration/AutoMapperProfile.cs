using AutoMapper;
using UZUSIS.Application.Dtos.Administrador;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        #region Administrador

        CreateMap<Administrador, AdministradorDto>().ReverseMap();
        CreateMap<Administrador, AdicionarUsuarioDto>().ReverseMap();
        CreateMap<Administrador, LoginUsuarioDto>().ReverseMap();

        #endregion

    }
}