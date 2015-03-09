# HtmlOptimization
ASP.NET http modules for optimizing HTML output suitable for both WebForms and MVC

##What is it?
I got the idea to write this library after optimizing my site to get highest Google PageSpeed score. One of t things was to minify html and to compress the output. 
Compressiong i supported on IIS out of the box but on shared host this is ususaly not the case. So I wrote two modules and put it in a library.
After some time I added configuration and some additional helpers to make my life easier when adding minified stylesheet and JavaScript with some additional options which are not mplemented in so far in ASP.NET MVC.

##How to use?
###Http modules for html minify and html compression
After adding the reference to a HtmlOPtimizaion library, you need to add these two modules in web.config
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <modules>
      <add name="MinifyModule" type="HtmlOptimization.HttpModules.MinifyModule, HtmlOptimization" />
      <add name="CompressModule" type="HtmlOptimization.HttpModules.CompressModule, HtmlOptimization" />
    </modules>
  </system.webServer>
</configuration>
```

Modules need to be in this order, otherwise output will be invalid.
In case your host supports compression, then you do not need to use compress module, you can remove it from the configuration modules section.
For custom settings for the modules, the following section needs to be added to web.config file.
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="htmlOptimization" type="HtmlOptimization.Config.Sections.ConfigSection, HtmlOptimization" allowDefinition="Everywhere" allowLocation="true" />
  </configSections>
  <system.webServer>
    <modules>
      <add name="MinifyModule" type="HtmlOptimization.HttpModules.MinifyModule, HtmlOptimization" />
      <add name="CompressModule" type="HtmlOptimization.HttpModules.CompressModule, HtmlOptimization" />
    </modules>
  </system.webServer>
</configuration>
```
###Html helpers
The library provides two html helpers (for now) for minifying and rendering stylesheet and javascript tags. To use them the only thing you need to do is to add 
@using HtmlOptimization.HtmlHelpers
