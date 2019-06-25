using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cBelt2.Models
{
  public class User
  {
    [Key]
    public int UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    [Required]
    public string Password {get; set;}  
    public List<Like> UserLikes {get; set;}
    public List<Idea> UsersIdeas {get; set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public User()
    {
      UserLikes = new List<Like>();
      UsersIdeas = new List<Idea>();
    }
  }
}