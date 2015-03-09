# HtmlOptimization
ASP.NET http modules for optimizing HTML output suitable for both WebForms and MVC

##What is it?
I got the idea to write this library after optimizing my site to get highest Google PageSpeed score. One of t things was to minify html and to compress the output. 
Compressiong i supported on IIS out of the box but on shared host this is ususaly not the case. So I wrote two modules and put it in a library.
After some time I added configuration and some additional helpers to make my life easier when adding minified stylesheet and JavaScript with some additional options which are not mplemented in so far in ASP.NET MVC.

##How to use?
###Http modules for html minify and html compression
After adding the reference to a HtmlOPtimizaion library, you need to add these two modules in web.config
