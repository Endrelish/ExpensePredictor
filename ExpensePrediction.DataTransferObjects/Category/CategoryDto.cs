﻿namespace ExpensePrediction.DataTransferObjects.Category
{
    public class CategoryDto : IDataTransferObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}