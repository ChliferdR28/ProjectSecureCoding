using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSecureCoding.Models
{
    public class Mahasiswa
    {
        [Key]
        public int Nim { get; set; }
        public string Nama { get; set; } = null!;
        public string Alamat { get; set; } = null!;
    }
}