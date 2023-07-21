using BusinessObject.DataAccess;
using BusinessObject.DTO;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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



        [HttpGet("{memberID}/tree1")]
        public IActionResult GetFamilyTree1(int memberID)
        {
            var memberFamily = _dbContext.FamilyMembers.FirstOrDefault(c => c.Id == memberID);

            if (memberFamily == null)
            {
                return NotFound();
            }

            var family = _dbContext.Families
                .Include(f => f.FamilyMembers)
                    .ThenInclude(m => m.Relatives)
                        .ThenInclude(r => r.Relation)
                .FirstOrDefault(f => f.Id == memberFamily.FamilyId);

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

        [HttpPost("addNode")]
        public IActionResult AddFamilyNode([FromBody] FamilyMemberNode newNode)
        {
            if (newNode == null)
            {
                return BadRequest("Invalid request");
            }

            using (var dbContext = new PRN231FamilyTreeContext())
            {
                var existingMember = dbContext.FamilyMembers.FirstOrDefault(m => m.FullName == newNode.FullName && m.FamilyId == newNode.FamilyId);

                if (existingMember != null)
                {
                    // Node đã tồn tại, gắn mối quan hệ cho node đó và node mới ở trên hoặc dưới
                    foreach (var relativeNode in newNode.Relatives)
                    {
                        var relation = dbContext.Relationships.FirstOrDefault(r => r.RelationType == relativeNode.RelationType);

                        var relativeMember = dbContext.FamilyMembers.FirstOrDefault(m => m.FullName == relativeNode.FullName && m.FamilyId == newNode.FamilyId);

                        if (relativeMember == null)
                        {
                            relativeMember = new FamilyMember
                            {
                                FullName = relativeNode.FullName,
                                Gender = relativeNode.Gender,
                                Dob = relativeNode.DOB,
                                Phone = relativeNode.Phone,
                                Email = relativeNode.Email,
                                Address = relativeNode.Address,
                                FamilyId = newNode.FamilyId
                            };

                            dbContext.FamilyMembers.Add(relativeMember);
                            dbContext.SaveChanges();
                        }

                        var relative = new Relative
                        {
                            MemberId = existingMember.Id,
                            MemberRelativeId = relativeMember.Id,
                            RelationId = relation.Id,
                            FamilyId = newNode.FamilyId
                        };

                        dbContext.Relatives.Add(relative);
                    }
                }
                else
                {
                    // Node không tồn tại, tạo mới node và thêm mối quan hệ giữa node đã tồn tại và node mới ở trên hoặc dưới
                    var familyMember = new FamilyMember
                    {
                        FullName = newNode.FullName,
                        Gender = newNode.Gender,
                        Dob = newNode.DOB,
                        Phone = newNode.Phone,
                        Email = newNode.Email,
                        Address = newNode.Address,
                        FamilyId = newNode.FamilyId
                    };

                    dbContext.FamilyMembers.Add(familyMember);
                    dbContext.SaveChanges();

                    foreach (var relativeNode in newNode.Relatives)
                    {
                        var relation = dbContext.Relationships.FirstOrDefault(r => r.RelationType == relativeNode.RelationType);

                        var relativeMember = dbContext.FamilyMembers.FirstOrDefault(m => m.FullName == relativeNode.FullName && m.FamilyId == newNode.FamilyId);

                        if (relativeMember == null)
                        {
                            relativeMember = new FamilyMember
                            {
                                FullName = relativeNode.FullName,
                                Gender = relativeNode.Gender,
                                Dob = relativeNode.DOB,
                                Phone = relativeNode.Phone,
                                Email = relativeNode.Email,
                                Address = relativeNode.Address,
                                FamilyId = newNode.FamilyId
                            };

                            dbContext.FamilyMembers.Add(relativeMember);
                            dbContext.SaveChanges();
                        }

                        var relative = new Relative
                        {
                            MemberId = familyMember.Id,
                            MemberRelativeId = relativeMember.Id,
                            RelationId = relation.Id,
                            FamilyId = familyMember.FamilyId
                        };

                        dbContext.Relatives.Add(relative);
                    }
                }

                dbContext.SaveChanges();

                var familyTree = BuildFamilyTree(dbContext.FamilyMembers.ToList(), newNode.FamilyId);

                return Ok(familyTree);
            }
        }



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
                Relatives = new List<FamilyMemberNode>(),
                FamilyId = rootMember.FamilyId,
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

        [HttpGet("{familyId}/members")]
        public IActionResult GetAllFamilyMemberByFamily(int familyId)
        {
            try
            {
                var familyMembers = _jWTManager.GetAllFamilyMemberByFamily(familyId);
                return Ok(familyMembers);
            }
            catch (Exception ex)
            {
                // Handle exception and return appropriate response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}