namespace HtmlOptimization.Modules.HtmlMinify.Internal
{
	public interface ICompressor
	{
		string compress(string source);
	}
}