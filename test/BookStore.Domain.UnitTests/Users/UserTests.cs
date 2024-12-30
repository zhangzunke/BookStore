﻿using BookStore.Domain.UnitTests.Infrastructure;
using BookStore.Domain.Users;
using BookStore.Domain.Users.Events;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.UnitTests.Users
{
    public class UserTests : BaseTest
    {
        [Fact]
        public void Create_Should_SetPropertyValue()
        {
            // Act
            var user = User.CreateUser(UserData.FirstName, UserData.LastName, UserData.Email);

            // Assert
            user.FirstName.Should().Be(UserData.FirstName);
            user.LastName.Should().Be(UserData.LastName);
            user.Email.Should().Be(UserData.Email);
        }

        [Fact]
        public void Create_Should_RaiseUserCreatedDomainEvent()
        {
            // Act
            var user = User.CreateUser(UserData.FirstName, UserData.LastName, UserData.Email);

            // Assert
            var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

            userCreatedDomainEvent.UserId.Should().Be(user.Id);
        }

        [Fact]
        public void Create_Should_AddRegisteredRoleToUser()
        {
            // Act
            var user = User.CreateUser(UserData.FirstName, UserData.LastName, UserData.Email);

            // Assert
            user.Roles.Should().Contain(Role.Registered);
        }
    }
}
