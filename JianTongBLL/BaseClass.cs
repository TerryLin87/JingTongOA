using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongBLL
{
    public class BaseClass
    {
        public BaseClass()
        {
            if (CreateTime == DateTime.MinValue)
            {
                CreateTime = DateTime.Now;
            }
        }
        public DateTime CreateTime { get; set; }

        public DateTime LastTime { get; set; }

        public bool IsDelete { get; set; }
    }

    public class StringIdClass : BaseClass
    {
        public StringIdClass()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = System.Guid.NewGuid().ToString("N");
            }
        }
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class IntIdClass : BaseClass
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
