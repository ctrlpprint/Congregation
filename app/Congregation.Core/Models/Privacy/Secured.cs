namespace Congregation.Core.Models.Privacy
{
	public class Secured<T> : IComponent
	{
		public virtual T Value { get; set; }
		public virtual Visibility Visibility { get; set; }

	}
}