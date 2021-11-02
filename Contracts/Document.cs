using System;

namespace Contracts
{
	public class Document
	{
		public string Name { get; set; }
		public string Base64Bytes { get; set; }
		public bool Optimize { get; set; }
		public Guid Id { get; set; }
	}
}
