using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Cliente : Usuario
{
    public Cliente()
    {
        TipoUsuario = ETipoUsuario.Cliente;
    }   
}