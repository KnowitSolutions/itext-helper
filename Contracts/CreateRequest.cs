using System.Collections.Generic;

namespace Contracts
{
    public class CreateRequest
    {
		public Document Document { get; set; }
		public List<FieldItem> Globals { get; set; }
		public List<Mapping> Mappings { get; set; }
		public List<RepeatedField> Values { get; set; }
    }
}
