using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Administrador : Usuario
{
    public Administrador()
    {
        TipoUsuario = ETipoUsuario.Administrador;
    }
    
}