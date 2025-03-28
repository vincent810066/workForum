﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace workForum.Models;

public partial class Album
{
    [Key]
    public int Alb_Id { get; set; }

    [Required]
    [StringLength(50)]
    [DisplayName("相片名稱")]
    public string FileName { get; set; }

    [Required]
    public string Url { get; set; }
    [DisplayName("相片尺寸")]
    public int Size { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Type { get; set; }

    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string Account { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("上傳時間")]
    public DateTime CreateTime { get; set; }
}