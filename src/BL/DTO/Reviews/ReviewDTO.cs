﻿using System;

namespace BL.DTO
{
    public class ReviewDTO : ReviewEditDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EditDate { get; set; }

        public string CreatedByFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string CreatedByFullName { get { return $"{CreatedByFirstName} {CreatedByLastName}"; } }

        public string CreatedByImageStorageFileName { get; set; }

        public int QualityInt { get { return (int)Quality; } }
    }
}