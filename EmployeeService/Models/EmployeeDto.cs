using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class EmployeeDto
{
    [DataMember(Order = 1)]
    public int ID { get; set; }
    [DataMember(Order = 2)]
    public string Name { get; set; }
    [DataMember(Order = 3)]
    public int? ManagerID { get; set; }
    [DataMember(Order = 4)]
    public bool Enable { get; set; }
    [DataMember(Order = 5)]
    public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
}