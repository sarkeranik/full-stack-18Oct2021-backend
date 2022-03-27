using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs.Restaurant
{
    public class RestaurantDetailsModel
    {
        public RestaurantDetailsModel()
        {
            CollectionModels = new List<CollectionModel>();
        }

        /// <summary>
        /// Gets or sets the Model Id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the opening time.
        /// </summary>
        public string OpeningTime { get; set; }

        /// <summary>
        /// Gets or sets the closing time.
        /// </summary>
        public string ClosingTime { get; set; }

        /// <summary>
        /// Gets or sets the Time Period
        /// </summary>
        public string TimePeriod { get; set; }

        /// <summary>
        /// Gets or sets the Collection Models
        /// </summary>
        public List<CollectionModel> CollectionModels { get; set; }
    }
}
