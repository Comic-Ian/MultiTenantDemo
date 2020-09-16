using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenantDemo3.Model
{
    [Table("Tb_People")]
    public class People
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public int BirNum { get; set; }
    }
}
