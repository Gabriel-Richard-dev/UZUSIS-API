using UZUSIS.Application.Dtos.Endereco;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Dtos.Cliente;

public class AdicionarClienteDto : AdicionarUsuarioDto
{
    public string CPF { get; set; }
    public string Celular { get; set; }
    public DateTime DataNascimento { get; set; }
}