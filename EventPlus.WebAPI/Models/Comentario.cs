using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Comentario
{
    [Key]
    [Column("idComentario")]
    public Guid IdComentario { get; set; }

    [Column("idUsuario")]
    public Guid? IdUsuario { get; set; }

    [Column("idEvento")]
    public Guid? IdEvento { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataComentario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Descricao { get; set; } = null!;

    public bool Exibe { get; set; }

    [ForeignKey("IdEvento")]
    [InverseProperty("Comentario")]
    public virtual Evento? IdEventoNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Comentario")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
