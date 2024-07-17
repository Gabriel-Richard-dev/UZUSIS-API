using UZUSIS.Core.Enums;

namespace UZUSIS.Domain.Abstractions;

public abstract class Usuario : Entity
{
    public ETipoUsuario TipoUsuario { get; set; }
}