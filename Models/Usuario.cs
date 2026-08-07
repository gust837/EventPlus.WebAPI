using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

[Index("Email", Name = "UQ__Usuario__A9D10534E4B87371", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("idUsuario")]
    public Guid IdUsuario { get; set; }

    [Column("idTipoUsuario")]
    public Guid? IdTipoUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [ForeignKey("IdTipoUsuario")]
    [InverseProperty("Usuario")]
    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();
}
