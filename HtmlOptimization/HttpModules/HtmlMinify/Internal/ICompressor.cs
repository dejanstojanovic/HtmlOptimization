namespace HtmlOptimization.HttpModules.HtmlMinify.Internal
{
	public interface ICompressor
	{
		string compress(string source);
	}
}