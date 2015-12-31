//-----------------------------------------------------------------------
// <copyright file="UserControllerTest.cs" company="Interactive Intelligence">
//     Copyright (c) Interactive Intelligence. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Author: Paul Simpson
/// Version: 1.0 - Initial build.
/// </summary>
namespace Labinator2016R1.Tests.Controllers
{
    using Labinator2016R1.Controllers;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Labinator2016R1.Lib.Models;
    using NUnit.Framework;
    using Labinator2016R1.ViewModels.DatatablesViewModel;
    using TestData;
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public void UsersAjaxList()
        {
            var db = new FakeDatabase();
            db.AddSet(TestUserData.Users);
            var controller = new UsersController(db);
            controller.ControllerContext = new FakeControllerContext();
            DTParameters param = new DTParameters() { Start = 10, Length = 5, Search = new DTSearch(), Order = new DTOrder[1] { new DTOrder() { Column = 1, Dir = DTOrderDir.ASC } } };
            JsonResult result = controller.Ajax(param) as JsonResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, ((List<User>)((DTResult<User>)result.Data).data).Count);
            Assert.AreEqual("TestUser10@test.com", ((List<User>)((DTResult<User>)result.Data).data)[0].EmailAddress);
        }
        [Test]
        public void UsersControllerIndexTest()
        {
            // Arrange
            UsersController controller = new UsersController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void UserEditNewStartTest()
        {
            var db = new FakeDatabase();
            db.AddSet(TestUserData.Users);
            //db.AddSet(TestCourseData.Courses);
            //db.AddSet(TestClassroomData.Classrooms);
            var controller = new UsersController(db);
            controller.ControllerContext = new FakeControllerContext();
            ViewResult result = controller.Edit(0) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(User), result.Model.GetType());
            Assert.AreEqual("New", ((User)result.Model).EmailAddress);
        }
        [Test]
        public void UserEditExistingStartTest()
        {
            var db = new FakeDatabase();
            //var st = new FakeSkyTap();
            db.AddSet(TestUserData.Users);
            //db.AddSet(TestCourseData.Courses);
            //db.AddSet(TestClassroomData.Classrooms);
            //st.AddSet(TestTemplateRESTData.templates);
            var controller = new UsersController(db);
            controller.ControllerContext = new FakeControllerContext();
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(User), result.Model.GetType());
            Assert.AreEqual("TestUser1@test.com", ((User)result.Model).EmailAddress);
        }
        [Test]
        public void UserEditNewWriteTest()
        {
            var db = new FakeDatabase();
            //var st = new FakeSkyTap();
            db.AddSet(TestUserData.Users);
            //db.AddSet(TestCourseData.Courses);
            //db.AddSet(TestClassroomData.Classrooms);
            //st.AddSet(TestTemplateRESTData.templates);
            var controller = new UsersController(db);
            controller.ControllerContext = new FakeControllerContext();
            User testCourse = new User() { UserId = 0, EmailAddress = "TestNew" };
            var result = controller.Edit(testCourse);
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual(1, db.Added.Count);
            Assert.AreEqual("TestNew", ((User)db.Added[0]).EmailAddress);
            Assert.AreEqual(1, db.saved);
        }
        [Test]
        public void UserEditExistingWriteTest()
        {
            var db = new FakeDatabase();
            //var st = new FakeSkyTap();
            db.AddSet(TestUserData.Users);
            //st.AddSet(TestTemplateRESTData.templates);
            var controller = new UsersController(db);
            controller.ControllerContext = new FakeControllerContext();
            User testCourse = new User() { UserId = 1, EmailAddress = "TestUpdate" };
            var result = controller.Edit(testCourse);
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual(0, db.Added.Count);
            Assert.AreEqual(1, db.Updated.Count);
            Assert.AreEqual("TestUpdate", ((User)db.Updated[0]).EmailAddress);
            Assert.AreEqual(1, db.saved);
        }
    }
}