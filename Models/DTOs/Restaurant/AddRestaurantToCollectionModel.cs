using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs.Restaurant
{
    public class AddRestaurantToCollectionModel
    {
        public AddRestaurantToCollectionModel()
        {
            CollectionList= new List<string>();
        }
        public string Name { get; set; }
        public List<string> CollectionList { get; set; }
    }
}
