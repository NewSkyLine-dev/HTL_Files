using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osterhasen_Hilfsprogramm;

[Table("Person")]
public class Person
{
    [PrimaryKey, Identity, NotNull]
    public int PersonID { get; set; }

    [Column, NotNull]
    public string Name { get; set; } = string.Empty;

    [Column]
    public double Latitude { get; set; }

    [Column]
    public double Longitude { get; set; }
}