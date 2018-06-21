using System;
using System.Collections.Generic;
using System.Linq;
using KnowledgeManagement.BLL.Mapper;
using KnowledgeManagement.BLL.Services;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;
using Moq;
using NUnit.Framework;

namespace KnowledgeManagement.BLLTest
{
    [TestFixture]
    public class SkillsTest
    {
        private ISkillService _skillService;
        private ISubSkillService _subSkillService;
        [SetUp]
        public void SetUp()
        {
            var listSkills = new Skill[]
            {
                new Skill() {Id = 1, Name = "Nokia Lumia 630"},
                new Skill() {Id = 2, Name = "Programming languages"},
                new Skill() {Id = 3, Name = "Databases"}
            };

            Mock<IRepository<Skill>> skillRepositoy = new Mock<IRepository<Skill>>();
            skillRepositoy.Setup(m => m.GetAll()).Returns(listSkills.AsQueryable());
            skillRepositoy.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) => { return skillRepositoy.Object.GetAll().FirstOrDefault(x => x.Id == id); });


            Mock<IRepository<SubSkill>> subSkillRepositoy = new Mock<IRepository<SubSkill>>();
            var listSubSkills = new List<SubSkill>();
            var skill1 = skillRepositoy.Object.GetAll().FirstOrDefault(x => x.Name == "Programming languages");
            int subskillId = 1;
            if (skill1 != null)
            {
                listSubSkills.Add(new SubSkill()
                {
                    SkillId = skill1.Id,
                    Name = "C/C++",
                    Id = subskillId++
                });
                listSubSkills.Add(new SubSkill()
                {
                    SkillId = skill1.Id,
                    Name = "JavaScript / HTML / CSS"
                });
                listSubSkills.Add(new SubSkill()
                {
                    SkillId = skill1.Id,
                    Name = "Delphi",
                    Id = subskillId++
                });
            }
            var skill2 = skillRepositoy.Object.GetAll().FirstOrDefault(x => x.Name == "Databases");
            if (skill2 != null)
            {
                listSubSkills.Add(new SubSkill()
                {
                    SkillId = skill2.Id,
                    Name = "Microsoft SQL Server",
                    Id = subskillId++
                });
                listSubSkills.Add(new SubSkill()
                {
                    SkillId = skill2.Id,
                    Name = "Oracle",
                    Id = subskillId++
                });
            }

            
            subSkillRepositoy.Setup(m => m.GetAll()).Returns(listSubSkills.AsQueryable());
            subSkillRepositoy.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                (int id) => { return subSkillRepositoy.Object.GetAll().FirstOrDefault(x => x.Id == id); });

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Skills).Returns(skillRepositoy.Object);
            unitOfWork.Setup(x => x.SubSkills).Returns(subSkillRepositoy.Object);
            _skillService = new SkillService(unitOfWork.Object, new MapperFactory());
            _subSkillService = new SubSkillService(unitOfWork.Object, new MapperFactory()); 
        }

        [Test]
        public void GetAll()
        {
            Assert.IsTrue(_skillService.GetAll().ToList().Count == 3);
            Assert.AreEqual(_skillService.GetAll().ToList()[0].Name, "Nokia Lumia 630");
            Assert.AreEqual(_skillService.GetByIdAsync(1).Result.Name, "Nokia Lumia 630");
        }

        [Test]
        public void GetByIdAsync()
        {
            Assert.AreEqual(_skillService.GetByIdAsync(1).Result.Name, "Nokia Lumia 630");
        }
        [Test]
        public void GetSubSkillBySkillId()
        {
            Assert.IsTrue(_subSkillService.GetSubSkillBySkillId(2).Result.Count() == 3);
            Assert.ThrowsAsync< ArgumentException> (() =>  _subSkillService.GetSubSkillBySkillId(4));
        }
    }
}
