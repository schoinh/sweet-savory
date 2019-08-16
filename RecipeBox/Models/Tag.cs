using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<TagRecipe> Recipes { get; }
        public virtual ApplicationUser User { get; set; }

        public Tag()
        {
            this.Recipes = new HashSet<TagRecipe>();
        }
    }
}