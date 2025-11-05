using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoWave.Core.Models.ResponseModels
{
    public class LoginResponseVm
    {
        public string? Token { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Guid? UserId { get; set; }
    }
}
