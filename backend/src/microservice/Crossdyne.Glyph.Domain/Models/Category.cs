using Crossdyne.Glyph.Domain.Primitives;
using Crossdyne.Glyph.Domain.ValueObjects.Categories;
using Crossdyne.Glyph.Domain.ValueObjects.Shared;

namespace Crossdyne.Glyph.Domain.Models
{
    public sealed class Category : AggregateRoot<CategoryId>
    {
        public UserId? UserId { get; private set; }
        public CategoryName Name { get; private set; }
        public bool IsPublic { get; private set; }

        private Category()
        {
            
        }

        private Category(CategoryName name, UserId? userId) : base(CategoryId.New())
        {
            UserId = userId;
            Name = name;
            IsPublic = userId == null;
        }

        public static Category Create(CategoryName name, UserId? userId = null)
        {
            return new(name, userId);
        }

        public bool CanAccess(UserId userId) => UserId == null || UserId == userId;

        public void UpdateName(CategoryName categoryName)
        {
            Name = categoryName;
        }
    }
}