using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DbEntities
{
    public class Collection : BaseEntity
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int? FavoriteCollectionId { get; set; }
    }
}
