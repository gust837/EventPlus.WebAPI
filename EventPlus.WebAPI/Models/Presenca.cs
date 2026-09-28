using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Presenca
{
    [Key]
    [Column("idPresenca")]
    public Guid IdPresenca { get; set; }

    [Column("idUsuario")]
    public Guid? IdUsuario { get; set; }

    [Column("idEvento")]
    public Guid? IdEvento { get; set; }

    public bool Situacao { get; set; }

    [ForeignKey("IdEvento")]
    [InverseProperty("Presenca")]
    public virtual Evento? IdEventoNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Presenca")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
