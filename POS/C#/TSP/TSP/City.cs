using System.ComponentModel.DataAnnotations.Schema;

namespace TSP;

[Table("places")]
public class City
{
    [Column("pk_uid")]
    public int PK_UID {get; set;}
    public required string Name { get; set; }
    public required string Adm0name {get; set;}
    public required string Adm0_a3 {get; set;}
    public required string Adm1name {get; set;}
    public required double Latitude {get; set;}
    public required double Longitude { get; set; }
}
