using System;
using System.Collections.Generic;

public class FamilyMemberNode
{
    public int ID { get; set; }
    public string FullName { get; set; }
    public int Gender { get; set; }
    public DateTime DOB { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string RelationType { get; set; }
    public List<FamilyMemberNode> Relatives { get; set; }
}