//using Bogus;
//using RedditMockup.Model.Entities;
//using System.Collections.Generic;

//namespace RedditMockup.IntegrationTest;

//internal class FakeData
//{
//    public FakeData()
//    {

//    }

//    public static IEnumerable<object[]> FakeUsers()
//    {
//        int id = 0;

//        var user = new Faker<User>()
//            .RuleFor(user => user.Id, faker => id++)
//            .RuleFor(user => user.Password, faker => faker.Internet.Password())
//            .RuleFor(user => user.Username, faker => faker.Internet.UserName())
//            .RuleFor(user => user.Score, faker => faker.Random.Number(0, int.MaxValue);

//        var users = new User[10000];

//        for (int i = 0; i < 10000; i++)
//        {
//            users[i] = (user.Generate());
//        }

//        var result = new List<object[]>
//        {
//            users
//        };

//        return result;

//    }


//    public static IEnumerable<object[]> FakePersons()
//    {
//        int id = 0;

//        var person = new Faker<Model.Entities.Person>()
//            .RuleFor(person => person.Id, faker => id++)
//            .RuleFor(person => person.Name, faker => faker.Name.FirstName())
//            .RuleFor(person => person.Family, faker => faker.Name.LastName());

//        var users = new User[10000];

//        for (int i = 0; i < 10000; i++)
//        {
//            users[i] = (user.Generate());
//        }

//        var result = new List<object[]>
//        {
//            users
//        };

//        return result;

//    }

//}