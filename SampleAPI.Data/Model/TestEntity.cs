using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleAPI.Data.Model
{
    public partial class TestEntity
    {
        [Key]
        public int ID { get; set; }
    }
}
