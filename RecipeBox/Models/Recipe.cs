using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(60000)]
        public string Ingredients { get; set; }

        [StringLength(60000)]
        public string Instructions { get; set; }

        public int Rating { get; set; }

        public ICollection<TagRecipe> Tags { get; set; } // optional setter?
        public virtual ApplicationUser User { get; set; }

        public Recipe()
        {
            this.Tags = new HashSet<TagRecipe>();
        }
    }
}