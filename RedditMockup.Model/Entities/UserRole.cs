using Hoorbakht.RedisService.Contracts;
using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedditMockup.Model.Entities;

public class UserRole : BaseEntity
{
    #region [Properties]

    [Sieve(CanFilter = true, CanSort = true)]
    public int UserId { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int RoleId { get; set; }

    [CacheableContract]
    [ForeignKey("UserId")]
    public User? User { get; set; }

    [CacheableContract]
    [ForeignKey("RoleId")]
    public Role? Role { get; set; }

    #endregion
}