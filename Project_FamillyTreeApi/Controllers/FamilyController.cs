using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : Controller
    {
        private readonly FamilyRepository _jWTManager;
        private readonly PRN231FamilyTreeContext _dbContext;

        public FamilyController(FamilyRepository jWTManager, PRN231FamilyTreeContext context)
        {
            _jWTManager = jWTManager;
            _dbContext = context;
        }

        [HttpGet("all")]
        public List<Family> GetAllFamily()
        {
            var list = _jWTManager.GetAllFamily();
            return list;
        }
        [HttpGet("{id}")]
        public Family GetFamilyById(int id)
        {
            return _jWTManager.GetFamily(id);
        }


        [HttpGet("{familyId}/tree")]
        public IActionResult GetFamilyTree(int familyId)
        {
            var family = _dbContext.Families
                .Include(f => f.FamilyMembers)
                    .ThenInclude(m => m.Relatives)
                        .ThenInclude(r => r.Relation)
                .FirstOrDefault(f => f.Id == familyId);

            if (family == null)
            {
                return NotFound();
            }

            var familyTree = new List<FamilyMemberNode>();

            foreach (var member in family.FamilyMembers)
            {
                var memberNode = BuildFamilyTree(family.FamilyMembers.ToList(), member.Id);

                if (memberNode != null)
                {
                    familyTree.Add(memberNode);
                }
            }
            return Ok(familyTree);
        }

        //[HttpPost("tree")]
        //public IActionResult CreateFamilyTree([FromBody] FamilyTreeRequest request)
        //{
        //    var family = _dbContext.Families.FirstOrDefault(f => f.ID == request.FamilyId);

        //    if (family == null)
        //    {
        //        return NotFound();
        //    }

        //    var familyTree = BuildFamilyTree(request.Members, request.Relatives);

        //    // Lưu cây gia đình vào cơ sở dữ liệu
        //    SaveFamilyTreeToDatabase(familyTree, family.ID);

        //    return Ok();
        //}

        //private void SaveFamilyTreeToDatabase(FamilyMemberNode memberNode, int familyId)
        //{
        //    var member = new FamilyMember
        //    {
        //        FullName = memberNode.FullName,
        //        Gender = memberNode.Gender,
        //        Dob = memberNode.DOB,
        //        Phone = memberNode.Phone,
        //        Email = memberNode.Email,
        //        Address = memberNode.Address,
        //        FamilyId = familyId
        //    };

        //    _dbContext.FamilyMembers.Add(member);
        //    _dbContext.SaveChanges();

        //    foreach (var relativeNode in memberNode.Relatives)
        //    {
        //        var relation = _dbContext.Relationships.FirstOrDefault(r => r.RelationType == relativeNode.RelationType);

        //        var relativeMember = _dbContext.FamilyMembers.FirstOrDefault(m => m.ID == relativeNode.ID);

        //        var relative = new Relative
        //        {
        //            Id = member.Id,
        //            RelationId = relation.Id,
        //            MemberRelativeId = relativeMember.ID,
        //            Family = familyId
        //        };

        //        _dbContext.Relatives.Add(relative);
        //    }

        //    _dbContext.SaveChanges();
        //}


        private FamilyMemberNode BuildFamilyTree(List<FamilyMember> members, int memberId)
        {
            var rootMember = members.FirstOrDefault(m => m.Id == memberId);
            if (rootMember == null)
            {
                return null;
            }

            var memberNode = new FamilyMemberNode
            {
                ID = rootMember.Id,
                FullName = rootMember.FullName,
                Gender = rootMember.Gender,
                DOB = rootMember.Dob,
                Phone = rootMember.Phone,
                Email = rootMember.Email,
                Address = rootMember.Address,
                Relatives = new List<FamilyMemberNode>()
            };

            var relatives = rootMember.Relatives.Where(r => r.MemberRelativeId != memberId).ToList();

            foreach (var relative in relatives)
            {
                var relation = relative.Relation;
                var relativeMemberId = relative.MemberRelativeId;
                var relativeMemberNode = BuildFamilyTree(members, relativeMemberId);

                if (relativeMemberNode != null)
                {
                    relativeMemberNode.RelationType = relation.RelationType;
                    memberNode.Relatives.Add(relativeMemberNode);
                }
            }
            return memberNode;
        }



        [HttpGet("search")]
        public List<Family> SearchFamilies(string family)
        {
            var list = _jWTManager.SearchFamily(family);
            return list;
        }

        [HttpPost]
        public IActionResult CreateFamily([FromBody] Family family)
        {
            _jWTManager.Add(family);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateFamily([FromBody] Family family)
        {
            _jWTManager.Update(family);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFamily(int id)
        {
            var family = _jWTManager.GetFamily(id);
            if (family == null)
            {
                return NotFound();
            }
            _jWTManager.Delete(family);
            return Ok();
        }
    }
}