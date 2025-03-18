﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace workForum.Models;

public partial class Message
{
    [Key]
    public int M_Id { get; set; }

    public int A_Id { get; set; }

    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string Account { get; set; }

    [Required]
    [StringLength(100)]
    public string Content { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }
}