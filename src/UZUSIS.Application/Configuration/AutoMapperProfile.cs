using AutoMapper;
using UZUSIS.Application.Dtos.Administrador;
using UZUSIS.Application.Dtos.Foto;
using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Application.Dtos.Tamanho;
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
        
        #region Tamanho

        CreateMap<TamanhoDto, Tamanho>().ReverseMap();

        #endregion
        
        #region Produto

        CreateMap<ProdutoDto, Produto>().ReverseMap();
        CreateMap<AtualizarProdutoDto, Produto>().ReverseMap();
        CreateMap<AdicionarProdutoDto, Produto>().ReverseMap();
        CreateMap<AdicionarProdutoDto, ProdutoDto>().ReverseMap();
        
        #endregion

        #region Foto

        CreateMap<Foto, FotoProdutoDto>().ReverseMap();

        #endregion

    }
}