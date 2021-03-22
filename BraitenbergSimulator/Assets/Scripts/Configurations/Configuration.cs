namespace Configurations {
	public interface Configuration {
		string Name();
		string Description();

		object Get();
		void Set(object value);
	}
}