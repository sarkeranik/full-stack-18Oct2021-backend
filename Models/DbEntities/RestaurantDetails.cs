// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestaurantDetails.cs" company="Anik">
//   
// </copyright>
// <summary>
//   The restaurant details.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Models.DbEntities
{
    /// <summary>
    /// The restaurant details.
    /// </summary>
    public class RestaurantDetails : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the available week day's id.
        /// </summary>
        public int DayOfWeeKId { get; set; }

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
        public string TimePeriod { get; set; } = string.Empty;
    }
}
