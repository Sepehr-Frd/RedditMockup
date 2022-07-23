using RedditMockup.Common.Dtos;
using RedditMockup.Common.ViewModels;
using System.Collections.Generic;

namespace RedditMockup.IntegrationTest;

internal class AccountControllerTest : IntegrationTest
{
    private const string BaseAddress = "/api/Account";

    public AccountControllerTest()
    {

    }

    #region [Data Method(s)]

    public static IEnumerable<object[]> GenerateGetAllData()
    {

        yield return new object[] {
                new UserViewModel()
            {
                Id = 1,
                Username = "sepehr_frd",
                PersonFullName = "Sepehr Foroughi Rad",
                Roles = new string[] { "User" }
            }
            };

        yield return new object[] {
                new UserViewModel()
            {
                Id = 2,
                Username = "ali_alipour",
                PersonFullName = "Ali Alipour",
                Roles = new string[] { "Admin" }
            }
            };

        yield return new object[] {
                new UserViewModel()
            {
                Id = 3,
                Username = "admin_admin",
                PersonFullName = "Abbas Abbasi",
                Roles = new string[] { "Admin" }
            }
            };

        yield return new object[] {
                new UserViewModel()
            {
                Id = 4,
                Username = "ahmad_ahmadi",
                PersonFullName = "Ahmad Ahmadi",
                Roles = new string[] { "User" }
            }
            };

        yield return new object[] {
                new UserViewModel()
            {
                Id = 5,
                Username = "hossein_hosseini",
                PersonFullName = "Hossein Hosseini",
                Roles = new string[] { "User" }
            }
            };

        yield return new object[] {
                new UserViewModel()
            {
                Id = 6,
                Username = "sara_abbasi",
                PersonFullName = "Sara Abbasi",
                Roles = new string[] { "User" }
            }
            };


    }

    public static IEnumerable<object[]> GenerateLoginData()
    {
        yield return new object[] {
                new LoginDto()
            {
                Username = "sepehr_frd",
                Password = "sfr1376"
            }
            };
        yield return new object[] {
                new LoginDto()
            {
                Username = "admin_admin",
                Password = "adminnnn"
            }
            };
        yield return new object[] {
                new LoginDto()
            {
                Username = "sepehr_frd",
                Password = "asdasdasdasd"
            }
            };

        yield return new object[] {
                new LoginDto()
            {
                Username = "sepehr_d",
                Password = "sfr1376"
            }
            };

        yield return new object[] {
                new LoginDto()
            {
                Username = "223",
                Password = "sd2"
            }
            };





    }

    #endregion

}
