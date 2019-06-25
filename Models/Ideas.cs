using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cBelt2.Models
{
  public class Idea
  {
    [Key]
    public int IdeaId {get; set;}
    public int UserId {get; set;}
    [Required]
    [MinLength(5, ErrorMessage = "Idea must be at least 5 characters")]
    public string UserIdea {get; set;}
    public User Owner {get; set;}
    public List<Like> LikedBy {get; set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public Idea()
    {
      LikedBy = new List<Like>();
    }
  }
}